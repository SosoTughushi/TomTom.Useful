namespace TomTom.Useful.Demo.WebApi.Models
{
    public class AcceptedResponse
    {
        public AcceptedResponse(Guid requestId)
        {
            this.RequestId = requestId;
        }
        public Guid RequestId { get; set; }
    }
}
