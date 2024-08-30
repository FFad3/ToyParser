namespace ToyParser.Utils
{
    public static class FileSizeExtension
    {
        public static string FormatFileSizeAsString(long bytes)
        {
            const long kilobyte = 1024;
            const long megabyte = kilobyte * 1024;
            const long gigabyte = megabyte * 1024;
            const long terabyte = gigabyte * 1024;

            if (bytes >= terabyte)
            {
                return $"{bytes / (double)terabyte:F2} TB";
            }
            else if (bytes >= gigabyte)
            {
                return $"{bytes / (double)gigabyte:F2} GB";
            }
            else if (bytes >= megabyte)
            {
                return $"{bytes / (double)megabyte:F2} MB";
            }
            else if (bytes >= kilobyte)
            {
                return $"{bytes / (double)kilobyte:F2} KB";
            }
            else
            {
                return $"{bytes} Bytes";
            }
        }
    }
}
