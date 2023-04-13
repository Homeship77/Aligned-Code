using Core;
using UnityEngine;

namespace Interfaces
{
    public interface IGameEvent : IGlobalSubscriber
    {
        void ProcessEvent(EInteractableType eventType, int value);

        void CriticalEvent(EEventType eventType);

        void ObjectReturningToPool(Vector3 objPosition);
    }
}
