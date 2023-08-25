using Accounting.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class AccountingController : ControllerBase
{
    private readonly ILogger<AccountingController> _logger;

    public AccountingController(ILogger<AccountingController> logger)
    {
        _logger = logger;
    }

    [HttpGet("balance")]
    public ActionResult<GetBalanceResponse> GetMyBalance()
    {
        var currentUserId = Guid.Parse(User.Claims.First(x => x.Type == "Id").Value);
        using var db = new AccountingDbContext();

        var account = db.Accounts.FirstOrDefault(a => a.PublicUserId == currentUserId);
        if (account is null)
        {
            return NotFound();
        }

        var transactions = db.Transactions
            .Where(t => t.AccountId == account.Id)
            .OrderByDescending(x => x.Date)
            .Take(100);

        var response = new GetBalanceResponse
        {
            Balance = account.Balance,
            Transactions = transactions.Select(x => new GetBalanceResponse.TransactionDto
            {
                Description = x.Description,
                Credit = x.Credit,
                Debit = x.Debit,
                TransactionType = x.TransactionType.ToString()
            }).ToList()
        };
        
        return Ok(response);
    }
    
    [HttpGet("balance")]
    public ActionResult<GetBalanceResponse> GetStaffBalance(Guid staffId)
    {
        var userRole = User.Claims.FirstOrDefault(x => x.Type == "Role");
        var canReassignTasks = userRole?.Value == "Accountant" || userRole?.Value == "Admin";
        
        using var db = new AccountingDbContext();

        var account = db.Accounts.FirstOrDefault(a => a.PublicUserId == staffId);
        if (account is null)
        {
            return NotFound();
        }

        var transactions = db.Transactions
            .Where(t => t.AccountId == account.Id)
            .OrderByDescending(x => x.Date)
            .Take(100);

        var response = new GetBalanceResponse
        {
            Balance = account.Balance,
            Transactions = transactions.Select(x => new GetBalanceResponse.TransactionDto
            {
                Description = x.Description,
                Credit = x.Credit,
                Debit = x.Debit,
                TransactionType = x.TransactionType.ToString()
            }).ToList()
        };
        
        return Ok(response);
    }
}

public record GetBalanceResponse
{
    public double Balance { get; set; }
    public IReadOnlyCollection<TransactionDto> Transactions { get; set; }

    public record TransactionDto
    {
        public string Description { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public string TransactionType { get; set; }
    }
}