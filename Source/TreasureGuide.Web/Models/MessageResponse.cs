namespace TreasureGuide.Web.Models
{
    public class MessageResponse
    {
        public MessageResponse()
        {
        }

        public MessageResponse(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
