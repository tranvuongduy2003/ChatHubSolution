namespace ChatHubSolution.DTOs
{
    public class ConversationDto
    {
        public string Id { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string ConnectionId { get; set; }
        public string Status { get; set; }
        public string? LastMessage { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
