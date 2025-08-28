## Game Object Architecture

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
    +Components: Dictionary<Type, Component>
    +AddChild(obj: GameObject) void
    +AddComponent(c: Component) void
    +GetComponent<T>() Component
}

class Collider{
    +Offset: Vector2
    +Size: Vector2
}

class Component {
    <<abstract>>
}

GameObject  o-- GameObject : children
GameObject  "1" o-- "1" Collider
GameObject "1" *-- "*" Component : components


Component <|-- IsSomething
Component <|-- HasSomething
Component <|-- CanDoSomething
```

## Attack System

```mermaid
classDiagram
direction LR

class Hurtbox {
    +Health: Health
    +Zone: TriggerZone
}

class IDamageCalculator {
    <<interface>>
    +CalculateDamage() int
}

class NoDamage {
    +CalculateDamage() int
}

class SimpleDamage {
    +Amount: int
    +CriticalChance: float
    +CriticalMultiplier: float
    +CalculateDamage() int
}



class Hitbox {
    +DamageCalculator: IDamageCalculator
}

Resource ()-- Health
Hitbox ..> IDamageCalculator : delegates
GameObject ()-- Hitbox

Hurtbox o-- "1" Health
Hitbox ..> Hurtbox : OnEnter -> Health.TakeDamage()


GameObject ()-- Hurtbox


class Health {
    +Max: int
    +Current: int
    +IsDead: bool
    +OnDead: Signal
    +HealthChanged: Signal
    +TakeDamage(amount: int) void
    +Heal(amount: int) void
}

IDamageCalculator <|.. NoDamage
IDamageCalculator <|.. SimpleDamage

Resource ()-- SimpleDamage
```

## Interaction System

```mermaid
---
config:
  layout: elk
  class:
    hideEmptyMembersBox: true
---
classDiagram
direction TB
    class Interaction {
	    +CanPerformInteraction(actor: GameObject) bool
	    +Interact(actor: GameObject) void
    }
    class ConcreteInteraction {
	    +CanPerformInteraction(actor: GameObject) bool
	    +Interact(actor: GameObject) void
    }
    class HasInteraction {
	    +Interactions: List
	    +HandleInteractinos(actor: GameObject) void
    }
    class GameObject {
	    +Traits: List
    }
    class Trait {
    }
    class CollisionManager {
	    +TriggerInteraction(actor: GameObject, direction: Vector2)
    }
    class Resource {
    }

	<<abstract>> Interaction
	<<abstract>> Trait
	<<static>> CollisionManager

    Interaction <|-- ConcreteInteraction
    Trait <|-- HasInteraction
    HasInteraction *-- "0..*" Interaction : delegates
    GameObject *-- "0..*" Trait
    CollisionManager -- HasInteraction : onInteraction
    Resource <|-- Interaction

```

## Inventory System

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
    +OnItemAdded: Signal<Item>
    +OnItemRemoved: Signal<Item>
    +AddItem(item: Item): void
    +RemoveItem(item: Item): void
}

class CollectableItem {
    +Item: Item
    +AutoCollectionAllowed: bool
}

IUsable <|.. UseConcreteItem
Item ..> IUsable : delegates
Inventory *-- "n" Item
CollectableItem *-- "1" Item
Component <|-- Inventory
GameObject <|-- CollectableItem
```
