namespace Reanimator.XmlDefinitions
{
    class ConditionDefinition : XmlDefinition
    {
        private static readonly XmlCookElement nType = new XmlCookElement();
        private static readonly XmlCookElement nState = new XmlCookElement();
        private static readonly XmlCookElement nUnitType = new XmlCookElement();
        private static readonly XmlCookElement nSkill = new XmlCookElement();
        private static readonly XmlCookElement nMonsterClass = new XmlCookElement();
        private static readonly XmlCookElement nObjectClass = new XmlCookElement();
        private static readonly XmlCookElement nStat = new XmlCookElement();
        private static readonly XmlCookElement tParams0 = new XmlCookElement();
        private static readonly XmlCookElement tParams1 = new XmlCookElement();
        private static readonly XmlCookElement ConditionBitCheckOwner = new XmlCookElement();
        private static readonly XmlCookElement ConditionBitCheckTarget = new XmlCookElement();
        private static readonly XmlCookElement ConditionBitCheckWeapon = new XmlCookElement();
        private static readonly XmlCookElement ConditionBitNotDeadOrDying = new XmlCookElement();
        private static readonly XmlCookElement ConditionBitIsYourPlayer = new XmlCookElement();
        private static readonly XmlCookElement ConditionBitOwnerIsYourPlayer = new XmlCookElement();
        private static readonly XmlCookElement ConditionBitCheckStateSource = new XmlCookElement();

        public ConditionDefinition()
        {
            RootElement = "CONDITION_DEFINITION";

            nType.Name = "nType";
            nType.DefaultValue = null;
            nType.ExcelTableCode = 0; // todo
            nType.ElementType = ElementType.ExcelIndex;
            Elements.Add(nType);

            nState.Name = "nState";
            nState.DefaultValue = null;
            nState.ExcelTableCode = 0; // todo
            nState.ElementType = ElementType.ExcelIndex;
            Elements.Add(nState);

            nUnitType.Name = "nUnitType";
            nUnitType.DefaultValue = null;
            nUnitType.ExcelTableCode = 0; // todo
            nUnitType.ElementType = ElementType.ExcelIndex;
            Elements.Add(nUnitType);

            nSkill.Name = "nSkill";
            nSkill.DefaultValue = null;
            nSkill.ExcelTableCode = 0; // todo
            nSkill.ElementType = ElementType.ExcelIndex;
            Elements.Add(nSkill);

            nMonsterClass.Name = "nMonsterClass";
            nMonsterClass.DefaultValue = null;
            nMonsterClass.ExcelTableCode = 0; // todo
            nMonsterClass.ElementType = ElementType.ExcelIndex;
            Elements.Add(nMonsterClass);

            nObjectClass.Name = "nObjectClass";
            nObjectClass.DefaultValue = null;
            nObjectClass.ExcelTableCode = 0; // todo
            nObjectClass.ElementType = ElementType.ExcelIndex;
            Elements.Add(nObjectClass);

            nStat.Name = "nStat";
            nStat.DefaultValue = null;
            nStat.ExcelTableCode = 0; // todo
            nStat.ElementType = ElementType.ExcelIndex;
            Elements.Add(nStat);

            tParams0.Name = "tParams0.fValue"; // note: should be "tParams[0].fValue"
            tParams0.DefaultValue = 0.0f;
            tParams0.ElementType = ElementType.Float;
            Elements.Add(tParams0);

            tParams1.Name = "tParams1.fValue"; // note: should be "tParams[1].fValue"
            tParams1.DefaultValue = 0.0f;
            tParams1.ElementType = ElementType.Float;
            Elements.Add(tParams1);

            ConditionBitCheckOwner.Name = "CONDITION_BIT_CHECK_OWNER";
            ConditionBitCheckOwner.ElementType = ElementType.Flag;
            ConditionBitCheckOwner.DefaultValue = false;
            ConditionBitCheckOwner.FlagId = 1;
            Elements.Add(ConditionBitCheckOwner);

            ConditionBitCheckTarget.Name = "CONDITION_BIT_CHECK_TARGET";
            ConditionBitCheckTarget.ElementType = ElementType.Flag;
            ConditionBitCheckTarget.DefaultValue = false;
            ConditionBitCheckTarget.FlagId = 1;
            Elements.Add(ConditionBitCheckTarget);

            ConditionBitCheckWeapon.Name = "CONDITION_BIT_CHECK_WEAPON";
            ConditionBitCheckWeapon.ElementType = ElementType.Flag;
            ConditionBitCheckWeapon.DefaultValue = false;
            ConditionBitCheckWeapon.FlagId = 1;
            Elements.Add(ConditionBitCheckWeapon);

            ConditionBitNotDeadOrDying.Name = "CONDITION_BIT_NOT_DEAD_OR_DYING";
            ConditionBitNotDeadOrDying.ElementType = ElementType.Flag;
            ConditionBitNotDeadOrDying.DefaultValue = false;
            ConditionBitNotDeadOrDying.FlagId = 1;
            Elements.Add(ConditionBitNotDeadOrDying);

            ConditionBitIsYourPlayer.Name = "CONDITION_BIT_IS_YOUR_PLAYER";
            ConditionBitIsYourPlayer.ElementType = ElementType.Flag;
            ConditionBitIsYourPlayer.DefaultValue = false;
            ConditionBitIsYourPlayer.FlagId = 1;
            Elements.Add(ConditionBitIsYourPlayer);

            ConditionBitOwnerIsYourPlayer.Name = "CONDITION_BIT_OWNER_IS_YOUR_PLAYER";
            ConditionBitOwnerIsYourPlayer.ElementType = ElementType.Flag;
            ConditionBitOwnerIsYourPlayer.DefaultValue = false;
            ConditionBitOwnerIsYourPlayer.FlagId = 1;
            Elements.Add(ConditionBitOwnerIsYourPlayer);

            ConditionBitCheckStateSource.Name = "CONDITION_BIT_CHECK_STATE_SOURCE";
            ConditionBitCheckStateSource.ElementType = ElementType.Flag;
            ConditionBitCheckStateSource.DefaultValue = false;
            ConditionBitCheckStateSource.FlagId = 1;
            Elements.Add(ConditionBitCheckStateSource);
        }
    }
}