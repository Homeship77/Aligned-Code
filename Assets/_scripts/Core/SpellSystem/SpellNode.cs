using System;
using UnityEngine;

namespace Core.SpellSystem
{
    [Serializable]
    public struct SpellNode
    {
        public ESpellClass SpellClass;
        public ESpellType Type;
        public ESpellID SpellID;
        public int NeedClassLevel;

        public int Power;
        public int Range;
        public float CoolDownTime;
        public int Cost;
        public int ActivityTime;
        public float ShotSpeed;

        public string SpellViewPrefabID;
    }
}
