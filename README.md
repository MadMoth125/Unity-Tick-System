# Unity Tick System Package

Streamline your game's time-based logic with ease!

The Unity Tick System Package provides a robust framework for managing interval-based events, perfect for idle games, real-time strategy games, and more.

## Key Features
- **Flexible:** Create and configure as many separate `TickGroup` instances as needed.
- **Inspector-Friendly:** Use `ScriptableObject`-based tick groups (`TickGroupAsset`) for easy editing.
- **Real-Time and Game-Time:** Decide if you want individual `TickGroup` intervals affected by `Time.timeScale`.
- **Automatic:** `TickGroup` instances are updated automatically through the `TickManager`.

# Example

Use a `TickGroup` when you want a tick group local to a class:
```csharp
// Create a new tick group instance with 'name', 'interval', 'enabled', and 'realTime' parameters
TickGroup myTickGroup = new TickGroup(name: "Tick Group", interval: 0.1f, enabled: true, realTime: true);
myTickGroup.Add(MyTickCallback);

// Defining a tick callback
void MyTickCallback()
{
    // Your periodic update logic here
    Debug.Log("Tick!");
}

// Assuming this is a Monobehaviour class
void OnDestroy()
{
    // Call Dispose() when you are done managing it.
    // Otherwise, it will tick continuously.
    myTickGroup.Dispose();
}
````

Use a `TickGroupAsset` when you want a `TickGroup` as an asset reference:
```csharp
// Assign and configure a TickGroupAsset in the inspector
public TickGroupAsset myTickGroup;
myTickGroup.Add(MyTickCallback);

// Defining a tick callback
void MyTickCallback()
{
    // Your periodic update logic here
    Debug.Log("Tick!");
}

// Assuming this is a Monobehaviour class
void OnDestroy()
{
    // Dispose() is automatically called on TickGroupAssets, so no manual intervention is required here.
}
````

# Tick Groups

## Layout

`TickGroup` has a relatively simple structure:

```csharp
// Simplified look into TickGroup class
public class TickGroup : IDisposable
{
    public string Name; // Identifier for group

    public float Interval; // Expected time between each tick

    public bool Enabled; // Whether group can be invoked

    public bool IsRealTime; // Whether Interval is affected by Time.timeScale


    public void Add(Action callback) { ... } // Registers a callback

    public void Remove(Action callback) { ... } // Un-registers a callback

    public void Clear() { ... } // Un-registers all callbacks

    public void Invoke() { ... } // Invokes all registered callbacks

    public void Dispose() { ... } // Un-registers all callbacks and stops auto-ticking
}
```
Note:
- You shouldn't call `Invoke()` manually, as it is called automatically by the `TickManager`.
- You should call `Dispose()` when you are done with the `TickGroup` instance.
- You don't have to call `Dispose()` on `TickGroupAsset` instances.

## Constructors

Several constructors are provided to assist with different use cases:

```csharp
public TickGroup(string name, float interval, bool? enabled = null, bool? realTime = null)

public TickGroup(GroupParams parameters)

public TickGroup(GroupParams parameters, IEnumerable<Action> callbacks)

public TickGroup(GroupParams parameters, params Action[] callbacks)

public TickGroup(IEnumerable<Action> callbacks)

public TickGroup(params Action[] callbacks)
```
Note:
- `GroupParams` is a struct containing the same `name`, `interval`, `enabled`, and `realTime` variables as in `TickGroup`.

# Tick Manager

You can find any existing `TickGroups` by name through the `TickManager` class:
```csharp
// Finds the first TickGroup with the matching name.
// Returns the matching TickGroup, 'null' otherwise.
public static TickGroup Find(string name)

// Finds the first TickGroup with the matching name.
// Returns 'true' if the TickGroup was found, 'false' otherwise.
public static bool Find(string name, out TickGroup tickGroup)
```

## Getting Started
1. **Import the Package:** Download and import the package.
2. **Set Up Tick Groups:** Create and configure tick groups using either `TickGroup` or `TickGroupAsset` instances.
3. **Add Callbacks:** Register update logic with tick groups.
4. **Run Your Game:** Enjoy smooth, efficient tick-based updates.
