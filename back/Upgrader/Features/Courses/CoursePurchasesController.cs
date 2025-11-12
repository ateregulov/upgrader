using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Upgrader.Auth;
using Upgrader.Features.Balance;

namespace Upgrader.Features.Courses;

[ApiController]
[Route("api/course-purchases")]
public class CoursePurchasesController : ControllerBase
{
    private readonly MyContext _dbContext;
    private readonly BalanceService _balanceService;

    public CoursePurchasesController(MyContext dbContext, BalanceService balanceService)
    {
        _dbContext = dbContext;
        _balanceService = balanceService;
    }

    [HttpGet("payment-info")]
    public async Task<IActionResult> GetCoursePaymentInfo(Guid courseId)
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var user = await _dbContext.Users.SingleOrDefaultAsync(x =>
            x.TelegramId == headersData.TelegramId
        );

        var price = await _dbContext
            .Courses.Where(x => x.Id == courseId)
            .Select(x => x.Price)
            .FirstOrDefaultAsync();

        var balance = await _balanceService.GetBalanceAsync(user.Id);

        return Ok(new CoursePaymentInfo { Price = price, Balance = balance });
    }

    public class CoursePaymentInfo
    {
        public decimal Price { get; set; }
        public decimal Balance { get; set; }
    }
}
