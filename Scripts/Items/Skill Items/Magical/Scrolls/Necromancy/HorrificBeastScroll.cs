namespace Server.Items
{
  public class HorrificBeastScroll : SpellScroll
  {
    [Constructible]
    public HorrificBeastScroll() : this(1)
    {
    }

    [Constructible]
    public HorrificBeastScroll(int amount) : base(105, 0x2265, amount)
    {
    }

    public HorrificBeastScroll(Serial serial) : base(serial)
    {
    }

    public override void Serialize(GenericWriter writer)
    {
      base.Serialize(writer);

      writer.Write(0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
      base.Deserialize(reader);

      int version = reader.ReadInt();
    }
  }
}