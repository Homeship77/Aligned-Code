using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.SpellSystem
{
    [Serializable]
    public class SpellUINode
    {
        [SerializeField]
        private string _nameKey;
        [SerializeField]
        private Sprite _icon;
        [SerializeField]
        private Vector2 _position;

        public string NameKey => _nameKey;
        public Sprite Icon => _icon;
        public Vector2 Position => _position;//graph position from parent


        public SpellNode Spell;

        public List<SpellUINode> ChildNodes = new List<SpellUINode>();

        public SpellUINode GetSpellChildByDirection(Vector2 dir)
        {
            foreach (SpellUINode node in ChildNodes)
            {
                if (node.Position.Equals(dir))
                {
                    return node;
                }
            }
            return null;
        }
    }
}
