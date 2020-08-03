using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CLP
{
    /// <summary>
    /// A struct to hold information about a process parent.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "This is a utility struct that is nowhere else used.")]
    internal struct ParentProcessInformation
    {
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1310 // Field names should not contain underscore
        internal IntPtr Reserved1;
        internal IntPtr PebBaseAddress;
        internal IntPtr Reserved2_0;
        internal IntPtr Reserved2_1;
        internal IntPtr UniqueProcessId;
        internal IntPtr InheritedFromUniqueProcessId;
#pragma warning restore SA1310 // Field names should not contain underscore
#pragma warning restore SA1600 // Elements should be documented
    }

    /// <summary>
    /// Contains all methods for producing colorful output.
    /// </summary>
    internal static class Color
    {
        private static readonly bool SupportANSI;

        /// <summary>
        /// Initializes static members of the <see cref="Color"/> class.
        /// Check weather or not the parent process is cmd or powershell and thus does not support ANSI.
        /// </summary>
        static Color()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ParentProcessInformation pbi = default;
                int status = NtQueryInformationProcess(Process.GetCurrentProcess().Handle, 0, ref pbi, Marshal.SizeOf(pbi), out int _);
                if (status != 0)
                {
                    throw new Exception(status.ToString());
                }

                try
                {
                    string parentProcessName = Process.GetProcessById(pbi.InheritedFromUniqueProcessId.ToInt32()).ProcessName;
                    if (parentProcessName == "cmd" || parentProcessName == "powershell")
                    {
                        SupportANSI = false;
                    }
                    else
                    {
                        SupportANSI = true;
                    }
                }
                catch (ArgumentException)
                {
                    SupportANSI = false;
                }
            }
            else
            {
                SupportANSI = true;
            }
        }

        /// <summary>
        /// Prints the specified string value to the standard output stream using <paramref name="consoleColor"/> as foreground color.
        /// </summary>
        /// <param name="useColors">Weather or not to print using <paramref name="consoleColor"/> as foreground color.</param>
        /// <param name="value">The string to print.</param>
        /// <param name="consoleColor">The foreground color.</param>
        internal static void Print(bool useColors, string value, ConsoleColor consoleColor)
        {
            if (useColors)
            {
                Print(value, consoleColor);
            }
            else
            {
                Console.Write(value);
            }
        }

        /// <summary>
        /// Prints the specified string value to the standard output stream using <paramref name="consoleColor"/> as foreground color.
        /// </summary>
        /// <param name="value">The string to print.</param>
        /// <param name="consoleColor">The foreground color.</param>
        internal static void Print(string value, ConsoleColor consoleColor)
        {
            ConsoleColor colorBackup = Console.ForegroundColor;
            Console.ForegroundColor = consoleColor;
            Console.Write(value);
            Console.ForegroundColor = colorBackup;
        }

        /// <summary>
        /// Prints the specified string value in bold to the standard output stream using <paramref name="consoleColor"/> as foreground color.
        /// </summary>
        /// <param name="useColors">Weather or not to print using a foreground color.</param>
        /// <param name="value">The string to print.</param>
        /// <param name="consoleColor">The foreground color.</param>
        internal static void PrintBold(bool useColors, string value, ConsoleColor consoleColor)
        {
            if (useColors)
            {
                PrintBold(value, consoleColor);
            }
            else
            {
                Console.Write(value);
            }
        }

        /// <summary>
        /// Prints the specified string value in bold to the standard output stream using <paramref name="consoleColor"/> as foreground color.
        /// </summary>
        /// <param name="useColors">Weather or not to print using a foreground color.</param>
        /// <param name="value">The string to print.</param>
        internal static void PrintBold(bool useColors, string value)
        {
            if (useColors)
            {
                PrintBold(value, Console.ForegroundColor);
            }
            else
            {
                Console.Write(value);
            }
        }

        /// <summary>
        /// Prints the specified string value in bold to the standard output stream using <paramref name="consoleColor"/> as foreground color.
        /// </summary>
        /// <param name="value">The string to print.</param>
        /// <param name="consoleColor">The foreground color.</param>
        internal static void PrintBold(string value, ConsoleColor consoleColor)
        {
            if (SupportANSI)
            {
                Print("\u001b[1m" + value, consoleColor);
                Console.Write("\u001b[0m");
            }
            else
            {
                Print(value, consoleColor);
            }
        }

        /// <summary>
        /// Prints an error message to the standard error stream using red foreground color if <paramref name="useColors"/> is <c>true</c>.
        /// </summary>
        /// <param name="useColors">Weather or not to print using red foreground color.</param>
        /// <param name="value">The string to print.</param>
        internal static void PrintError(bool useColors, string value)
        {
            if (useColors)
            {
                PrintError(value);
            }
            else
            {
                Console.Error.Write(value);
            }
        }

        /// <summary>
        /// Prints an error message to the standard error stream using red foreground color.
        /// </summary>
        /// <param name="value">The string to print.</param>
        internal static void PrintError(string value)
        {
            ConsoleColor colorBackup = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.Write(value);
            Console.ForegroundColor = colorBackup;
        }

        [DllImport("ntdll.dll")]
        private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref ParentProcessInformation processInformation, int processInformationLength, out int returnLength);
    }
}