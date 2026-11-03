namespace Exam.Api.Configuration;

public static class ApiLimits
{
    public const int DescriptionCharacters = 20000;
    public const long ImageBytes = 8 * 1024 * 1024;
    public const long RequestBytes = 9 * 1024 * 1024;
}
