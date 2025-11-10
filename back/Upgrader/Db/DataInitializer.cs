using Microsoft.EntityFrameworkCore;
using Upgrader.Users;

namespace Upgrader.Db
{
    public class DataInitializer
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<MyContext>();
            await SeedDevUser(context!);
        }

        private static async Task SeedDevUser(MyContext myContext)
        {
            var devUser = await myContext.Users.FirstOrDefaultAsync(u => u.TelegramId == 1);
            if (devUser == null)
            {
                var user = new User
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), // Dev user ID
                    TelegramId = 1,
                    Created = DateTimeOffset.UtcNow,
                };
                myContext.Users.Add(user);
                await myContext.SaveChangesAsync();
            }
        }
    }
}
