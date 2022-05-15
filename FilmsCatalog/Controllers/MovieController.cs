using FilmsCatalog.Models;
using FilmsCatalog.Services;
using FilmsCatalog.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task<IActionResult> Index(int page=1)
        {
            int pageSize = 5;
            var movies = (await _movieService.GetAllMovie()).AsQueryable();
            var count =  movies.Count();
            var items =  movies.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Movies = items
            };
            return View(viewModel);
        }



        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateMovieViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string userEmail = User.Identity.Name;
            await _movieService.Create(model, userEmail);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        //[Route("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var movie = await _movieService.GetMovie(id);
                ViewBag.Author = await _movieService.GetAuthor(id);
                return View(movie);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _movieService.GetMovieView(id);
            return View(movie);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(CreateMovieViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _movieService.Edit(model);
            return RedirectToAction("Index");
        }
    }
}
