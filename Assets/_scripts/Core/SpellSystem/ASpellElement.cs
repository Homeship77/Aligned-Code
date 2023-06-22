using Interfaces;
using System;
using System.Collections.Generic;
using SpellFactory = Core.Factory<Core.SpellSystem.ASpellElement, Core.SpellSystem.SpellNode>;
using UnityEngine;
using Core.Attributes;

namespace Core.SpellSystem
{
    public abstract class ASpellElement : IGameSpell
    {
        protected SpellNode _data;
        public SpellNode Data => _data;

        private float _lastActivation = 0f;
        protected GameObject go;
        public ASpellElement(ESpellID perkID, SpellNode data)
        {
            _data = data;
        }

        public void Action(Vector3 pos, Vector3 trg)
        {
            if (Time.time - _lastActivation < _data.CoolDownTime)
                return;

            _lastActivation = Time.time;
            OnAction(pos, trg);
        }

        public abstract void OnAction(Vector3 pos, Vector3 trg);

        
        static Dictionary<ESpellID, SpellFactory> s_factories;

        static void Init()
        {
            s_factories = new Dictionary<ESpellID, SpellFactory>();

            var baseSpellVar = typeof(ASpellElement);
            Type[] types = baseSpellVar.Assembly.GetTypes();

            foreach (Type t in types)
            {
                if (t.IsAbstract)
                    continue;

                if (!baseSpellVar.IsAssignableFrom(t))
                    continue;

                var attr = t.GetCustomAttributes(typeof(SpellVariantAttribute), false);
                if (attr == null || attr.Length == 0)
                {
                    Debug.Log("Spell without SpellVariantAttribute: " + t);
                    continue;
                }

                var spellID = ((SpellVariantAttribute)attr[0]).SpellID;
                if (s_factories.ContainsKey(spellID))
                    throw new Exception("Spell " + spellID + " already added ");

                s_factories.Add(spellID, new SpellFactory(t));
            }
        }

        public static ASpellElement Create(SpellNode spelldata)
        {
            if (s_factories == null)
                Init();


            var spellID = spelldata.SpellID;

            if (!s_factories.TryGetValue(spellID, out var factory))
            {
                Debug.Log("Unknown Spell " + spellID);
                return null;
            }

            return factory.Build(spelldata);
        }

    }
}
