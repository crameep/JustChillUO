using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using ModernUO.Serialization;

namespace Server.Items
{
    [Flippable(0x1F14, 0x1F15)] // Add appropriate item IDs if needed
    [SerializationGenerator(0)]
    public partial class SkillManagementStone : Item
    {
        [Constructible]
        public SkillManagementStone() : base(0x1F14) // Change to an appropriate item ID
        {
            Name = "Template Managment";
            Movable = true;
            Hue = 1153; // Set a unique hue if desired
        }

        public SkillManagementStone(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile pm)
            {
                pm.SendGump(new SkillManagementGump(pm));
            }
        }
    }
}
