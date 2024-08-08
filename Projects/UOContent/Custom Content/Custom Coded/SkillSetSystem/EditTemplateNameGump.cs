using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Custom;

public class EditTemplateNameGump : Gump
{
    private PlayerMobile m_Player;
    private int m_TemplateIndex;

    public EditTemplateNameGump(PlayerMobile player, int templateIndex) : base(50, 50)
    {
        m_Player = player;
        m_TemplateIndex = templateIndex;

        AddPage(0);

        AddBackground(0, 0, 300, 150, 5054);
        AddLabel(100, 20, 1153, "Edit Template Name");

        AddLabel(10, 50, 1153, "New Name:");
        AddTextEntry(100, 50, 180, 20, 0, 0, player.GetSkillTemplates()[templateIndex].Name);

        AddButton(50, 100, 4005, 4007, 1, GumpButtonType.Reply, 0);
        AddLabel(85, 100, 1153, "Save");

        AddButton(150, 100, 4005, 4007, 0, GumpButtonType.Reply, 0);
        AddLabel(185, 100, 1153, "Cancel");
    }

    public override void OnResponse(NetState sender, in RelayInfo info)
    {
        PlayerMobile from = sender.Mobile as PlayerMobile;

        if (from == null)
            return;

        if (info.ButtonID == 1) // Save button
        {
            string newName = info.GetTextEntry(0);
            if (newName == null)
                newName = string.Empty;

            from.SetTemplateName(m_TemplateIndex, newName);
            from.SendMessage($"Template {m_TemplateIndex + 1} renamed to {newName}.");
        }

        from.SendGump(new SkillManagementGump(from));
    }
}
