using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.UI
{
    public class FloatingJoystick : Joystick
    {
        protected override void Start()
        {
            base.Start();
            background.gameObject.SetActive(false);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (Mathf.Abs(eventData.pointerId) != UseTouchNumber)
                return;
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (Mathf.Abs(eventData.pointerId) != UseTouchNumber)
                return;
            background.gameObject.SetActive(false);
            base.OnPointerUp(eventData);
        }
    }
}