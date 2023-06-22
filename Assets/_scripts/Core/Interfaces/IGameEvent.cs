using Core;
using Core.Enemies;
using Core.SpellSystem;
using System;
using UnityEngine;

namespace Interfaces
{
    public interface IGameEvent : IGlobalSubscriber
    {
        void ProcessEvent(EInteractableType eventType, int value);

        void CriticalEvent(EEventType eventType);

        void ObjectReturningToPool(Vector3 objPosition);

        void CriticalEnemyEvent(EEnemyEventType eventType, EnemyView enemy);

        void StartSpellEvent(SpellNode data, Vector3 target, Action<Vector3, Vector3> callback);
    }
}
