using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Custom;

public class SkillManagementGump : Gump
{
    private PlayerMobile m_Player;

    public SkillManagementGump(PlayerMobile player) : base(50, 50)
    {
        m_Player = player;

        AddPage(0);

        AddBackground(0, 0, 350, 500, 5054);
        AddLabel(125, 20, 1153, "Template Manager");

        AddButton(10, 460, 4017, 4018, 1, GumpButtonType.Reply, 0);
        AddLabel(45, 460, 1153, "Close");

        AddButton(10, 430, 4005, 4007, 2, GumpButtonType.Reply, 0);
        AddLabel(45, 430, 1153, "Create New Template");

        var templates = m_Player.GetSkillTemplates();

        for (int i = 0; i < m_Player.GetMaxTemplates(); i++)
        {
            if (i < templates.Count)
            {
                string templateName = templates[i].Name;
                AddLabel(10, 60 + (i * 80), 1153, $"{templateName}");
                AddButton(250, 60 + (i * 80), 4005, 4007, 300 + i, GumpButtonType.Reply, 0); // Edit Name Button

                AddButton(10, 85 + (i * 80), 4005, 4007, 100 + i, GumpButtonType.Reply, 0);
                AddLabel(45, 85 + (i * 80), 1153, "Save");

                AddButton(200, 85 + (i * 80), 4005, 4007, 200 + i, GumpButtonType.Reply, 0);
                AddLabel(235, 85 + (i * 80), 1153, "Load");

                AddButton(275, 85 + (i * 80), 4005, 4007, 400 + i, GumpButtonType.Reply, 0);
                AddLabel(310, 85 + (i * 80), 1153, "Copy");
            }
            else
            {
                AddLabel(10, 60 + (i * 80), 1153, $"Empty Slot {i + 1}");
            }
        }
    }

    public override void OnResponse(NetState sender, in RelayInfo info)
    {
        PlayerMobile from = sender.Mobile as PlayerMobile;

        if (from == null)
            return;

        if (info.ButtonID == 1) // Close button
        {
            from.SendMessage("Closed template manager.");
            return;
        }

        if (info.ButtonID == 2) // Create New Template
        {
            from.CreateNewTemplate("New Template");
            from.SendGump(new SkillManagementGump(from));
            return;
        }

        if (info.ButtonID >= 100 && info.ButtonID < 200) // Save Template
        {
            int templateIndex = info.ButtonID - 100;
            if (templateIndex < from.SkillTemplates.Count)
            {
                from.SaveCurrentSkillsToTemplate(templateIndex);
                from.SendMessage($"Skills saved to template {templateIndex + 1}.");
                from.SendGump(new SkillManagementGump(from));
            }
        }
        else if (info.ButtonID >= 200 && info.ButtonID < 300) // Load Template
        {
            int templateIndex = info.ButtonID - 200;
            if (templateIndex < from.SkillTemplates.Count)
            {
                from.LoadSkillsFromTemplate(templateIndex);
                from.SendMessage($"Skills loaded from template {templateIndex + 1}.");
                from.SendGump(new SkillManagementGump(from));
            }
        }
        else if (info.ButtonID >= 300 && info.ButtonID < 400) // Edit Template Name
        {
            int templateIndex = info.ButtonID - 300;
            if (templateIndex < from.SkillTemplates.Count)
            {
                from.SendGump(new EditTemplateNameGump(from, templateIndex));
            }
        }
        else if (info.ButtonID >= 400 && info.ButtonID < 500) // Copy Skills
        {
            int templateIndex = info.ButtonID - 400;
            if (templateIndex < from.SkillTemplates.Count)
            {
                from.SendGump(new CopySkillsGump(from, templateIndex));
            }
        }
    }
}
