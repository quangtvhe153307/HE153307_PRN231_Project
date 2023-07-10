using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class MovieView
    {
        public int UserId { get; set; }
        public int EpisodeId { get; set; }
        public DateTime ViewedDate { get; set; }
        public virtual User User { get; set; }
        public virtual MovieEpisode MovieEpisode { get; set; }
    }
}
