using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class MovieEpisode
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EpisodeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string EpisodeImage { get; set; }
        public int MovieSeasonId { get; set; }
        [ForeignKey("MovieSeasonId")]
        public virtual MovieSeason MovieSeason { get; set; }
        public virtual ICollection<MovieView> MovieViews { get; set; }
    }
}
