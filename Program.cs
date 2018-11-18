namespace httpc
{
    using System;
    using System.Collections.Generic;
    using CommandLine;

    internal class Options
    {
        [Option('u', Required = true, HelpText = "The URLs to process.")]
        public IEnumerable<string> Urls { get; set; }

        [Option('p', HelpText = "The pattern that discovered URLs must match.")]
        public string Pattern { get; set; }

        [Option('d', Default = false, HelpText = "Download discovered URLs.")]
        public bool Download { get; set; }

        [Option("print", Default = false, HelpText = "Print the contents of the URL to stdout.")]
        public bool Print { get; set; }

        [Option('v', Default = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }
    }

    internal static class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptionsAndReturnExitCode)
                .WithNotParsed(HandleParseError);
        }

        private static void HandleParseError(IEnumerable<Error> errs)
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

                foreach (var m in page.Media)
                {
                    Console.WriteLine(m);
                }
            }
        }
    }
}
