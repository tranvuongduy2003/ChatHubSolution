namespace ChatHubSolution.DTOs
{
    public class MessageDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ConversationId { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
