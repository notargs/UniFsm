using NUnit.Framework;
using UniFsm;
using UnityEngine;
using UnityEngine.TestTools;

namespace UnitTests
{
    public sealed class UnitTest
    {
        [Test]
        public void Run()
        {
            var stateMachine = new StateMachine<MockState>(MockState.StateA);
            stateMachine.RegisterState(MockState.StateA, new MockStateA());
            stateMachine.RegisterState(MockState.StateB, new MockStateB());
            stateMachine.Tick();
            stateMachine.Dispose();

            LogAssert.Expect(LogType.Log, "StateA Enabled");
            LogAssert.Expect(LogType.Log, "StateA Tick");
            LogAssert.Expect(LogType.Log, "StateA Disabled");
            LogAssert.Expect(LogType.Log, "StateB Enabled");
            LogAssert.Expect(LogType.Log, "StateB Tick");
            LogAssert.Expect(LogType.Log, "StateB Disabled");
        }

        private enum MockState
        {
            StateA,
            StateB
        }

        private class MockStateA : StateBehaviour<MockState>
        {
            public override void OnEnabled()
            {
                Debug.Log("StateA Enabled");
            }

            public override void OnDisabled()
            {
                Debug.Log("StateA Disabled");
            }

            public override OptionalEnum<MockState> Tick()
            {
                Debug.Log("StateA Tick");
                return MockState.StateB;
            }
        }

        private class MockStateB : StateBehaviour<MockState>
        {
            public override void OnEnabled()
            {
                Debug.Log("StateB Enabled");
            }

            public override void OnDisabled()
            {
                Debug.Log("StateB Disabled");
            }

            public override OptionalEnum<MockState> Tick()
            {
                Debug.Log("StateB Tick");
                return OptionalEnum<MockState>.None;
            }
        }
    }
}