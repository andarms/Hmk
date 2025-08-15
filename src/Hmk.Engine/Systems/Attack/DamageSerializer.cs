using System;
using System.Xml.Linq;
using Hmk.Engine.Systems.Attack;

namespace Hamaze.Engine.Components.Attack;

public static class DamageSerializer
{

  public static XElement Serialize(IDamageCalculator damageCalculator, string elementName)
  {
    var element = new XElement(elementName);
    if (damageCalculator is NoDamage)
    {
      element.SetAttributeValue("Type", "NoDamage");
      return element;
    }

    if (damageCalculator is SimpleDamage simpleDamage)
    {
      element.SetAttributeValue("Type", "SimpleDamage");
      element.Add(new XElement("Amount", simpleDamage.Amount));
      return element;
    }

    throw new NotSupportedException("Unknown damage calculator type.");
  }

  // public static IDamageCalculator Deserialize(XElement element)
  // {
  //   var type = element.Attribute("Type")?.Value;
  //   if (type == "NoDamage") return new NoDamage();

  //   if (type == "SimpleDamage")
  //   {
  //     var amount = int.Parse(element.Element("Amount")?.Value ?? "0");
  //     return new SimpleDamage(amount);
  //   }

  //   throw new NotSupportedException("Unknown serialized damage calculator.");
  // }
}