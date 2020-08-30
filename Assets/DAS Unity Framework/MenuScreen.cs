using UnityEngine;

namespace MenuScreens
{
    /// <summary>
    /// For usage documentation, see the MenuScreenManager.cs usage example
    /// </summary>
    public class MenuScreen : MonoBehaviour
    {
        public Vector3 StartPosition { get; private set; }

        private void Awake()
        {
            StartPosition = transform.position;
        }

        public void SetMeActive()
        {
            MenuScreenManager.Instance.SetScreen(this);
        }
    }
}