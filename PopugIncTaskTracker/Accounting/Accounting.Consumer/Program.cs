using System.Text.Json;
using Accounting.Consumer;
using Accounting.Data.Context;
using Accounting.Data.Entities;
using Confluent.Kafka;

var config = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "foo",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

using var consumer = new ConsumerBuilder<string, string>(config).Build();
consumer.Subscribe(new [] {"users-stream", "tasks-stream", "tasks-lifecycle"});
using var db = new AccountingDbContext();

while (true)
{
    var consumeResult = consumer.Consume();
    var user = JsonSerializer.Deserialize<UserCreatedEvent>(consumeResult.Message.Value);

    if (user is null)
    {
        //Log error
        continue;
    }

    var existingUser = db.Users.FirstOrDefault(u => u.PublicId == user.PublicId);
    if (existingUser is not null)
    {
        continue;
    }

    var localUser = new User
    {
        Id = Guid.NewGuid(),
        PublicId = user.PublicId,
        Name = user.Name,
        Email = user.Email,
        Role = user.Role
    };
    
    db.Users.Add(localUser);

    db.Accounts.Add(new Account
    {
        Id = Guid.NewGuid(),
        PublicId = Guid.NewGuid(),
        Balance = 0,
        UserId = localUser.Id,
        PublicUserId = localUser.PublicId,
        Transactions = Array.Empty<Transaction>()
    });
    
    db.SaveChangesAsync();
}