using Core.SpellSystem;
using EventSystems;
using Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.UI
{
    public  class TouchUIElementSibling: MonoBehaviour, InTouchUIElement
    {
        [SerializeField]
        private UnityEngine.UI.Image _image;
        [SerializeField]
        private RectTransform _selection= null;

        [SerializeField]
        private List<SSiblingElement> _siblingElements = new List<SSiblingElement>();
        public SSiblingElement[] SiblingElements => _siblingElements.ToArray();

        public RectTransform RTransform => GetComponent<RectTransform>();

        private ASpellElement _spell;

        public void Init(Sprite icon, ASpellElement spell, Vector2 pos, RectTransform parent)
        {
            if (_image!=null)
                _image.sprite = icon;
            _spell = spell;

            var rect = GetComponent<RectTransform>();
            Vector2 radius = parent.sizeDelta / 2 + rect.sizeDelta / 2;
            rect.anchoredPosition = pos * radius;
            Selected(false);
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
            if (!value)
            {
                Selected(false);
            }
        }

        public void Selected(bool value) 
        {
            _selection.gameObject.SetActive(value);
            foreach (SSiblingElement element in _siblingElements)
            {
                if (element.Link != null)
                {
                    element.Link.SetActive(value);
                }
            }
        }

        public TouchUIElementSibling SelectSibling(Vector2 direction)
        {
            TouchUIElementSibling res = null;
            foreach (SSiblingElement element in _siblingElements)
            {
                var active = element.Link != null && element.PositionX == direction.x && element.PositionY == direction.y;
                element.Link.SetActive(active);
                if (active) 
                    res = element.Link;
            }
            return res;
        }

        public void AddSibling(SSiblingElement sibling)
        {
            _siblingElements.Add(sibling);
            sibling.Link.transform.parent = transform;
        }

        public void AddSibling(TouchUIElementSibling sib, Vector2 pos)
        {
            SSiblingElement sibling;
            sibling.Link = sib;
            sibling.PositionX = (int)pos.x;
            sibling.PositionY = (int)pos.y;
            AddSibling(sibling);
        }

        public void Activate(Vector3 trgPoint)
        {
            if (_spell != null)
            {
                Vector3 pos = Vector3.zero;
                EventManager.RaiseEvent<IGameEvent>(handler => handler.StartSpellEvent(_spell.Data, trgPoint, _spell.Action));
            }
        }
    }
}
