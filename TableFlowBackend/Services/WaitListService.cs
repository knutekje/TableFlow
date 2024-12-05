using Microsoft.AspNetCore.SignalR;
using TableFlowBackend.Hubs;

namespace TableFlowBackend.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TableFlowBackend.Models;

public class WaitlistService
{
  private readonly IRepository<Waitlist> _waitlistRepository;
  private readonly IHubContext<NotificationHub> _hubContext;
  
  
  public WaitlistService(IRepository<Waitlist> waitlistRepository, IHubContext<NotificationHub> hubContext)
  {
    _waitlistRepository = waitlistRepository;
    _hubContext = hubContext;
    
    
  }

  public async Task<IEnumerable<Waitlist>> GetAllWaitlistEntriesAsync()
  {
    return await _waitlistRepository.GetAllAsync();
  }

  public async Task AddToWaitlistAsync(Waitlist waitlistEntry)
  {
    if (string.IsNullOrWhiteSpace(waitlistEntry.CustomerName))
      throw new ArgumentException("Customer name cannot be empty.");
    if (waitlistEntry.PartySize <= 0)
      throw new ArgumentException("Party size must be greater than zero.");

    waitlistEntry.RequestedTime = DateTime.UtcNow;
    await _waitlistRepository.AddAsync(waitlistEntry);
  }

  public async Task<Waitlist?> NotifyNextCustomerAsync()
  {
    var allEntries = await _waitlistRepository.GetAllAsync() ?? new List<Waitlist>();
    var nextEntry = allEntries.OrderBy(e => e.RequestedTime).FirstOrDefault();

    if (nextEntry != null)
    {
      await _hubContext.Clients.All.SendAsync("ReceiveNotification", 
        $"Table is now available for {nextEntry.CustomerName}.");
      await _waitlistRepository.DeleteAsync(nextEntry.WaitlistId);
    }

    return nextEntry;
  }
}
