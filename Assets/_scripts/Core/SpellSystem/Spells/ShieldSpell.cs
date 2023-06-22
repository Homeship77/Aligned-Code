
using UnityEngine;

namespace Core.SpellSystem
{
    public class ShieldSpell : ASpellElement
    {
        public ShieldSpell(ESpellID spellID, SpellNode spell) : base(spellID, spell)
        { }
        public override void OnAction(Vector3 pos, Vector3 trg)
        {

        }

        public virtual void OnDisable()
        {

        }
    }
}
