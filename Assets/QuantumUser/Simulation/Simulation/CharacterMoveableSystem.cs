namespace Quantum {
  using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
  public unsafe class CharacterMoveableSystem : SystemMainThreadFilter<CharacterMoveableSystem.Filter>, ISignalOnPlayerAdded {
    

    public override void Update(Frame f, ref Filter filter)
    {
      var input = f.GetPlayerInput(filter.PlayerLink->Player);
      var direction = input->Direction.XOY;

      if (direction.Magnitude > 1)
      {
        direction = direction.Normalized;
      }

      filter.CharacterController3D->Move(f, filter.Entity, direction);
    }

    public struct Filter 
    {
      public EntityRef Entity;
      public CharacterController3D* CharacterController3D;
      public PlayerLink* PlayerLink;
    }

    public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
    {
      var playerdata = f.GetPlayerData(player);
      var playerEntity = f.Create(playerdata.PlayerAvatar);

      PlayerLink playerLink = new PlayerLink() {
        Player = player
      };

      f.Add(playerEntity, playerLink);
    }
  }
}
