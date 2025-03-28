using NUnit.Framework;
using R3;

namespace TanitakaTech.StateVariable.Tests
{
    [TestFixture]
    public class ObservableVariableTest
    {
        [Test]
        public void ShouldFireOnlyOnceWithLastSetValueWhenSetMultipleTimesBeforeObserve()
        {
            // Arrange
            using var observableVariable = new ObservableState<int>(0);
            int observedValue = 0;
            int observeCount = 0;

            // Act
            observableVariable.Set(5);
            observableVariable.Set(10);
            using var disposable = observableVariable.Observe()
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
            using var observableVariable = new ObservableState<int>(default);
            int observedValue = 0;
            int observeCount = 0;

            // Act
            using var disposable = observableVariable.Observe()
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