using System;
using System.Collections.Generic;

namespace CLP
{
    /// <summary>
    /// The ArgumentParserResult enum.
    /// </summary>
    internal enum ArgumentParserResult
    {
        /// <summary>
        /// Indicates that the argument has not beed parsed successfully.
        /// </summary>
        NotParsed,

        /// <summary>
        /// Indicates that the argument has beed parsed successfully.
        /// </summary>
        Parsed,
    }

    /// <summary>
    /// The ArgumentParser class.
    /// </summary>
    internal class ArgumentParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParser"/> class.
        /// </summary>
        /// <param name="result">The dictionary to write the parsed values to.</param>
        public ArgumentParser(Dictionary<string, dynamic> result)
        {
            Result = result ?? throw new ArgumentNullException(nameof(result));
        }

        /// <summary>
        /// Gets a List containing all <see cref="Argument"/> that can be parsed.
        /// </summary>
        public List<Argument> Arguments { get; } = new List<Argument>();

        private Dictionary<string, dynamic> Result { get; }

        /// <summary>
        /// Try to parse the string as an argument.
        /// </summary>
        /// <param name="arg">The string to parse.</param>
        /// <returns>Weather or not the string has been parsed successfully.</returns>
        public ArgumentParserResult Parse(string arg)
        {
            foreach (Argument argument in Arguments)
            {
                try
                {
                    if (argument.Count < argument.MaxCount)
                    {
                        Type type = argument.GetType().GetGenericArguments()[0];
                        if (argument.MaxCount > 1)
                        {
                            // Parse Enum differnetly
                            if (argument.GetType().GetGenericArguments()[0].IsEnum)
                            {
                                Result[argument.Name].GetType().GetMethod("Add").Invoke(Result[argument.Name], new object[] { Enum.Parse(type, arg) });
                                return ArgumentParserResult.Parsed;
                            }

                            dynamic x = Convert.ChangeType(arg, type);
                            Result[argument.Name].Add(x);
                        }
                        else
                        {
                            // Parse Enum differnetly
                            if (argument.GetType().GetGenericArguments()[0].IsEnum)
                            {
                                Result[argument.Name] = Enum.Parse(type, arg);
                                return ArgumentParserResult.Parsed;
                            }

                            // Warning: Convert.ChangeType() uses Locale Format
                            Result[argument.Name] = Convert.ChangeType(arg, type);
                        }

                        argument.Count += 1;
                        return ArgumentParserResult.Parsed;
                    }
                }
                catch (FormatException)
                {
                    throw new ArgumentParserException("Invalid value \"" + arg + "\" to argument " + argument);
                }
            }

            return ArgumentParserResult.NotParsed;
        }
    }
}
