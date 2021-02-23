using System;

namespace Game.Scripts
{
    public abstract class StateBehaviour<TState>
        where TState : Enum
    {
        public virtual OptionalEnum<TState> Tick()
        {
            return OptionalEnum<TState>.None;
        }

        public virtual void OnEnabled()
        {
        }

        public virtual void OnDisabled()
        {
        }
    }
}