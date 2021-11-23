using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Definitions/Game Action/Aim")]
public class AimAction : GameActionBlock
{
    public override void Invoke(float abilityStat, List<object> currentFilteredTargets, Dictionary<string, object> otherTargets)
    {
        Unit aimingUnit = (Unit) currentFilteredTargets[0];
        Vector3 aimingTarget = (Vector3) otherTargets["Target Point"];

        aimingUnit.movementSpeed = 0f;
        aimingUnit.turnRate = abilityStat;
    }
}
