//
//Filename: maxCamera.cs
//
// original: http://www.unifycommunity.com/wiki/index.php?title=MouseOrbitZoom
//
// --01-18-2010 - create temporary target, if none supplied at start

using System;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine.EventSystems;

public class MaxCamera : MonoBehaviour
{
    public Camera camera;

    public float squareMagnitudeStopValue = .0001f;
    public Bounds target;
    public Transform reference; //This is the object that will be used like world zero
    
    
    public float maxDistance = 20;
    public float minDistance = .6f;
    public float minFocusDistance = 3.5f;
    public float minimumMaxDistance = 100;
    public float maxDistanceFactor = 1.75f;
    
    
    public float xSpeed = 200.0f;
    public float ySpeed = 200.0f;
    public int yMinLimit = -80;
    public int yMaxLimit = 80;
    public int zoomRate = 40;
    public float panSpeed = 4f;
    
    
    public float zoomDampening = 5.0f;
    public float orbitDampening = 5;
    public float refocusDampening = 5;
    public float viewportVisualCenterOffsetDampening = 5;
    
    

    [CanBeNull] public Action<MaxCamera> OnCameraStopMoving; 
    
    
    
    [SerializeField] private float xDeg = 0.0f;
    [SerializeField] private float yDeg = 0.0f;
    private bool moving;
    
    [SerializeField] private float currentDistance;
    [SerializeField] public float desiredDistance;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Vector3 currentEasedTargetCenter;
    
    
    //values from 0 to 1 where 0,0 is bottom left of screen, 1,1 is top right
    private Vector2 viewportVisualCenter = new Vector2(.5f,.5f);
    public float desiredDegreeOffsetY;
    private float currentDegreeOffsetY;


    public void Init(List<GameObject> newTarget, Transform newReference)
    {
        Focus(newTarget);
        maxDistance = Mathf.Max(minimumMaxDistance,DistanceToSeeWholeBounds(target));
        
        reference = newReference;
        //be sure to grab the current rotations as starting points.
        currentRotation = transform.rotation;
        desiredRotation = transform.rotation;

        SetViewportVisualCenter(viewportVisualCenter);

        SetToDefaultView(true);
    }

    public void Focus(GameObject obj)
    {
        Focus(new List<GameObject>{obj});
    }
    public void Focus(List<GameObject> newTarget)
    {
        target = MakeBoundingBoxForObjectRenderers(newTarget);
        
        
        desiredDistance = Mathf.Max(minFocusDistance, DistanceToSeeWholeBounds(target));
        //minDistance = target.extents.magnitude / 10;
    }
    
    
    /*
     * Camera logic on LateUpdate to only update after all character movement logic has been handled. 
     */
    void Update()
    {
        Vector3 cameraStartPosition = camera.transform.position;
        
        
        Vector2 panXY = Vector2.zero;

        if (Application.isPlaying && !EventSystem.current.IsPointerOverGameObject())
        {
            // If right mouse button? ORBIT
            if (Input.GetMouseButton(1))
            {
                xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            }
        }


        Orbit();

        Zoom();

        target.center -= camera.transform.up * panXY.y;
        target.center -= camera.transform.right * panXY.x;

        ApplyViewportVisualCenterOffset();
        //Panning
        //CalculatePanOffset();

        Vector3 cameraEndPosition = camera.transform.position;

        if ((cameraEndPosition-cameraStartPosition).sqrMagnitude < squareMagnitudeStopValue)
        {
            if (moving)
            {
                OnCameraStopMoving?.Invoke(this);
                moving = false;
            }
        }
        else
        {
            moving = true;
        }
        
    }

    private void ApplyViewportVisualCenterOffset()
    {
        currentDegreeOffsetY = Mathf.Lerp(currentDegreeOffsetY, desiredDegreeOffsetY,
            Time.deltaTime * viewportVisualCenterOffsetDampening);
        
        camera.transform.localPosition =
            new Vector3(0, -Mathf.Tan(currentDegreeOffsetY * Mathf.Deg2Rad) * currentDistance);
    }
    
    private void EaseTargetCenter()
    {
        currentEasedTargetCenter =
            Vector3.Lerp(currentEasedTargetCenter, target.center, Time.deltaTime * refocusDampening);
    }

    private void Zoom()
    {
        if (Application.isPlaying && !EventSystem.current.IsPointerOverGameObject())
        {
            float distanceModifiedZoomRate = zoomRate * Mathf.Abs(desiredDistance);

            float distanceDelta = -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * distanceModifiedZoomRate;
            
            
            //if we're close to our focal point, we should start moving the focal point forward
            if (distanceModifiedZoomRate < 160 && distanceDelta < 0)
            {
                target.center -= distanceDelta * camera.transform.forward;
            }
            else
            {
                // affect the desired Zoom distance if we roll the scrollwheel
                desiredDistance += distanceDelta;
                //clamp the zoom min/max
                desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);                
            }
        }


        // For smoothing of the zoom, lerp distance
       currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);

        
        
        EaseTargetCenter();
        // calculate position based on the new currentDistance 
        transform.position = currentEasedTargetCenter - (transform.rotation * Vector3.forward * currentDistance);

    }

    private void Orbit()
    {
        ////////OrbitAngle

        //Clamp the vertical axis for the orbit
        yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
        // set camera rotation 
        desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
        currentRotation = transform.rotation;

        transform.rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * orbitDampening);
    }
    
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
    
    
    public void SetToDefaultView(bool instant = false)
    {
        Vector3 view = reference.forward;
        view.Normalize();

        desiredDistance = DistanceToSeeWholeBounds(target);
        if (instant)
            currentDistance = desiredDistance;
        ViewFromDirection(view);
    }

    public void ViewFromDirection(Vector3 direction)
    {
        //Bounds bounds = GetGameObjectToCenterOn().MakeBoundingBoxForObjectRenderers(false);
        //desiredDistance = bounds.extents.magnitude * 2;
        //transform.LookAt(bounds.center);

        
        xDeg = 180 + Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        yDeg = direction.y * 90;

        //Orbit();
    }

    public void SetToCloseDefaultView()
    {
        transform.position = target.center + target.extents.magnitude * 1f * Vector3.one;
        transform.LookAt(target.center);
    }
    
    private Bounds MakeBoundingBoxForObjectRenderers(List<GameObject> objects)
    {
        Bounds bounds = new Bounds();
        bounds.center = objects[0].transform.position;
        bounds.size = Vector3.zero;
        
        foreach (GameObject o in objects)
        {
            foreach (Renderer rend in o.GetComponentsInChildren<Renderer>())
            {
                bounds.Encapsulate(rend.bounds);
            }    
        }

        return bounds;
    }

    //0,0 is bottom left, 1,1 is top right
    public void SetViewportVisualCenter(Vector2 p0)
    {
        viewportVisualCenter = p0;
        desiredDegreeOffsetY = camera.fieldOfView * (viewportVisualCenter.y - .5f);
    }

    public float DistanceToSeeWholeBounds(Bounds? bounds = null)
    {
        if (bounds == null)
            bounds = target;
        float theta = Mathf.Max(camera.fieldOfView, camera.fieldOfView * camera.aspect);
        float d = bounds.Value.extents.Max() / 2 * Mathf.Tan(Mathf.Deg2Rad * theta / 2) * maxDistanceFactor;
        return d;
    }

    public void Init(GameObject obj)
    {
        Init(new List<GameObject>{obj}, obj.transform);
    }
}