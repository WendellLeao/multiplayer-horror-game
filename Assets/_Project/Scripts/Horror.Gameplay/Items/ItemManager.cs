using Horror.Gameplay.Enemies.Events;
using System.Collections.Generic;
using Horror.Gameplay.Enemies;
using Horror.Gameplay.Cameras;
using Horror.Events;
using UnityEngine;
using Mirror;

namespace Horror.Gameplay.Items
{
    public sealed class ItemManager : NetworkBehaviour
    {
        [SerializeField] private int _maximumItemsCount;
        [SerializeField] private ItemSpawnData[] _itemSpawnDatas;
        
        public List<Item> _items = new List<Item>();
        private int _itemSpawnDataIterator;
        private ICameraService _cameraService;
        private IEventService _eventService;
        private IHasEvidences _enemy;

        public void Initialize(ICameraService cameraService, IEventService eventService)
        {
            _cameraService = cameraService;
            _eventService = eventService;
            
            _eventService.AddEventListener<EnemyCreatedEvent>(HandleEnemyCreated);
        }

        [Server]
        public void Begin(NetworkConnectionToClient conn)
        {
            ServerCreateItems(conn);
        }

        public void Dispose()
        {
            foreach (Item item in _items)
            {
                item.Dispose();
            }
            
            _eventService.RemoveEventListener<EnemyCreatedEvent>(HandleEnemyCreated);
        }
        
        public void Stop()
        {
            if (_items.Count <= 0)
            {
                return;
            }
            
            foreach (Item item in _items)
            {
                if (item == null)
                {
                    continue;
                }
                
                item.Stop();
            }
            
            foreach (ItemSpawnData itemSpawnData in _itemSpawnDatas)
            {
                itemSpawnData.Reset();
            }
                
            _itemSpawnDataIterator = 0;

            _items.Clear();
        }

        public void Tick(float deltaTime)
        {
            foreach (Item item in _items)
            {
                if (item == null)
                {
                    continue;
                }
                
                item.Tick(deltaTime);
            }
        }

        [Server]
        private void ServerCreateItems(NetworkConnectionToClient conn)//TODO: UPDATE CLIENTS ITEMS LIST 
        {
            // Debug.Log("itemsCount: " + _items.Count);
            
            if (_items.Count >= _maximumItemsCount)
            {
                return;
            }

            _itemSpawnDataIterator = Random.Range(0, _itemSpawnDatas.Length);
            
            ItemSpawnData itemSpawnData = _itemSpawnDatas[_itemSpawnDataIterator];
            
            GameObject itemObject = ServerSpawnItem(itemSpawnData);

            ServerAddItemToList(itemObject);
            
            TargetRpcInitializeItem();

            RpcSetItemPosition(itemSpawnData.CurrentSpawnPoint.position, itemObject);
            
            itemSpawnData.IncreaseIterator();

            ServerIncreaseSpawnDataIterator();
        }

        [Server]
        private GameObject ServerSpawnItem(ItemSpawnData itemSpawnData)
        {
            ItemData itemData = itemSpawnData.ItemData;
                
            GameObject itemObject = Instantiate(itemData.Prefab);
            
            NetworkServer.Spawn(itemObject);

            return itemObject;
        }
        
        [ClientRpc]
        private void RpcSetItemPosition(Vector3 spawnPoint, GameObject itemObject)
        {
            Transform itemTransform = itemObject.transform;
            
            itemTransform.position = spawnPoint;
        }

        [Server]
        private void ServerAddItemToList(GameObject itemObject)
        {
            _items.Add(itemObject.GetComponent<Item>());

            for (int i = 0; i < _items.Count; i++)
            {
                Item item = _items[i];

                RpcAddItemToList(item.gameObject);
            }
        }
        
        [ClientRpc]
        private void RpcAddItemToList(GameObject itemObject)
        {
            Item item = itemObject.GetComponent<Item>();
            
            if (_items.Contains(item))
            {
                return;
            }
            
            _items.Add(item);
        }
        
        [ClientRpc]
        private void TargetRpcInitializeItem()
        {
            foreach (Item item in _items)
            {
                if (item.HasInitialized)
                {
                    continue;
                }
                
                item.Initialize(_cameraService, _eventService, _enemy);
            }
        }

        [Server]
        private void ServerIncreaseSpawnDataIterator()
        {
            _itemSpawnDataIterator++;

            int lastSpawnDataIndex = _itemSpawnDatas.Length - 1;
            
            _itemSpawnDataIterator = Mathf.Clamp(_itemSpawnDataIterator, 0, lastSpawnDataIndex);
        }
        
        [Client]
        private void HandleEnemyCreated(ServiceEvent serviceEvent)
        {
            if (serviceEvent is EnemyCreatedEvent enemyCreatedEvent)
            {
                _enemy = enemyCreatedEvent.Enemy;
            }
        }
    }
}
