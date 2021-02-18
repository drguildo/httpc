namespace httpc
{
    using System;
    using System.Collections.Generic;

    using CommandLine;

    using httpc.Model;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class Options
    {
        [Option('u', "url", Required = true, HelpText = "The URLs to process.")]
        public IEnumerable<string> Urls { get; set; }

        [Option('p', "pattern", HelpText = "The pattern that discovered URLs must match.")]
        public string Pattern { get; set; }

        [Option('d', "download", Default = false, HelpText = "Download discovered URLs.")]
        public bool Download { get; set; }

        [Option("print", Default = false, HelpText = "Print the contents of the URL to stdout.")]
        public bool Print { get; set; }

        [Option('v', "verbose", Default = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }
    }

    internal static class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunWithOptions);
        }

        private static void RunWithOptions(Options options)
        {
            foreach (var url in options.Urls)
            {
                var page = new Page(new Uri(url));

                if (options.Print)
                {
                    Console.WriteLine(page.Text);
                    Environment.Exit(0);
                }

                if (string.IsNullOrWhiteSpace(options.Pattern))
                {
                    page.FindMedia();
                }
                else
                {
                    page.FindMedia(options.Pattern);
                }

                foreach (var mediaUrl in page.Media)
                {
                    Console.WriteLine(mediaUrl);
                    if (options.Download)
                    {
                        using var webClient = new System.Net.WebClient();
                        webClient.DownloadFile(mediaUrl, Utilities.FilenameFromUri(mediaUrl));
                    }
                }
            }
        }
    }
}