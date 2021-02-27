#if UNIFSM_UNITASK_SUPPORT
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace UniFsm
{
    internal sealed class UniTaskFunctionStateBehaviour<TState> : UniTaskStateBehaviour<TState> where TState : Enum
    {
        private readonly Func<IUniTaskAsyncEnumerable<AsyncUnit>, CancellationToken, UniTask<TState>> _runAsync;

        public UniTaskFunctionStateBehaviour(Func<IUniTaskAsyncEnumerable<AsyncUnit>, CancellationToken, UniTask<TState>> runAsync)
        {
            _runAsync = runAsync;
        }

        public override UniTask<TState> RunAsync(IUniTaskAsyncEnumerable<AsyncUnit> onTick, CancellationToken cancellationToken)
        {
            return _runAsync(onTick, cancellationToken);
        }
    }
}
#endif