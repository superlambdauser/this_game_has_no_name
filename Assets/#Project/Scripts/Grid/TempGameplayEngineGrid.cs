using System.Collections.Generic;

public class GameplayEngine : Singleton<GameplayEngine>
{
    private List<ISystem> systems = new List<ISystem>();


    public GameplayEngine(GridData gridData)
    {
    }

    public void RegisterSystem(ISystem system)
    {
        systems.Add(system);
    }

    public void UpdateSystems(float dt)
    {
        foreach (ISystem system in systems)
        {
            system.Process(this, dt); // By passing the engine itself, all systems can access shared data (like GridData) -> Bridge
        }
    }
}
