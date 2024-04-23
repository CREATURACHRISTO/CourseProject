using FishNet.Object;
using FishNet.Object.Synchronizing;
using SS3D.Core.Behaviours;
using SS3D.Systems.Tile;
using System;
using UnityEngine;
using Actor = Coimbra.Actor;

namespace SS3D.Systems.Tile.TileMapCreator
{
    public class RegisterSpawnPoint : NetworkActor
    {
        [SyncObject]
        public readonly SyncList<GameObject> SpawnLocations = new();
        
        private int _blockingPlayers;
        
        [Server]
        public void Start()
        {
            base.Start();
            
            _blockingPlayers = 0;
            SpawnLocations.Add(GameObject);
        }
        
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != 11)
            {
                return;
            }
            
            _blockingPlayers += 1;
            MoveToTail();
        }
        
        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != 11)
            {
                return;
            }
            
            _blockingPlayers -= 1;
            
            if (_blockingPlayers == 0)
            {
                MoveToHead();
            }
        }
        
        [Server]
        private void MoveToHead()
        {
            SpawnLocations.Remove(GameObject);
            SpawnLocations.Insert(0, GameObject);
            Debug.Log("Moving to head.");
            
            foreach (GameObject x in SpawnLocations)
            {
                Debug.Log(x.name);
            }
        }
        
        [Server]
        private void MoveToTail()
        {
            SpawnLocations.Remove(GameObject);
            SpawnLocations.Add(GameObject);
            Debug.ClearDeveloperConsole();
            Debug.Log("Moving to tail.");
            
            foreach (GameObject x in SpawnLocations)
            {
                Debug.Log(x.name);
            }
        }
    }
}
