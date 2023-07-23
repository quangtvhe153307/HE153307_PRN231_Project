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
        public string TransactionDateStr { get; set; }
        public int TransactionType { get; set; }
        public int UserId { get; set; }
        public GetUserResponseDTO User { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public GetUserResponseDTO ModifiedUser { get; set; }
        public int CreatedBy { get; set; }
        public GetUserResponseDTO CreatedUser { get; set; }
    }
}
