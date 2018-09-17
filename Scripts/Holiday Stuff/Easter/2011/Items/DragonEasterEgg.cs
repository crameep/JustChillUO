﻿namespace Server.Items
{
  public class DragonEasterEgg : Item, IDyable
  {
    [Constructible]
    public DragonEasterEgg()
      : base(0x47E6)
    {
    }

    public DragonEasterEgg(Serial serial)
      : base(serial)
    {
    }

    public override int LabelNumber => 1097278;

    public bool Dye(Mobile from, DyeTub sender)
    {
      if (Deleted || !sender.AllowDyables)
        return false;

      Hue = sender.DyedHue;

      return true;
    }

    public override void Serialize(GenericWriter writer)
    {
      base.Serialize(writer);

      writer.Write(0);
    }

    public override void Deserialize(GenericReader reader)
    {
      base.Deserialize(reader);

      int version = reader.ReadInt();
    }
  }
}