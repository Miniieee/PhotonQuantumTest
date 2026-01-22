namespace Quantum {
    using System;
    using Photon.Deterministic;
    using Quantum.Collections;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class SpawnSystem : SystemSignalsOnly, ISignalOnPlayerAdded
    {
        public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
        {
          if(!firstTime)
              return;
          
          var playerEntityRef = CreatePlayer(f, player);
          
          PlacePlayerOnSpawnPosition(f, playerEntityRef);
        }

        private void PlacePlayerOnSpawnPosition(Frame f, EntityRef playerEntityRef)
        {
            var spawnPointManager = f.Unsafe.GetPointerSingleton<SpawnPointManager>();
            var availableSpawnPoint = f.ResolveList(spawnPointManager->AvailableSpawnPoints);
            var usedSpawnPoints = f.ResolveList(spawnPointManager->UsedSpawnPoints); 

            if(availableSpawnPoint.Count == 0 && usedSpawnPoints.Count == 0)
            {
              foreach (var componentPair in f.GetComponentIterator<SpawnPoint>())
              {
                  // Collect all available spawn points by finding all spawn points
                  // referencing the entity because it's contains all of it's components (in unity OOP it makes more sense to get the actual component on the object)
                  availableSpawnPoint.Add(componentPair.Entity);
              }
            }

            //deterministic seed
            var randomIndex = f.RNG->Next(0, availableSpawnPoint.Count);
            var spawnPointEntityRef = availableSpawnPoint[randomIndex];
            var spawnPointTransform = f.Get<Transform2D>(spawnPointEntityRef);

            var playerTransform = f.Unsafe.GetPointer<Transform2D>(playerEntityRef);
            playerTransform->Position = spawnPointTransform.Position;

            //update spawn point lists
            availableSpawnPoint.RemoveAt(randomIndex);
            usedSpawnPoints.Add(spawnPointEntityRef);

            if (availableSpawnPoint.Count == 0)
            {
                spawnPointManager->AvailableSpawnPoints = usedSpawnPoints;
                spawnPointManager->UsedSpawnPoints = new QList<EntityRef>();
                // availableSpawnPoint = usedSpawnPoints;
                // usedSpawnPoints = f.ResolveList(spawnPointManager->UsedSpawnPoints);
            }
        } 

        private static EntityRef CreatePlayer(Frame f, PlayerRef player)
        {
            var playerdata = f.GetPlayerData(player);
            var playerEntity = f.Create(playerdata.PlayerAvatar);

            PlayerLink playerLink = new PlayerLink()
            {
                Player = player
            };

            f.Add(playerEntity, playerLink);

            var kcc = f.Unsafe.GetPointer<KCC>(playerEntity);
            var kccSettings = f.FindAsset(kcc->Settings);

            kcc->Acceleration = kccSettings.Acceleration;
            kcc->MaxSpeed = kccSettings.BaseSpeed;

            return playerEntity;
        }
    }
}
