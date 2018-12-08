namespace httpc
{
    using System;
    using System.Collections.Generic;
    using CommandLine;

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
                .WithParsed(RunOptionsAndReturnExitCode)
                .WithNotParsed(HandleParseErrors);
        }

        private static void HandleParseErrors(IEnumerable<Error> errs)
        {
            foreach (var err in errs)
            {
                Console.Error.WriteLine(err);
            }
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            foreach (var url in opts.Urls)
            {
                var page = new Page(url);

                if (opts.Print)
                {
                    Console.WriteLine(page.Text);
                    Environment.Exit(0);
                }

                if (string.IsNullOrWhiteSpace(opts.Pattern))
                {
                    page.FindMedia();
                }
                else
                {
                    page.FindMedia(opts.Pattern);
                }

                foreach (var mediaUrl in page.Media)
                {
                    Console.WriteLine(mediaUrl);
                    if (opts.Download)
                    {
                        var webClient = new System.Net.WebClient();
                        webClient.DownloadFile(mediaUrl, Utilities.FilenameFromUri(mediaUrl));;
                    }
                }
            }
        }
    }
}
