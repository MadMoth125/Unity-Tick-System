# Unity Tick System Package

Streamline your game's time-based logic with ease!

The Unity Tick System Package provides a robust framework for managing interval-based events, perfect for idle games, real-time strategy games, and more.

## Key Features
- **Flexible Tick Groups:** Create and configure tick groups with `TickGroup`.
- **Inspector-Friendly:** Use `ScriptableObject`-based tick groups (`TickGroupAsset`) for easy editing.
- **Real-Time and Game-Time:** If you want the tick rate to be affected by `Time.timeScale`.

## Benefits
- **Improved Performance:** Logic can run periodically instead of *every* frame.
- **Ease of Use:** TickGroups are updated automatically so you can enjoy a smaller code footprint.
- **Flexibility:** Configure tick rates, active states, and time modes.

## Use Cases
- **Real-Time Strategy Games:** Manage unit updates and resource generation.
- **Tycoon Games:** Periodic updates for agents and environments.
- **Idle and Incremental Games:** Manage incremental updates and timed events.

## Getting Started
1. **Import the Package:** Download and import the package.
2. **Set Up Tick Groups:** Create and configure tick groups using either `TickGroup` or `TickGroupAsset` instances.
3. **Add Callbacks:** Register update logic with tick groups.
4. **Run Your Game:** Enjoy smooth, efficient tick-based updates.

# Examples

Using a TickGroupAsset
```csharp
// Assign and configure a tick group in the inspector
public TickGroupAsset myTickGroup;

// You can change the parameters
myTickGroup.TickRate = 30;
myTickGroup.UseRealTime = true;

myTickGroup.Add(MyTickCallback);

// Defining a tick callback
void MyTickCallback()
{
    // Your periodic update logic here
    Debug.Log("Tick!");
}
````

Using a TickGroup
```csharp
// Create a new tick group instance with 'name', 'interval', 'active', and 'useRealTime' parameters
TickGroup myTickGroup = new TickGroup("Tick Group", 0.1f, true, true);
myTickGroup.Add(MyTickCallback);

// Defining a tick callback
void MyTickCallback()
{
    // Your periodic update logic here
    Debug.Log("Tick!");
}
````
