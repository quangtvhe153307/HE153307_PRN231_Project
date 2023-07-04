using BusinessObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using APIProject.DTO.User;

namespace APIProject.DTO.Transaction
{
    public class GetTransactionResponseDTO
    {
        [Key]
        public int TransactionId { get; set; }
        public string TransactionDescription { get; set; }
        public DateTime TransactionDate { get; set; }
        public int TransactionType { get; set; }
        public int UserId { get; set; }
        public GetUserResponseDTO User { get; set; }
    }
}
