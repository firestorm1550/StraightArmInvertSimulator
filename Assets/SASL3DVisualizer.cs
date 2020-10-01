using System;
using System.Collections;
using System.Collections.Generic;
using DAS_Unity_Framework.ExtensionMethods;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Animations;

public class SASL3DVisualizer : MonoBehaviour
{
    public float testRotation;
    public Vector2 testPosition;
    
    public SASLInvertCalculator data;
    public CoGMarker CoGMarkerPrefab;


    public Transform headJoint;
    public Transform neckJoint;
    
    public Transform rightShoulderJoint;
    public Transform leftShoulderJoint;

    public Transform rightHipJoint;
    public Transform leftHipJoint;

    public Transform rightHand;
    public Transform leftHand;
    
    
    
    private Vector3 HandAxis => (rightHand.position - leftHand.position).normalized;
    private Vector3 ShoulderAxis => (rightShoulderJoint.position - leftShoulderJoint.position).normalized;
    private Vector3 HipAxis => (rightHipJoint.position - leftHipJoint.position).normalized;


    private CoGMarker testMarker;
    private CoGMarker armsCoGMarker;
    private CoGMarker headCoGMarker;
    private CoGMarker torsoCoGMarker;
    private CoGMarker legsCoGMarker;
    private CoGMarker wholeBodyCoGMarker;
    private CoGMarker bodyCoGMarker;

    private Vector3 _leftHandStartPos;
    private Dictionary<Transform, Quaternion> startRotations;
    private Vector3 startForward;
    private Vector3 startUp;


    private void Start()
    {
        //PrepCoGMarkers();
        _leftHandStartPos = leftHand.transform.position;

        startRotations = new Dictionary<Transform, Quaternion>();
        startRotations.Add(rightShoulderJoint, rightShoulderJoint.localRotation);
        startRotations.Add(leftShoulderJoint, leftShoulderJoint.localRotation);
        startRotations.Add(rightHipJoint, rightHipJoint.localRotation);
        startRotations.Add(leftHipJoint, leftHipJoint.localRotation);

        startForward = transform.forward;
        startUp = transform.up;
        PrepCoGMarkers();
    }

    // Update is called once per frame
    void Update()
    {
        ApplySASLData();
        
        transform.rotation = Quaternion.identity;
        transform.RotateAround(transform.position, HandAxis, data.shoulderToTorsoAngleDegrees - 180 - data.armsElevationAngle);
        transform.position = _leftHandStartPos + (transform.position - leftHand.position);
        
        PlaceCoGMarkers();
    }

    private void ApplySASLData()
    {
        foreach (Transform joint in startRotations.Keys)
        {
            joint.localRotation = startRotations[joint];
        }
        
        rightShoulderJoint.RotateAround(rightShoulderJoint.position, ShoulderAxis, data.A);
        leftShoulderJoint.RotateAround(rightShoulderJoint.position, ShoulderAxis, data.A);
        
        float alpha = 180 - data.torsoToLegsAngleDegrees;
        rightHipJoint.RotateAround(rightHipJoint.position, HipAxis, alpha);
        leftHipJoint.RotateAround(rightHipJoint.position, HipAxis, alpha);
        
    }

    private void PlaceCoGMarkers()
    {
        Vector2 CoGZeroPoint = (leftShoulderJoint.position + rightShoulderJoint.position) / 2;// + .05f * transform.forward;
        
        testMarker.Place(CoGZeroPoint,startForward,startUp, data.HandsPosition);
        
        armsCoGMarker.Place(CoGZeroPoint,startForward,startUp, data.ArmsCG);
        headCoGMarker.Place(CoGZeroPoint,startForward,startUp, data.HeadCG);
        torsoCoGMarker.Place(CoGZeroPoint,startForward,startUp, data.TorsoCG);
        legsCoGMarker.Place(CoGZeroPoint,startForward,startUp, data.LegsCG);
        
        wholeBodyCoGMarker.Place(CoGZeroPoint,startForward,startUp, data.CombinedCG);
        
        
    }
    

    private void PrepCoGMarkers()
    {
        testMarker = Instantiate(CoGMarkerPrefab);
        testMarker.Initialize("Test Marker", Color.black, data.ArmsMassKg);
        
        armsCoGMarker = Instantiate(CoGMarkerPrefab);
        armsCoGMarker.Initialize("Arms CoG", Color.red, data.ArmsMassKg);
        
        headCoGMarker = Instantiate(CoGMarkerPrefab);
        headCoGMarker.Initialize("Head CoG", Color.white, data.HeadMassKg);
        
        torsoCoGMarker = Instantiate(CoGMarkerPrefab);
        torsoCoGMarker.Initialize("Torso CoG", Color.blue, data.TorsoMassKg);
        
        legsCoGMarker = Instantiate(CoGMarkerPrefab);
        legsCoGMarker.Initialize("Legs CoG", Color.yellow, data.LegsMassKg);
        
        wholeBodyCoGMarker = Instantiate(CoGMarkerPrefab);
        wholeBodyCoGMarker.Initialize("Whole Body CoG", Color.green, data.CombinedMass);

        // bodyCoGMarker = Instantiate(CoGMarkerPrefab);
        // bodyCoGMarker.gameObject.name = "Whole Body CoG";
    }
}
