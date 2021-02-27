#if UNIFSM_UNITASK_SUPPORT
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

namespace UniFsm
{
    public abstract class UniTaskStateBehaviour<TState> : StateBehaviour<TState> where TState : Enum
    {
        private CancellationTokenSource _cancellationTokenSource;

        private Channel<AsyncUnit> _tickChannel;
        private OptionalEnum<TState> _nextState;

        public sealed override void OnEnabled()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            
            _nextState = OptionalEnum<TState>.None;
            
            _tickChannel = Channel.CreateSingleConsumerUnbounded<AsyncUnit>();
            var asyncEnumerable = _tickChannel.Reader.ReadAllAsync().Publish();
            asyncEnumerable.Connect().AddTo(_cancellationTokenSource.Token);

            UniTask.Void(async () =>
            {
                _nextState = await RunAsync(asyncEnumerable, _cancellationTokenSource.Token);
            });
        }

        public sealed override void OnDisabled()
        {
            _tickChannel.Writer.TryComplete();
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public sealed override OptionalEnum<TState> Tick()
        {
            _tickChannel.Writer.TryWrite(AsyncUnit.Default);
            return _nextState;
        }

        public abstract UniTask<TState> RunAsync(IUniTaskAsyncEnumerable<AsyncUnit> onTick, CancellationToken cancellationToken);
    }
}
#endif