using System;
using CLP;

namespace Echo
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineParser commandLineParser = new CommandLineParser(new string[] { "Write arguments to the standard output" }, copyright: new string[] { "Copyright (c) 2020 Julius Behrens.", "All rights reserved." }, examples: new string[] { "Print a message without the trailing newline", "-n Hello World", "Enable interpretation of backslash escapes (special characters)", "-e Column 1\\tColumn 2" });
            commandLineParser.AddFlag("no-newline", 'n', "Do not output a trailing newline");
            commandLineParser.AddFlag("escape", 'e', "Enable interpretation of backslash escape sequences");
            commandLineParser.AddArgument<string>("STRING", "The string to display on the standard output followed by a newline.", maxCount:99);
            var result = commandLineParser.ParseWithException(args);

            Console.WriteLine(commandLineParser.ProgramName + " " + commandLineParser.ProgramVersion);
            Console.WriteLine("no-newline\t" + result["no-newline"]);
            Console.WriteLine("escape\t\t" + result["escape"]);
            Console.WriteLine("STRING\t");
            foreach(string s in result["STRING"])
            {
                Console.Write(" " + s);
            }
        }
    }
}
