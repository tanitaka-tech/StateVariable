using NUnit.Framework;
using UniRx;

namespace TanitakaTech.StateVariable.Tests
{
    [TestFixture]
    public class ObservableVariableTest
    {
        [Test]
        public void ShouldFireOnlyOnceWithLastSetValueWhenSetMultipleTimesBeforeObserve()
        {
            // Arrange
            using var observableVariable = new ObservableVariable<int>(0);
            int observedValue = 0;
            int observeCount = 0;

            // Act
            observableVariable.Set(5);
            observableVariable.Set(10);
            observableVariable.Observe()
                .Subscribe(value =>
                {
                    observedValue = value;
                    observeCount++;
                });

            // Assert
            Assert.AreEqual(10, observedValue);
            Assert.AreEqual(1, observeCount);
        }
        
        [Test]
        public void ShouldFireOnlyOnceWithInitialValueWhenNotSetBeforeObserve()
        {
            // Arrange
            using var observableVariable = new ObservableVariable<int>(default);
            int observedValue = 0;
            int observeCount = 0;

            // Act
            observableVariable.Observe()
                .Subscribe(value =>
                {
                    observedValue = value;
                    observeCount++;
                });

            // Assert
            Assert.AreEqual((int)default, observedValue);
            Assert.AreEqual(1, observeCount);
        }
    }
}