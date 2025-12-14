using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    private GameplayEngine instance;

    private void Update()
    {
        instance?.UpdateSystems(Time.deltaTime); // Update() if gameplayEngine != null
    }


    public void Initialize(GameplayEngine gameplayEngine)
    {
        instance = gameplayEngine;
    }
}
