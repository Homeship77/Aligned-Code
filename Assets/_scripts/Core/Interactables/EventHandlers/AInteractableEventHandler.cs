using Core.Player;
using System;

namespace Core.Interactables
{
    public abstract class AInteractableEventHandler
    {
        protected PlayerSessionData _plData;
        public AInteractableEventHandler(PlayerSessionData plData)
        {
            _plData = plData;
        }
        public abstract void ProcessInteractableEvent(int value);

        public virtual void InitEventHandler(Action<float> updateEvent)
        {

        }

        public virtual void NeedUpdate(float delta)
        {

        }
    }
}
