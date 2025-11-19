using UnityEngine;

[CreateAssetMenu(fileName = "NewTurretBluePrint", menuName = "TD/Turret BluePrint")]
public class TurretBluePrint : ScriptableObject
{
    public GameObject prefab;
    public int cost;
    public int sellPrice;
    public int level;
}