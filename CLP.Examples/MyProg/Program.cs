using System;
using CLP;

namespace MyProg
{
    class Program
    {
        public enum status
        {
            ON,
            OFF,
            AUTO
        }

        static void Main(string[] args)
        {
            CommandLineParser commandLineParser = new CommandLineParser(description: new string[] {"This is an example program.", "It does nothing."}, copyright: new string[] {"MyProg 1.0.0", "Copyright (c) 2020 Julius Behrens.", "All rights reserved."}, examples: new string[] { "Do nothing and again nothing", "-v -i 99 text", "To do absolutely nothing ", "--status AUTO -i 12" });
            commandLineParser.AddFlag("verbose", 'v', "Enable verbose output");
            commandLineParser.AddFlag<int>("integer", 'i', "The Integer that must be provided", minCount: 1);
            commandLineParser.AddFlag<status>("status", 's', "Spcifiy the status (Default: AUTO)", defaultValue: status.AUTO);
            commandLineParser.AddFlag<string>("text", 't', "Provide some text");
            commandLineParser.AddFlag<string>("invisible", '\0', "A string Flag.", hidden: true);
            commandLineParser.AddFlag<char>("char", 'c', "A char Argument", maxCount: 3);
            commandLineParser.AddArgument<string>("STRING", "A string Argument", minCount: 2, maxCount: 3);
            var result = commandLineParser.Parse();
        }
    }
}
