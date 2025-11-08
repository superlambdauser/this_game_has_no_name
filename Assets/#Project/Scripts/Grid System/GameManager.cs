using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    private GameplayEngine instance;

    private void Update()
    {
        instance?.Update(Time.deltaTime); // Update() if gameplayEngine != null
    }


    public void Initiate(GameplayEngine gameplayEngine)
    {
        instance = gameplayEngine;
    }
}
