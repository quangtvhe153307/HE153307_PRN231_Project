using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Apptransid { get; set; }
        public int UserId { get; set; }
        public long Amount { get; set; }
        public bool Status { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
