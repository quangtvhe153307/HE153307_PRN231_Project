namespace Client.Models
{
    public class EmailConfirmModel
    {
        public int UserId { get; set; }
        public string Token { get; set; }
    }
}
