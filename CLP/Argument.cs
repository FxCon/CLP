using System;

namespace CLP
{
    /// <summary>
    /// The Argument class.
    /// </summary>
    public abstract class Argument
    {
        /// <summary>
        /// Gets or sets the name of the argument.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the argument.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of command-line arguments that should be consumed.</param>
        /// </summary>
        public uint MinCount { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of command-line arguments that should be consumed.</param>
        /// </summary>
        public uint MaxCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the argument should be shown in the help.
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the <paramref name="defaultValue"/> should be added to the result list if the argument is absent from the command line. (ignored if <paramref name="maxCount"/> is not greater than 1).
        /// </summary>
        public bool DefaultValueToList { get; set; }

        /// <summary>
        /// Gets or sets the number of times this argument has been parsed.
        /// </summary>
        public uint Count { get; set; }

        /// <summary>
        /// Returns a string that represents the current argument.
        /// </summary>
        /// <returns>A string that represents the current argument.</returns>
        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// The generic Argument class.
    /// </summary>
    /// <typeparam name="T">T.</typeparam>
    internal class Argument<T> : Argument
        where T : IConvertible
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Argument{T}"/> class.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="description">A description of what the argument does.</param>
        /// <param name="minCount">The minimum number of command-line arguments that should be consumed.</param>
        /// <param name="maxCount">The minimum number of command-line argumetns that should be consumed.</param>
        /// <param name="defaultValue">The value produced if the argument is absent from the command line.</param>
        /// <param name="hidden">Whether or not the argument should be shown in the help.</param>
        /// <param name="defaultValueToList">Whether or not the <paramref name="defaultValue"/> should be added to the result list if the argument is absent from the command line. (ignored if <paramref name="maxCount"/> is not greater than 1).</param>
        public Argument(string name, string description, uint minCount, uint maxCount, T defaultValue, bool hidden, bool defaultValueToList)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            MinCount = minCount;
            MaxCount = maxCount;
            DefaultValue = defaultValue;
            Hidden = hidden;
            DefaultValueToList = defaultValueToList;
        }

        /// <summary>
        /// Gets or sets the default Value.
        /// </summary>
        public T DefaultValue { get; set; }
    }
}
