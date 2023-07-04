using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens{ get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
        public virtual ICollection<PurchasedMovie> PurchasedMovies { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
