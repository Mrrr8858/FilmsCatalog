using FilmsCatalog.Storage;
using System.Collections.Generic;

namespace FilmsCatalog.Models
{
    public class IndexViewModel
    {
        public IEnumerable<ShortMovieViewModel> Movies { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
