using System.Collections.Generic;
using UnityEngine;

namespace Core.SpellSystem
{
    [CreateAssetMenu(fileName = "GameSpellsStore", menuName = "Game/Game Spells database", order = 2)]
    public class SpellsDatabase: ScriptableObject
    {
        [SerializeField]
        private List<SpellUINode> _spells = new List<SpellUINode> ();

        public List<SpellUINode> Spells => _spells;

    }
}
