using BusinessObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIProject.DTO.Transaction
{
    public class CreateTransactionRequestDTO
    {
        [Required]
        public string TransactionDescription { get; set; }
        [Required]
        public int TransactionType { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
