using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyConfigurator : MonoBehaviour
{
    public MassPoint torso;
    [Range(.2f,.7f)]
    public float torsoCoG;

    public MassPoint rightLeg;
    public MassPoint leftLeg;
    [Range(.2f,.4f)]
    public float legsCoG;
    
    // Start is called before the first frame update
    void Start()
    {

        torsoCoG = Mathf.Lerp(0, 1, torso.transform.localPosition.x / -.42f);
        legsCoG = (Mathf.Lerp(0, 1, -rightLeg.transform.localPosition.x) +
                  Mathf.Lerp(0, 1, -leftLeg.transform.localPosition.x))/2;

    }

    // Update is called once per frame
    void Update()
    {
        float torsoX = Mathf.Lerp(0, -.42f, torsoCoG);
        Vector3 torsoPos = torso.transform.localPosition;
        torsoPos.x = torsoX;
        torso.transform.localPosition = torsoPos;
        
        
        float legsX = Mathf.Lerp(0, -1, legsCoG);

        Vector3 rightLegPos = rightLeg.transform.localPosition;
        rightLegPos.x = legsX;
        rightLeg.transform.localPosition = rightLegPos;
        
        Vector3 leftLegPos = leftLeg.transform.localPosition;
        leftLegPos.x = legsX;
        leftLeg.transform.localPosition = leftLegPos;
        

    }
}
