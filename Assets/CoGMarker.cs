using UnityEngine;

namespace DefaultNamespace
{
    public class CoGMarker : MonoBehaviour
    {
        public MeshRenderer meshRenderer;

        public void Initialize(string markerName, Color color)
        {
            gameObject.name = markerName;
            meshRenderer.material.color = color;
        }

        public void Place(Vector3 zeroPoint, Vector3 fwd, Vector3 up, Vector2 coords2d)
        {
            transform.position = zeroPoint + fwd * coords2d.x + up * coords2d.y;
        }
    }
}