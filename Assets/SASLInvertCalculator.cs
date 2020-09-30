using System;
using System.Collections;
using System.Collections.Generic;
using DAS_Unity_Framework.ExtensionMethods;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SASLInvertCalculator : MonoBehaviour
{
    public Text shoulderTorqueText;
    public Text hipsTorqueText;
    public LabelledSlider shoulderAngle;
    public LabelledSlider hipAngle;
    
    [HideInInspector] public float shoulderToTorsoAngleDegrees;
    [HideInInspector] public float torsoToLegsAngleDegrees;


    public float headAndArmsMassKg = 18;
    public float headAndArmsLengthMeters = .6f;
    public float headAndArmsCenterOfGravity = .3f;

    public float torsoMassKg = 42;
    public float torsoLengthMeters = .6f;
    public float torsoCenterOfGravity = .4f;
 
    public float legsMassKg = 40;
    public float legsLengthMeters = .9f;
    public float legsCenterOfGravity = .4f;


    private void Awake()
    {
        shoulderAngle.Init(0,180,180);
        hipAngle.Init(20, 180, 180);
    }

    private void Update()
    {
        shoulderTorqueText.text = "Torque on shoulders:\n" + GetTorqueOnShoulder().RoundToNearest(.01f) + " KN*M";
        hipsTorqueText.text = "Torque on hips:\n" + GetTorqueOnHips().RoundToNearest(.01f) + " KN*M";
        
        shoulderToTorsoAngleDegrees = shoulderAngle.slider.value;
        torsoToLegsAngleDegrees = hipAngle.slider.value;
    }

    private float GetTorqueOnShoulder()
    {
        return GetTorqueOnShoulderFromLegs() + GetTorqueOnShoulderFromTorso();
    }

    private float GetTorqueOnHips()
    {
        float hipsToToesX = GetDistanceHipsToToesInX();
        return legsMassKg * legsCenterOfGravity * hipsToToesX;
    }
    
    
    
    private float GetTorqueOnShoulderFromTorso()
    {
        float shouldersToHipsX = GetDistanceShouldersToHipsInX();
        return torsoMassKg * torsoCenterOfGravity * shouldersToHipsX;
    }

    private float GetDistanceShouldersToHipsInX()
    {
        // x is the distance in the x-axis from the shoulders to hips
        // l is distance from shoulders to hips (magnitude)
        // Θ is the elevation angle of the shoulders
        // Θ = 180 - shoulder angle
        // x = l * sin(Θ)

        float l = torsoLengthMeters;
        float theta = 180 - shoulderToTorsoAngleDegrees;


        float retVal = l * Mathf.Sin(theta * Mathf.Deg2Rad);
        return retVal;
    }

    private float GetDistanceHipsToToesInX()
    {
        // Θ is the elevation angle of the shoulders
        // Θ = 180 - shoulder angle
        
        // B = hips angle
        // alpha = 180 - B - (90 - Θ)  
        
        // l is the distance from hips to toes (magnitude)
        // x is the horizontal (x-axis) distance from the hips to the toes
        // x = l * cos(a)

        float theta = 180 - shoulderToTorsoAngleDegrees;
        float B = torsoToLegsAngleDegrees;
        float alpha = 180 - B - (90 - theta);

        float l = legsLengthMeters;

        return l * Mathf.Cos(alpha * Mathf.Deg2Rad);
    }



    private float GetTorqueOnShoulderFromLegs()
    {
        float hipsToToesX = GetDistanceHipsToToesInX();
        return legsMassKg * legsCenterOfGravity * hipsToToesX;
    }

    private float GetArmsAngleFromVertical()
    {
        return 0;
    }

}
