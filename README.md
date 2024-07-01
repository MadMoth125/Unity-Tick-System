# Unity Tick System Package

Streamline your game's time-based logic with ease!

The Unity Tick System Package provides a robust framework for managing interval-based events, perfect for idle games, real-time strategy games, and more.

## Key Features
- **Flexible Tick Groups:** Create and configure tick groups with `TickGroup`.
- **Inspector-Friendly:** Use `ScriptableObject`-based tick groups (`TickGroupAsset`) for easy editing.
- **Real-Time and Game-Time:** Decide if you want the tick rate affected by `Time.timeScale`.
- **Automatic:** `TickGroup` instances are called automatically through the `TickManager`.

# Examples

Using a TickGroup
```csharp
// Create a new tick group instance with 'name', 'interval', 'enabled', and 'realTime' parameters
TickGroup myTickGroup = new TickGroup("Tick Group", 0.1f, true, true);
myTickGroup.Add(MyTickCallback);

// Defining a tick callback
void MyTickCallback()
{
    // Your periodic update logic here
    Debug.Log("Tick!");
}
````

Using a TickGroupAsset
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
````

## Getting Started
1. **Import the Package:** Download and import the package.
2. **Set Up Tick Groups:** Create and configure tick groups using either `TickGroup` or `TickGroupAsset` instances.
3. **Add Callbacks:** Register update logic with tick groups.
4. **Run Your Game:** Enjoy smooth, efficient tick-based updates.
