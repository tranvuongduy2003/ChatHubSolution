namespace ChatHubSolution.DTOs
{
    public class SendMessageRequestDto
    {
        public string ConversationId { get; set; }
        public string SenderId { get; set; }
        public string Content { get; set; }
    }
}
