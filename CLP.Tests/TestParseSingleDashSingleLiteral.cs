using System;
using Xunit;

namespace CLP.Tests
{
    public class TestParseSingleDashSingleLiteral
    {
        [Fact]
        public void TestParseSingleDashSingleLiteralWithBool1()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag("verbose", 'v');
            var result = clp.ParseWithException(new string[] { });
            Assert.IsType<bool>(result["verbose"]);
            Assert.Equal(false, result["verbose"]);
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithBool2()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag("verbose", 'v');
            var result = clp.ParseWithException(new string[] { "-v" });
            Assert.IsType<bool>(result["verbose"]);
            Assert.Equal(true, result["verbose"]);
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithBool3()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag("verbose", 'v', defaultValue: true);
            var result = clp.ParseWithException(new string[] { });
            Assert.IsType<bool>(result["verbose"]);
            Assert.Equal(true, result["verbose"]);
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithBool4()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag("verbose", 'v', minCount: 1);
            var result = clp.ParseWithException(new string[] { "-v" });
            Assert.IsType<bool>(result["verbose"]);
            Assert.Equal(true, result["verbose"]);
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithBool5()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag("verbose", 'v', minCount: 1);
            Assert.Throws<CommandLineParserException>(() => clp.ParseWithException(new string[] { }));
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithBool6()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag("verbose", 'v');
            Assert.Throws<FlagParserException>(() => clp.ParseWithException(new string[] { "-x" }));
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithBool7()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag("verbose", 'v', minCount: 1);
            clp.AddFlag("flag", 'f', minCount: 1);
            var result = clp.ParseWithException(new string[] { "-v", "-f"});

            Assert.IsType<bool>(result["verbose"]);
            Assert.IsType<bool>(result["flag"]);

            Assert.Equal(true, result["verbose"]);
            Assert.Equal(true, result["flag"]);
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithBool8()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag("verbose", 'v');
            clp.AddFlag("flag", 'f');
            var result = clp.ParseWithException(new string[] { });

            Assert.IsType<bool>(result["verbose"]);
            Assert.IsType<bool>(result["flag"]);

            Assert.Equal(false, result["verbose"]);
            Assert.Equal(false, result["flag"]);
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithBool9()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag("verbose", 'v', defaultValue: true);
            clp.AddFlag("flag", 'f', defaultValue: true, minCount: 1);
            var result = clp.ParseWithException(new string[] { "-f" });

            Assert.IsType<bool>(result["verbose"]);
            Assert.IsType<bool>(result["flag"]);

            Assert.Equal(true, result["verbose"]);
            Assert.Equal(true, result["flag"]);
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithInt1()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int", 'i');
            var result = clp.ParseWithException(new string[] { });
            Assert.IsType<int>(result["int"]);
            Assert.Equal(0, result["int"]);
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithInt2()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int", 'i');
            var result = clp.ParseWithException(new string[] { "-i", "99" });
            Assert.IsType<int>(result["int"]);
            Assert.Equal(99, result["int"]);
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithInt3()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int", 'i', defaultValue: 99);
            var result = clp.ParseWithException(new string[] { });
            Assert.IsType<int>(result["int"]);
            Assert.Equal(99, result["int"]);
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithInt4()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int", 'i', minCount: 1);
            var result = clp.ParseWithException(new string[] { "-i", "99" });
            Assert.IsType<int>(result["int"]);
            Assert.Equal(99, result["int"]);
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithInt5()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int", 'i', minCount: 1);
            Assert.Throws<CommandLineParserException>(() => clp.ParseWithException(new string[] { }));
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithInt6()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int", 'i');
            Assert.Throws<FlagParserException>(() => clp.ParseWithException(new string[] { "-x" }));
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithInt7()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int", 'i', minCount: 1);
            clp.AddFlag<int>("flag", 'f', minCount: 1);
            var result = clp.ParseWithException(new string[] { "-i", "99", "-f", "99" });

            Assert.IsType<int>(result["int"]);
            Assert.IsType<int>(result["flag"]);

            Assert.Equal(99, result["int"]);
            Assert.Equal(99, result["flag"]);
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithInt8()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int", 'i');
            clp.AddFlag<int>("flag", 'f');
            var result = clp.ParseWithException(new string[] { });

            Assert.IsType<int>(result["int"]);
            Assert.IsType<int>(result["flag"]);

            Assert.Equal(0, result["int"]);
            Assert.Equal(0, result["flag"]);
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithInt9()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int", 'i', defaultValue: 99);
            clp.AddFlag<int>("flag", 'f', defaultValue: 99, minCount: 1);
            var result = clp.ParseWithException(new string[] { "-f", "100" });

            Assert.IsType<int>(result["int"]);
            Assert.IsType<int>(result["flag"]);

            Assert.Equal(99, result["int"]);
            Assert.Equal(100, result["flag"]);
        }

        [Fact]
        public void TestParseSingleDashSingleLiteralWithInt10()
        {
            CommandLineParser clp = new CommandLineParser();
            clp.AddFlag<int>("int", 'i');
            Assert.Throws<FlagParserException>(() => clp.ParseWithException(new string[] { "-i" }));
        }

    }
}
