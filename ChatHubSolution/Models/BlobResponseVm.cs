namespace ChatHubSolution.Models
{
    public class BlobResponseVm
    {
        public BlobResponseVm()
        {
            Blob = new BlobVm();
        }

        public string? Status { get; set; }
        public bool Error { get; set; }
        public BlobVm Blob { get; set; }
    }
}
