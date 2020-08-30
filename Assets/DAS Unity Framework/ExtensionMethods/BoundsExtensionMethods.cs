using System.Collections.Generic;
using UnityEngine;

namespace DAS_Unity_Framework.ExtensionMethods
{
    public static class BoundsExtensionMethods
    {
        
    //=============== Bounds Extensions ==========================================

    public static Bounds MakeBoundingBoxForObjectColliders(this Transform rootObject, bool includeInactive = false)
    {
        return MakeBoundingBoxForObjectColliders(rootObject.gameObject, includeInactive);
    }

    public static Bounds MakeBoundingBoxForObjectColliders(this GameObject rootObject, bool includeInactive = false)
    {
        Collider[] colliders = rootObject.GetComponentsInChildren<Collider>(includeInactive);
        if (colliders.Length == 0)
        {
            return new Bounds(rootObject.transform.position, Vector3.zero);
        }

        return colliders.MakeBoundingBoxForObjectColliders();
    }

    public static Bounds MakeBoundingBoxForObjectColliders(this Collider[] colliders)
    {
        Bounds bounds = colliders[0].bounds;
        foreach (Collider c in colliders)
        {
            bounds.Encapsulate(c.bounds);
        }

        return bounds;
    }

    public static Bounds MakeBoundingBoxForObjectRenderers(this Transform rootObject, bool includeInactive = false)
    {
        return MakeBoundingBoxForObjectRenderers(rootObject.gameObject, includeInactive);
    }

    /// <summary>
    /// Makes a bounding box that contains ever renderer on the "rootObject" and all of its children.
    /// </summary>
    /// <param name="rootObject"></param>
    /// <param name="includeInactive"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public static Bounds MakeBoundingBoxForObjectRenderers(this GameObject rootObject, bool includeInactive = false)
    {

        Renderer[] renderers = rootObject.GetComponentsInChildren<Renderer>(includeInactive);
        //Debug.Log("renderers.length: " + renderers.Length);
        if (renderers.Length == 0)
        {
            return new Bounds(rootObject.transform.position, Vector3.zero);
        }

        //We start with one of the existing bounds to ensure no unnecessary space in the bounds
        Bounds bounds = new Bounds(renderers[0].bounds.center, renderers[0].bounds.size);
        foreach (Renderer r in renderers)
        {
            bounds.Encapsulate(r.bounds);
        }

        return bounds;
    }

    public static bool ContainsAnyPoint(this Bounds boundsToCheck, IEnumerable<Vector3> points)
    {
        foreach (Vector3 point in points)
        {
            if (boundsToCheck.Contains(point))
                return true;
        }

        return false;
    }

    public static List<Vector3> GetBoundsCorners(this Bounds bounds, Vector3 boundsWorldSpacePosition)
    {
        Vector3 extents = bounds.extents;

        List<Vector3> points = new List<Vector3>();
        for (int i = -1; i <= 1; i += 2)
        for (int j = -1; j <= 1; j += 2)
        for (int k = -1; k <= 1; k += 2)
            points.Add(new Vector3(extents.x * i, extents.y * j, extents.z * k) + boundsWorldSpacePosition);
        return points;
    }

    public static List<Vector3> GetBoundsFaceCenters(this Bounds bounds, Vector3 boundsWorldSpacePosition)
    {
        List<Vector3> points = new List<Vector3>()
        {
            boundsWorldSpacePosition + new Vector3(bounds.extents.x, 0, 0),
            boundsWorldSpacePosition + new Vector3(-bounds.extents.x, 0, 0),
            boundsWorldSpacePosition + new Vector3(0, bounds.extents.y, 0),
            boundsWorldSpacePosition + new Vector3(0, -bounds.extents.y, 0),
            boundsWorldSpacePosition + new Vector3(0, 0, bounds.extents.z),
            boundsWorldSpacePosition + new Vector3(0, 0, -bounds.extents.z),
        };
        return points;
    }

        
    }
}