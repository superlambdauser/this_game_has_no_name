using UnityEngine;

public class PlayerBehaviour : HealthSystem
{
    private PlayerBehaviour instance;
    public PlayerBehaviour Instance
    {
        get
        {
            return instance;
        }
    }

}
