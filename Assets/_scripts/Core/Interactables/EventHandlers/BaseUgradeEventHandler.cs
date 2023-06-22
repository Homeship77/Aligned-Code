using Core.Attributes;
using Core.Player;

namespace Core.Interactables
{
    [InteractableHandler(EInteractableType.eit_upgrade)]
    public class BaseUgradeEventHandler : AInteractableEventHandler
    {
        public BaseUgradeEventHandler(PlayerSessionData plData): base(plData) { }

        public override void ProcessInteractableEvent(int value)
        {
            if (value < 0)
            {
                _plData.AddLowUpgrade(-value);
            }
            else
            {
                _plData.AddHighUpgrade(value);
            }
        }
    }
}
