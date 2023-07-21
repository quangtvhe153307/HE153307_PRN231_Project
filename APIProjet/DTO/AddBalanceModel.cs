using System.ComponentModel.DataAnnotations;

namespace APIProject.DTO
{
    public class AddBalanceModel
    {
        [Range(1000, long.MaxValue)]
        public long Amount { get; set; }
    }
}
