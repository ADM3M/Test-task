namespace api.Common;

public static class ExternalId
{
    public static string Generate()
    {
        return Guid.NewGuid().ToString("D");
    }
}