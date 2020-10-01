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
    }
}