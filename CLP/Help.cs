using System;
using System.Collections.Generic;

namespace CLP
{
    /// <summary>
    /// Contains all methods for printing the help messsage.
    /// </summary>
    internal static class Help
    {
        /// <summary>
        /// Print a help message to the standard output stream.
        /// </summary>
        /// <param name="programName">The name of the program.</param>
        /// <param name="description">Text lines to display after the usage.</param>
        /// <param name="examples">Text lines to display after the glossary.</param>
        /// <param name="copyright">Copyright information to display after examples.</param>
        /// <param name="useColors">Wether or not to use colorful output.</param>
        /// <param name="flags">A list of possible flags.</param>
        /// <param name="arguments">A list of possible arguments.</param>
        internal static void Print(string programName, string[] description, string[] examples, string[] copyright, bool useColors, List<Flag> flags, List<Argument> arguments)
        {
            Color.Print(" USAGE\n   ", ConsoleColor.Magenta);
            PrintUsage(programName, useColors, flags, arguments);
            if (description != null)
            {
                Color.Print("\n DESCRIPTION\n", ConsoleColor.Magenta);
                foreach (string s in description)
                {
                    Console.WriteLine("   " + s);
                }
            }

            PrintGlossary(useColors, flags, arguments);

            if (examples != null)
            {
                Color.Print("\n EXAMPLES\n", ConsoleColor.Magenta);
                for (int i = 0; i < examples.Length; ++i)
                {
                    if (i % 2 == 0)
                    {
                        Console.Write("   ");
                        Color.Print(useColors, examples[i] + "\n", ConsoleColor.Green);
                    }
                    else
                    {
                        Console.WriteLine("   " + programName + " " + examples[i]);
                        if (i != examples.Length - 1)
                        {
                            Console.WriteLine();
                        }
                    }
                }
            }

            if (copyright != null)
            {
                Color.Print("\n COPYRIGHT\n", ConsoleColor.Magenta);
                foreach (string s in copyright)
                {
                    Console.WriteLine("   " + s);
                }
            }
        }

        private static void PrintUsage(string programName, bool useColors, List<Flag> flags, List<Argument> arguments)
        {
            List<Flag> shortBoolMin1Max1 = new List<Flag>(); // -abc
            List<Flag> shortBoolMinMax = new List<Flag>(); // -a -a -a
            List<Flag> shortBoolMin0Max1 = new List<Flag>(); // [-abc]
            List<Flag> shortBoolMaxMinMany = new List<Flag>(); // -a -a [-a]...

            List<Flag> shortMinMax = new List<Flag>(); // -k <int> -k <int>
            List<Flag> shortMin0Max1 = new List<Flag>(); // [-a <int>]
            List<Flag> shortMaxMinMany = new List<Flag>(); // -a <int> -a <int> [-a <int>]...

            List<Flag> longMinMax = new List<Flag>(); // --val <int> --val <int>
            List<Flag> longMin0Max1 = new List<Flag>(); // [--verbose]
            List<Flag> longMaxMinMany = new List<Flag>(); //  --val <int> --val <int> [--val <int>]...

            List<Argument> argumentMinMax = new List<Argument>(); // <FILE> <FILE>
            List<Argument> argumentMin0Max1 = new List<Argument>(); // [<FILE>]
            List<Argument> argumentMaxMinMany = new List<Argument>(); // <FILE> [<FILE>]...

            foreach (Flag flag in flags)
            {
                if (!flag.Hidden && flag.ShortName != '\0' && flag.GetType() == typeof(Flag<bool>) && flag.MinCount == 1 && flag.MaxCount == 1)
                {
                    shortBoolMin1Max1.Add(flag);
                }
                else if (!flag.Hidden && flag.ShortName != '\0' && flag.GetType() == typeof(Flag<bool>) && flag.MinCount == flag.MaxCount)
                {
                    shortBoolMinMax.Add(flag);
                }
                else if (!flag.Hidden && flag.ShortName != '\0' && flag.GetType() == typeof(Flag<bool>) && flag.MinCount == 0 && flag.MaxCount == 1)
                {
                    shortBoolMin0Max1.Add(flag);
                }
                else if (!flag.Hidden && flag.ShortName != '\0' && flag.GetType() == typeof(Flag<bool>) && flag.MaxCount > flag.MinCount)
                {
                    shortBoolMaxMinMany.Add(flag);
                }
                else if (!flag.Hidden && flag.ShortName != '\0' && flag.MinCount == flag.MaxCount)
                {
                    shortMinMax.Add(flag);
                }
                else if (!flag.Hidden && flag.ShortName != '\0' && flag.MinCount == 0 && flag.MaxCount == 1)
                {
                    shortMin0Max1.Add(flag);
                }
                else if (!flag.Hidden && flag.ShortName != '\0' && flag.MaxCount > flag.MinCount)
                {
                    shortMaxMinMany.Add(flag);
                }
                else if (!flag.Hidden && flag.ShortName == '\0' && flag.MinCount == flag.MaxCount)
                {
                    longMinMax.Add(flag);
                }
                else if (!flag.Hidden && flag.ShortName == '\0' && flag.MinCount == 0 && flag.MaxCount == 1)
                {
                    longMin0Max1.Add(flag);
                }
                else if (!flag.Hidden && flag.ShortName == '\0' && flag.MaxCount > flag.MinCount)
                {
                    longMaxMinMany.Add(flag);
                }
            }

            foreach (Argument argument in arguments)
            {
                if (argument.MinCount == argument.MaxCount)
                {
                    argumentMinMax.Add(argument);
                }
                else if (argument.MinCount == 0 && argument.MaxCount == 1)
                {
                    argumentMin0Max1.Add(argument);
                }
                else if (argument.MaxCount > argument.MinCount)
                {
                    argumentMaxMinMany.Add(argument);
                }
            }

            Color.PrintBold(useColors, programName + " ");

            if (shortBoolMin1Max1.Count > 0)
            {
                Color.Print(useColors, "-", ConsoleColor.Yellow);
                foreach (Flag flag in shortBoolMin1Max1)
                {
                    Color.Print(useColors, flag.ShortName.ToString(), ConsoleColor.Yellow);
                }

                Console.Write(" ");
            }

            if (shortBoolMinMax.Count > 0)
            {
                foreach (Flag flag in shortBoolMinMax)
                {
                    for (uint i = flag.MinCount; i > 0; --i)
                    {
                        Color.Print(useColors, "-" + flag.ShortName + " ", ConsoleColor.Yellow);
                    }
                }
            }

            if (shortBoolMin0Max1.Count > 0)
            {
                Console.Write("[-");
                foreach (Flag flag in shortBoolMin0Max1)
                {
                    Console.Write(flag.ShortName.ToString());
                }

                Console.Write("] ");
            }

            if (shortBoolMaxMinMany.Count > 0)
            {
                foreach (Flag flag in shortBoolMaxMinMany)
                {
                    for (uint i = flag.MinCount; i > 0; --i)
                    {
                        Color.Print(useColors, "-" + flag.ShortName + " ", ConsoleColor.Yellow);
                    }

                    Console.Write("[-" + flag.ShortName + "]... ");
                }
            }

            // Short flags following
            if (shortMinMax.Count > 0)
            {
                foreach (Flag flag in shortMinMax)
                {
                    for (uint i = flag.MinCount; i > 0; --i)
                    {
                        Color.Print(useColors, "-" + flag.ShortName + " " + flag.Metavar + " ", ConsoleColor.Yellow);
                    }
                }
            }

            if (shortMin0Max1.Count > 0)
            {
                foreach (Flag flag in shortMin0Max1)
                {
                    Console.Write("[-" + flag.ShortName.ToString() + " " + flag.Metavar + "] ");
                }
            }

            if (shortMaxMinMany.Count > 0)
            {
                foreach (Flag flag in shortMaxMinMany)
                {
                    for (uint i = flag.MinCount; i > 0; --i)
                    {
                        Color.Print(useColors, "-" + flag.ShortName + " " + flag.Metavar + " ", ConsoleColor.Yellow);
                    }

                    Console.Write("[-" + flag.ShortName + " " + flag.Metavar + "]... ");
                }
            }

            // Long flags following
            if (longMinMax.Count > 0)
            {
                foreach (Flag flag in longMinMax)
                {
                    for (uint i = flag.MinCount; i > 0; --i)
                    {
                        Color.Print(useColors, "--" + flag.Name + " " + flag.Metavar + " ", ConsoleColor.Yellow);
                    }
                }
            }

            if (longMin0Max1.Count > 0)
            {
                foreach (Flag flag in longMin0Max1)
                {
                    Console.Write("[--" + flag.ShortName.ToString() + " " + flag.Metavar + "] ");
                }
            }

            Console.Write("[--help] ");
            Console.Write("[--version] ");

            if (longMaxMinMany.Count > 0)
            {
                foreach (Flag flag in longMaxMinMany)
                {
                    for (uint i = flag.MinCount; i > 0; --i)
                    {
                        Color.Print(useColors, "--" + flag.Name + " " + flag.Metavar + " ", ConsoleColor.Yellow);
                    }

                    Console.Write("[--" + flag.Name + " " + flag.Metavar + "]... ");
                }
            }

            // Arguments following
            if (argumentMinMax.Count > 0)
            {
                foreach (Argument argument in argumentMinMax)
                {
                    for (uint i = argument.MinCount; i > 0; --i)
                    {
                        Color.Print(useColors, argument.Name + " ", ConsoleColor.Yellow);
                    }
                }
            }

            if (argumentMin0Max1.Count > 0)
            {
                foreach (Argument argument in argumentMin0Max1)
                {
                    Console.Write("[" + argument.Name + "] ");
                }
            }

            if (argumentMaxMinMany.Count > 0)
            {
                foreach (Argument argument in argumentMaxMinMany)
                {
                    for (uint i = argument.MinCount; i > 0; --i)
                    {
                        Color.Print(useColors, argument.Name + " ", ConsoleColor.Yellow);
                    }

                    Console.Write("[" + argument.Name + "]... ");
                }
            }

            Console.WriteLine();
        }

        private static void PrintGlossary(bool useColors, List<Flag> flags, List<Argument> arguments)
        {
            int maxStringLength = 0;
            const int indentation = 6;
            foreach (Flag flag in flags)
            {
                if (!flag.Hidden && flag.ToString().Length > maxStringLength)
                {
                    maxStringLength = flag.ToString().Length;
                }
            }

            foreach (Argument argument in arguments)
            {
                if (!argument.Hidden && argument.ToString().Length > maxStringLength)
                {
                    maxStringLength = argument.ToString().Length;
                }
            }

            if (arguments.Count > 0)
            {
                Color.Print("\n ARGUMENTS\n", ConsoleColor.Magenta);
            }

            foreach (Argument argument in arguments)
            {
                if (!argument.Hidden)
                {
                    if (argument.MinCount > 0)
                    {
                        // or Console.Write(argument.ToString());
                        Color.Print(useColors, "  * " + argument.ToString(), ConsoleColor.Yellow);
                        Console.WriteLine(new string(' ', maxStringLength - argument.ToString().Length + indentation) + argument.Description);
                    }
                    else
                    {
                        Console.Write("    ");
                        Console.WriteLine(argument.ToString() + new string(' ', maxStringLength - argument.ToString().Length + indentation) + argument.Description);
                    }
                }
            }

            Color.Print("\n FLAGS\n", ConsoleColor.Magenta);
            foreach (Flag flag in flags)
            {
                if (!flag.Hidden)
                {
                    if (flag.MinCount > 0)
                    {
                        Color.Print(useColors, "  * ", ConsoleColor.Yellow);
                        flag.Print(useColors);
                        Console.WriteLine(new string(' ', maxStringLength - flag.ToString().Length + indentation) + flag.Description);
                    }
                    else
                    {
                        Console.Write("    ");
                        flag.Print(useColors);
                        Console.WriteLine(new string(' ', maxStringLength - flag.ToString().Length + indentation) + flag.Description);
                    }
                }
            }

            Color.PrintBold(useColors, "    --help");
            Console.WriteLine(new string(' ', maxStringLength - "--help".Length + indentation) + "Display this help and exit");
            Color.PrintBold(useColors, "    --version");
            Console.WriteLine(new string(' ', maxStringLength - "--version".Length + indentation) + "Display version information and exit");
        }
    }
}