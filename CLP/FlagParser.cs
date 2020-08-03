using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CLP
{
    /// <summary>
    /// The FlagParserResult enum.
    /// </summary>
    internal enum FlagParserResult
    {
        /// <summary>
        /// Indicates that the flag has not beed parsed successfully.
        /// </summary>
        NotParsed,

        /// <summary>
        /// Indicates that the flag has beed parsed successfully.
        /// </summary>
        Parsed,

        /// <summary>
        /// Indicates that the flag has beed parsed successfully and the next string on the command line has been consumed.
        /// </summary>
        ParsedAndConsumedNextArg,
    }

    /// <summary>
    /// The FlagParser class.
    /// </summary>
    internal class FlagParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlagParser"/> class.
        /// </summary>
        /// <param name="result">The dictionary to write the parsed values to.</param>
        public FlagParser(Dictionary<string, dynamic> result)
        {
            Result = result ?? throw new ArgumentNullException(nameof(result));
        }

        /// <summary>
        /// Gets a List containing all <see cref="Flag"/> that can be parsed.
        /// </summary>
        public List<Flag> Flags { get; } = new List<Flag>();

        private Dictionary<string, dynamic> Result { get; }

        /// <summary>
        /// Try to parse the currentArg as a flag and consim nextArg if  necessary.
        /// </summary>
        /// <param name="currentArg">The string to parse.</param>
        /// <param name="nextArg">The value to parse.</param>
        /// <returns>Weather or not the string has been parsed successfully.</returns>
        public FlagParserResult Parse(string currentArg, string nextArg)
        {
            FlagParserResult flagParserResult = FlagParserResult.NotParsed;

            flagParserResult = ParseSingleDashSingleLiteral(currentArg, nextArg);
            if (flagParserResult != FlagParserResult.NotParsed)
            {
                return flagParserResult;
            }

            flagParserResult = ParseSingleDashSingleLiteralWithEqualSignRegex(currentArg);
            if (flagParserResult != FlagParserResult.NotParsed)
            {
                return flagParserResult;
            }

            flagParserResult = ParseSingleDashMulipleLiteralsRegex(currentArg);
            if (flagParserResult != FlagParserResult.NotParsed)
            {
                return flagParserResult;
            }

            flagParserResult = ParseDoubleDashRegex(currentArg, nextArg);
            if (flagParserResult != FlagParserResult.NotParsed)
            {
                return flagParserResult;
            }

            flagParserResult = ParseDoubleDashWithEqualSignRegex(currentArg);
            if (flagParserResult != FlagParserResult.NotParsed)
            {
                return flagParserResult;
            }

            return flagParserResult;
        }

        private bool ParseFlag(Flag flag, string arg)
        {
            if (flag.Count == flag.MaxCount)
            {
                throw new FlagParserException("Excess flag " + flag);
            }
            else
            {
                flag.Count += 1;
            }

            if (flag.GetType() == typeof(Flag<bool>))
            {
                if (flag.MaxCount > 1)
                {
                    Result[flag.Name] += 1;
                }
                else
                {
                    Result[flag.Name] = true;
                }

                return false;
            }
            else
            {
                if (string.IsNullOrEmpty(arg))
                {
                    throw new FlagParserException("Flag " + flag + " requires an argument");
                }

                try
                {
                    Type type = flag.GetType().GetGenericArguments()[0];
                    if (flag.MaxCount > 1)
                    {
                        // Parse Enum differnetly
                        if (flag.GetType().GetGenericArguments()[0].IsEnum)
                        {
                            Result[flag.Name].GetType().GetMethod("Add").Invoke(Result[flag.Name], new object[] { Enum.Parse(type, arg) });
                            return true;
                        }

                        // Warning: Convert.ChangeType() uses Locale Format
                        dynamic value = Convert.ChangeType(arg, type);
                        Result[flag.Name].Add(value);
                    }
                    else
                    {
                        // Parse Enum differnetly
                        if (flag.GetType().GetGenericArguments()[0].IsEnum)
                        {
                            Result[flag.Name] = Enum.Parse(type, arg);
                            return true;
                        }

                        // Warning: Convert.ChangeType() uses Locale Format
                        Result[flag.Name] = Convert.ChangeType(arg, type);
                    }
                }
                catch (FormatException)
                {
                    throw new FlagParserException("Invalid argument \"" + arg + "\" to flag " + flag);
                }

                return true;
            }
        }

        private FlagParserResult ParseSingleDashSingleLiteral(string currentArg, string nextArg)
        {
            if (currentArg.Length == 2 && currentArg[0] == '-')
            {
                foreach (Flag flag in Flags)
                {
                    if (flag.ShortName == currentArg[1])
                    {
                        return ParseFlag(flag, nextArg) ? FlagParserResult.ParsedAndConsumedNextArg : FlagParserResult.Parsed;
                    }
                }

                throw new FlagParserException("Unknown flag \"-" + currentArg[1] + "\"");
            }

            return FlagParserResult.NotParsed;
        }

        private FlagParserResult ParseSingleDashSingleLiteralRegex(string currentArg, string nextArg)
        {
            Match singleDashSingleLiteral = Regex.Match(currentArg, @"^-[a-zA-Z]$");
            if (singleDashSingleLiteral.Success)
            {
                char parsedFlagName = singleDashSingleLiteral.Value.Substring(1).ToCharArray()[0];
                foreach (Flag flag in Flags)
                {
                    if (flag.ShortName == parsedFlagName)
                    {
                        return ParseFlag(flag, nextArg) ? FlagParserResult.ParsedAndConsumedNextArg : FlagParserResult.Parsed;
                    }
                }

                throw new FlagParserException("Unknown flag \"-" + parsedFlagName + "\"");
            }

            return FlagParserResult.NotParsed;
        }

        private FlagParserResult ParseSingleDashSingleLiteralWithEqualSignRegex(string currentArg)
        {
            Match singleDashSingleLiteral = Regex.Match(currentArg, @"^-[a-zA-Z]=.*$");
            if (singleDashSingleLiteral.Success)
            {
                char parsedFlagName = singleDashSingleLiteral.Value.ToCharArray()[1];
                string parsedArg = singleDashSingleLiteral.Value.Substring(3);
                foreach (Flag flag in Flags)
                {
                    if (flag.ShortName == parsedFlagName)
                    {
                        if (flag.GetType() == typeof(Flag<bool>))
                        {
                            throw new FlagParserException("Flag " + flag + " takes no argument");
                        }

                        ParseFlag(flag, parsedArg);
                        return FlagParserResult.Parsed;
                    }
                }

                throw new FlagParserException("Unknown flag \"-" + parsedFlagName + "\"");
            }

            return FlagParserResult.NotParsed;
        }

        private FlagParserResult ParseSingleDashMulipleLiteralsRegex(string currentArg)
        {
            FlagParserResult flagParserResult = FlagParserResult.NotParsed;
            Match singleDashSingleLiteral = Regex.Match(currentArg, @"^-[a-zA-Z][a-zA-Z]+$");
            bool currentFlagParsedSuccessfully = false;
            if (singleDashSingleLiteral.Success)
            {
                foreach (char parsedFlagName in currentArg.Substring(1).ToCharArray())
                {
                    foreach (Flag flag in Flags)
                    {
                        if (flag.ShortName == parsedFlagName)
                        {
                            ParseFlag(flag, null);
                            flagParserResult = FlagParserResult.Parsed;
                            currentFlagParsedSuccessfully = true;
                        }
                    }

                    if (!currentFlagParsedSuccessfully)
                    {
                        throw new FlagParserException("Unknown flag \"-" + parsedFlagName + "\"");
                    }
                    else
                    {
                        currentFlagParsedSuccessfully = false;
                    }
                }
            }

            return flagParserResult;
        }

        private FlagParserResult ParseDoubleDashRegex(string currentArg, string nextArg)
        {
            Match doubleDashFlag = Regex.Match(currentArg, @"^--[a-zA-Z-]+$");
            if (doubleDashFlag.Success)
            {
                string parsedFlagName = doubleDashFlag.Value.Substring(2);
                foreach (Flag flag in Flags)
                {
                    if (flag.Name == parsedFlagName)
                    {
                        return ParseFlag(flag, nextArg) ? FlagParserResult.ParsedAndConsumedNextArg : FlagParserResult.Parsed;
                    }
                }

                throw new FlagParserException("Unknown flag \"--" + parsedFlagName + "\"");
            }

            return FlagParserResult.NotParsed;
        }

        private FlagParserResult ParseDoubleDashWithEqualSignRegex(string currentArg)
        {
            Match doubleDashFlag = Regex.Match(currentArg, @"^--[a-zA-Z-]+=.*$");
            if (doubleDashFlag.Success)
            {
                string parsedFlagName = currentArg.Substring(2, currentArg.IndexOf('=') - 2);
                string parsedArg = currentArg.Substring(currentArg.IndexOf('=') + 1);

                foreach (Flag flag in Flags)
                {
                    if (flag.Name == parsedFlagName)
                    {
                        if (flag.GetType() == typeof(Flag<bool>))
                        {
                            throw new FlagParserException("Flag " + flag + " takes no argument");
                        }

                        ParseFlag(flag, parsedArg);
                        return FlagParserResult.Parsed;
                    }
                }

                throw new FlagParserException("Unknown flag \"--" + parsedFlagName + "\"");
            }

            return FlagParserResult.NotParsed;
        }
    }
}
