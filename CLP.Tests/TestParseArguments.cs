using System.Collections.Generic;
using Xunit;

namespace CLP.Tests
{
    public class TestParseArguments
    {
        [Fact]
        public void TestParseArguments1()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddArgument<int>("int");
            var result = clp.ParseWithException(new string[] { "99" });
            Assert.IsType<int>(result["int"]);
            Assert.Equal(99, result["int"]);
        }

        [Fact]
        public void TestParseArguments2()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddArgument<int>("int", minCount:1);
            Assert.Throws<CommandLineParserException>(() => clp.ParseWithException(new string[] {  }));
        }

        [Fact]
        public void TestParseArguments3()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddArgument<int>("int");
            Assert.Throws<ArgumentParserException>(() => clp.ParseWithException(new string[] { "a" }));
        }

        [Fact]
        public void TestParseArguments4()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddArgument<string>("string", maxCount: 10);
            var result = clp.ParseWithException(new string[] { "Text", "MoreText" });
            Assert.IsType<List<string>>(result["string"]);
            Assert.Equal("Text", result["string"][0]);
            Assert.Equal("MoreText", result["string"][1]);
        }

        [Fact]
        public void TestParseArguments5()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddArgument<string>("string", maxCount: 2);
            clp.AddArgument<int>("int");
            var result = clp.ParseWithException(new string[] { "Text", "MoreText", "99" });
            Assert.IsType<List<string>>(result["string"]);
            Assert.IsType<int>(result["int"]);
            Assert.Equal("Text", result["string"][0]);
            Assert.Equal("MoreText", result["string"][1]);
            Assert.Equal(99, result["int"]);
        }


    }
}
