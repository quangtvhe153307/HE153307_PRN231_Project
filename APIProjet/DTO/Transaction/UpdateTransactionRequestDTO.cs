using BusinessObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIProject.DTO.Transaction
{
    public class UpdateTransactionRequestDTO
    {
        [Required]
        public int TransactionId { get; set; }
        [Required]
        public string TransactionDescription { get; set; }
    }
}
