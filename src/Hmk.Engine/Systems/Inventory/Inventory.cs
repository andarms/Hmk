using Hmk.Engine.Core;
using Hmk.Engine.Resources;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Systems.Inventory;

public class Inventory : Resource
{
  [Save]
  public List<Item> Items { get; set; } = [];

  public Signal<Item> OnItemAdded { get; } = new Signal<Item>();
  public Signal<Item> OnItemRemoved { get; } = new Signal<Item>();

  public void AddItem(Item item)
  {
    Console.WriteLine($"Item added: {item.Name}");
    Items.Add(item);
    OnItemAdded.Emit(item);
  }

  public void RemoveItem(Item item)
  {
    Items.Remove(item);
    OnItemRemoved.Emit(item);
  }

  public IReadOnlyList<Item> GetItems()
  {
    return Items.AsReadOnly();
  }
}
