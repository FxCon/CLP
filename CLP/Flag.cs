using System;

namespace CLP
{
    /// <summary>
    /// The Flag class.
    /// </summary>
    public abstract class Flag
    {
        /// <summary>
        /// Gets or sets the long name of the flag without leading '--'.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the short name of the flag without leading '-'.
        /// </summary>
        public char ShortName { get; set; }

        /// <summary>
        /// Gets or sets a description of what the flag does.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a name for the flag in usage and help messages.
        /// </summary>
        public string Metavar { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of command-line flags that should be consumed.
        /// </summary>
        public uint MinCount { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of command-line flags that should be consumed. If maxCount if greater than 1 a List will be added to the dictionary returned by Parse().
        /// </summary>
        public uint MaxCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether or not the flag should be shown in the help.
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the default value should be added to the result list if the flag is absent from the command line. (ignored if <c>MaxCount</c> is not greater than 1).
        /// </summary>
        public bool DefaultValueToList { get; set; }

        /// <summary>
        /// Gets or sets the number of times this flag has been parsed on the command line.
        /// </summary>
        public uint Count { get; set; }

        /// <summary>
        /// Prints the flag to the standart output.
        /// </summary>
        /// <param name="useColors">Whether or not to use colorful output.</param>
        public abstract void Print(bool useColors);
    }

    /// <summary>
    /// The Flag class.
    /// </summary>
    /// <typeparam name="T">The type of the flag.</typeparam>
    internal class Flag<T> : Flag
        where T : IConvertible
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flag{T}"/> class.
        /// </summary>
        /// <param name="name">The long name of the flag without leading '--'.</param>
        /// <param name="shortName">The short name of the flag without leading '-'.</param>
        /// <param name="description">A description of what the flag does.</param>
        /// <param name="metavar">A name for the argument in usage messages.</param>
        /// <param name="minCount">The minimum number of command-line flags that should be consumed.</param>
        /// <param name="maxCount">The maximum number of command-line flags that should be consumed.</param>
        /// <param name="defaultValue">The value produced if the flag is absent from the command line.</param>
        /// <param name="hidden">Whether or not the flag should be shown in the help.</param>
        /// <param name="defaultValueToList">Whether or not the <paramref name="defaultValue"/> should be added to the result list if the flag is absent from the command line. (ignored if <paramref name="maxCount"/> is not greater than 1).</param>
        public Flag(string name, char shortName, string description, string metavar, uint minCount, uint maxCount, T defaultValue, bool hidden, bool defaultValueToList)
        {
            if (typeof(T).IsEnum && metavar == null)
            {
                metavar = "{";
                foreach (string s in Enum.GetNames(typeof(T)))
                {
                    metavar += s + ",";
                }

                metavar = metavar.Remove(metavar.Length - 1);
                metavar += "}";
            }

            Name = name ?? throw new ArgumentNullException(nameof(name));
            ShortName = shortName;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Metavar = metavar ?? ("<" + typeof(T).ToString().Replace("System.", string.Empty) + ">");
            MinCount = minCount;
            MaxCount = maxCount;
            Hidden = hidden;
            DefaultValueToList = defaultValueToList;
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        public T DefaultValue { get; set; }

        /// <summary>
        /// Returns a string that represents the current flag.
        /// </summary>
        /// <returns>A string that represents the current flag.</returns>
        public override string ToString()
        {
            if (ShortName == char.MinValue)
            {
                return "--" + Name + (typeof(T) == typeof(bool) ? string.Empty : " " + Metavar);
            }
            else
            {
                return "-" + ShortName + "|--" + Name + (typeof(T) == typeof(bool) ? string.Empty : " " + Metavar);
            }
        }

        /// <summary>
        /// Prints the flag to the standart output.
        /// </summary>
        /// <param name="useColors">Whether or not to use colorful output.</param>
        public override void Print(bool useColors)
        {
            if (ShortName == char.MinValue)
            {
                Color.PrintBold(useColors, "--" + Name);
                Console.Write(typeof(T) == typeof(bool) ? string.Empty : "=" + Metavar);
            }
            else
            {
                if (MinCount > 0)
                {
                    Color.PrintBold(useColors, "-" + ShortName, ConsoleColor.Yellow);
                    Console.Write("|");
                    Color.PrintBold(useColors, "--" + Name, ConsoleColor.Yellow);
                    Console.Write(typeof(T) == typeof(bool) ? string.Empty : "=" + Metavar);
                    return;
                }

                Color.PrintBold(useColors, "-" + ShortName);
                Console.Write("|");
                Color.PrintBold(useColors, "--" + Name);
                Console.Write(typeof(T) == typeof(bool) ? string.Empty : "=" + Metavar);
            }
        }
    }
}