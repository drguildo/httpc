namespace httpc
{
    using System;

    public static class Utilities
    {
        /// <summary>
        /// Retrieve the filename component from a URI.
        /// </summary>
        /// <param name="uri">A URI to a file.</param>
        /// <returns>The filename from the specified URI.</returns>
        public static string FilenameFromUri(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentException("URI cannot be null");
            }

            return uri.Segments[uri.Segments.Length - 1];
        }
    }
}