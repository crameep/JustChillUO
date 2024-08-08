using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom
{
    public class SkillTemplate
    {
        public string Name { get; set; }
        public Dictionary<SkillName, double> Skills { get; private set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }

        public SkillTemplate()
        {
            Name = "New Template";
            Skills = new Dictionary<SkillName, double>();
            Strength = 75; // Set default stats to 75
            Dexterity = 75;
            Intelligence = 75;
        }

        public void SaveSkills(PlayerMobile player)
        {
            Skills.Clear();
            foreach (Skill skill in player.Skills)
            {
                if (skill.Base > 0)
                {
                    Skills[skill.SkillName] = skill.Base;
                }
            }

            Strength = player.Str;
            Dexterity = player.Dex;
            Intelligence = player.Int;
        }

        public void LoadSkills(PlayerMobile player)
        {
            foreach (var skill in Skills)
            {
                player.Skills[skill.Key].Base = skill.Value;
            }

            // Do not load stats here to avoid overwriting current stats
        }

        public void CopySkillsFrom(SkillTemplate source, List<SkillName> skillNames)
        {
            foreach (var skillName in skillNames)
            {
                if (source.Skills.ContainsKey(skillName))
                {
                    Skills[skillName] = source.Skills[skillName];
                }
            }

            // Do not copy stats here to avoid overwriting current stats
        }

        public double GetTotalSkillPoints()
        {
            double total = 0;
            foreach (var skill in Skills.Values)
            {
                total += skill;
            }
            return total;
        }
    }
}
