namespace Hellgate.Xml
{
    class GameGlobalDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "nLevelDefinition",
                DefaultValue = null,
                ExcelTableCode = 29233, // LEVEL
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nDRLGOverride",
                DefaultValue = null,
                ExcelTableCode = 21553, // LEVEL_DRLGS
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nRoomOverride",
                DefaultValue = null,
                ExcelTableCode = 20017, // ROOM_INDEX
                ElementType = ElementType.ExcelIndex
            }
        };


        public GameGlobalDefinition()
        {
            RootElement = "GAME_GLOBAL_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}
