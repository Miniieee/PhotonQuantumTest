namespace Quantum
{
  using Photon.Deterministic;
  using UnityEngine.Scripting;

  [Preserve]
  public unsafe class GameManagerSystem : SystemMainThread, ISignalOnComponentAdded<GameManager>, ISignalPlayerKilled, ISignalOnPlayerDisconnected
  {
    public unsafe void OnAdded(Frame f, EntityRef entity, GameManager* component)
    {
      var config = f.FindAsset<GameManagerConfig>(component->GameManagerConfig);
      component->TimeToWaitForPlayers = config.TimeToWaitForPlayers;
    }

    public void OnPlayerDisconnected(Frame f, PlayerRef player)
    {
      foreach (var entityPair in f.GetComponentIterator<PlayerLink>())
      {
        if (entityPair.Component.Player == player)
        {
          f.Destroy(entityPair.Entity);
          break;
        }
      }

      EvaluateGameOverCondition(f);
    }

    public void PlayerKilled(Frame f)
    {
      var gameManager = f.Unsafe.GetPointerSingleton<GameManager>();
      if (gameManager->CurrentGameState != GameState.Playing)
        return;

      if (f.ComponentCount<PlayerLink>() > 1)
        return;

      if (GetWinner(f, out var winner))
      {
        f.Events.OnGameOver(winner);
        gameManager->CurrentGameState = GameState.GameOver;
      }
    }

    public override void Update(Frame f)
    {
      var gameManager = f.Unsafe.GetPointerSingleton<GameManager>();
      if (gameManager->CurrentGameState != GameState.WaitingForPlayers)
        return;

      gameManager->TimeToWaitForPlayers -= f.DeltaTime;
      if (gameManager->TimeToWaitForPlayers <= FP._0)
      {
        gameManager->CurrentGameState = f.ComponentCount<PlayerLink>() > 1 ? GameState.Playing : GameState.GameOver;
        f.Events.OnGameStateChanged();

        if (gameManager->CurrentGameState == GameState.GameOver)
        {

          if (GetWinner(f, out var entityRef))
          {
            f.Events.OnGameOver(entityRef);
          }
          else
          {
            Log.Info("No winner could be determined.");
          }
        }
      }
    }

    private bool GetWinner(Frame f, out EntityRef entityRef)
    {
      entityRef = EntityRef.None;
      foreach (var entityPair in f.GetComponentIterator<PlayerLink>())
      {
        entityRef = entityPair.Entity;
        break;
      }
      return entityRef != EntityRef.None;
    }

    private void EvaluateGameOverCondition(Frame f)
    {
      var gameManager = f.Unsafe.GetPointerSingleton<GameManager>();

      if (f.ComponentCount<PlayerLink>() > 1)
        return;

      if (GetWinner(f, out var winner))
      {
        f.Events.OnGameOver(winner);
        gameManager->CurrentGameState = GameState.GameOver;
      }
    }
  }
}
