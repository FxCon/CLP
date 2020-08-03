using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CLP
{
    /// <summary>
    /// The CommandLineParser class.
    /// </summary>
    public class CommandLineParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineParser"/> class.
        /// </summary>
        public CommandLineParser()
            : this(System.Diagnostics.Process.GetCurrentProcess().ProcessName, Assembly.GetEntryAssembly().GetName().Version.ToString(), null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineParser"/> class.
        /// </summary>
        /// <param name="description">Text lines to display after the usage.</param>
        /// <param name="examples">Text lines to display after the glossary.</param>
        /// <param name="copyright">Copyright information to display after examples.</param>
        /// <param name="provideHelpFlag">Whether or not to add a --help flag to the parser.</param>
        /// <param name="provideVersionFlag">Whether or not to add a --version flag to the parser.</param>
        /// <param name="stopParsingFlagsAfterDoubleDash">Whether to stop parsing flags after -- has been parsed.</param>
        /// <param name="useColors">Wether or not to use colorful output.</param>
        public CommandLineParser(string[] description, string[] examples = null, string[] copyright = null, bool provideHelpFlag = true, bool provideVersionFlag = true, bool stopParsingFlagsAfterDoubleDash = true, bool useColors = true)
            : this(System.Diagnostics.Process.GetCurrentProcess().ProcessName, Assembly.GetEntryAssembly().GetName().Version.ToString(), description, examples, copyright, provideHelpFlag, provideVersionFlag, stopParsingFlagsAfterDoubleDash, useColors)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineParser"/> class.
        /// </summary>
        /// <param name="programName">The name of the program.</param>
        /// <param name="programVersion">The version of the program.</param>
        /// <param name="description">Text lines to display after the usage.</param>
        /// <param name="examples">Text lines to display after the glossary.</param>
        /// <param name="copyright">Copyright information to display after examples.</param>
        /// <param name="provideHelpFlag">Whether or not to add a --help flag to the parser.</param>
        /// <param name="provideVersionFlag">Whether or not to add a --version flag to the parser.</param>
        /// <param name="stopParsingFlagsAfterDoubleDash">Whether to stop parsing flags after -- has been parsed.</param>
        /// <param name="useColors">Wether or not to use colorful output.</param>
        public CommandLineParser(string programName, string programVersion, string[] description = null, string[] examples = null, string[] copyright = null, bool provideHelpFlag = true, bool provideVersionFlag = true, bool stopParsingFlagsAfterDoubleDash = true, bool useColors = true)
        {
            ProgramName = programName ?? throw new ArgumentNullException(nameof(programName));
            ProgramVersion = programVersion ?? throw new ArgumentNullException(nameof(programVersion));
            Description = description;
            Examples = examples;
            Copyright = copyright;
            ProvideHelpFlag = provideHelpFlag;
            ProvideVersionFlag = provideVersionFlag;
            StopParsingFlagsAfterDoubleDash = stopParsingFlagsAfterDoubleDash;
            UseColors = useColors;

            FlagParser = new FlagParser(Result);
            ArgumentParser = new ArgumentParser(Result);
        }

        /// <summary>
        /// Gets or sets the name of the program.
        /// </summary>
        public string ProgramName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the version of the program.
        /// </summary>
        public string ProgramVersion { get; set; } = "n/a";

        /// <summary>
        /// Gets or sets text lines to display after the usage.
        /// </summary>
        public string[] Description { get; set; } = null;

        /// <summary>
        /// Gets or sets examples the text lines to display after the glossary.
        /// </summary>
        public string[] Examples { get; set; } = null;

        /// <summary>
        /// Gets or sets the copyright information to display after examples.
        /// </summary>
        public string[] Copyright { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether or not to add a --help flag to the parser.
        /// </summary>
        public bool ProvideHelpFlag { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether or not to add a --version flag to the parser.
        /// </summary>
        public bool ProvideVersionFlag { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to stop parsing flags after -- has been parsed.
        /// </summary>
        public bool StopParsingFlagsAfterDoubleDash { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether or not to use colorful output.
        /// </summary>
        public bool UseColors { get; set; } = true;

        private FlagParser FlagParser { get; }

        private ArgumentParser ArgumentParser { get; }

        private Dictionary<string, dynamic> Result { get; } = new Dictionary<string, dynamic>();

        /// <summary>
        /// Define how a single command-line flag should be parsed.
        /// </summary>
        /// <param name="name">The long name of the flag without leading '--'.</param>
        /// <param name="shortName">The short name of the flag without leading '-'.</param>
        /// <param name="description">A description of what the flag does.</param>
        /// <param name="minCount">The minimum number of command-line flags that should be consumed.</param>
        /// <param name="maxCount">The maximum number of command-line flags that should be consumed.</param>
        /// <param name="hidden">Whether or not the flag should be shown in the help.</param>
        public void AddFlag(string name, char shortName = char.MinValue, string description = "", uint minCount = 0, uint maxCount = 1, bool hidden = false)
        {
            AddFlag<bool>(name, shortName, description, null, minCount, maxCount, false, hidden);
        }

        /// <summary>
        /// Define how a single command-line flag should be parsed.
        /// </summary>
        /// <typeparam name="T">The type of the flag.</typeparam>
        /// <param name="name">The long name of the flag without leading '--'.</param>
        /// <param name="shortName">The short name of the flag without leading '-'.</param>
        /// <param name="description">A description of what the flag does.</param>
        /// <param name="metavar">A name for the flag in usage and help messages.</param>
        /// <param name="minCount">The minimum number of command-line flags that should be consumed.</param>
        /// <param name="maxCount">The maximum number of command-line flags that should be consumed. If maxCount if greater than 1 a List will be added to the dictionary returned by Parse().</param>
        /// <param name="defaultValue">The value produced if the flag is absent from the command line.</param>
        /// <param name="hidden">Whether or not the flag should be shown in the help.</param>
        /// <param name="defaultValueToList">Whether or not the <paramref name="defaultValue"/> should be added to the result list if the flag is absent from the command line. (ignored if <paramref name="maxCount"/> is not greater than 1).</param>
        public void AddFlag<T>(string name, char shortName = char.MinValue, string description = "", string metavar = null, uint minCount = 0, uint maxCount = 1, T defaultValue = default, bool hidden = false, bool defaultValueToList = false)
            where T : IConvertible
        {
            FlagParser.Flags.Add(new Flag<T>(name, shortName, description, metavar, minCount, maxCount, defaultValue, hidden, defaultValueToList));
            if (maxCount > 1 && typeof(T) == typeof(bool))
            {
                Result[name] = 0;
            }
            else if (maxCount > 1)
            {
                Result[name] = new List<T>();
            }
            else
            {
                Result[name] = defaultValue;
            }
        }

        /// <summary>
        /// Define how a single command-line argument should be parsed.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="name">The name of the flag.</param>
        /// <param name="description">A description of what the argument does.</param>
        /// <param name="minCount">The minimum number of command-line arguments that should be consumed.</param>
        /// <param name="maxCount">The maximum number of command-line flags that should be consumed. If maxCount if greater than 1 a List will be added to the dictionary returned by <c>Parse()</c>.</param>
        /// <param name="defaultValue">The value produced if the argument is absent from the command line.</param>
        /// <param name="hidden">Whether or not the argument should be shown in the help.</param>
        /// <param name="defaultValueToList">Whether or not the <paramref name="defaultValue"/> should be added to the result list if the argument is absent from the command line. (ignored if <paramref name="maxCount"/> is not greater than 1).</param>
        public void AddArgument<T>(string name, string description = "", uint minCount = 0, uint maxCount = 1, T defaultValue = default, bool hidden = false, bool defaultValueToList = false)
            where T : IConvertible
        {
            ArgumentParser.Arguments.Add(new Argument<T>(name, description, minCount, maxCount, defaultValue, hidden, defaultValueToList));
            if (maxCount > 1)
            {
                Result[name] = new List<T>();
            }
            else
            {
                Result[name] = defaultValue;
            }
        }

        /// <summary>
        /// Parse the command line flags and arguments.
        /// </summary>
        /// <param name="args">List of strings to parse.</param>
        /// <returns>A dictionary where the key is the Name of the flags or arguments and the value is a parsed value from the command line.</returns>
        public Dictionary<string, dynamic> ParseWithException(string[] args)
        {
            bool parsedDoubleDash = false;
            for (int i = 0; i < args.Length; i += 1)
            {
                // Parse double dash i.e. --
                if (!parsedDoubleDash && StopParsingFlagsAfterDoubleDash && args[i] == "--")
                {
                    parsedDoubleDash = true;
                    continue;
                }
                else if (!parsedDoubleDash)
                {
                    // Special case: '--help' takes precedence over parsing and error reporting
                    if (ProvideHelpFlag)
                    {
                        foreach (string s in args)
                        {
                            if (s == "--help")
                            {
                                Help.Print(ProgramName, Description, Examples, Copyright, UseColors, FlagParser.Flags, ArgumentParser.Arguments);

                                Environment.Exit(0);
                            }
                        }
                    }

                    // Special case: '--version' takes precedence over parsing and error reporting
                    if (ProvideVersionFlag)
                    {
                        foreach (string s in args)
                        {
                            if (s == "--version")
                            {
                                Console.WriteLine(ProgramName + " " + ProgramVersion);
                                Environment.Exit(0);
                            }
                        }
                    }

                    FlagParserResult flagParserResult = FlagParser.Parse(args[i], (i + 1 < args.Length) ? args[i + 1] : null);
                    if (flagParserResult == FlagParserResult.Parsed)
                    {
                        continue;
                    }
                    else if (flagParserResult == FlagParserResult.ParsedAndConsumedNextArg)
                    {
                        i += 1;
                        continue;
                    }
                }

                ArgumentParserResult argumentParserResult = ArgumentParser.Parse(args[i]);
                if (argumentParserResult == ArgumentParserResult.NotParsed)
                {
                    throw new ArgumentParserException("Unknown argument \"" + args[i] + "\"");
                }
            }

            foreach (Flag flag in FlagParser.Flags)
            {
                if (flag.Count < flag.MinCount)
                {
                    throw new CommandLineParserException("Missing flag " + flag);
                }

                if (flag.DefaultValueToList && flag.MaxCount > 1 && flag.Count == 0)
                {
                    // dynamic defaultValue = flag.GetType().GetProperty("DefaultValue").GetValue(flag, null);
                    // Result[flag.Name].Add(defaultValue);
                    Result[flag.Name].GetType().GetMethod("Add").Invoke(Result[flag.Name], new object[] { flag.GetType().GetProperty("DefaultValue").GetValue(flag, null) });
                }
            }

            foreach (Argument argument in ArgumentParser.Arguments)
            {
                if (argument.Count < argument.MinCount)
                {
                    throw new CommandLineParserException("Missing argument " + argument);
                }

                if (argument.DefaultValueToList && argument.MaxCount > 1 && argument.Count == 0)
                {
                    // dynamic defaultValue = argument.GetType().GetProperty("DefaultValue").GetValue(argument, null);
                    // Result[argument.Name].Add(defaultValue);
                    Result[argument.Name].GetType().GetMethod("Add").Invoke(Result[argument.Name], new object[] { argument.GetType().GetProperty("DefaultValue").GetValue(argument, null) });
                }
            }

            return Result;
        }

        /// <summary>
        /// Parse the command line flags and arguments.
        /// </summary>
        /// <param name="args">List of strings to parse.</param>
        /// <returns>A dictionary where the key is the Name of the flags or arguments and the value is a parsed value from the command line.</returns>
        public Dictionary<string, dynamic> Parse(string[] args)
        {
            try
            {
                return ParseWithException(args);
            }
            catch (CommandLineParserException e)
            {
                Color.PrintError(UseColors, "Error: " + e.Message + "\nTry \'" + ProgramName + " --help\' for more information\n");
                System.Environment.Exit(1);
            }
            catch (Exception e)
            {
                Color.PrintError(UseColors, "Internal Error: " + e.Message + "\n" + e.StackTrace);
                System.Environment.Exit(1);
            }

            return null;
        }

        /// <summary>
        /// Parse the command line flags and arguments taken from <c>Environment.GetCommandLineArgs()</c>.
        /// </summary>
        /// <returns>A dictionary where the key is the Name of the flags or arguments and the value is a parsed value from the command line.</returns>
        public Dictionary<string, dynamic> Parse()
        {
            return Parse(Environment.GetCommandLineArgs().Skip(1).ToArray());
        }
    }
}
