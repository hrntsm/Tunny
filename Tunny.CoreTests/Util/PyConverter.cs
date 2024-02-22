using Python.Included;
using Python.Runtime;

using Xunit;

namespace Tunny.Core.Util.Tests
{
    public class PyConverterTests
    {
        public PyConverterTests()
        {
            Installer.SetupPython();
            PythonEngine.Initialize();
        }

        [Fact()]
        public void IntEnumeratorToPyListTest()
        {
            int[] list = new[] { 1, 2, 3, 4, 5 };
            PyList pyList = PyConverter.EnumeratorToPyList(list);
            Assert.Equal(5, pyList.Length());
            Assert.Equal(1, pyList[0].As<int>());
        }

        [Fact()]
        public void DoubleEnumeratorToPyListTest()
        {
            double[] list = new[] { 1d, 2d, 3d, 4d, 5d };
            PyList pyList = PyConverter.EnumeratorToPyList(list);
            Assert.Equal(5, pyList.Length());
            Assert.Equal(1d, pyList[0].As<double>());
        }

        [Fact()]
        public void StringEnumeratorToPyListTest()
        {
            string[] list = new[] { "a", "b", "c", "d", "e"};
            PyList pyList = PyConverter.EnumeratorToPyList(list);
            Assert.Equal(5, pyList.Length());
            Assert.Equal("a", pyList[0].As<string>());
        }
    }
}
