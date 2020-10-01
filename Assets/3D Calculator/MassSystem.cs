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
    public float TotalMass { get; private set; }
    
    public List<MassPoint> subsystem;
    
    
    private List<MassPoint> _massPoints;
    private CoGMarker centerOfGravityMarker;
    private CoGMarker subSystemCoGMarker;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _massPoints = GetComponentsInChildren<MassPoint>().ToList();

        TotalMass = _massPoints.Sum(m => m.mass);
        foreach (MassPoint massPoint in _massPoints)
        {
            massPoint.Initialize(TotalMass);
        }
        
        centerOfGravityMarker = Instantiate(CoGMarkerPrefab,transform);
        centerOfGravityMarker.Initialize("Center of Gravity", Color.blue, TotalMass);

        subSystemCoGMarker = Instantiate(CoGMarkerPrefab, transform);
        subSystemCoGMarker.Initialize("Legs CoG", Color.magenta, subsystem.Sum(m=>m.mass));
    }

    // Update is called once per frame
    void Update()
    {
        centerOfGravityMarker.transform.position = CalculateCoG(_massPoints);
        subSystemCoGMarker.transform.position = CalculateCoG(subsystem);
    }

    public Vector3 CalculateCoG(IEnumerable<MassPoint> massPoints)
    {

        float mass = massPoints.Sum(m => m.mass);
        Vector3 sum = new Vector3();
        foreach (MassPoint massPoint in massPoints)
        {
            sum += massPoint.mass / mass * massPoint.transform.position;
        }

        return sum;    
    }
    
}
