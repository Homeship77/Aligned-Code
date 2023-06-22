using Core.Attributes;
using EventSystems;
using Interfaces;
using UnityEngine;

namespace Core.SpellSystem
{
    [SpellVariant(ESpellID.esID_fireball)]
    public class FireballSpell: AttackSpell
    {
        public FireballSpell(SpellNode spellData) : base(ESpellID.esID_fireball, spellData)        
        { }
    }
}
