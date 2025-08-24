using Hmk.Engine.Core;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Resources;

public class GameObjectTemplate<T>(string filePath) : Resource where T : GameObject, new()
{
  readonly string objectTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Objects", $"{filePath}.xobj");

  public T Instantiate()
  {
    var newObject = GameObjectSerializerExtensions.LoadFromXml(objectTemplatePath);
    return newObject as T ?? throw new InvalidOperationException($"Resource at '{objectTemplatePath}' is not a {typeof(T).Name}.");
  }
}