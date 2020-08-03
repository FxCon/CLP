using System;
using System.Collections.Generic;
using Xunit;

namespace CLP.Tests
{
    public class TestCommandLineParser
    {
        [Fact]
        public void TestCommandLineParser1()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag("verbose", 'v');
            clp.AddFlag("optimize", 'o', maxCount: 3);
            clp.AddFlag<int>("number", 'n', maxCount: 10);
            clp.AddFlag<string>("input", 'i');
            clp.AddFlag<DayOfWeek>("day", 'd');
            clp.AddArgument<string>("value");
            clp.AddArgument<string>("text", maxCount: 2);
            var result = clp.ParseWithException(new string[] { "ThisIsArgument", "-n=99", "ThisIsText", "-d", "Monday", "AnotherText", "--input=AInputString", "-o", "--verbose", "--optimize" });

            Assert.IsType<bool>(result["verbose"]);
            Assert.IsType<int>(result["optimize"]);
            Assert.IsType<List<int>>(result["number"]);
            Assert.IsType<string>(result["input"]);
            Assert.IsType<DayOfWeek>(result["day"]);
            Assert.IsType<string>(result["value"]);
            Assert.IsType<List<string>>(result["text"]);


            Assert.Equal("ThisIsArgument", result["value"]);
            Assert.Equal(99, result["number"][0]);
            Assert.Equal("ThisIsText", result["text"][0]);
            Assert.Equal(DayOfWeek.Monday, result["day"]);
            Assert.Equal("AnotherText", result["text"][1]);
            Assert.Equal("AInputString", result["input"]);
            Assert.Equal(2, result["optimize"]);
            Assert.Equal(true, result["verbose"]);
        }

        [Fact]
        public void TestCommandLineParser2()
        {
            CommandLineParser clp = new CommandLineParser();

            clp.AddFlag("bool", 'b', "Enable Bool Option");
            clp.AddFlag("verbose", 'v', "Enable Verbose Option", defaultValue: false, maxCount: 3);
            clp.AddFlag<int>("integer", 'i', "The Integer ");
            clp.AddFlag<double>("double", 'd', "The double ");
            clp.AddFlag<int>("list", 'l', "A List", "<int>", 0, 2, 99);
            clp.AddArgument<string>("string", defaultValue: "program");
            clp.AddArgument<int>("number", maxCount: 2, defaultValue: 666, defaultValueToList: true);
            var result = clp.ParseWithException(new string[] { "--list=100", "-d=1,5", "--verbose", "-bv", "-i=12" });

            Assert.IsType<bool>(result["bool"]);
            Assert.IsType<int>(result["verbose"]);
            Assert.IsType<double>(result["double"]);
            Assert.IsType<string>(result["string"]);
            Assert.IsType<List<int>>(result["list"]);
            Assert.IsType<List<int>>(result["number"]);

            Assert.Equal(666, result["number"][0]);
            Assert.Equal(100, result["list"][0]);
            Assert.Equal("program", result["string"]);
            Assert.Equal(1.5, result["double"]);
            Assert.Equal(12, result["integer"]);
            Assert.True(result["bool"]);
            Assert.Equal(2, result["verbose"]);
            Assert.IsType<int>(result["integer"]);
        }
    }
}
