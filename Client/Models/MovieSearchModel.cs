using BusinessObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class MovieSearchModel
    {
        public string Title { get; set; }
        public List<int> Categories { get; set; }
    }
}
