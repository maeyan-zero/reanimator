namespace Reanimator.XmlDefinitions
{
    class SkillEvent : XmlDefinition
    {
        private static readonly XmlCookElement nType = new XmlCookElement
        {
            Name = "nType",
            DefaultValue = null,
            ExcelTableCode = 0x00006E30, // (28208)	SKILLEVENTTYPES
            ElementType = ElementType.ExcelIndex
        };
        private static readonly XmlCookElement FlagLaserTurns = new XmlCookElement();
        private static readonly XmlCookElement FlagRequiresTarget = new XmlCookElement();
        private static readonly XmlCookElement FlagForceNew = new XmlCookElement();
        private static readonly XmlCookElement FlagLaserSeeksSurfaces = new XmlCookElement();
        private static readonly XmlCookElement FlagFaceTarget = new XmlCookElement();
        private static readonly XmlCookElement FlagUseUnitTarget = new XmlCookElement();
        private static readonly XmlCookElement FlagUseEventOffset = new XmlCookElement();
        private static readonly XmlCookElement FlagLoop = new XmlCookElement();
        private static readonly XmlCookElement FlagUseEventOffsetAbsolute = new XmlCookElement();
        private static readonly XmlCookElement FlagPlaceOnTarget = new XmlCookElement();
        private static readonly XmlCookElement FlagUseAnimContactPoint = new XmlCookElement();
        private static readonly XmlCookElement FlagTransferStats = new XmlCookElement();
        private static readonly XmlCookElement FlagDoWhenTargetInRange = new XmlCookElement();
        private static readonly XmlCookElement FlagAddToCenter = new XmlCookElement();
        private static readonly XmlCookElement Flag360Targeting = new XmlCookElement();
        private static readonly XmlCookElement FlagUseSkillTargetLocation = new XmlCookElement();
        private static readonly XmlCookElement FlagUseAITarget = new XmlCookElement();
        private static readonly XmlCookElement FlagUseOffhandWeapon = new XmlCookElement();
        private static readonly XmlCookElement FlagFloat = new XmlCookElement();
        private static readonly XmlCookElement FlagDontValidateTarget = new XmlCookElement();
        private static readonly XmlCookElement FlagRandomFiringDirection = new XmlCookElement();
        private static readonly XmlCookElement FlagProjectile = new XmlCookElement();
        private static readonly XmlCookElement FlagWeapon = new XmlCookElement();
        private static readonly XmlCookElement FlagUseWeaponForCondition = new XmlCookElement();
        private static readonly XmlCookElement FlagForceConditionOnEvent = new XmlCookElement();
        private static readonly XmlCookElement FlagUseHolyRadiusForRange = new XmlCookElement();
        private static readonly XmlCookElement FlagUseChancePCode = new XmlCookElement();
        private static readonly XmlCookElement FlagServerOnly = new XmlCookElement();
        private static readonly XmlCookElement FlagClientOnly = new XmlCookElement();
        private static readonly XmlCookElement FlagLaserAttacksLocation = new XmlCookElement();
        private static readonly XmlCookElement FlagAtNextCooldown = new XmlCookElement();
        private static readonly XmlCookElement FlagAimWithWeapon = new XmlCookElement();
        private static readonly XmlCookElement Flag2AimWithWeaponZero = new XmlCookElement();
        private static readonly XmlCookElement Flag2UseParam0PCode = new XmlCookElement();
        private static readonly XmlCookElement Flag2UseParam1PCode = new XmlCookElement();
        private static readonly XmlCookElement Flag2UseParam2PCode = new XmlCookElement();
        private static readonly XmlCookElement Flag2UseParam3PCode = new XmlCookElement();
        private static readonly XmlCookElement Flag2UseUltimateOwner = new XmlCookElement();
        private static readonly XmlCookElement Flag2ChargePowerAndCooldown = new XmlCookElement();
        private static readonly XmlCookElement Flag2MarkSkillAsSuccessful = new XmlCookElement();
        private static readonly XmlCookElement Flag2LaserIncludeInUI = new XmlCookElement();
        private static readonly XmlCookElement fTime = new XmlCookElement();
        private static readonly XmlCookElement fRandChance = new XmlCookElement();
        private static readonly XmlCookElement tParam0 = new XmlCookElement();
        private static readonly XmlCookElement tParam1 = new XmlCookElement();
        private static readonly XmlCookElement tParam2 = new XmlCookElement();
        private static readonly XmlCookElement tParam3 = new XmlCookElement();

        //tAttachmentDef
        private static readonly XmlCookElement eType = new XmlCookElement();
        private static readonly XmlCookElement dwFlags = new XmlCookElement();
        private static readonly XmlCookElement pszAttached = new XmlCookElement();
        private static readonly XmlCookElement nVolume = new XmlCookElement();
        private static readonly XmlCookElement nAttachedDefId = new XmlCookElement();
        private static readonly XmlCookElement pszBone = new XmlCookElement();
        private static readonly XmlCookElement nBoneId = new XmlCookElement();
        private static readonly XmlCookElement vPositionX = new XmlCookElement();
        private static readonly XmlCookElement vPositionY = new XmlCookElement();
        private static readonly XmlCookElement vPositionZ = new XmlCookElement();
        private static readonly XmlCookElement vNormalX = new XmlCookElement();
        private static readonly XmlCookElement vNormalY = new XmlCookElement();
        private static readonly XmlCookElement vNormalZ = new XmlCookElement();
        private static readonly XmlCookElement Condition = new XmlCookElement();

        public SkillEvent()
        {
            RootElement = "SKILL_EVENT";

            Elements.Add(nType);

            FlagLaserTurns.Name = "SKILL_EVENT_FLAG_LASER_TURNS";
            FlagLaserTurns.ElementType = ElementType.Flag;
            FlagLaserTurns.DefaultValue = false;
            FlagLaserTurns.FlagId = 1;
            Elements.Add(FlagLaserTurns);

            FlagRequiresTarget.Name = "SKILL_EVENT_FLAG_REQUIRES_TARGET";
            FlagRequiresTarget.ElementType = ElementType.Flag;
            FlagLaserTurns.DefaultValue = false;
            FlagLaserTurns.FlagId = 1;
            Elements.Add(FlagLaserTurns);

            FlagForceNew.Name = "SKILL_EVENT_FLAG_FORCE_NEW";
            FlagForceNew.ElementType = ElementType.Flag;
            FlagForceNew.DefaultValue = false;
            FlagForceNew.FlagId = 1;
            Elements.Add(FlagForceNew);

            FlagLaserSeeksSurfaces.Name = "SKILL_EVENT_FLAG_LASER_SEEKS_SURFACES";
            FlagLaserSeeksSurfaces.ElementType = ElementType.Flag;
            FlagLaserSeeksSurfaces.DefaultValue = false;
            FlagLaserSeeksSurfaces.FlagId = 1;
            Elements.Add(FlagLaserSeeksSurfaces);

            FlagFaceTarget.Name = "SKILL_EVENT_FLAG_FACE_TARGET";
            FlagFaceTarget.ElementType = ElementType.Flag;
            FlagFaceTarget.DefaultValue = false;
            FlagFaceTarget.FlagId = 1;
            Elements.Add(FlagFaceTarget);

            FlagUseUnitTarget.Name = "SKILL_EVENT_FLAG_USE_UNIT_TARGET";
            FlagUseUnitTarget.ElementType = ElementType.Flag;
            FlagUseUnitTarget.DefaultValue = false;
            FlagUseUnitTarget.FlagId = 1;
            Elements.Add(FlagUseUnitTarget);

            FlagUseEventOffset.Name = "SKILL_EVENT_FLAG_USE_EVENT_OFFSET";
            FlagUseEventOffset.ElementType = ElementType.Flag;
            FlagUseEventOffset.DefaultValue = false;
            FlagUseEventOffset.FlagId = 1;
            Elements.Add(FlagUseEventOffset);

            FlagLoop.Name = "SKILL_EVENT_FLAG_LOOP";
            FlagLoop.ElementType = ElementType.Flag;
            FlagLoop.DefaultValue = false;
            FlagLoop.FlagId = 1;
            Elements.Add(FlagLoop);

            FlagUseEventOffsetAbsolute.Name = "SKILL_EVENT_FLAG_USE_EVENT_OFFSET_ABSOLUTE";
            FlagUseEventOffsetAbsolute.ElementType = ElementType.Flag;
            FlagUseEventOffsetAbsolute.DefaultValue = false;
            FlagUseEventOffsetAbsolute.FlagId = 1;
            Elements.Add(FlagUseEventOffsetAbsolute);

            FlagPlaceOnTarget.Name = "SKILL_EVENT_FLAG_PLACE_ON_TARGET";
            FlagPlaceOnTarget.ElementType = ElementType.Flag;
            FlagPlaceOnTarget.DefaultValue = false;
            FlagPlaceOnTarget.FlagId = 1;
            Elements.Add(FlagPlaceOnTarget);

            FlagUseAnimContactPoint.Name = "SKILL_EVENT_FLAG_USE_ANIM_CONTACT_POINT";
            FlagUseAnimContactPoint.ElementType = ElementType.Flag;
            FlagUseAnimContactPoint.DefaultValue = false;
            FlagUseAnimContactPoint.FlagId = 1;
            Elements.Add(FlagUseAnimContactPoint);

            FlagTransferStats.Name = "SKILL_EVENT_FLAG_TRANSFER_STATS";
            FlagTransferStats.ElementType = ElementType.Flag;
            FlagTransferStats.DefaultValue = false;
            FlagTransferStats.FlagId = 1;
            Elements.Add(FlagTransferStats);

            FlagDoWhenTargetInRange.Name = "SKILL_EVENT_FLAG_DO_WHEN_TARGET_IN_RANGE";
            FlagDoWhenTargetInRange.ElementType = ElementType.Flag;
            FlagDoWhenTargetInRange.DefaultValue = false;
            FlagDoWhenTargetInRange.FlagId = 1;
            Elements.Add(FlagDoWhenTargetInRange);

            FlagAddToCenter.Name = "SKILL_EVENT_FLAG_ADD_TO_CENTER";
            FlagAddToCenter.ElementType = ElementType.Flag;
            FlagAddToCenter.DefaultValue = false;
            FlagAddToCenter.FlagId = 1;
            Elements.Add(FlagAddToCenter);

            Flag360Targeting.Name = "SKILL_EVENT_FLAG_360_TARGETING";
            Flag360Targeting.ElementType = ElementType.Flag;
            Flag360Targeting.DefaultValue = false;
            Flag360Targeting.FlagId = 1;
            Elements.Add(Flag360Targeting);

            FlagUseSkillTargetLocation.Name = "SKILL_EVENT_FLAG_USE_SKILL_TARGET_LOCATION";
            FlagUseSkillTargetLocation.ElementType = ElementType.Flag;
            FlagUseSkillTargetLocation.DefaultValue = false;
            FlagUseSkillTargetLocation.FlagId = 1;
            Elements.Add(FlagUseSkillTargetLocation);

            FlagUseAITarget.Name = "SKILL_EVENT_FLAG_USE_AI_TARGET";
            FlagUseAITarget.ElementType = ElementType.Flag;
            FlagUseAITarget.DefaultValue = false;
            FlagUseAITarget.FlagId = 1;
            Elements.Add(FlagUseAITarget);

            FlagUseOffhandWeapon.Name = "SKILL_EVENT_FLAG_USE_OFFHAND_WEAPON";
            FlagUseOffhandWeapon.ElementType = ElementType.Flag;
            FlagUseOffhandWeapon.DefaultValue = false;
            FlagUseOffhandWeapon.FlagId = 1;
            Elements.Add(FlagUseOffhandWeapon);

            FlagFloat.Name = "SKILL_EVENT_FLAG_FLOAT";
            FlagFloat.ElementType = ElementType.Flag;
            FlagFloat.DefaultValue = false;
            FlagFloat.FlagId = 1;
            Elements.Add(FlagFloat);

            FlagDontValidateTarget.Name = "SKILL_EVENT_FLAG_DONT_VALIDATE_TARGET";
            FlagDontValidateTarget.ElementType = ElementType.Flag;
            FlagDontValidateTarget.DefaultValue = false;
            FlagDontValidateTarget.FlagId = 1;
            Elements.Add(FlagDontValidateTarget);

            FlagRandomFiringDirection.Name = "SKILL_EVENT_FLAG_RANDOM_FIRING_DIRECTION";
            FlagRandomFiringDirection.ElementType = ElementType.Flag;
            FlagRandomFiringDirection.DefaultValue = false;
            FlagRandomFiringDirection.FlagId = 1;
            Elements.Add(FlagRandomFiringDirection);

            FlagProjectile.Name = "SKILL_EVENT_FLAG_AUTOAIM_PROJECTILE";
            FlagProjectile.ElementType = ElementType.Flag;
            FlagProjectile.DefaultValue = false;
            FlagProjectile.FlagId = 1;
            Elements.Add(FlagProjectile);

            FlagWeapon.Name = "SKILL_EVENT_FLAG_TARGET_WEAPON";
            FlagWeapon.ElementType = ElementType.Flag;
            FlagWeapon.DefaultValue = false;
            FlagWeapon.FlagId = 1;
            Elements.Add(FlagWeapon);

            FlagUseWeaponForCondition.Name = "SKILL_EVENT_FLAG_USE_WEAPON_FOR_CONDITION";
            FlagUseWeaponForCondition.ElementType = ElementType.Flag;
            FlagUseWeaponForCondition.DefaultValue = false;
            FlagUseWeaponForCondition.FlagId = 1;
            Elements.Add(FlagUseWeaponForCondition);

            FlagForceConditionOnEvent.Name = "SKILL_EVENT_FLAG_FORCE_CONDITION_ON_EVENT";
            FlagForceConditionOnEvent.ElementType = ElementType.Flag;
            FlagForceConditionOnEvent.DefaultValue = false;
            FlagForceConditionOnEvent.FlagId = 1;
            Elements.Add(FlagForceConditionOnEvent);

            FlagUseHolyRadiusForRange.Name = "SKILL_EVENT_FLAG_USE_HOLY_RADIUS_FOR_RANGE";
            FlagUseHolyRadiusForRange.ElementType = ElementType.Flag;
            FlagUseHolyRadiusForRange.DefaultValue = false;
            FlagUseHolyRadiusForRange.FlagId = 1;
            Elements.Add(FlagUseHolyRadiusForRange);

            FlagUseChancePCode.Name = "SKILL_EVENT_FLAG_USE_CHANCE_PCODE";
            FlagUseChancePCode.ElementType = ElementType.Flag;
            FlagUseChancePCode.DefaultValue = false;
            FlagUseChancePCode.FlagId = 1;
            Elements.Add(FlagUseChancePCode);

            FlagServerOnly.Name = "SKILL_EVENT_FLAG_SERVER_ONLY";
            FlagServerOnly.ElementType = ElementType.Flag;
            FlagServerOnly.DefaultValue = false;
            FlagServerOnly.FlagId = 1;
            Elements.Add(FlagServerOnly);

            FlagClientOnly.Name = "SKILL_EVENT_FLAG_CLIENT_ONLY";
            FlagClientOnly.ElementType = ElementType.Flag;
            FlagClientOnly.DefaultValue = false;
            FlagClientOnly.FlagId = 1;
            Elements.Add(FlagClientOnly);

            FlagLaserAttacksLocation.Name = "SKILL_EVENT_FLAG_LASER_ATTACKS_LOCATION";
            FlagLaserAttacksLocation.ElementType = ElementType.Flag;
            FlagLaserAttacksLocation.DefaultValue = false;
            FlagLaserAttacksLocation.FlagId = 1;
            Elements.Add(FlagLaserAttacksLocation);

            FlagAtNextCooldown.Name = "SKILL_EVENT_FLAG_AT_NEXT_COOLDOWN";
            FlagAtNextCooldown.ElementType = ElementType.Flag;
            FlagAtNextCooldown.DefaultValue = false;
            FlagAtNextCooldown.FlagId = 1;
            Elements.Add(FlagAtNextCooldown);

            FlagAimWithWeapon.Name = "SKILL_EVENT_FLAG_AIM_WITH_WEAPON";
            FlagAimWithWeapon.ElementType = ElementType.Flag;
            FlagAimWithWeapon.DefaultValue = false;
            FlagAimWithWeapon.FlagId = 1;
            Elements.Add(FlagAimWithWeapon);

            Flag2AimWithWeaponZero.Name = "SKILL_EVENT_FLAG2_AIM_WITH_WEAPON_ZERO";
            Flag2AimWithWeaponZero.ElementType = ElementType.Flag;
            Flag2AimWithWeaponZero.DefaultValue = false;
            Flag2AimWithWeaponZero.FlagId = 2;
            Elements.Add(Flag2AimWithWeaponZero);

            Flag2UseParam0PCode.Name = "SKILL_EVENT_FLAG2_USE_PARAM0_PCODE";
            Flag2UseParam0PCode.ElementType = ElementType.Flag;
            Flag2UseParam0PCode.DefaultValue = false;
            Flag2UseParam0PCode.FlagId = 2;
            Elements.Add(Flag2UseParam0PCode);

            Flag2UseParam1PCode.Name = "SKILL_EVENT_FLAG2_USE_PARAM1_PCODE";
            Flag2UseParam1PCode.ElementType = ElementType.Flag;
            Flag2UseParam1PCode.DefaultValue = false;
            Flag2UseParam1PCode.FlagId = 2;
            Elements.Add(Flag2UseParam1PCode);

            Flag2UseParam2PCode.Name = "SKILL_EVENT_FLAG2_USE_PARAM2_PCODE";
            Flag2UseParam2PCode.ElementType = ElementType.Flag;
            Flag2UseParam2PCode.DefaultValue = false;
            Flag2UseParam2PCode.FlagId = 2;
            Elements.Add(Flag2UseParam2PCode);

            Flag2UseParam3PCode.Name = "SKILL_EVENT_FLAG2_USE_PARAM3_PCODE";
            Flag2UseParam3PCode.ElementType = ElementType.Flag;
            Flag2UseParam3PCode.DefaultValue = false;
            Flag2UseParam3PCode.FlagId = 2;
            Elements.Add(Flag2UseParam3PCode);

            Flag2UseUltimateOwner.Name = "SKILL_EVENT_FLAG2_USE_ULTIMATE_OWNER";
            Flag2UseUltimateOwner.ElementType = ElementType.Flag;
            Flag2UseUltimateOwner.DefaultValue = false;
            Flag2UseUltimateOwner.FlagId = 2;
            Elements.Add(Flag2UseUltimateOwner);

            Flag2ChargePowerAndCooldown.Name = "SKILL_EVENT_FLAG2_CHARGE_POWER_AND_COOLDOWN";
            Flag2ChargePowerAndCooldown.ElementType = ElementType.Flag;
            Flag2ChargePowerAndCooldown.DefaultValue = false;
            Flag2ChargePowerAndCooldown.FlagId = 2;
            Elements.Add(Flag2ChargePowerAndCooldown);

            Flag2MarkSkillAsSuccessful.Name = "SKILL_EVENT_FLAG2_MARK_SKILL_AS_SUCCESSFUL";
            Flag2MarkSkillAsSuccessful.ElementType = ElementType.Flag;
            Flag2MarkSkillAsSuccessful.DefaultValue = false;
            Flag2MarkSkillAsSuccessful.FlagId = 2;
            Elements.Add(Flag2MarkSkillAsSuccessful);

            Flag2LaserIncludeInUI.Name = "SKILL_EVENT_FLAG2_LASER_INCLUDE_IN_UI";
            Flag2LaserIncludeInUI.ElementType = ElementType.Flag;
            Flag2LaserIncludeInUI.DefaultValue = false;
            Flag2LaserIncludeInUI.FlagId = 2;
            Elements.Add(Flag2LaserIncludeInUI);

            fTime.Name = "fTime";
            fTime.DefaultValue = 1.0f;
            fTime.ElementType = ElementType.Float;
            Elements.Add(fTime);

            fRandChance.Name = "fRandChance";
            fRandChance.DefaultValue = 1.0f;
            fRandChance.ElementType = ElementType.Float;
            Elements.Add(fRandChance);

            tParam0.Name = "tParam0.flValue"; // note: should be "tParam[0].fValue"
            tParam0.DefaultValue = 0.0f;
            tParam0.ElementType = ElementType.Float;
            Elements.Add(tParam0);

            tParam1.Name = "tParam1.flValue"; // note: should be "tParam[1].fValue"
            tParam1.DefaultValue = 0.0f;
            tParam1.ElementType = ElementType.Float;
            Elements.Add(tParam1);

            tParam2.Name = "tParam2.flValue"; // note: should be "tParam[2].fValue"
            tParam2.DefaultValue = 0.0f;
            tParam2.ElementType = ElementType.Float;
            Elements.Add(tParam2);

            tParam3.Name = "tParam3.flValue"; // note: should be "tParam[3].fValue"
            tParam3.DefaultValue = 0.0f;
            tParam3.ElementType = ElementType.Float;
            Elements.Add(tParam3);

            // tAttachmentDef
            eType.Name = "tAttachmentDef.eType";
            eType.DefaultValue = 0;
            eType.ElementType = ElementType.Int32;
            Elements.Add(eType);

            dwFlags.Name = "tAttachmentDef.dwFlags";
            dwFlags.DefaultValue = 0;
            dwFlags.ElementType = ElementType.Int32;
            Elements.Add(dwFlags);

            pszAttached.Name = "tAttachmentDef.pszAttached";
            pszAttached.DefaultValue = null;
            pszAttached.ElementType = ElementType.String;
            Elements.Add(pszAttached);

            nVolume.Name = "tAttachmentDef.nVolume";
            nVolume.DefaultValue = 1;
            nVolume.ElementType = ElementType.Int32;
            Elements.Add(nVolume);

            nAttachedDefId.Name = "tAttachmentDef.nAttachedDefId";
            nAttachedDefId.DefaultValue = -1;
            nAttachedDefId.ElementType = ElementType.NonCookedInt32;
            Elements.Add(nAttachedDefId);

            pszBone.Name = "tAttachmentDef.pszBone";
            pszBone.DefaultValue = null;
            pszBone.ElementType = ElementType.String;
            Elements.Add(pszBone);

            nBoneId.Name = "tAttachmentDef.nBoneId";
            nBoneId.DefaultValue = -1;
            nBoneId.ElementType = ElementType.NonCookedInt32;
            Elements.Add(nBoneId);

            vPositionX.Name = "tAttachmentDef.vPosition.fX";
            vPositionX.DefaultValue = 0.0f;
            vPositionX.ElementType = ElementType.Float;
            Elements.Add(vPositionX);

            vPositionY.Name = "tAttachmentDef.vPosition.fY";
            vPositionY.DefaultValue = 0.0f;
            vPositionY.ElementType = ElementType.Float;
            Elements.Add(vPositionY);

            vPositionZ.Name = "tAttachmentDef.vPosition.fZ";
            vPositionZ.DefaultValue = 0.0f;
            vPositionZ.ElementType = ElementType.Float;
            Elements.Add(vPositionZ);

            vNormalX.Name = "tAttachmentDef.vNormal.fX";
            vNormalX.DefaultValue = -1.0f;
            vNormalX.ElementType = ElementType.Float;
            Elements.Add(vNormalX);

            vNormalY.Name = "tAttachmentDef.vNormal.fY";
            vNormalY.DefaultValue = 0.0f;
            vNormalY.ElementType = ElementType.Float;
            Elements.Add(vNormalY);

            vNormalZ.Name = "tAttachmentDef.vNormal.fZ";
            vNormalZ.DefaultValue = 0.0f;
            vNormalZ.ElementType = ElementType.Float;
            Elements.Add(vNormalZ);

            Condition.Name = "tCondition";
            Condition.DefaultValue = null;
            Condition.ElementType = ElementType.Table;
            Condition.ChildType = typeof(ConditionDefinition);
            Elements.Add(Condition);
        }
    }
}