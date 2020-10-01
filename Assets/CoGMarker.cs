using UnityEngine;

namespace DefaultNamespace
{
    public class CoGMarker : MonoBehaviour
    {
        public MeshRenderer meshRenderer;

        public void Initialize(string markerName, Color color, float mass)
        {
            gameObject.name = markerName;
            meshRenderer.material.color = color;
            transform.localScale = Mathf.Sqrt(mass) / 100 * Vector3.one;
            transform.position = Vector3.one * 1000;
        }

        public void Place(Vector3 zeroPoint, Vector3 fwd, Vector3 up, Vector2 coords2d)
        {
            transform.position = zeroPoint + fwd * coords2d.x + up * coords2d.y;
        }
    }
}