namespace IntegrationHub.Web.Models
{
    public class RegisterDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        // Username'i Email ile eşitlediğimiz için burada ayrıca Username istemiyoruz
        public string Username => Email;
    }
}