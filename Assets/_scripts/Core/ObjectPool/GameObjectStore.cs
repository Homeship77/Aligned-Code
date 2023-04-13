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
        public bool SingleInstance;
        public bool Takeable;
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

        public GameObjectItem GetItemByIndex(int index)
        {
            if (ItemListData.Count > index)
                return ItemListData[index];
            return new GameObjectItem();
        }

        public string[] GetItemsIDs(int filter = 0)
        {
            List<string> res = new List<string>();
            for(int i =0; i< ItemListData.Count; i++)
            {
                switch(filter)
                {
                    case 0:
                        res.Add(ItemListData[i].ID);
                        break;
                    case 1: // takeable
                        if (ItemListData[i].Takeable && !ItemListData[i].SingleInstance)
                            res.Add(ItemListData[i].ID);
                        break; 
                    case 2://treatable
                        if (!ItemListData[i].Takeable && !ItemListData[i].SingleInstance)
                            res.Add(ItemListData[i].ID);
                        break;
                    case 3: //unique objects
                        if (ItemListData[i].SingleInstance)
                            res.Add(ItemListData[i].ID);
                        break;
                }
                
            }
            return res.ToArray();
        }
    }
}
