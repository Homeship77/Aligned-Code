using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.ObjectPool
{
    [Serializable]
    public struct GameObjectItem
    {
        public string ID;
        public GameObject Prefab;
        public int PoolCount;
    }

    [CreateAssetMenu(fileName = "GameObjectStore", menuName = "Game/Game Object Store", order = 0)]
    public class GameObjectStore: ScriptableObject
    {
        public List<GameObjectItem> ItemListData = new List<GameObjectItem>();

        public GameObjectItem GetItem(string id)
        {
            foreach(var item in ItemListData) 
            { 
                if (item.ID == id)
                    return item;
            }
            return new GameObjectItem();
        }
    }
}
