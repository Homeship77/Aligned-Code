using Core.SpellSystem;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Core.UI
{
    public class UIBuilder
    {
        private SpellsDatabase _store;
        private UIElementsDatabase _uiPrefabs;
        private TouchUIElement _rootUIElement;
        public UIBuilder(SpellsDatabase store, UIElementsDatabase uiPrefabs, TouchUIElement rootUIElement)
        {
            _store = store;
            _uiPrefabs = uiPrefabs;
            _rootUIElement = rootUIElement;
            //EventManager.Subscribe(this);
            UIRebuild();
        }

        private void UIRebuild()
        {
            if (_rootUIElement.Background.childCount > 1)
            {
                foreach (Transform ch in _rootUIElement.Background)
                {
                    if (_rootUIElement.Handle.transform == ch)
                        continue;
                    Object.DestroyImmediate(ch.gameObject);
                }
            }

            var uiSpellNodePref = _uiPrefabs.GetUIPrefabByType(EUIPrefabType.euit_spellTouch);

            foreach (var spell in _store.Spells)
            {
                var sibling = AddNewSpellNode(_rootUIElement.Background, uiSpellNodePref, spell);
                _rootUIElement.AddSibling(sibling, spell.Position);
            }
        }

        private TouchUIElementSibling AddNewSpellNode(RectTransform parentNode, GameObject pref, SpellUINode data)
        {
            var newNode = Object.Instantiate(pref, parentNode);
            var touchElement = newNode.GetComponent<TouchUIElementSibling>();
            var sprite = data.Icon;

            ASpellElement spElem = null;

            if (data.Spell.Type != ESpellType.None) 
            {
                spElem = ASpellElement.Create(data.Spell);
            }

            touchElement.Init(sprite, spElem, data.Position, parentNode);

            foreach(var childNode in data.ChildNodes)
            {
                var sibling = AddNewSpellNode(touchElement.RTransform, pref, childNode);
                touchElement.AddSibling(sibling, childNode.Position);
            }
            return touchElement;
        }

        public GameObject GetUIElement(EUIPrefabType type)
        {
            return _uiPrefabs.GetUIPrefabByType(type);
        }
    }
}
