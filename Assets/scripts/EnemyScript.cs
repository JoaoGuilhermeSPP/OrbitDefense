using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy/NewEnemy")]
public class EnemyScript : ScriptableObject 
{
    public int Life;
    public int speed;
    public int Damage;
}
