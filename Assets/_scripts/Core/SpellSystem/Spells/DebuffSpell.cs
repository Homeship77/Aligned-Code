using UnityEngine;

namespace Core.SpellSystem
{
    public class DebuffSpell : ASpellElement
    {
        public DebuffSpell(ESpellID spellID, SpellNode spell) : base(spellID, spell)
        { }
        public override void OnAction(Vector3 pos, Vector3 trg)
        {

        }

        public virtual void OnDisable()
        {

        }
    }
}
