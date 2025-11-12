using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OrisAppBack.Other.Settings;

namespace Upgrader.Features.Bonuses;

[ApiController]
[Route("api/bonuses")]
public class BonusesController : ControllerBase
{
    private readonly BonusSettings _bonusSettings;

    public BonusesController(IOptions<AppSettings> appSettingsOts)
    {
        _bonusSettings = appSettingsOts.Value.BonusSettings;
    }

    [HttpGet("register")]
    public async Task<IActionResult> GetRegisterBonus()
    {
        return Ok(_bonusSettings.RegisterBonus);
    }
}
