using UnityEngine.EventSystems;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Windows;
using Interfaces;

namespace Core.UI
{
    [Serializable]
    public struct SSiblingElement
    {
        public TouchUIElementSibling Link;
        [Range(-1, 1)]
        public int PositionX;
        [Range(-1, 1)]
        public int PositionY;
    }

    public class TouchUIElement : Joystick, InTouchUIElement
    {
        [SerializeField]
        private List<SSiblingElement> _siblingElements = new List<SSiblingElement>();
        public SSiblingElement[] SiblingElements => _siblingElements.ToArray();

        public RectTransform Background => background;
        public RectTransform Handle => handle;

        protected InTouchUIElement _selectedTouchUISibling = null;

        protected override void Start()
        {
            base.Start();
            Selected(false);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (Mathf.Abs(eventData.pointerId) != UseTouchNumber)
                return;
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            Selected(true);

            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (Mathf.Abs(eventData.pointerId) != UseTouchNumber)
                return;
            Selected(false);
            base.OnPointerUp(eventData);
            if (_selectedTouchUISibling != null)
            {
                _selectedTouchUISibling.Activate(ScreenPointToWorldPoint(eventData.position));
            }
            _selectedTouchUISibling = null;
        }

        public TouchUIElementSibling SelectSibling(Vector2 direction)
        {
            foreach (SSiblingElement element in _siblingElements)
            {
                if (element.Link != null && element.PositionX == direction.x && element.PositionY == direction.y)
                {
                    return element.Link;
                }
            }
            return null;
        }

        public void AddSibling(SSiblingElement sibling)
        {
            _siblingElements.Add(sibling);
            //sibling.Link.transform.parent = transform;
        }

        public void AddSibling(TouchUIElementSibling sib, Vector2 pos)
        {
            SSiblingElement sibling;
            sibling.Link = sib;
            sibling.PositionX = (int)pos.x;
            sibling.PositionY = (int)pos.y;
            AddSibling(sibling);
        }

        private void ClearSelectedSibling(InTouchUIElement element)
        {
            _selectedTouchUISibling = element;
            foreach (SSiblingElement el in element.SiblingElements)
            {
                if (el.Link != null)
                {
                    el.Link.SetActive(true);
                }
            }
        }

        protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
        {
            base.HandleInput(magnitude, normalised, radius, cam);
            if (_selectedTouchUISibling != null)
            {
                var selection = _selectedTouchUISibling.SelectSibling(normalised);
                if (selection != null)
                { 
                    _selectedTouchUISibling = selection;
                }
                else
                {
                    return;
                }
            }
            else
            {
                _selectedTouchUISibling = SelectSibling(normalised);

            }
            if (_selectedTouchUISibling != null)
                _selectedTouchUISibling.Selected(true);
            else
                ClearSelectedSibling(this);
        }

        public void Activate(Vector3 trgPoint)
        {

        }

        public void Selected(bool value)
        {
            background.gameObject.SetActive(value);

            foreach (SSiblingElement element in _siblingElements)
            {
                if (element.Link != null)
                {
                    element.Link.SetActive(value);
                }
            }
        }
    }
}
