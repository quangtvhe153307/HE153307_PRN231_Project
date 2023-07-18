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
        public virtual List<RefreshToken> RefreshTokens{ get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
        public virtual ICollection<PurchasedMovie> PurchasedMovies { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool EmailConfirmed { get; set; }
        public double Balance { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("other");
            }
            User user = obj as User;
            if(user == null)
            {
                throw new ArgumentNullException("other");
            }
            if (this.FirstName.Equals(user.FirstName)
                && this.LastName.Equals(user.LastName)
                && this.UserId == user.UserId
                && this.RoleId == user.RoleId
                && this.Email.Equals(user.Email)
                && this.Password.Equals(user.Password)
                )
            {
                return true;
            }
            return false;
        }
    }
}
