using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FilmsCatalog.Models
{
    public class CreateMovieViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Режиссер")]
        public string Director { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Год выпуска")]
        [Range(1888, Int32.MaxValue, ErrorMessage = "Введите корректный год выхода фильма")]
        public int Year { get; set; }
        public IFormFile Poster { get; set; }
    }

    public class ShortMovieViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
    public class MovieViewModel: ShortMovieViewModel
    {
        public string Director { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string? Poster { get; set; }
    }
}
