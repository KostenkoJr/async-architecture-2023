using System.Text.Json;
using Confluent.Kafka;
using TaskTracker.Consumer.Events;
using TaskTracker.Data.Context;
using TaskTracker.Data.Models;

var config = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "foo",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

using var consumer = new ConsumerBuilder<string, string>(config).Build();
consumer.Subscribe("users-stream");
using var db = new TaskTrackerDbContext();

while (true)
{
    var consumeResult = consumer.Consume();
    var user = JsonSerializer.Deserialize<UserCreatedEvent>(consumeResult.Message.Value);

    if (user is null)
    {
        //Log error
        continue;
    }
    
    db.Users.Add(new User
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        Role = user.Role
    });
    
    db.SaveChangesAsync();
}