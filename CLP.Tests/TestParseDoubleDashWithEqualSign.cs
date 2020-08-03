using System;
using System.Collections.Generic;
using Xunit;

namespace CLP.Tests
{
    public class TestParseDoubleDashWithEqualSign
    {
        [Fact]
        public void TestParseDoubleDashWithEqualSign1()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<string>("string");
            clp.AddFlag<int>("int");
            var result = clp.ParseWithException(new string[] { "--string=Text", "--int=99" });
            Assert.IsType<string>(result["string"]);
            Assert.IsType<int>(result["int"]);
            Assert.Equal("Text", result["string"]);
            Assert.Equal(99, result["int"]);
        }

        [Fact]
        public void TestParseDoubleDashWithEqualSign2()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int");
            Assert.Throws<FlagParserException>(() => clp.ParseWithException(new string[] { "--int=a" }));
        }

        [Fact]
        public void TestParseDoubleDashWithEqualSign3()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int", maxCount: 3);
            var result = clp.ParseWithException(new string[] { "--int=22", "--int=33" });
            Assert.IsType<List<int>>(result["int"]);
            Assert.Equal(22, result["int"][0]);
            Assert.Equal(33, result["int"][1]);
        }

        [Fact]
        public void TestParseDoubleDashWithEqualSign4()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<string>("string");
            var result = clp.ParseWithException(new string[] { "--string=HelloWorld" });
            Assert.IsType<string>(result["string"]);
            Assert.Equal("HelloWorld", result["string"]);
        }

        [Fact]
        public void TestParseDoubleDashWithEqualSign5()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<string>("string", maxCount: 3);
            var result = clp.ParseWithException(new string[] { "--string=HelloWorld" });
            Assert.IsType<List<string>>(result["string"]);
            Assert.Equal("HelloWorld", result["string"][0]);
        }


        [Fact]
        public void TestParseDoubleDashWithEqualSign6()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int", maxCount: 3);
            var result = clp.ParseWithException(new string[] { "--int=99", "--int=100" });
            Assert.IsType<List<int>>(result["int"]);
            Assert.Equal(99, result["int"][0]);
            Assert.Equal(100, result["int"][1]);
        }

        [Fact]
        public void TestParseDoubleDashWithEqualSign7()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<DayOfWeek>("day", maxCount: 3);
            var result = clp.ParseWithException(new string[] { "--day=Monday", "--day=2" });
            Assert.IsType<List<DayOfWeek>>(result["day"]);
            Assert.Equal(DayOfWeek.Monday, result["day"][0]);
            Assert.Equal(DayOfWeek.Tuesday, result["day"][1]);
        }

        [Fact]
        public void TestParseDoubleDashWithEqualSign8()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<DayOfWeek>("day");
            var result = clp.ParseWithException(new string[] { "--day=Monday" });
            Assert.IsType<DayOfWeek>(result["day"]);
            Assert.Equal(DayOfWeek.Monday, result["day"]);
        }

    }
}
