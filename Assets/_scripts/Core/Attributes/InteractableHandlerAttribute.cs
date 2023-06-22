using System;

namespace Core.Attributes
{
    public class InteractableHandlerAttribute : Attribute
    {
        public readonly EInteractableType InteractableID;
        public InteractableHandlerAttribute(EInteractableType id)
            : base()
        {
            InteractableID = id;
        }
    }
}
