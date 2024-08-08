using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Custom;

public class CopySkillsGump : Gump
{
    private const int SkillsPerPage = 10;
    private PlayerMobile m_Player;
    private int m_SourceIndex;
    private int m_Page;

    public CopySkillsGump(PlayerMobile player, int sourceIndex, int page = 0) : base(50, 50)
    {
        m_Player = player;
        m_SourceIndex = sourceIndex;
        m_Page = page;

        AddPage(0);

        AddBackground(0, 0, 350, 500, 5054);
        AddLabel(125, 20, 1153, "Copy Skills");

        AddLabel(10, 50, 1153, "Select skills to copy:");
        var templates = m_Player.GetSkillTemplates();
        var sourceTemplate = templates[m_SourceIndex];
        var skills = sourceTemplate.Skills;
        var skillNames = new List<SkillName>(skills.Keys);

        int start = m_Page * SkillsPerPage;
        int end = Math.Min(start + SkillsPerPage, skillNames.Count);

        for (int i = start; i < end; i++)
        {
            var skillName = skillNames[i];
            var skillValue = skills[skillName];
            if (skillValue > 0)
            {
                AddCheck(10, 75 + ((i - start) * 25), 210, 211, false, i);
                AddLabel(45, 75 + ((i - start) * 25), 1153, $"{skillName} ({skillValue:F1})");
            }
        }

        if (m_Page > 0)
        {
            AddButton(10, 475, 4014, 4015, 2, GumpButtonType.Reply, 0); // Previous page
            AddLabel(45, 475, 1153, "Previous");
        }

        if (end < skillNames.Count)
        {
            AddButton(150, 475, 4005, 4006, 3, GumpButtonType.Reply, 0); // Next page
            AddLabel(185, 475, 1153, "Next");
        }

        AddButton(250, 475, 4005, 4007, 1, GumpButtonType.Reply, 0); // Copy
        AddLabel(285, 475, 1153, "Copy");
    }

    public override void OnResponse(NetState sender, in RelayInfo info)
    {
        PlayerMobile from = sender.Mobile as PlayerMobile;

        if (from == null)
            return;

        if (info.ButtonID == 1) // Copy button
        {
            var skillNames = new List<SkillName>();
            foreach (var check in info.Switches)
            {
                if (check < SkillsPerPage)
                {
                    skillNames.Add((SkillName)check + m_Page * SkillsPerPage);
                }
            }

            var templates = from.GetSkillTemplates();
            var sourceTemplate = templates[m_SourceIndex];

            // Calculate the total skill points that will be applied
            double totalSkillPoints = from.Skills.Total;

            foreach (var skillName in skillNames)
            {
                if (sourceTemplate.Skills.ContainsKey(skillName))
                {
                    totalSkillPoints += sourceTemplate.Skills[skillName] - from.Skills[skillName].Base;
                }
            }

            if (totalSkillPoints > 720.0)
            {
                from.SendMessage("Total skill points in the template cannot exceed 720.");
                return;
            }

            // Apply the selected skills to the player's current skills
            foreach (var skillName in skillNames)
            {
                if (sourceTemplate.Skills.ContainsKey(skillName))
                {
                    from.Skills[skillName].Base = sourceTemplate.Skills[skillName];
                }
            }

            from.SendMessage("Skills copied to the current template.");
            from.SendGump(new SkillManagementGump(from));
        }
        else if (info.ButtonID == 2) // Previous page
        {
            from.SendGump(new CopySkillsGump(m_Player, m_SourceIndex, m_Page - 1));
        }
        else if (info.ButtonID == 3) // Next page
        {
            from.SendGump(new CopySkillsGump(m_Player, m_SourceIndex, m_Page + 1));
        }
    }
}
