using System.Collections.Generic;

public class GameplayEngine
{
    private List<ISystem> systems = new List<ISystem>();
    private GridData gridData;


    public GameplayEngine(GridData gridData)
    {
        this.gridData = gridData;
    }

    public void RegisterSystem(ISystem system)
    {
        systems.Add(system);
    }

    public void Update(float dt)
    {
        foreach (ISystem system in systems)
        {
            system.Process(this, dt); // By passing the engine itself, all systems can access shared data (like GridData) -> Bridge
        }
    }
}
