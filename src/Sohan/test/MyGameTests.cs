using GdUnit4;
using static GdUnit4.Assertions;

namespace ExampleProject.Tests
{
    [TestSuite]
    public class ExampleTest
    {
        [TestCase]
        public void HelloWorld()
        {
            AssertThat("Hello world").IsEqual("Hello world");
            // Using explicit the typed version
            AssertString("Hello world").IsEqual("Hello world");
        }
    }
}

