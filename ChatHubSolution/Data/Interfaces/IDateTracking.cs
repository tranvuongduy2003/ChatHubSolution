namespace ChatHubSolution.Data.Interfaces
{
    public interface IDateTracking
    {
        string CreatedAt { get; set; }
        string? UpdatedAt { get; set; }
    }
}
