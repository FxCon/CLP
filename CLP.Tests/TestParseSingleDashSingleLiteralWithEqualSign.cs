using Xunit;

namespace CLP.Tests
{
    public class TestParseSingleDashSingleLiteralWithEqualSign
    {
        [Fact]
        public void TestParseSingleDashSingleLiteralWithEqualSign1()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<bool>("bool", 'b');
            Assert.Throws<FlagParserException>(() => clp.ParseWithException(new string[] { "-b=" }));
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithEqualSign2()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int", 'i');
            var result = clp.ParseWithException(new string[] { "-i=99" });
            Assert.IsType<int>(result["int"]);
            Assert.Equal(99, result["int"]);
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithEqualSign3()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<string>("string", 's');
            var result = clp.ParseWithException(new string[] { "-s=HelloWorld" });
            Assert.IsType<string>(result["string"]);
            Assert.Equal("HelloWorld", result["string"]);
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithEqualSign4()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<string>("string", 'i', defaultValue: "HelloWorld");
            var result = clp.ParseWithException(new string[] {  });
            Assert.IsType<string>(result["string"]);
            Assert.Equal("HelloWorld", result["string"]);
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithEqualSign5()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int", 'i', minCount: 1);
            Assert.Throws<FlagParserException>(() => clp.ParseWithException(new string[] { "-i=wrongFormat" }));
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithEqualSign6()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int", 'i');
            Assert.Throws<FlagParserException>(() => clp.ParseWithException(new string[] { "-x=unknownFlag" }));
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithEqualSign7()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int", 'i', minCount: 1);
            clp.AddFlag<int>("flag", 'f', minCount: 1);
            var result = clp.ParseWithException(new string[] { "-i=99", "-f=99" });

            Assert.IsType<int>(result["int"]);
            Assert.IsType<int>(result["flag"]);

            Assert.Equal(99, result["int"]);
            Assert.Equal(99, result["flag"]);
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithEqualSign8()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int", 'i', defaultValue: 99);
            clp.AddFlag<int>("flag", 'f', defaultValue: 99, minCount: 1);
            var result = clp.ParseWithException(new string[] { "-f=100" });

            Assert.IsType<int>(result["int"]);
            Assert.IsType<int>(result["flag"]);

            Assert.Equal(99, result["int"]);
            Assert.Equal(100, result["flag"]);
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithEqualSign9()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int", 'i');
            Assert.Throws<FlagParserException>(() => clp.ParseWithException(new string[] { "-i=" }));
        }

    }
}
