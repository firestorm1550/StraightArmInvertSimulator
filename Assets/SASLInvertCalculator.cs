using System;
using System.Collections;
using System.Collections.Generic;
using DAS_Unity_Framework.ExtensionMethods;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SASLInvertCalculator : MonoBehaviour
{
    public Text output;
    
    public float shoulderToTorsoAngleDegrees;
    public float torsoToLegsAngleDegrees;

    public float torsoMassKg = 55f;
    public float torsoLengthMeters = .6f;
    public float torsoCenterOfGravity = .4f;
    
    public float legsMassKg = 35f;
    public float legsLengthMeters = .9f;
    public float legsCenterOfGravity = .4f;


    private void Update()
    {
        output.text = GetTorqueOnShoulder().RoundToNearest(.01f) + " KN*M";
    }


    public float GetTorqueOnShoulder()
    {
        return GetTorqueOnShoulderFromLegs() + GetTorqueOnShoulderFromTorso();
    }
    
    private float GetTorqueOnShoulderFromTorso()
    {
        float shouldersToHipsX = GetDistanceShouldersToHipsInX();
        return torsoMassKg * shouldersToHipsX * torsoCenterOfGravity;
    }

    private float GetDistanceShouldersToHipsInX()
    {
        float portionOfLengthInX = Mathf.Sin(shoulderToTorsoAngleDegrees * Mathf.Deg2Rad);
        return torsoLengthMeters * portionOfLengthInX;
    }
    
    private float GetTorqueOnShoulderFromLegs()
    {
        float xDistanceHipsToLegCoG =
            Mathf.Sin((torsoToLegsAngleDegrees - shoulderToTorsoAngleDegrees) * Mathf.Deg2Rad) * legsCenterOfGravity;
        float xDistanceShouldersToLegCoG = GetDistanceShouldersToHipsInX() + xDistanceHipsToLegCoG;
            
        return legsLengthMeters * xDistanceShouldersToLegCoG;
    }
}
