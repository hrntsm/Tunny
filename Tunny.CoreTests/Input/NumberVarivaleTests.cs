using System;

using Xunit;

namespace Tunny.Core.Input
{
    public class NumberVariableTests
    {
        [Fact]
        public void NumberVariableTest()
        {
            var guid = Guid.NewGuid();
            var numberVariable = new NumberVariable(0, 1, false, "test", 0.1, 0.5, guid);
            Assert.Equal(0, numberVariable.LowerBond);
            Assert.Equal(1, numberVariable.UpperBond);
            Assert.False(numberVariable.IsInteger);
            Assert.Equal(0.1, numberVariable.Epsilon);
            Assert.Equal(0.5, numberVariable.Value);
            Assert.Equal(guid, numberVariable.InstanceId);
        }
    }
}
