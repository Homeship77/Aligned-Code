using Core;

namespace Interfaces
{
    public interface IUIEvent : IGlobalSubscriber
    {
        void ProcessUIEvent(EInteractableType eventType, int value);
        void CriticalUIEvent(EEventType eventType);
    }
}
