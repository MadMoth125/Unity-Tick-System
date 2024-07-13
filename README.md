# Unity Tick System Package

Streamline your game's time-based logic with ease!

The Unity Tick System Package provides a robust framework for managing interval-based events, perfect for idle games, real-time strategy games, and more.

## Key Features
- **Flexible:** Create and configure as many separate `TickGroup` instances as needed.
- **Inspector-Friendly:** Use `ScriptableObject`-based tick groups (`TickGroupAsset`) for easy editing.
- **Real-Time and Game-Time:** Decide if you want individual `TickGroup` intervals affected by `Time.timeScale`.
- **Automatic:** `TickGroup` instances are updated automatically through the `TickManager`.

# Example

Instantiate a new `TickGroup` instance.
```csharp
TickGroup myTickGroup = new TickGroup(name: "Tick Group", interval: 0.1f, enabled: true, realTime: true);
```

Or reference a `TickGroupAsset` instance.
```csharp
[SerializeField]
TickGroupAsset myTickGroupAsset;
```

Handle adding/removing callbacks to `TickGroup` and `TickGroupAsset` instances.
```csharp
// Syntax is the same for TickGroups and TickGroupAssets

// Add callback(s)
void OnEnable()
{
    myTickGroup.Add(MyTickCallback);
    // or
    myTickGroupAsset.Add(MyTickCallback);
}

// Remove callback(s)
void OnDisable()
{
    myTickGroup.Remove(MyTickCallback);
    // or
    myTickGroupAsset.Remove(MyTickCallback);
}

// Defining a tick callback
void MyTickCallback()
{
    // Your periodic update logic here
    Debug.Log("Tick!");
}
```

Or you can use `event` syntax for callbacks.
```csharp
// Syntax is the same for TickGroups and TickGroupAssets

// Add callback(s)
void OnEnable()
{
    myTickGroup.OnTick += MyTickCallback;
    // or
    myTickGroupAsset.OnTick += MyTickCallback;
}

// Remove callback(s)
void OnDisable()
{
    myTickGroup.OnTick -= MyTickCallback;
    // or
    myTickGroupAsset.OnTick -= MyTickCallback;
}

// Defining a tick callback
void MyTickCallback()
{
    // Your periodic update logic here
    Debug.Log("Tick!");
}
```

When using a `TickGroup`, you should call `Dispose()` once the tick group reference should be discarded.

*(This isn't necessary for `TickGroupAsset` instances, as they handle `Dispose()` internally.)*
```csharp
void OnDestroy()
{
    // Call Dispose() when you are done managing it.
    // Otherwise, it will tick continuously.
    myTickGroup.Dispose();
}
```

# Tick Groups

## Constructors

Several constructors are provided to assist with different use cases:
```csharp
public TickGroup(string name, float interval, bool? enabled = null, bool? realTime = null) { ... };

public TickGroup(GroupParams parameters) { ... };

public TickGroup(GroupParams parameters, IEnumerable<Action> callbacks) { ... };

public TickGroup(GroupParams parameters, params Action[] callbacks) { ... };

public TickGroup(IEnumerable<Action> callbacks) { ... };

public TickGroup(params Action[] callbacks) { ... };
```

**Note:**
- `GroupParams` is a struct containing the same `name`, `interval`, `enabled`, and `realTime` variables as in `TickGroup`.

# Tick Manager

You can find any existing `TickGroups` by name through the `TickManager` class:
```csharp
// Finds the first TickGroup with the matching name.
public static TickGroup Find(string name)

// Finds the first TickGroup with the matching name.
public static bool Find(string name, out TickGroup tickGroup)
```

## Getting Started
1. **Import the Package:** Download and import the package.
2. **Set Up Tick Groups:** Create and configure tick groups using either `TickGroup` or `TickGroupAsset` instances.
3. **Add Callbacks:** Register update logic with tick groups.
4. **Run Your Game:** Enjoy smooth, efficient tick-based updates.