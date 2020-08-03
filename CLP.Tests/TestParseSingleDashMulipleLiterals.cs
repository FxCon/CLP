using Xunit;

namespace CLP.Tests
{
    public class TestParseSingleDashMulipleLiterals
    {

        [Fact]
        public void TestParseSingleDashMulipleLiterals1()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<bool>("bool", 'b');
            clp.AddFlag<bool>("verbose", 'v');
            var result = clp.ParseWithException(new string[] { "-vb" });
            Assert.IsType<bool>(result["bool"]);
            Assert.IsType<bool>(result["verbose"]);
            Assert.Equal(true, result["bool"]);
            Assert.Equal(true, result["verbose"]);
        }

        [Fact]
        public void TestParseSingleDashMulipleLiterals2()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<bool>("bool", 'b');
            clp.AddFlag<bool>("verbose", 'v');
            Assert.Throws<FlagParserException>(() => clp.ParseWithException(new string[] { "-vbc" }));
        }

        [Fact]
        public void TestParseSingleDashMulipleLiterals3()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<bool>("bool", 'b');
            clp.AddFlag<bool>("verbose", 'v');
            Assert.Throws<FlagParserException>(() => clp.ParseWithException(new string[] { "-vv" }));
        }

        [Fact]
        public void TestParseSingleDashMulipleLiterals4()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<bool>("bool", 'b', minCount: 1);
            Assert.Throws<CommandLineParserException>(() => clp.ParseWithException(new string[] { }));
        }

        [Fact]
        public void TestParseSingleDashMulipleLiterals5()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<bool>("bool", 'b', minCount: 2);
            Assert.Throws<CommandLineParserException>(() => clp.ParseWithException(new string[] { "-b" }));
        }

        [Fact]
        public void TestParseSingleDashMulipleLiterals6()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<bool>("bool", 'b', maxCount: 2);
            var result = clp.ParseWithException(new string[] { "-bb" });
            Assert.IsType<int>(result["bool"]);
            Assert.Equal(2, result["bool"]);
        }

    }
}
