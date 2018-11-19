namespace httpc
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    public class Downloader : WebClient
    {
        private IEnumerable<Uri> urlsToDownload;

        public Downloader(IEnumerable<Uri> urlsToDownload)
        {
            this.urlsToDownload = urlsToDownload;
        }

        public void BeginDownloading(bool verbose)
        {
            foreach (var url in urlsToDownload)
            {
                string localFilename = this.GetFilename(url);

                if (verbose)
                {
                    Console.WriteLine($"Downloading {url} to {localFilename}...");
                }

                this.DownloadFile(url, localFilename);
            }
        }

        private string GetFilename(Uri url)
        {
            return url.Segments[url.Segments.Length - 1];
        }

        protected override void OnDownloadProgressChanged(DownloadProgressChangedEventArgs e)
        {
            base.OnDownloadProgressChanged(e);
        }
    }
}