using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reanimator.Forms
{
    enum ItemQuality
    {
        Normal = 12336,
        NormalMod = 14384,

        Uncommon = 12592,

        Rare = 18480,
        RareMod = 18736,

        Legendary = 13104,
        LegendaryMod = 16944,

        Mutant = 13360,
        MutantMod = 17200,

        Unique = 13616,
        UniqueMod = 17456
    };

    enum ItemValueNames
    {
        level,
        gold,
        stat_points,
        skill_points,

        skill_right,
        skill_left,
        skill_level,
        skill_shift_enabled,

        holy_radius,
        level_def_start,
        level_def_return,

        save_quest_state_1,
        save_quest_state_2,
        save_quest_state_3,
        save_quest_state_4,
        save_quest_state_5,

        save_quest_status,
        save_quest_version,
        save_quest_data_version,

        save_task_version,
        save_task_count,

        faction_score,
        last_trigger,
        player_visited_level_bitfield,

        badge_reward_received,
        no_tutorial_tips,

        experience,
        experience_prev,
        experience_next,
        achievement_progress,

        quest_global_fix_flags,

        hp_cur,
        power_cur,

        accuracy,
        stamina,
        strength,
        willpower,

        accuracy_feed,
        stamina_feed,
        strength_feed,
        willpower_feed,

        applied_affix,
        attached_affix_hidden,

        armor,
        armor_buffer_max,
        armor_buffer_regen,

        critical_chance,
        critical_mult,

        base_dmg_max,
        base_dmg_min,
        damage_increment,
        damage_increment_field,
        damage_increment_radial,
        damage_max,
        damage_min,

        energy_decrease_source,
        energy_increase_source,
        energy_max,

        firing_error_max,
        firing_error_decrease_source_weapon,
        firing_error_increase_source_weapon,
        firing_error_max_weapon,

        hp_regen,

        identified,

        interrupt_attack,
        item_augmented_count,

        item_difficulty_spawned,
        item_level_limit,
        item_level_req,
        item_lookup_group,
        item_pickedup,

        item_quality,
        item_quantity,
        item_quantity_max,

        item_slots,
        item_spawner_level,
        item_upgraded_count,

        level_req,
        no_trade,

        offer_id,
        offweapon_melee_speed,
        pet_damage_bonus,

        power_cost_pct_skillgroup,
        quest_reward,
        reward_original_location,

        sfx_attack,
        sfx_attack_focus,
        sfx_defense_bonus,
        sfx_duration_in_ticks,
        sfx_strength_pct,

        shield_buffer_cur,
        shield_buffer_max,
        shield_buffer_regen,
        shield_overload_pct,
        shield_penetration_dir,
        shields,

        splash_increment,
        unlimited_in_merchant_inventory,

        inventory_width,
        inventory_height,

        power_max,

        achievement_points_total,
        achievement_points_cur,
        played_time_in_seconds,
        waypoint_flags,
        minigame_category_needed
    };
}
