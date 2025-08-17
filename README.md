<div align="center">
	<img src="https://www.hamaka.dev/images/hamaka.svg" alt="hamaka logo" width="100" />

  <h1 align="center">Hmk Engine</h1>
  <p><strong>Hmk Engine</strong> is a 2D Game Engine created with C# and Raylib</p>
</div>

# Architecture Design

### Game Object Architecture

```mermaid
classDiagram
direction LR

class GameObject {
    +Name: string
    +Position: Vector2
    +Parent?: GameObject
    +Collider?: Collider
    +Initialize()
    +Update(float dt)
    +Draw()
    +Terminate()

    +Children: List<GameObject>
    +Traits: List<Trait>
    +AddChild(obj: GameObject) void
    +AddTrait(t: Trait) void
    +Trait<T>() Trait
}

class Collider{
    +Offset: Vector2
    +Size: Vector2
}

class Trait {
    <<abstract>>
}

GameObject  o-- GameObject : children
GameObject  "1" o-- "1" Collider
GameObject "1" *-- "*" Trait : traits


Trait <|-- IsSomething
Trait <|-- HasSomething
Trait <|-- CanDoSomething
```

### Inventory System

```mermaid
classDiagram
direction LR

class IUsable  {
    <<interface>>
    +use() void
}

class UseConcreteItem {
    +use() void
}

class Item {
    -behavior: IUsable
    +use() void
}


class Inventory {
    +Items: List<Item>
}

class HasInventory {
    +Inventory: Inventory
}

class CollectableItem

class CanBeCollected{
+Item: Item
}



IUsable <|.. UseConcreteItem
Item ..> IUsable : delegates
HasInventory o-- "1" Inventory
Inventory *-- "n" Item

CanBeCollected *-- "1" Item
CanBeCollected *-- "1" CollectableItem
Trait ()-- HasInventory
Trait ()-- CanBeCollected
GameObject ()-- CollectableItem
```
