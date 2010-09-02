using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reanimator.XmlDefinitions
{
    class SoundEffect : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "eType",
                ElementType = ElementType.Int32,
                DefaultValue = 0,
            },
            new XmlCookElement
            {
                Name = "pszName",
                ElementType = ElementType.String,
                DefaultValue = null,
            },
            new XmlCookElement
            {
                Name = "pfParamsBase",
                ElementType = ElementType.FloatArray,
                Count = 8,
                DefaultValue = 0.0f,
            },
            new XmlCookElement
            {
                Name = "pfParamsVariation",
                ElementType = ElementType.FloatArray,
                Count = 8,
                DefaultValue = 0.0f,
            },
            new XmlCookElement
            {
                Name = "nWeight",
                ElementType = ElementType.Int32,
                DefaultValue = 0,
            },
            new XmlCookElement
            {
                Name = "nDelay",
                ElementType = ElementType.Int32,
                DefaultValue = 0,
            },
            new XmlCookElement
            {
                Name = "bReadOnly",
                ElementType = ElementType.NonCookedInt32, // bool??
                DefaultValue = 0, // FALSE??
            },
            new XmlCookElement
            {
                Name = "bEnabled",
                ElementType = ElementType.NonCookedInt32, // bool??
                DefaultValue = 0, // TRUE??
            },
            new XmlCookElement
            {
                Name = "pEffects",
                ElementType = ElementType.TableCount,
                DefaultValue = null,
                ChildType = typeof (SoundEffect)
            },
        };

        public SoundEffect()
        {
            RootElement = "SOUND_EFFECT";
            base.Elements.AddRange(Elements);
        }
    }
}
