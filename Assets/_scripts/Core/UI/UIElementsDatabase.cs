using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.UI
{
    [Serializable]
    public struct SUIElementData
    {
        public EUIPrefabType PrefabType;
        public GameObject Prefab;
    }
    [CreateAssetMenu(fileName = "GameUIPrefabsStore", menuName = "Game/Game UI Prefabs database", order = 3)]
    public class UIElementsDatabase: ScriptableObject
    {
        public List<SUIElementData> UIPrefabs = new List<SUIElementData>();

        private Dictionary<EUIPrefabType, GameObject> _indexedData = new Dictionary<EUIPrefabType, GameObject>();


        public GameObject GetUIPrefabByType(EUIPrefabType type) 
        { 
            if (_indexedData.Count != UIPrefabs.Count)
            {
                _indexedData.Clear();
                foreach (var item in UIPrefabs)
                {
                    if (!_indexedData.ContainsKey(item.PrefabType))
                        _indexedData.Add(item.PrefabType, item.Prefab);
                }
            }
            return _indexedData[type];
        }
    }
}
