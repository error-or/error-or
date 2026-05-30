using ErrorOr;

namespace Tests;

public static class Convert
{
    public static ErrorOr<string> ToString(int num) => num.ToString();

    public static ErrorOr<int> ToInt(string str) => int.Parse(str);

    public static Task<ErrorOr<int>> ToIntAsync(string str) => ErrorOrFactory.FromAsync(int.Parse(str));

    public static Task<ErrorOr<string>> ToStringAsync(int num) => ErrorOrFactory.FromAsync(num.ToString());
}
