using FilmsCatalog.Models;
using System.ComponentModel.DataAnnotations;

namespace FilmsCatalog.Storage
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string? Poster { get; set; }
    }
}
