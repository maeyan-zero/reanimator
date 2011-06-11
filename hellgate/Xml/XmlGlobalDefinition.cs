using System;

namespace Hellgate.Xml
{
    class XmlGlobalDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "fPlayerSpeed",
                DefaultValue = 7.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_INVERTMOUSE",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00000002
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_NO_SHRINKING_BONES",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00000004
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_NORAGDOLLS",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00000001
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_CHEAT_LEVELS",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00000008
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_NOMONSTERS",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00000010
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_NO_CONVERTS",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00000020
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_DATA_WARNINGS",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00000040
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_BACKGROUND_WARNINGS",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00000080
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_STRING_WARNINGS",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x01000000
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_FORCE_SYNCH",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00000100
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_LOAD_DEBUG_BACKGROUND_TEXTURES",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00000200
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_SKILL_LEVEL_CHEAT",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00000400
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_SILENT_ASSERT",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00000800
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_NOLOOT",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00001000
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_ABSOLUTELYNOMONSTERS",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00002000
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_FULL_LOGGING",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00004000
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_UPDATE_TEXTURES_IN_GAME",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00008000
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_AUTOMAP_ROTATE",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00010000
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_FORCEBLOCKINGSOUNDLOAD",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00020000
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_HAVOKFX_ENABLED",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00040000
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_MULTITHREADED_HAVOKFX_ENABLED",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00080000
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_HAVOKFX_RAGDOLL_ENABLED",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00100000
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_NO_POPUPS",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00200000
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_NO_CLEANUP_BETWEEN_LEVELS",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00400000
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_CAST_ON_HOTKEY",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x00800000
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_MAX_POWER",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x02000000
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_SKILL_COOLDOWN",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x04000000
            },
            new XmlCookElement
            {
                Name = "GLOBAL_FLAG_USE_HQ_SOUNDS",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x08000000
            },
            new XmlCookElement
            {
                Name = "dwGameFlags",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "dwSeed",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "szPlayerName",
                DefaultValue = "Marcus",
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "szEnvironment",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nDefWidth",
                DefaultValue = 800,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nDefHeight",
                DefaultValue = 600,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nDebugOutputLevel",
                DefaultValue = 4,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nDebugSoundDelayTicks",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "szLanguage",
                DefaultValue = "English",
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "szAuthenticationServer",
                DefaultValue = "192.168.50.162",
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "ACCT_TITLE_ADMINISTRATOR",
                DefaultValue = false,
                ElementType = ElementType.BitFlag,
                BitFlagIndex = 0x00,
                BitFlagCount = 256
            },
            new XmlCookElement
            {
                Name = "ACCT_TITLE_DEVELOPER",
                DefaultValue = false,
                ElementType = ElementType.BitFlag,
                BitFlagIndex = 0x01,
                BitFlagCount = 256
            },
            new XmlCookElement
            {
                Name = "ACCT_TITLE_FSSPING0_EMPLOYEE",
                DefaultValue = false,
                ElementType = ElementType.BitFlag,
                BitFlagIndex = 0x02,
                BitFlagCount = 256
            },
            new XmlCookElement
            {
                Name = "ACCT_TITLE_CUSTOMER_SERVICE_REPRESENTATIVE",
                DefaultValue = false,
                ElementType = ElementType.BitFlag,
                BitFlagIndex = 0x03,
                BitFlagCount = 256
            },
            new XmlCookElement
            {
                Name = "ACCT_TITLE_SUBSCRIBER",
                DefaultValue = false,
                ElementType = ElementType.BitFlag,
                BitFlagIndex = 0x04,
                BitFlagCount = 256
            },
            new XmlCookElement
            {
                Name = "ACCT_TITLE_BOT",
                DefaultValue = false,
                ElementType = ElementType.BitFlag,
                BitFlagIndex = 0x05,
                BitFlagCount = 256
            },
            new XmlCookElement
            {
                Name = "ACCT_MODIFIER_TRIAL_SUBSCRIPTION",
                DefaultValue = false,
                ElementType = ElementType.BitFlag,
                BitFlagIndex = 0x40,
                BitFlagCount = 256
            },
            new XmlCookElement
            {
                Name = "ACCT_MODIFIER_STANDARD_SUBSCRIPTION",
                DefaultValue = false,
                ElementType = ElementType.BitFlag,
                BitFlagIndex = 0x41,
                BitFlagCount = 256
            },
            new XmlCookElement
            {
                Name = "ACCT_MODIFIER_LIFETIME_SUBSCRIPTION",
                DefaultValue = false,
                ElementType = ElementType.BitFlag,
                BitFlagIndex = 0x42,
                BitFlagCount = 256
            },
            new XmlCookElement
            {
                Name = "ACCT_STATUS_UNDERAGE",
                DefaultValue = false,
                ElementType = ElementType.BitFlag,
                BitFlagIndex = 0x82,
                BitFlagCount = 256
            },
            new XmlCookElement
            {
                Name = "ACCT_STATUS_SUSPENDED",
                DefaultValue = false,
                ElementType = ElementType.BitFlag,
                BitFlagIndex = 0x80,
                BitFlagCount = 256
            },
            new XmlCookElement
            {
                Name = "ACCT_STATUS_BANNED_FROM_GUILDS",
                DefaultValue = false,
                ElementType = ElementType.BitFlag,
                BitFlagIndex = 0x81,
                BitFlagCount = 256
            },
            new XmlCookElement
            {
                Name = "ACCT_ACCOMPLISHMENT_ALPHA_TESTER",
                DefaultValue = false,
                ElementType = ElementType.BitFlag,
                BitFlagIndex = 0xC0,
                BitFlagCount = 256
            },
            new XmlCookElement
            {
                Name = "ACCT_ACCOMPLISHMENT_BETA_TESTER",
                DefaultValue = false,
                ElementType = ElementType.BitFlag,
                BitFlagIndex = 0xC1,
                BitFlagCount = 256
            },
            new XmlCookElement
            {
                Name = "ACCT_ACCOMPLISHMENT_HARDCORE_MODE_BEATEN",
                DefaultValue = false,
                ElementType = ElementType.BitFlag,
                BitFlagIndex = 0xC2,
                BitFlagCount = 256
            },
            new XmlCookElement
            {
                Name = "ACCT_ACCOMPLISHMENT_REGULAR_MODE_BEATEN",
                DefaultValue = false,
                ElementType = ElementType.BitFlag,
                BitFlagIndex = 0xC3,
                BitFlagCount = 256
            }
        };


        public XmlGlobalDefinition()
        {
            RootElement = "GLOBAL_DEFINITION";
            base.Elements.AddRange(Elements);
            Flags = new Int32[] { -1 };
            BitFlags = new UInt32[8]; // 256 / 32 = 8
        }
    }
}
