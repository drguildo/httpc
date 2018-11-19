namespace httpc
{
    public static class Utilities
    {
        public static string FilenameFromUri(System.Uri uri)
        {
            return uri.Segments[uri.Segments.Length - 1];
        }
    }
}