namespace httpc
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using HtmlAgilityPack;

    public class Page
    {
        public Uri BaseUrl { get; private set; }
        public IEnumerable<Uri> Media { get { return foundMedia; } }
        public string Text
        {
            get
            {
                return this.document.Text;
            }
        }

        private HtmlDocument document;
        private readonly HashSet<Uri> foundMedia;

        public Page(string url)
        {
            this.BaseUrl = new Uri(url);
            this.foundMedia = new HashSet<Uri>();

            var web = new HtmlWeb();
            this.document = web.Load(this.BaseUrl);
            if (!(web.StatusCode == System.Net.HttpStatusCode.OK))
            {
                Console.Error.WriteLine($"Received HTTP status code {web.StatusCode}");
                Environment.Exit(-1);
            }
            this.document.OptionEmptyCollection = true;
        }

        public void FindMedia(string pattern = "")
        {
            foreach (var image in this.document.DocumentNode.SelectNodes("//img"))
            {
                HtmlAttribute src = image.Attributes["src"];
                if (src != null && Regex.IsMatch(src.Value, pattern))
                {
                    AddIfValid(src.Value);
                }
            }

            foreach (var link in this.document.DocumentNode.SelectNodes("//a"))
            {
                HtmlAttribute href = link.Attributes["href"];
                if (href != null && Regex.IsMatch(href.Value, pattern))
                {
                    AddIfValid(href.Value);
                }
            }

            foreach (var video in this.document.DocumentNode.SelectNodes("//video/source"))
            {
                HtmlAttribute src = video.Attributes["src"];
                if (src != null && Regex.IsMatch(src.Value, pattern))
                {
                    AddIfValid(src.Value);
                }
            }
        }

        /// <summary>
        /// Add a string to the collection of found media if it's a valid URL.
        /// </summary>
        /// <param name="url"></param>
        private void AddIfValid(string url)
        {
            var decodedUrl = System.Net.WebUtility.HtmlDecode(url);

            if (Uri.IsWellFormedUriString(decodedUrl, UriKind.Relative))
            {
                var absoluteUrl = new Uri(this.BaseUrl, new Uri(decodedUrl, UriKind.Relative));
                foundMedia.Add(absoluteUrl);
            }
            else if (Uri.IsWellFormedUriString(decodedUrl, UriKind.Absolute))
            {
                foundMedia.Add(new Uri(decodedUrl));
            }
        }
    }
}