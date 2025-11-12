using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Upgrader.Auth;
using Upgrader.Features.Balance;
using Upgrader.Features.Transactions;

namespace Upgrader.Features.Courses;

[ApiController]
[Route("api/course-purchases")]
public class CoursePurchasesController : ControllerBase
{
    private readonly MyContext _dbContext;
    private readonly BalanceService _balanceService;
    private readonly TransactionService _transactionService;

    public CoursePurchasesController(
        MyContext dbContext,
        BalanceService balanceService,
        TransactionService transactionService
    )
    {
        _dbContext = dbContext;
        _balanceService = balanceService;
        _transactionService = transactionService;
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

    [HttpPost]
    public async Task<IActionResult> PurchaseCourse(PurchaseCourseDto dto)
    {
        var headersData = await this.GetHeadersData();
        if (headersData == null)
            return Unauthorized();

        var course = await _dbContext
            .Courses.Where(x => x.Id == dto.CourseId)
            .FirstOrDefaultAsync();
        if (course == null)
            return NotFound("Курс не найден");

        var user = await _dbContext.Users.SingleOrDefaultAsync(x =>
            x.TelegramId == headersData.TelegramId
        );

        var isCourseAlreadyBought = await _dbContext.CoursePurchases.AnyAsync(x =>
            x.UserId == user.Id && x.CourseId == dto.CourseId
        );
        if (isCourseAlreadyBought)
            return BadRequest("Курс уже был куплен");

        var balance = await _balanceService.GetBalanceAsync(user.Id);
        if (course.Price > balance)
            return BadRequest("Недостаточно средств на счету");

        await _transactionService.CreateTransactionAsync(
            amount: course.Price,
            type: TransactionType.CoursePurchase,
            senderId: user.Id,
            uniqueKey: $"course-purchase-{user.Id}-{dto.CourseId}"
        );

        await _dbContext.CoursePurchases.AddAsync(
            new CoursePurchase
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                CourseId = dto.CourseId,
                PaidAmount = course.Price,
            }
        );
        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    public class PurchaseCourseDto
    {
        public Guid CourseId { get; set; }
    }

    public class CoursePaymentInfo
    {
        public decimal Price { get; set; }
        public decimal Balance { get; set; }
    }
}
