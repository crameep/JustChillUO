namespace Server.Items
{
  public class BatWing : BaseReagent, ICommodity
  {
    [Constructible]
    public BatWing() : this(1)
    {
    }

    [Constructible]
    public BatWing(int amount) : base(0xF78, amount)
    {
    }

    public BatWing(Serial serial) : base(serial)
    {
    }

    int ICommodity.DescriptionNumber => LabelNumber;
    bool ICommodity.IsDeedable => true;


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