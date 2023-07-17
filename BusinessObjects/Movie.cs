using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Movie
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public bool IsSingleEpisode { get; set; }
        public double Price { get; set; }
        public bool IsFree => Price == 0;
        public bool IsActive { get; set; }
        public string? MovieImage { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? TrailerUrl { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<MovieSeason> MovieSeasons { get; set; }
    }
}
