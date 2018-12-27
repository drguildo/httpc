namespace httpc
{
    public static class Utilities
    {
        /// <summary>
        /// Retrieve the filename component from a URI.
        /// </summary>
        /// <param name="uri">A URI to a file.</param>
        /// <returns>The filename from the specified URI.</returns>
        public static string FilenameFromUri(System.Uri uri)
        {
            return uri.Segments[uri.Segments.Length - 1];
        }
    }
}