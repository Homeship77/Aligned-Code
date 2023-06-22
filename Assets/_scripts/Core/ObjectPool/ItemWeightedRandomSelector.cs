using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Core.ObjectPool
{
    public class ItemWeightedRandomSelector
    {
        private GameObjectStore _store;
        public ItemWeightedRandomSelector(GameObjectStore store, EItemFilterType filter)
        {
            _store = store;

            Prepare(filter);
        }

        private int _totalItemsWeight;
        private List<GameObjectItem> _filteredList;
        void Prepare(EItemFilterType filter)
        {
            int totalWeight = 0;
            _filteredList = _store.GetFiltererdItems(filter);
            foreach (var item in _filteredList)
            {
                totalWeight += item.Weight;
            }

            _totalItemsWeight = totalWeight;
        }

        internal GameObjectItem GetRandomItem()
        {
            var fastRandom = new UnityStandardAssets.Utility.FastRandom();
            fastRandom.SetByTicks();

            var rnd = fastRandom.Range(0, _totalItemsWeight);

            foreach (var item in _filteredList)
            {
                rnd -= item.Weight;

                if (rnd <= 0)
                {
                    return item;
                }
            }

            throw new System.Exception("Item pool randomizer error!");
        }
    }
}
