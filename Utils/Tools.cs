namespace FileUploader.Utils;


public static class Tools
{
    public static string TokenGenerator()
    {
        return Guid.NewGuid().ToString().Replace("-", "");
    }

    public static string HumanReadableBytes(long byteCount)
    {
        string[] suffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
        if (byteCount == 0)
            return "0B";

        long bytes = Math.Abs(byteCount);
        int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
        double num = Math.Round(bytes / Math.Pow(1024, place), 1);

        return (Math.Sign(byteCount) * num).ToString() + " " + suffixes[place];
    }

}