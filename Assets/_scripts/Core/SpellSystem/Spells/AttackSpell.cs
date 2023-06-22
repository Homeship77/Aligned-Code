using EventSystems;
using Interfaces;
using UnityEngine;

namespace Core.SpellSystem
{
    public class AttackSpell: ASpellElement
    {
        public AttackSpell(ESpellID spellID, SpellNode spell) : base(spellID, spell)
        { }

        public override void OnAction(Vector3 pos, Vector3 trg)
        {
            GameObject go = null;
            EventManager.RaiseEvent<IGameEffectEvent>(handler => handler.AddEffect(_data.SpellViewPrefabID, pos, pos, out go, null));
            if (go == null)
                return;
            go.transform.rotation = Quaternion.LookRotation((trg - pos).normalized, Vector3.up);

            var spellShot = go.GetComponent<SpellShotView>();
            if (spellShot != null)
            {
                spellShot.Init(_data);
            }
        }

        public virtual void OnReady()
        {

        }
    }
}
