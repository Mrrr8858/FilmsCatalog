using FilmsCatalog.Data;
using FilmsCatalog.Models;
using FilmsCatalog.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Services
{
    public interface IMovieService
    {
        Task Create(CreateMovieViewModel model, string email);
        Task Edit(CreateMovieViewModel model);
        Task<List<ShortMovieViewModel>> GetAllMovie();
        Task<MovieViewModel> GetMovie(int id);
        Task<CreateMovieViewModel> GetMovieView(int id);
        Task<string> GetAuthor(int id);
    }
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private static string[] AllowedExtensions { get; set; } = { "jpg", "jpeg", "png" };
        public MovieService(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<string> AddFile(IFormFile file)
        {
            var isFileAttached = file != null;
            string fileNameWithPath = null;
            if (isFileAttached)
            {
                var extension = Path.GetExtension(file.FileName).Replace(".", "");
                if (!AllowedExtensions.Contains(extension))
                {
                    throw new ArgumentException("Attached file has not supported extension");
                }
                fileNameWithPath = $"files/{Guid.NewGuid()}-{file.FileName}";
                using (var fs = new FileStream(Path.Combine(_environment.WebRootPath, fileNameWithPath), FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }
            }
            return fileNameWithPath;
        }

        public async Task Create(CreateMovieViewModel model, string email)
        {
            var fileNameWithPath = await AddFile(model.Poster);
            Movie movie = new Movie
            {
                Title = model.Title,
                Year = model.Year,  
                Description = model.Description,
                Director = model.Director,
                Author = email,
                Poster = fileNameWithPath,
            };

            await _context.Movie.AddAsync(movie);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ShortMovieViewModel>> GetAllMovie()
        {
            var movie = await _context.Movie.Select(x => new ShortMovieViewModel
            {
                Id = x.Id,
                Title = x.Title
            }).ToListAsync();
            return movie;
        }

        public async Task<MovieViewModel> GetMovie(int id)
        {
            var movie = await Get(id);
            return new MovieViewModel
            {
                Description = movie.Description,
                Title = movie.Title,
                Id = id,
                Year = movie.Year,
                Director = movie.Director,
                Poster = movie.Poster,
            };
        }
        public async Task<string> GetAuthor(int id)
        {
            var movie = await Get(id);
            return movie.Author;
        }

        public async Task Edit(CreateMovieViewModel model)
        {
            var movie = await Get(model.Id);
            movie.Year = model.Year;
            movie.Title = model.Title;
            movie.Description = model.Description;
            movie.Director = model.Director;

            var fileNameWithPath = await AddFile(model.Poster);
            if(fileNameWithPath is not null)
            {
                if (File.Exists("wwwroot/" + movie.Poster))
                {
                    File.Delete("wwwroot/" + movie.Poster);
                }
                
                movie.Poster = fileNameWithPath;
            }
            await _context.SaveChangesAsync();
        }

        private async Task<Movie> Get(int? id)
        {
            var movie = await _context.Movie.FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                throw new KeyNotFoundException($"Movie with Id={id} does not found!");
            }
            return movie;
        }

        public async Task<CreateMovieViewModel> GetMovieView(int id)
        {
            var movie = await Get(id);
            return new CreateMovieViewModel
            {
                Description = movie.Description,
                Title = movie.Title,
                Id = id,
                Year = movie.Year,
                Director = movie.Director,
            };
        }
    }
}
