# UniFSM
UniFsm is a simple state machine library.


## Installation
Add this line in Packages/manifest.json directly.
```json
"com.notargs.unifsm": "https://github.com/notargs/UniFsm.git?path=Assets/Plugins/UniFsm",
```
   
## Basic Using
First, add state enumeration type.
```c#
private enum GameState
{
    Title,
    Game
}
```

And create state behaviours.

```c#
public sealed class TitleStateBehaviour : StateBehaviour<GameState>
{
    public override void OnEnabled()
    {
        Debug.Log("Title enabled");
    }

    public override void OnDisabled()
    {
        Debug.Log("Title disabled");
    }

    public override OptionalEnum<GameState> Tick()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Moving to game state.
            return GameState.Game;
        }
        return OptionalEnum<GameState>.None;
    }
}

public sealed class GameStateBehaviour : StateBehaviour<GameState>
{
    public override void OnEnabled()
    {
        Debug.Log("Game enabled");
    }

    public override void OnDisabled()
    {
        Debug.Log("Game disabled");
    }

    public override OptionalEnum<GameState> Tick()
    {
        return OptionalEnum<GameState>.None;
    }
}
```

Create StateMachine, register behaviours and run.

```c#
public sealed class GameLoop : MonoBehaviour
{
    private StateMachine<GameState> _stateMachine;
    
    private void Start()
    {
        // Set the default state
        _stateMachine = new StateMachine<GameState>(GameState.Title);
        
        // Register state behaviours
        _stateMachine.RegisterStateBehaviour(GameState.Title, new TitleStateBehaviour());
        _stateMachine.RegisterStateBehaviour(GameState.Game, new GameStateBehaviour());
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    private void OnDestroy()
    {
        _stateMachine.Dispose();
    }
}
```

## Register state behaviour by lambda expression
```c#
stateMachine.RegisterStateBehaviour(GameState.Game, () =>
    {
        Debug.Log("Game tick");
        return OptionalEnum<MockState>.None;
    },
    () => Debug.Log("Game enabled"),
    () => Debug.Log("Game disabled")
);
```

## UniTask support
### UniTaskStateBehaviour
```c#
public sealed class TitleStateBehaviour : UniTaskStateBehaviour<GameState>
{
    public override async UniTask<GameState> RunAsync(IUniTaskAsyncEnumerable<AsyncUnit> onTick, CancellationToken cancellationToken)
    {
        try
        {
            Debug.Log("Title enabled");
            await onTick.TakeWhile(_ => !Input.GetKeyDown(KeyCode.Z)).ForEachAsync(_ => Debug.Log("StateA Tick"), cancellationToken);
            return GameState.Game;
        }
        finally
        {
            Debug.Log("Title disabled");
        }
    }
}
```
### Function registration
```
stateMachine.RegisterStateBehaviour(GameState.Game, async (enumerable, token) =>
{
    try
    {
        Debug.Log("Title enabled");
        await onTick.TakeWhile(_ => !Input.GetKeyDown(KeyCode.Z))
            .ForEachAsync(_ => Debug.Log("StateA Tick"), cancellationToken);
        return GameState.Game;
    }
    finally
    {
        Debug.Log("Title disabled");
    }
});
```

## License
This library is under the MIT License.
