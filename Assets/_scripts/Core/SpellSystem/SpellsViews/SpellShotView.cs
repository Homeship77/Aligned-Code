using Core.Enemies;
using EventSystems;
using Interfaces;
using UnityEngine;
namespace Core.SpellSystem
{
    public class SpellShotView : ASpellView
    {
        [SerializeField]
        private Rigidbody _rig;
        [SerializeField]
        private string _shotDeactivationEffect;
        [SerializeField]
        private string _hitEffect;

        private float _ttl;
        private GameObject go;

        private void OnCollisionEnter(Collision collision)
        {
            EventManager.RaiseEvent<IGameEffectEvent>(handler => handler.AddEffect(_hitEffect, _rig.position, _rig.position, out go, null));
            gameObject.SetActive(false);

            var enemyView = collision.gameObject.GetComponent<EnemyView>();

            if (enemyView != null)
            {
                EventManager.RaiseEvent<IGameEvent>(handler => handler.CriticalEnemyEvent(EEnemyEventType.enet_hited, enemyView));
            }
        }


        public override void Init(SpellNode data)
        {
            base.Init(data);
            _ttl = _data.Range / _data.ShotSpeed;
        }

        public override void OnDisabled()
        {
            //throw new System.NotImplementedException();
        }

        public override void OnEnabled()
        {
            //throw new System.NotImplementedException();
        }

        public override void OnFixedUpdate()
        {
            _rig.velocity = transform.forward * _data.ShotSpeed;
            _ttl -= Time.fixedDeltaTime;
        }

        public override void OnUpdate()
        {
            if (_ttl <= 0)
            {
                EventManager.RaiseEvent<IGameEffectEvent>(handler => handler.AddEffect(_shotDeactivationEffect, _rig.position, _rig.position, out go, null));
                gameObject.SetActive(false);
            }
        }
    }
}