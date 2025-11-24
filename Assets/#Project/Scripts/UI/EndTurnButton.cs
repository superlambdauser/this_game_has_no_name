using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
    public void OnClick()
    {
        Debug.Log("End Turn Button clicked");
        EnemyTurnGA enemyTurnGA = new();
        ActionSystem.Instance.Perform(enemyTurnGA);
    }
}
