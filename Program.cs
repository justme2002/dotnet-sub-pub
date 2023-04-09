using MessageBroker.Data;
using MessageBroker.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => {
  options.UseSqlite("Data Source=MessageBroker.db");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Create Topic
app.MapPost("api/topics", async (AppDbContext context, Topic topic) => {
  await context.Set<Topic>().AddAsync(topic);
  await context.SaveChangesAsync();
  return Results.Created(@$"api/topics/{topic.Id}", topic);
});

app.MapGet("api/topics", async (AppDbContext context) => {
  var topics = await context.Set<Topic>().ToListAsync();
  return Results.Ok(topics);
});

//Publish Message

app.MapPost("api/topics/{id}/messages", async (AppDbContext context, string id, Message message) => {
  var topics = await context.Set<Topic>().AnyAsync(t => t.Id == id);
  if (!topics) return Results.NotFound("Topic not found");

  var subs = context.Set<Subscription>().Where(s => s.TopicId == id);
  if (subs.Count() == 0) return Results.NotFound("There are no sub for this topic");

  foreach (var sub in subs)
  {
    var msg = new Message
    {
      TopicMessage = message.TopicMessage,
      SubscriptionId = sub.Id,
      ExpireAfter = message.ExpireAfter,
      MessageStatus = message.MessageStatus
    };
    await context.Messages!.AddAsync(msg);
  }
  await context.SaveChangesAsync();

  return Results.Ok("Message has been added");
});

//
app.MapPost("api/topics/{id}/subscription", async (AppDbContext context, string id, Subscription sub) => {
  var topics = await context.Topics.AllAsync(t => t.Id == id);
  if (!topics) return Results.NotFound("Topic not found");
  sub.TopicId = id;
  await context.Subscriptions.AddAsync(sub);
  await context.SaveChangesAsync();
  return Results.Created($"api/topics{id}/subs/{sub.Id}", sub);
});

app.Run();
