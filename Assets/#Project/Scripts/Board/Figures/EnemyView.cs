using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    public EnemyData EnemyData => data;

    [SerializeField] private Sprite enemyImage;
}
