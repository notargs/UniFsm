using Cysharp.Threading.Tasks.Linq;
using NUnit.Framework;
using UniFsm;
using UnityEngine;
using UnityEngine.TestTools;

namespace UnitTests
{
    public sealed class UnitTest
    {
        [Test]
        public void RegisterState()
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

        [Test]
        public void RegisterStateBehaviour()
        {
            var stateMachine = new StateMachine<MockState>(MockState.StateA);
            stateMachine.RegisterStateBehaviour(MockState.StateA, new MockStateA());
            stateMachine.RegisterStateBehaviour(MockState.StateB, new MockStateB());
            stateMachine.Tick();
            stateMachine.Dispose();

            LogAssert.Expect(LogType.Log, "StateA Enabled");
            LogAssert.Expect(LogType.Log, "StateA Tick");
            LogAssert.Expect(LogType.Log, "StateA Disabled");
            LogAssert.Expect(LogType.Log, "StateB Enabled");
            LogAssert.Expect(LogType.Log, "StateB Tick");
            LogAssert.Expect(LogType.Log, "StateB Disabled");
        }

        [Test]
        public void RegisterFunctionStateBehaviour()
        {
            var stateMachine = new StateMachine<MockState>(MockState.StateA);
            stateMachine.RegisterStateBehaviour(MockState.StateA, () =>
                {
                    Debug.Log("StateA Tick");
                    return MockState.StateB;
                },
                () => Debug.Log("StateA Enabled"),
                () => Debug.Log("StateA Disabled")
            );
            stateMachine.RegisterStateBehaviour(MockState.StateB, () =>
                {
                    Debug.Log("StateB Tick");
                    return OptionalEnum<MockState>.None;
                },
                () => Debug.Log("StateB Enabled"),
                () => Debug.Log("StateB Disabled")
            );
            stateMachine.Tick();
            stateMachine.Dispose();

            LogAssert.Expect(LogType.Log, "StateA Enabled");
            LogAssert.Expect(LogType.Log, "StateA Tick");
            LogAssert.Expect(LogType.Log, "StateA Disabled");
            LogAssert.Expect(LogType.Log, "StateB Enabled");
            LogAssert.Expect(LogType.Log, "StateB Tick");
            LogAssert.Expect(LogType.Log, "StateB Disabled");
        }

        [Test]
        public void RegisterUniTaskFunctionStateBehaviour()
        {
            var stateMachine = new StateMachine<MockState>(MockState.StateA);
            stateMachine.RegisterStateBehaviour(MockState.StateA, async (enumerable, token) =>
            {
                try
                {
                    Debug.Log("StateA Enabled");
                    await enumerable.Take(2).ForEachAsync(_ => Debug.Log("StateA Tick"), token);
                    return MockState.StateB;
                }
                finally
                {
                    Debug.Log("StateA Disabled");
                }
            });
            stateMachine.RegisterStateBehaviour(MockState.StateB, async (enumerable, token) =>
                {
                    try
                    {
                        Debug.Log("StateB Enabled");
                        await enumerable.Take(2).ForEachAsync(_ => Debug.Log("StateB Tick"), token);
                        while (true)
                        {
                            await enumerable.FirstOrDefaultAsync(token);
                        }
                    }
                    finally
                    {
                        Debug.Log("StateB Disabled");
                    }
                }
            );
            for (var i = 0; i < 6; ++i)
            {
                stateMachine.Tick();
            }
            stateMachine.Dispose();

            LogAssert.Expect(LogType.Log, "StateA Enabled");
            LogAssert.Expect(LogType.Log, "StateA Tick");
            LogAssert.Expect(LogType.Log, "StateA Tick");
            LogAssert.Expect(LogType.Log, "StateA Disabled");
            LogAssert.Expect(LogType.Log, "StateB Enabled");
            LogAssert.Expect(LogType.Log, "StateB Tick");
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