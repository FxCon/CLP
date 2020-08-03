using System.Collections.Generic;
using Xunit;

namespace CLP.Tests
{
    public class TestParseDoubleDash
    {
        [Fact]
        public void TestParseDoubleDash1()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag("verbose");
            var result = clp.ParseWithException(new string[] { "--verbose" });
            Assert.IsType<bool>(result["verbose"]);
            Assert.Equal(true, result["verbose"]);
        }

        [Fact]
        public void TestParseDoubleDash2()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag("verbose", 'v');
            var result = clp.ParseWithException(new string[] { "--verbose" });
            Assert.IsType<bool>(result["verbose"]);
            Assert.Equal(true, result["verbose"]);
        }

        [Fact]
        public void TestParseDoubleDash3()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag("verbose", 'v', minCount: 1);
            Assert.Throws<CommandLineParserException>(() => clp.ParseWithException(new string[] { }));
        }

        [Fact]
        public void TestParseDoubleDash4()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag("verbose");
            clp.AddFlag<int>("int");
            var result = clp.ParseWithException(new string[] { "--verbose", "--int", "99" });
            Assert.IsType<bool>(result["verbose"]);
            Assert.IsType<int>(result["int"]);
            Assert.Equal(true, result["verbose"]);
            Assert.Equal(99, result["int"]);
        }

        [Fact]
        public void TestParseDoubleDash5()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int");
            Assert.Throws<FlagParserException>(() => clp.ParseWithException(new string[] { "--int", "a" }));
        }

        [Fact]
        public void TestParseDoubleDash6()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag("verbose", maxCount: 3);
            var result = clp.ParseWithException(new string[] { "--verbose", "--verbose" });
            Assert.IsType<int>(result["verbose"]);
            Assert.Equal(2, result["verbose"]);
        }

        [Fact]
        public void TestParseDoubleDash7()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag("verbose", defaultValue: true);
            var result = clp.ParseWithException(new string[] {  });
            Assert.IsType<bool>(result["verbose"]);
            Assert.Equal(true, result["verbose"]);
        }

        [Fact]
        public void TestParseDoubleDash8()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<string>("string");
            var result = clp.ParseWithException(new string[] { "--string", "HelloWorld" });
            Assert.IsType<string>(result["string"]);
            Assert.Equal("HelloWorld", result["string"]);
        }

        [Fact]
        public void TestParseDoubleDash9()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<string>("string", maxCount: 3);
            var result = clp.ParseWithException(new string[] { "--string", "HelloWorld" });
            Assert.IsType<List<string>>(result["string"]);
            Assert.Equal("HelloWorld", result["string"][0]);
        }

        [Fact]
        public void TestParseDoubleDash10()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int", maxCount: 3, defaultValueToList:true);
            var result = clp.ParseWithException(new string[] {  });
            Assert.IsType<List<int>>(result["int"]);
            Assert.Equal(0, result["int"][0]);
        }

        [Fact]
        public void TestParseDoubleDash11()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int", maxCount: 3);
            var result = clp.ParseWithException(new string[] {"--int", "99", "--int", "100" });
            Assert.IsType<List<int>>(result["int"]);
            Assert.Equal(99, result["int"][0]);
            Assert.Equal(100, result["int"][1]);
        }

    }
}
