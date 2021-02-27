using System;
using System.Threading;
#if UNIFSM_UNITASK_SUPPORT
using Cysharp.Threading.Tasks;
#endif

namespace UniFsm
{
    public static class StateMachineExtension
    {
        public static void RegisterStateBehaviour<TState>(
            this StateMachine<TState> stateMachine,
            TState stateType,
            Func<OptionalEnum<TState>> tickFunc = default,
            Action onEnabledFunc = default,
            Action onDisabledFunc = default
        ) where TState : Enum
        {
            stateMachine.RegisterStateBehaviour(stateType,
                new FunctionStateBehaviour<TState>(tickFunc, onEnabledFunc, onDisabledFunc));
        }

#if UNIFSM_UNITASK_SUPPORT
        public static void RegisterStateBehaviour<TState>(
            this StateMachine<TState> stateMachine,
            TState stateType,
            Func<IUniTaskAsyncEnumerable<AsyncUnit>, CancellationToken, UniTask<TState>> runAsync = default
        ) where TState : Enum
        {
            stateMachine.RegisterStateBehaviour(stateType, new UniTaskFunctionStateBehaviour<TState>(runAsync));
        }
#endif
    }
}