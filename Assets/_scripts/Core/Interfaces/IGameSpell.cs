using Core.SpellSystem;
using UnityEngine;

namespace Interfaces
{
    public interface IGameSpell
    {
        void Action(Vector3 pos, Vector3 trg);
    }
}
