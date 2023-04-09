using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessageBroker.Models;

public class Subscription
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public string? Id { get; set; }
  public string? Name { get; set; }
  public string? TopicId { get; set; }
}