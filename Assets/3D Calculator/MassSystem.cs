using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class MassSystem : MonoBehaviour
{
    public CoGMarker CoGMarkerPrefab;
    public Vector3 CenterOfGravity => centerOfGravityMarker.transform.position;
    public Vector3 LocalCenterOfGravity => centerOfGravityMarker.transform.localPosition; 
    
    private List<MassPoint> _massPoints;
    private CoGMarker centerOfGravityMarker;
    private float totalMass;
    
    // Start is called before the first frame update
    void Start()
    {
        _massPoints = GetComponentsInChildren<MassPoint>().ToList();

        totalMass = _massPoints.Sum(m => m.mass);
        foreach (MassPoint massPoint in _massPoints)
        {
            massPoint.Initialize(totalMass);
        }
        
        centerOfGravityMarker = Instantiate(CoGMarkerPrefab,transform);
        centerOfGravityMarker.Initialize("Center of Gravity", Color.green, totalMass);
    }

    // Update is called once per frame
    void Update()
    {
        centerOfGravityMarker.transform.position = CalculateCoG();
    }

    private Vector3 CalculateCoG()
    {
        Vector3 sum = new Vector3();
        foreach (MassPoint massPoint in _massPoints)
        {
            sum += massPoint.mass / totalMass * massPoint.transform.position;
        }

        return sum;    
    }
}
