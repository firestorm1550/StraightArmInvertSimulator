using System.Collections;
using System.Collections.Generic;
using _3D_Calculator;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARGuyPlacer : MonoBehaviour
{
    public Camera camera;
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    public ARPointCloudManager pointCloudManager;
    public GameController gameController;


    private float _lastHitTime;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private GameObject ARGuyInstance;
    
    
    // Update is called once per frame
    void Update()
    {
        if (ARGuyInstance != null)
            return;
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (Time.time > _lastHitTime + 1)
            if(raycastManager.Raycast(touchPosition, _hits, TrackableType.PlaneWithinPolygon))
            {
                _lastHitTime = Time.time;
                ARGuyInstance = gameController.SpawnGuy();
                
                Vector3 position = _hits[0].pose.position;
                ARGuyInstance.transform.position = position;
                ARGuyInstance.transform.forward = (camera.transform.position - position).Multiply(1, 0, 1);
                //
                // //hide points and planes
                // planeManager.enabled = false;
                // pointCloudManager.enabled = false;
            }
    }
    
    
    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            touchPosition = new Vector2(mousePosition.x, mousePosition.y);
            return true;
        }
        
#else
        if (Input.touchCount > 0 )
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
#endif

        touchPosition = default;
        return false;
    }

    public void ClearARGuy()
    {
        gameController.DespawnGuy();
        ARGuyInstance = null;
        
        //show points and planes
        // planeManager.enabled = true;
        // pointCloudManager.enabled = true;
    }
    
}
