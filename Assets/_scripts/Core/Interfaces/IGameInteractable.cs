using Core;
using Core.Player;

namespace Interfaces
{
    public interface IGameInteractable : IGlobalSubscriber
    {
        EInteractableType InteractableType { get; }
        void Action(PlayerSessionData sessionData);
    }
}
