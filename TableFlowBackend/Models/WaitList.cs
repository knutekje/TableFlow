namespace TableFlowBackend.Models;

public class Waitlist
{
    public int WaitlistId { get; set; }
    public string CustomerName { get; set; }
    public DateTime RequestedTime { get; set; }
    public int PartySize { get; set; }
    public string? Notes { get; set; }
}
