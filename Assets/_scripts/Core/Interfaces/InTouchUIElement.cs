using Core.UI;
using UnityEngine;

namespace Interfaces
{
    public interface InTouchUIElement
    {
        SSiblingElement[] SiblingElements { get; }
        TouchUIElementSibling SelectSibling(Vector2 direction);
        void Selected(bool value);
        void Activate(Vector3 trgPoint);
    }
}
