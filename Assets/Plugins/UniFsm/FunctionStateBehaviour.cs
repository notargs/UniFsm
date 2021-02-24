using System;

namespace UniFsm
{
    internal sealed class FunctionStateBehaviour<TState> : StateBehaviour<TState>
        where TState : Enum
    {
        private readonly Func<OptionalEnum<TState>> _tickFunc;
        private readonly Action _onEnabledFunc;
        private readonly Action _onDisabledFunc;

        public FunctionStateBehaviour(Func<OptionalEnum<TState>> tickFunc, Action onEnabledFunc, Action onDisabledFunc)
        {
            _tickFunc = tickFunc;
            _onEnabledFunc = onEnabledFunc;
            _onDisabledFunc = onDisabledFunc;
        }

        public override OptionalEnum<TState> Tick()
        {
            return _tickFunc?.Invoke() ?? OptionalEnum<TState>.None;
        }

        public override void OnEnabled()
        {
            _onEnabledFunc?.Invoke();
        }

        public override void OnDisabled()
        {
            _onDisabledFunc?.Invoke();
        }
    }
}