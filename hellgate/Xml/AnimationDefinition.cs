namespace Hellgate.Xml
{
    class AnimationDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "dwFlags",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nUnitMode",
                DefaultValue = null,
                ElementType = ElementType.ExcelIndex,
                ExcelTableCode = 26160 // UNITMODES
            },
            new XmlCookElement
            {
                Name = "nGroup",
                DefaultValue = null,
                ElementType = ElementType.ExcelIndex,
                ExcelTableCode = 26928 // ANIMATION_GROUP
            },
            new XmlCookElement
            {
                Name = "pszFile",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nFileIndex",
                DefaultValue = -1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "fDuration",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fVelocity",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fStartOffset",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fTurnSpeed",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fEaseIn",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fEaseOut",
                DefaultValue = 0.5f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fStanceFadeTimePercent",
                DefaultValue = 0.75f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "nBoneWeights",
                DefaultValue = -1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nWeight",
                DefaultValue = 1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nStartStance",
                DefaultValue = null,
                ElementType = ElementType.ExcelIndex,
                ExcelTableCode = 26672 // ANIMATION_STANCE
            },
            new XmlCookElement
            {
                Name = "nStartStance2",
                DefaultValue = null,
                ElementType = ElementType.ExcelIndex,
                ExcelTableCode = 26672 // ANIMATION_STANCE
            },
            new XmlCookElement
            {
                Name = "nStartStance3",
                DefaultValue = null,
                ElementType = ElementType.ExcelIndex,
                ExcelTableCode = 26672 // ANIMATION_STANCE
            },
            new XmlCookElement
            {
                Name = "nEndStance",
                DefaultValue = null,
                ElementType = ElementType.ExcelIndex,
                ExcelTableCode = 26672 // ANIMATION_STANCE
            },
            new XmlCookElement
            {
                Name = "nPriorityBoost",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "tRagdollBlend",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tRagdollPower",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tSelfIllumation",
                DefaultValue = 1.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tSelfIllumationBlend",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "pBinding",
                DefaultValue = null,
                ElementType = ElementType.UnknownPTypeD_0x0D00
            },
            new XmlCookElement
            {
                Name = "pGrannyAnimation",
                DefaultValue = null,
                ElementType = ElementType.UnknownPTypeD_0x0D00
            },
            new XmlCookElement
            {
                Name = "pGrannyFile",
                DefaultValue = null,
                ElementType = ElementType.UnknownPTypeD_0x0D00
            },
            new XmlCookElement
            {
                Name = "pEvents",
                DefaultValue = 0,
                ElementType = ElementType.TableArrayVariable,
                ChildType = typeof (AnimEvent)
            },
            new XmlCookElement
            {
                Name = "nPreviewMode",
                DefaultValue = -1,
                ElementType = ElementType.Int32_0x0A00
            },
            new XmlCookElement
            {
                Name = "pNextInGroup",
                DefaultValue = null,
                ElementType = ElementType.UnknownPTypeD_0x0D00
            },
            new XmlCookElement
            {
                Name = "nAnimationCondition",
                DefaultValue = null,
                ElementType = ElementType.ExcelIndexArrayFixed,
                ExcelTableCode = 26416, // ANIMATION_CONDITION
                Count = 4
            },
            new XmlCookElement
            {
                Name = "tCondition",
                DefaultValue = null,
                ElementType = ElementType.Table,
                ChildType = typeof (ConditionDefinition)
            }
        };

        public AnimationDefinition()
        {
            RootElement = "ANIMATION_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}