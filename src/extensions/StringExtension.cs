using System.Text;

namespace extensions;

public static class StringExtension
{
    public static string ToBase64String(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
    }
}
