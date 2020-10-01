using System;
using System.Collections;
using System.Collections.Generic;
using DAS_Unity_Framework.ExtensionMethods;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SASLInvertCalculator : MonoBehaviour
{
    public float angleFudgeFactor = 1;
    
    public Text shoulderTorqueText;
    public Text hipsTorqueText;
    public LabelledSlider shoulderAngle;
    public LabelledSlider hipAngle;
    public Text variablesText;


    //mass percent values from http://robslink.com/SAS/democd79/body_part_weights.htm


    private float totalMass = 80;
    
    [SerializeField] float headMassPercent = 8.26f;
    [SerializeField] float headLengthMeters = .4f;
    [SerializeField] float headCenterOfGravity = .55f;
    
    [SerializeField] float armsMassPercent = 11.4f;
    [SerializeField] float armsLengthMeters = .66f;
    [SerializeField] float armsCenterOfGravity = .4f;
    
    [SerializeField] float torsoMassPercent = 46.84f;
    [SerializeField] float torsoLengthMeters = .46f;
    [SerializeField] float torsoCenterOfGravity = .4f;
    
    [SerializeField] float legsMassPercent = 33.4f;
    [SerializeField] float legsLengthMeters = .9f;
    [SerializeField] float legsCenterOfGravity = .4f;



    public float ArmsMassKg => armsMassPercent * totalMass / 100;
    public float HeadMassKg => headMassPercent * totalMass / 100;
    public float TorsoMassKg => torsoMassPercent * totalMass / 100;
    public float LegsMassKg => legsMassPercent * totalMass / 100;
    public float CombinedMass => totalMass;





    public float armsElevationAngle => Mathf.Asin(-LocalCombinedCG.x / armsLengthMeters) * Mathf.Rad2Deg;

    [HideInInspector] public float shoulderToTorsoAngleDegrees; //D
    [HideInInspector] public float torsoToLegsAngleDegrees; //B
    
    public float A => 180 - shoulderToTorsoAngleDegrees;// - armsElevationAngle;
    public float C => 180 - (90 - A) - torsoToLegsAngleDegrees;


    public float x1 => torsoLengthMeters * Mathf.Sin(A * Mathf.Deg2Rad);
    public float y1 => torsoLengthMeters * Mathf.Cos(A * Mathf.Deg2Rad);
    public float x2 => legsLengthMeters * Mathf.Cos(C * Mathf.Deg2Rad);
    public float y2 => legsLengthMeters * Mathf.Sin(C * Mathf.Deg2Rad);
    public Vector2 HandsPosition => new Vector2(-armsLengthMeters * Mathf.Sin(armsElevationAngle * Mathf.Deg2Rad),
                                                    armsLengthMeters * Mathf.Cos(armsElevationAngle * Mathf.Deg2Rad)).Rotate((- armsElevationAngle * angleFudgeFactor));
    
    
    private Vector2 LocalHeadCG => new Vector2(headLengthMeters * Mathf.Sin(-A * Mathf.Deg2Rad) * headCenterOfGravity,
                                        headLengthMeters * Mathf.Cos(-A * Mathf.Deg2Rad) * headCenterOfGravity);
    private Vector2 LocalArmsCG => new Vector2(0, armsLengthMeters * armsCenterOfGravity);
    private Vector2 LocalLegsCG => new Vector2(x1 + x2 * legsCenterOfGravity, -y1 + y2 *legsCenterOfGravity);
    private Vector2 LocalTorsoCG => new Vector2(x1 * torsoCenterOfGravity, -y1 * torsoCenterOfGravity);
    
    private Vector2 LocalCombinedCG =>
        (LocalLegsCG * LegsMassKg + 
         LocalTorsoCG * TorsoMassKg + 
         LocalHeadCG * HeadMassKg ) / (totalMass - ArmsMassKg);


    public Vector2 ArmsCG => LocalArmsCG.RotateAround(HandsPosition, armsElevationAngle * angleFudgeFactor);
    public Vector2 HeadCG => LocalHeadCG.RotateAround(HandsPosition, armsElevationAngle * angleFudgeFactor);
    public Vector2 TorsoCG => LocalTorsoCG.RotateAround(HandsPosition, armsElevationAngle * angleFudgeFactor);
    public Vector2 LegsCG => LocalLegsCG.RotateAround(HandsPosition, armsElevationAngle * angleFudgeFactor);
    public Vector2 CombinedCG =>  (LegsCG * LegsMassKg + 
                                   TorsoCG * TorsoMassKg + 
                                   HeadCG * HeadMassKg +
                                   ArmsCG * ArmsMassKg) / totalMass;

    public float TorqueOnHips => (LegsCG.x - x1) * LegsMassKg;
    public float TorqueOnShoulders => LocalCombinedCG.x * CombinedMass;
    
    


    private void Awake()
    {
        shoulderAngle.Init(0,180,180);
        hipAngle.Init(20, 180, 180);
    }

    private void Update()
    {
        shoulderTorqueText.text = "Torque on shoulders:\n" + TorqueOnShoulders.RoundToNearest(.01f) + " KN*M";
        hipsTorqueText.text = "Torque on hips:\n" + TorqueOnHips.RoundToNearest(.01f) + " KN*M";
        
        shoulderToTorsoAngleDegrees = shoulderAngle.slider.value;
        torsoToLegsAngleDegrees = hipAngle.slider.value;

        variablesText.text = "x1: " + x1.RoundToNearest(.1f) + "\ny1: " + y1.RoundToNearest(.1f) + 
                             "\nx2: " + x2.RoundToNearest(.1f) + "\ny2: " + y2.RoundToNearest(.1f);
    }
    
}
