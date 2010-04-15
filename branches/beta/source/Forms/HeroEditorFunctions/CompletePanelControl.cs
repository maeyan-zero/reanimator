using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reanimator.Forms.HeroEditorFunctions
{
    public partial class CompletePanelControl : UserControl
    {
        CharacterValuesControl _values;
        CharacterSkillsControl _skills;
        List<SkillControls> _controls;

        public CompletePanelControl()
        {
            InitializeComponent();

            _values = new CharacterValuesControl();
            _skills = new CharacterSkillsControl();
            _controls = new List<SkillControls>();
        }

        public void Initialize(ref DataTable skillTable, int characterClassId, Unit heroUnit)
        {
            try
            {
                CreateCharacterSkillControlsAndBmps skillImage = new CreateCharacterSkillControlsAndBmps();
                Bitmap skillBackground = skillImage.GetSkillPanelBmp(ref skillTable, characterClassId);
                _controls = skillImage.SkillControls;

                p_attributePanel.Controls.Add(_values);
                p_skillPanel.Controls.Add(_skills);

                _skills.SetBackground(skillBackground);

                foreach (SkillControls control in _controls)
                {
                    _skills.Controls.Add(control.AddButton);
                    _skills.Controls.Add(control.SubsButton);
                    _skills.Controls.Add(control.Label);
                }

                BindCharacterValuesToAttributeControls(heroUnit);
                BindCharacterValuesToSkillControls(heroUnit);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BindCharacterValuesToSkillControls(Unit heroUnit)
        {
            Unit.StatBlock.Stat skills = heroUnit.statBlock.GetStatByName("skill_level");

            foreach (SkillControls control in _controls)
            {
                foreach (Unit.StatBlock.Stat.Values value in skills.values)
                {
                    if (control._id == value.Attribute1)
                    {
                        control.CurrentLevel = value.Stat;
                    }
                }
            }

            skills = heroUnit.statBlock.GetStatByName("skill_points");

            if (skills != null)
            {
                _skills.nud_availableSkillPoints.DataBindings.Add("Text", skills.values[0], "Stat");
            }
        }

        public void GetSkillControls(Unit heroUnit)
        {
            Unit.StatBlock.Stat skills = heroUnit.statBlock.GetStatByName("skill_level");

            foreach (SkillControls control in _controls)
            {
                foreach (Unit.StatBlock.Stat.Values value in skills.values)
                {
                    if (control._id == value.Attribute1)
                    {
                        value.Stat = control.CurrentLevel;
                    }
                }
            }
        }

        const int LEVELOFFSET = 0;//8;
        const int SFXOFFSET = 0;//100;
        const int FACTIONOFFSET = 0;//900;

        private void BindCharacterValuesToAttributeControls(Unit heroUnit)
        {
            try
            {
                #region Main Controls
                this.tb_charakterName.DataBindings.Add("Text", heroUnit, "Name");

                //Binding levelBinding = AddFormatedBinding("level", heroUnit);
                //levelBinding.Format += new ConvertEventHandler(levelBinding_Format);
                //levelBinding.Parse += new ConvertEventHandler(levelBinding_Parse);
                //nud_level.DataBindings.Add(levelBinding);
                AddBinding(nud_level, "level", heroUnit, LEVELOFFSET);
                this.l_characterClass.Text = "Unknown";
                AddBinding(nud_currentExp, "experience", heroUnit, 0);
                AddBinding(nud_nextExp, "experience_next", heroUnit, 0);
                #endregion

                #region Attribute Controls
                AddBinding(_values.CAttributes.nud_attributePoints, "stat_points", heroUnit, 0);
                AddBinding(_values.CAttributes.nud_accuracy, "accuracy", heroUnit, 0);
                AddBinding(_values.CAttributes.nud_stamina, "stamina", heroUnit, 0);
                AddBinding(_values.CAttributes.nud_strength, "strength", heroUnit, 0);
                AddBinding(_values.CAttributes.nud_willpower, "willpower", heroUnit, 0);


                AddBinding(_values.CStatistics.nud_health, "hp_cur", heroUnit, 0);
                _values.CStatistics.nud_healthRegen.Enabled = false;
                //AddBinding(_values.CStatistics.nud_healthRegen, "health_regen", heroUnit, 0);
                AddBinding(_values.CStatistics.nud_power, "power_cur", heroUnit, 0);
                AddBinding(_values.CStatistics.nud_powerRecharge, "power_regen", heroUnit, 0);

                _values.CStatistics.nud_movementSpeed.Enabled = false;
                //AddBinding(_values.CStatistics.nud_movementSpeed, "health_regen", heroUnit, 0);
                AddBinding(_values.CStatistics.nud_criticalChanceLeft, "critical_chance", heroUnit, 0);
                AddBinding(_values.CStatistics.nud_criticalChanceRight, "critical_chance", heroUnit, 0);
                _values.CStatistics.nud_criticalDamageLeft.Enabled = false;
                //AddBinding(_values.CStatistics.nud_criticalDamageLeft, "health_regen", heroUnit, 0);
                _values.CStatistics.nud_criticalDamageRight.Enabled = false;
                //AddBinding(_values.CStatistics.nud_criticalDamageRight, "health_regen", heroUnit, 0);


                _values.CDefense.nud_armor.Enabled = false;
                //AddBinding(_values.CDefense.nud_armor, "armor", heroUnit, 0);
                AddBinding(_values.CDefense.nud_shields, "shield_buffer_cur", heroUnit, 0);
                AddBinding(_values.CDefense.nud_shieldRecharge, "shield_buffer_regen", heroUnit, 0);

                //Binding sfxBinding = AddFormatedBinding("sfx_defense_bonus", heroUnit);
                //sfxBinding.Format += new ConvertEventHandler(sfxBinding_Format);
                //sfxBinding.Parse += new ConvertEventHandler(sfxBinding_Parse);

                AddBinding(_values.CDefense.nud_ignite, "sfx_defense_bonus", heroUnit, SFXOFFSET);
                AddBinding(_values.CDefense.nud_phase, "sfx_defense_bonus", heroUnit, 0);
                AddBinding(_values.CDefense.nud_poison, "sfx_defense_bonus", heroUnit, 0);
                AddBinding(_values.CDefense.nud_shock, "sfx_defense_bonus", heroUnit, 0);
                AddBinding(_values.CDefense.nud_stun, "sfx_defense_bonus", heroUnit, 0);

                //Binding factionBinding;
                //factionBinding = AddFormatedBinding("faction_score", 16961, heroUnit);
                //factionBinding.Format += new ConvertEventHandler(factionBinding_Format);
                //factionBinding.Parse += new ConvertEventHandler(factionBinding_Parse);
                //_values.CFactions.nud_templarFaction.DataBindings.Add(factionBinding);

                //factionBinding = AddFormatedBinding("faction_score", 17473, heroUnit);
                //factionBinding.Format += new ConvertEventHandler(factionBinding_Format);
                //factionBinding.Parse += new ConvertEventHandler(factionBinding_Parse);
                //_values.CFactions.nud_thePeopleOfHolbornStation.DataBindings.Add(factionBinding);

                //factionBinding = AddFormatedBinding("faction_score", 17729, heroUnit);
                //factionBinding.Format += new ConvertEventHandler(factionBinding_Format);
                //factionBinding.Parse += new ConvertEventHandler(factionBinding_Parse);
                //_values.CFactions.nud_thePeopleOfCoventGardenStation.DataBindings.Add(factionBinding);

                //factionBinding = AddFormatedBinding("faction_score", 17985, heroUnit);
                //factionBinding.Format += new ConvertEventHandler(factionBinding_Format);
                //factionBinding.Parse += new ConvertEventHandler(factionBinding_Parse);
                //_values.CFactions.nud_thePeopleOfCharingCrossStation.DataBindings.Add(factionBinding);

                //factionBinding = AddFormatedBinding("faction_score", 18241, heroUnit);
                //factionBinding.Format += new ConvertEventHandler(factionBinding_Format);
                //factionBinding.Parse += new ConvertEventHandler(factionBinding_Parse);
                //_values.CFactions.nud_thePeopleOfTempleStation.DataBindings.Add(factionBinding);

                //factionBinding = AddFormatedBinding("faction_score", 18497, heroUnit);
                //factionBinding.Format += new ConvertEventHandler(factionBinding_Format);
                //factionBinding.Parse += new ConvertEventHandler(factionBinding_Parse);
                //_values.CFactions.nud_thePeopleOfTemplarBase.DataBindings.Add(factionBinding);

                //factionBinding = AddFormatedBinding("faction_score", 18753, heroUnit);
                //factionBinding.Format += new ConvertEventHandler(factionBinding_Format);
                //factionBinding.Parse += new ConvertEventHandler(factionBinding_Parse);
                //_values.CFactions.nud_thePeopleOfMonumentStation.DataBindings.Add(factionBinding);

                //factionBinding = AddFormatedBinding("faction_score", 19009, heroUnit);
                //factionBinding.Format += new ConvertEventHandler(factionBinding_Format);
                //factionBinding.Parse += new ConvertEventHandler(factionBinding_Parse);
                //_values.CFactions.nud_thePeopleOfLiverpoolStation.DataBindings.Add(factionBinding);

                //factionBinding = AddFormatedBinding("faction_score", 19265, heroUnit);
                //factionBinding.Format += new ConvertEventHandler(factionBinding_Format);
                //factionBinding.Parse += new ConvertEventHandler(factionBinding_Parse);
                //_values.CFactions.nud_Broker.DataBindings.Add(factionBinding);

                //factionBinding = AddFormatedBinding("faction_score", 19521, heroUnit);
                //factionBinding.Format += new ConvertEventHandler(factionBinding_Format);
                //factionBinding.Parse += new ConvertEventHandler(factionBinding_Parse);
                //_values.CFactions.nud_thePeopleOfStPaulsStation.DataBindings.Add(factionBinding);

                //factionBinding = AddFormatedBinding("faction_score", 19777, heroUnit);
                //factionBinding.Format += new ConvertEventHandler(factionBinding_Format);
                //factionBinding.Parse += new ConvertEventHandler(factionBinding_Parse);
                //_values.CFactions.nud_factionWithTheBrothers.DataBindings.Add(factionBinding);

                AddBinding(_values.CFactions.nud_templarFaction, "faction_score", 16961, heroUnit, 900);
                AddBinding(_values.CFactions.nud_thePeopleOfHolbornStation, "faction_score", 17473, heroUnit, 900);
                AddBinding(_values.CFactions.nud_thePeopleOfCoventGardenStation, "faction_score", 17729, heroUnit, 900);
                AddBinding(_values.CFactions.nud_thePeopleOfCharingCrossStation, "faction_score", 17985, heroUnit, 900);
                AddBinding(_values.CFactions.nud_thePeopleOfTempleStation, "faction_score", 18241, heroUnit, 900);
                AddBinding(_values.CFactions.nud_thePeopleOfTemplarBase, "faction_score", 18497, heroUnit, 900);
                AddBinding(_values.CFactions.nud_thePeopleOfMonumentStation, "faction_score", 18753, heroUnit, 900);
                AddBinding(_values.CFactions.nud_thePeopleOfLiverpoolStation, "faction_score", 19009, heroUnit, 900);
                AddBinding(_values.CFactions.nud_Broker, "faction_score", 19265, heroUnit, 900);
                AddBinding(_values.CFactions.nud_thePeopleOfStPaulsStation, "faction_score", 19521, heroUnit, 900);
                AddBinding(_values.CFactions.nud_factionWithTheBrothers, "faction_score", 19777, heroUnit, 900);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //void factionBinding_Parse(object sender, ConvertEventArgs e)
        //{
        //    e.Value = Int32.Parse(e.Value.ToString()) + FACTIONOFFSET;
        //}

        //void factionBinding_Format(object sender, ConvertEventArgs e)
        //{
        //    e.Value = Int32.Parse(e.Value.ToString()) - FACTIONOFFSET;
        //}

        //void sfxBinding_Parse(object sender, ConvertEventArgs e)
        //{
        //    e.Value = Int32.Parse(e.Value.ToString()) + SFXOFFSET;
        //}

        //void sfxBinding_Format(object sender, ConvertEventArgs e)
        //{
        //    e.Value = Int32.Parse(e.Value.ToString()) - SFXOFFSET;
        //}

        //void levelBinding_Parse(object sender, ConvertEventArgs e)
        //{
        //    e.Value = Int32.Parse(e.Value.ToString()) + LEVELOFFSET;
        //}

        //void levelBinding_Format(object sender, ConvertEventArgs e)
        //{
        //    e.Value = Int32.Parse(e.Value.ToString()) - LEVELOFFSET;
        //}

        //private Binding AddFormatedBinding(string name, Unit heroUnit)
        //{
        //    Unit.StatBlock.Stat.Values[] tmp = heroUnit.Stats.GetStatByName(name).values;

        //    if (tmp != null)
        //    {
        //        return new Binding("Text", tmp[0], "Stat", true, DataSourceUpdateMode.OnPropertyChanged);
        //    }

        //    return null;
        //}

        //private Binding AddFormatedBinding(string name, int attributeId, Unit heroUnit)
        //{
        //    Unit.StatBlock.Stat.Values tmp = heroUnit.Stats.GetStatByName(name).GetAttributeByAttributeId(attributeId);

        //    if (tmp != null)
        //    {
        //        return new Binding("Text", tmp, "Stat", true, DataSourceUpdateMode.OnPropertyChanged);
        //    }

        //    return null;
        //}

        private void AddBinding(Control control, string name, Unit heroUnit, int offset)
        {
            Unit.StatBlock.Stat.Values[] tmp = heroUnit.Stats.GetStatByName(name).values;

            if (tmp != null)
            {
                tmp[0].Stat -= offset;
                control.DataBindings.Add("Text", tmp[0], "Stat", true, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        private void AddBinding(Control control, string name, int attributeId, Unit heroUnit, int offset)
        {
            Unit.StatBlock.Stat.Values tmp = heroUnit.Stats.GetStatByName(name).GetAttributeByAttributeId(attributeId);

            if (tmp != null)
            {
                tmp.Stat -= offset;
                control.DataBindings.Add("Text", tmp, "Stat", true, DataSourceUpdateMode.OnPropertyChanged);
            }
        }
    }
}
