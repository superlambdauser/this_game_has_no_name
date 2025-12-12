using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New figure", menuName = "Figure/Enemy")]
public class EnemyData : FigureData
{
    [SerializeField] private int movementRange;
    public int MovementRange => movementRange;
    [SerializeField] private int attackRange;
    public int AttackRange => movementRange;

    protected override void CustomInstructions()
    {
        
    }
}
