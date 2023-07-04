using BusinessObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIProject.DTO.Transaction
{
    public class UpdateTransactionRequestDTO
    {
        public int TransactionId { get; set; }
        public string TransactionDescription { get; set; }
        public DateTime TransactionDate { get; set; }
        public int TransactionType { get; set; }
        public int UserId { get; set; }
    }
}
