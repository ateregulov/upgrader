using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Upgrader.Auth;

namespace Upgrader.Features.Balance;

[ApiController]
[Route("api/balances")]
public class BalanceController : ControllerBase
{
    private readonly BalanceService _balanceService;
    private readonly MyContext _dbContext;

    public BalanceController(BalanceService balanceService, MyContext dbContext)
    {
        _balanceService = balanceService;
        _dbContext = dbContext;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetBalance()
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.TelegramId == headersData.TelegramId);

        var balance = await _balanceService.GetBalanceAsync(user.Id);

        return Ok(balance);
    }
}
