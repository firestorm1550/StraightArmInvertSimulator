using System;
using System.Collections;
using System.Collections.Generic;
using DAS_Unity_Framework.ExtensionMethods;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Animations;

public class SASL3DVisualizer : MonoBehaviour
{
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


    private CoGMarker headAndArmsCoGMarker;
    private CoGMarker torsoCoGMarker;
    private CoGMarker legsCoGMarker;
    private CoGMarker legsAndTorsoCoGMarker;
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
        transform.RotateAround(transform.position, HandAxis, data.shoulderToTorsoAngleDegrees - 180);
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
        Vector3 zeroPoint = (leftShoulderJoint.position + rightShoulderJoint.position)/2 + .05f * transform.forward;//(leftShoulderJoint.position + rightShoulderJoint.position) / 2;
        Debug.Log(zeroPoint);
        
        torsoCoGMarker.Place(zeroPoint,startForward,startUp, data.TorsoCG);
        legsCoGMarker.Place(zeroPoint,startForward,startUp, data.LegsCG);
        legsAndTorsoCoGMarker.Place(zeroPoint,startForward,startUp, data.CombinedCG);
        
        
    }
    

    private void PrepCoGMarkers()
    {
        // headAndArmsCoGMarker = Instantiate(CoGMarkerPrefab);
        // headAndArmsCoGMarker.gameObject.name = "Head and Arms CoG";
        
        torsoCoGMarker = Instantiate(CoGMarkerPrefab);
        torsoCoGMarker.Initialize("Torso CoG", Color.blue);
        
        legsCoGMarker = Instantiate(CoGMarkerPrefab);
        legsCoGMarker.Initialize("Legs CoG", Color.yellow);;
        
        legsAndTorsoCoGMarker = Instantiate(CoGMarkerPrefab);
        legsAndTorsoCoGMarker.Initialize("Legs and Torso", Color.green);

        // bodyCoGMarker = Instantiate(CoGMarkerPrefab);
        // bodyCoGMarker.gameObject.name = "Whole Body CoG";
    }
}
