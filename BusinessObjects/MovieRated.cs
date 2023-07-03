using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class MovieRated
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public int Rated { get; set; }
        public DateTime RatedTime { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }
    }
}
