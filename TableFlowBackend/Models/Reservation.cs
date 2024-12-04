namespace TableFlowBackend.Models;

public class Reservation
{
    public int ReservationId { get; set; }
    public string CustomerName { get; set; }
    public DateTime ReservationDate { get; set; }
    public int PartySize { get; set; }
    public string SpecialRequests { get; set; } = "None";
    public int TableId { get; set; }
    public Table Table { get; set; }
}