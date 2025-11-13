using System.Text;

namespace Upgrader.Features.ReferralSystem;

public class RefCodeConverter
{
    private const string Base36Chars = "0123456789abcdefghijklmnopqrstuvwxyz";

    public static string IntToBase60(long number)
    {
        if (number == 0)
            return "0";

        StringBuilder result = new StringBuilder();

        while (number > 0)
        {
            result.Insert(0, Base36Chars[(int)(number % 36)]);
            number /= 36;
        }

        return result.ToString();
    }
}