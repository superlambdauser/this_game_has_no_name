using UnityEngine;

public class PlayerView : Singleton<PlayerView>
{
    [SerializeField] PlayerData data; // To init later

    protected override void Awake() // Protected because Singleton<> inheritance
    {
        if (data == null) // make sure the prefab gets the data
        {
            Debug.LogError("Player data is missing !", this);
            return;
        }
    }
}
