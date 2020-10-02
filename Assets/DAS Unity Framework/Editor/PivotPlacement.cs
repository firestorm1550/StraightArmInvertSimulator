using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Linq;
using UnityEngine;

public class PivotPlacement
{
    [MenuItem("Tools/Place Pivot/Center")]
    private static void Center()
    {
        GameObject rootObject = (GameObject)Selection.activeObject;
        PlacePivotInBoundingBox(Vector3.zero, rootObject);
    }

    [MenuItem("Tools/Place Pivot/Bottom-Center")]
    private static void BottomCenter()
    {
        GameObject rootObject = (GameObject)Selection.activeObject;
        PlacePivotInBoundingBox(new Vector3(0,-1,0), rootObject);
    }
    [MenuItem("Tools/Place Pivot/Back-Center")]
    private static void BackCenter()
    {
        GameObject rootObject = (GameObject)Selection.activeObject;
        PlacePivotInBoundingBox(new Vector3(0,0,-1),rootObject);
    }
    
    [MenuItem("Tools/Place Pivot/Front-Bottom")]
    private static void FrontBottom()
    {
        GameObject rootObject = (GameObject)Selection.activeObject;
        PlacePivotInBoundingBox(new Vector3(0,-1,1),rootObject);
    }

    [MenuItem("Tools/Place Pivot/Back-Bottom")]
    private static void BackBottom()
    {
        GameObject rootObject = (GameObject)Selection.activeObject;
        PlacePivotInBoundingBox(new Vector3(0,-1,-1),rootObject);
    }
    
    
    [MenuItem("Tools/Place Pivot/Top-Center")]
    private static void TopCenter()
    {
        GameObject rootObject = (GameObject)Selection.activeObject;
        PlacePivotInBoundingBox(new Vector3(0,1,0),rootObject);
    }

    static void PlacePivotInBoundingBox(Vector3 normalizedPosition, GameObject rootObject)
    {
        //selected is the root object
        //get all the mesh objects in children
        //make a bounding box containing all of them
        //create a new object as a child of the root and position it at the box's center point
        //make all the mesh objects children of that new object
        //move that new object to local pos vector zero
        
        
        List<Renderer> renderers = new List<Renderer>();
        List<MeshRenderer> meshRenderers = rootObject.GetComponentsInChildren<MeshRenderer>().ToList();
        List<SkinnedMeshRenderer> skinnedMeshRenderers = rootObject.GetComponentsInChildren<SkinnedMeshRenderer>().ToList();

        if(meshRenderers.Any())
            renderers.AddRange(meshRenderers);
        if(skinnedMeshRenderers.Any())
            renderers.AddRange(skinnedMeshRenderers);
        
        Bounds bounds = renderers[0].bounds;
        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }


        List<Transform> children = new List<Transform>();
        for(int i = rootObject.transform.childCount-1; i>=0; i--)
        {
            //This will d/c 
            Transform child = rootObject.transform.GetChild(i);
            children.Add(child);
            child.parent = null;
        }

        rootObject.transform.position = bounds.center + normalizedPosition.Multiply(bounds.extents);
        foreach (Transform child in children)
        {
            child.transform.parent = rootObject.transform;
            child.transform.SetSiblingIndex(0);
        }



    }
}