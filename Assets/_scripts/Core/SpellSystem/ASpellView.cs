using UnityEngine;

namespace Core.SpellSystem
{
    public abstract class ASpellView : MonoBehaviour
    {
        protected SpellNode _data;

        void OnEnable()
        {
            OnEnabled();
        }

        void Update()
        {
            OnUpdate();
        }

        void FixedUpdate()
        {
            OnFixedUpdate();
        }

        private void OnDisable()
        {
            OnDisabled();
        }

        public virtual void Init(SpellNode data)
        {
            _data = data;
        }

        public abstract void OnEnabled();
        public abstract void OnUpdate();
        public abstract void OnFixedUpdate();
        public abstract void OnDisabled();



    }
}
