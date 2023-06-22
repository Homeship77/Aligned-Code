using System;

namespace Core.Attributes
{
    public class SpellVariantAttribute : Attribute
    {
        public readonly ESpellID SpellID;
        public SpellVariantAttribute(ESpellID id)
            : base()
        {
            SpellID = id;
        }
    }
}
