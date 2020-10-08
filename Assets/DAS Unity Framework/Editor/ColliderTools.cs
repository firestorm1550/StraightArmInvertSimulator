using System.Collections.Generic;
using DAS_Unity_Framework.ExtensionMethods;
using UnityEditor;
using UnityEngine;

public class ColliderTools
{
    [MenuItem("Tools/ColliderTools/Create Box Collider for Renderer Bounds (Include Inactivate)")]
    static void MakeBoxColliderOfRenderersForSelectedObjectIncludeInactive()
    {
        GameObject rootObject = (GameObject)Selection.activeObject;
        MakeBoxColliderOfRenderers(rootObject,true);
    }
    [MenuItem("Tools/ColliderTools/Create Box Collider for Renderer Bounds")]
    static void MakeBoxColliderOfRenderersForSelectedObject()
    {
        GameObject rootObject = (GameObject)Selection.activeObject;
        MakeBoxColliderOfRenderers(rootObject);
    }
    
    
    public static BoxCollider MakeBoxColliderOfRenderers(GameObject rootObject, bool includeInactive = false)
    {
        BoxCollider col = rootObject.GetComponent<BoxCollider>();
        if(!col)
            //creates a box collider centered at 0,0,0 and size 1, 1, 1
            col = rootObject.AddComponent<BoxCollider>();
        Bounds renderBounds = rootObject.MakeBoundingBoxForObjectRenderers(includeInactive);
        
        //col.center = renderBounds.center + rootObject.transform.position;
        col.size = rootObject.transform.worldToLocalMatrix * renderBounds.size;//.Divide(rootObject.transform.localScale);

        Vector3 vector3FromRenderBoundsToPivot =  renderBounds.center - rootObject.transform.position;
        Vector3 localVector3FromRenderBoundsToPivot = rootObject.transform.worldToLocalMatrix * vector3FromRenderBoundsToPivot;

        col.center = localVector3FromRenderBoundsToPivot;

        return col;
    }
    public static BoxCollider MakeBoxColliderOfColliders(GameObject rootObject)
    {
        BoxCollider col = rootObject.GetComponent<BoxCollider>();
        if(!col)
            col = rootObject.AddComponent<BoxCollider>();
        Bounds allColBounds = rootObject.MakeBoundingBoxForObjectColliders(true);
        
        //col.center = renderBounds.center + rootObject.transform.position;
        col.size = rootObject.transform.worldToLocalMatrix * allColBounds.size;//.Divide(rootObject.transform.localScale);

        Vector3 vector3FromRenderBoundsToPivot =  allColBounds.center - rootObject.transform.position;
        Vector3 localVector3FromRenderBoundsToPivot = rootObject.transform.worldToLocalMatrix * vector3FromRenderBoundsToPivot;

        col.center = localVector3FromRenderBoundsToPivot;

        return col;
    }
}