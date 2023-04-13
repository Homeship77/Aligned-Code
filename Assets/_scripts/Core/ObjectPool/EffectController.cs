using System;
using UnityEngine;

namespace Core.ObjectPool
{
    public class EffectController: MonoBehaviour
    {


        private Vector3 _position;
        private Action _action;

        public void SetTargetPosition(Vector3 endPos)
        {
            _position = endPos;
        }

        public void SetCallback(Action callback)
        {
            _action = callback;
        }

        private void Start()
        {
                
        }

        private void Update()
        {
            
        }
    }
}
