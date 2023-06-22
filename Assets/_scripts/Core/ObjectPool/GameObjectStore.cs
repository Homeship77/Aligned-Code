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
        public EItemFilterType ItemType;
        public int Weight;
    }

    [Serializable]
    public enum EItemFilterType
    {
        eift_all = 0,
        eift_takeable = 1,
        eift_treatable = 2,
        eift_unique = 3,
        eift_effect = 4,
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

        public List<GameObjectItem> GetFiltererdItems(EItemFilterType filter = EItemFilterType.eift_all)
        {
            List<GameObjectItem> res = new List<GameObjectItem>();
            for (int i = 0; i < ItemListData.Count; i++)
            {
                if (filter == EItemFilterType.eift_all || ItemListData[i].ItemType == filter)
                {
                    res.Add(ItemListData[i]);
                }
            }
            return res;
        }

        public string[] GetFilteredItemsIDs(EItemFilterType filter = EItemFilterType.eift_all)
        {
            List<string> res = new List<string>();
            for(int i =0; i< ItemListData.Count; i++)
            {
                if (filter == EItemFilterType.eift_all || ItemListData[i].ItemType == filter)
                {
                    res.Add(ItemListData[i].ID);
                }
            }
            return res.ToArray();
        }
    }
}
