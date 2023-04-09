using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessageBroker.Models;

public class Message
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public string? Id { get; set; }
  public string? TopicMessage { get; set; }
  public string? SubscriptionId { get; set; }
  public DateTime ExpireAfter { get; set; } = DateTime.Now.AddDays(1);
  public string? MessageStatus { get; set; } = "NEW";
}