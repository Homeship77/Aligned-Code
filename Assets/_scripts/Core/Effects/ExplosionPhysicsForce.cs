using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Effects
{
    public class ExplosionPhysicsForce : MonoBehaviour
    {
        [SerializeField]
        private float explosionForce = 4;
        [SerializeField]
        private float explosionRadius = 4;
        [SerializeField]
        LayerMask _collisionMask;


        public bool ApplyExplosion()
        {
            float multiplier = GetComponent<ParticleSystemMultiplier>().multiplier;

            float r = explosionRadius * multiplier;
            Collider[] hits = new Collider[10];
            var cols = Physics.OverlapSphereNonAlloc(transform.position, r, hits, _collisionMask);
            if (cols > 0)
            {
                var rigidbodies = new List<Rigidbody>();
                for (int i = 0; i < cols; i++)
                {
                    if (hits[i].attachedRigidbody != null && !rigidbodies.Contains(hits[i].attachedRigidbody))
                    {
                        rigidbodies.Add(hits[i].attachedRigidbody);
                    }
                }
                foreach (var rb in rigidbodies)
                {
                    rb.AddExplosionForce(explosionForce * multiplier, transform.position, r, 1 * multiplier, ForceMode.Impulse);

                }
                return true;
            }
            return false;
        }
    }
}
