using ApplicationCore.Messages;

namespace Web.Endpoints
{
    public class AuthRequest : MessageRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
