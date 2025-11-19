using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretDatabase", menuName = "TD/Turret Database")]
public class TurretDatabase : ScriptableObject
{
    public List<TurretBluePrint> turretList; // 모든 터렛 블루프린트
}