using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class SpellUINodeView: MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private RectTransform _selection = null;
    }
}
