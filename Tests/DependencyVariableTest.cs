using NUnit.Framework;
using R3;

namespace TanitakaTech.StateVariable.Tests
{
    [TestFixture]
    public class DependencyStateTest
    {
        [Test]
        public void ShouldFireOnlyOnceWithLastSetValueWhenSetMultipleTimesBeforeObserve()
        {
            // Arrange
            using var observableState = new ObservableState<int>(0);
            int observedValue = 0;
            int observeCount = 0;
            
            var dependencyState = DependencyState<bool>.Create(observableState, (_) => _);

            // Act
            observableState.Set(5);
            observableState.Set(10);
            using var disposable = dependencyState.Observe()
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
            using var observableState = new ObservableState<int>(default);
            int observedValue = 0;
            int observeCount = 0;
            
            var dependencyState = DependencyState<int>.Create(observableState, (_) => _);

            // Act
            using var disposable = dependencyState.Observe()
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