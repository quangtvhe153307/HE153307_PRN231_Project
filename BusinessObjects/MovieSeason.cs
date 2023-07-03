using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class MovieSeason
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MovieSeasonId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReleasedDate { get; set; }
        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }
        public virtual ICollection<MovieEpisode> MovieEpisodes { get; set; }
    }
}
