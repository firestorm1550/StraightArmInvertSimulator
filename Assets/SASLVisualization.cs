using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SASLInvertCalculator))]
public class SASLVisualization : MonoBehaviour
{


    public Material pointColor;
    public Material lineColor;
    public Vector3 startPoint;
    
    private SASLInvertCalculator data;
    
    private LineRenderer shoulderJoint;
    private LineRenderer torso;
    private LineRenderer hipJoint;
    private LineRenderer legs;

    private bool initialized;


    // Update is called once per frame
    void Update()
    {
        if(initialized)
            DrawVisualization();
    }


    private void Awake()
    {
        data = GetComponent<SASLInvertCalculator>();
        
        shoulderJoint = InitializeJoint("Shoulders");
        torso = InitializeLine("Torso");
        hipJoint = InitializeJoint("Hips");
        legs = InitializeLine("Legs"); 
        initialized = true;
    }

    private LineRenderer InitializeJoint(string jointName)
    {
        GameObject jointObj = new GameObject(jointName);
        jointObj.transform.parent = transform;
        LineRenderer joint = jointObj.AddComponent<LineRenderer>();
        joint.material = pointColor;
        joint.numCapVertices = 12;
        joint.widthMultiplier = .15f;
        return joint;
    }
    
    private LineRenderer InitializeLine(string lineName)
    {
        GameObject lineObj = new GameObject(lineName);
        lineObj.transform.parent = transform;
        LineRenderer line = lineObj.AddComponent<LineRenderer>();
        line.material = lineColor;
        line.widthMultiplier = .05f;
        return line;
    }
    
    private void DrawVisualization()
    {
        shoulderJoint.SetPositions(new []
        {
            startPoint, startPoint
        });

        Vector3 hipJointPos = GetHipJointPosition();
        
        torso.SetPositions(new []{startPoint, hipJointPos});
        hipJoint.SetPositions(new[]{hipJointPos,hipJointPos});
        legs.SetPositions(new[]{hipJointPos, Vector3.zero, });
    }

    private Vector3 GetHipJointPosition()
    {
        float torsoLength = data.torsoLengthMeters;
        float shoulderAngle = data.shoulderToTorsoAngleDegrees;

        float theta = 90 - shoulderAngle;
        
        float x = torsoLength * Mathf.Sin(Mathf.Deg2Rad * shoulderAngle);
        float y = torsoLength * Mathf.Cos(Mathf.Deg2Rad * shoulderAngle);
        
        return startPoint + new Vector3(x,y,0);
    }

    private Vector3 GetToePosition(Vector3 hipJointPosition)
    {
        return new Vector3();
    }
}
