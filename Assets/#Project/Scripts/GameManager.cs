using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    private GameplayEngine gameplayEngine;

    private void Update()
    {
        gameplayEngine?.Update(Time.deltaTime); // Update() if gameplayEngine != null
    }


    public void Initiate(GameplayEngine gameplayEngine)
    {
        this.gameplayEngine = gameplayEngine;
    }
}
