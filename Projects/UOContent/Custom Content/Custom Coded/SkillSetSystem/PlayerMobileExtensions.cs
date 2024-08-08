using System.Collections.Generic;
using Server.Mobiles;
using Server.Custom;

namespace Server.Custom
{
    public static class PlayerMobileExtensions
    {
        public static List<SkillTemplate> GetSkillTemplates(this PlayerMobile player)
        {
            return player.SkillTemplates;
        }

        public static int GetMaxTemplates(this PlayerMobile player)
        {
            return player.MaxTemplates;
        }

        public static void SaveCurrentSkillsToTemplate(this PlayerMobile player, int index)
        {
            if (index < 0 || index >= player.MaxTemplates) return;

            if (player.SkillTemplates.Count <= index)
            {
                player.SkillTemplates.Add(new SkillTemplate());
            }

            player.SkillTemplates[index].SaveSkills(player);
        }

        public static void LoadSkillsFromTemplate(this PlayerMobile player, int index)
        {
            if (index < 0 || index >= player.SkillTemplates.Count) return;

            foreach (Skill skill in player.Skills)
            {
                skill.Base = 0; // Reset current skills to 0
            }

            player.SkillTemplates[index].LoadSkills(player);
        }

        public static void CopySkillsFrom(this SkillTemplate targetTemplate, SkillTemplate sourceTemplate, List<SkillName> skillNames)
        {
            foreach (var skillName in skillNames)
            {
                if (sourceTemplate.Skills.ContainsKey(skillName))
                {
                    targetTemplate.Skills[skillName] = sourceTemplate.Skills[skillName];
                }
            }

            // Do not copy stats here to avoid overwriting current stats
        }

        public static void SetTemplateName(this PlayerMobile player, int index, string name)
        {
            if (index < 0 || index >= player.SkillTemplates.Count) return;
            player.SkillTemplates[index].Name = name;
        }

        public static void CreateNewTemplate(this PlayerMobile player, string name)
        {
            if (player.SkillTemplates.Count < player.MaxTemplates)
            {
                var template = new SkillTemplate { Name = name };
                player.SkillTemplates.Add(template);
            }
        }
    }
}
