using System;
using UnityEngine;

namespace _3dDataModel
{
    /// <summary>
    /// This class is designed to handle the moderately frequent case in which an object is not supposed to handle its own OnMouse events.
    /// This is common when coding simple, generic, and interactible objects. Examples include UI handles, model variants, and AttachmentPoints.
    ///
    /// Additionally, this becomes necessary if a single object contains multiple mesh objects.
    ///
    /// USAGE EXAMPLE:
    /// You want mouse events on a tank to act the same, no matter whether they're on the turret, treads, or body.
    /// You'd create a script called Tank, put it on the root object. Then, put each mesh as a child of this one.
    ///
    ///
    /// Hierarchy:
    ///    Tank
    ///         Body
    ///         Treads
    ///         Turret
    ///
    /// On each of body, treads, and turret, you would put this script. Then in the tank's awake or initialization function,
    /// you would set each relavent mouse event action on each of the three MouseEventRelayer's in it's children.
    /// 
    /// </summary>
    public class MouseEventRelayer : MonoBehaviour
    {
        public Action mouseEnter;
        public Action mouseDrag;
        public Action mouseDown;
        public Action mouseOver;
        public Action mouseExit;
        public Action mouseUp;
        public Action mouseUpAsButton;


        private void Update()
        {
            //This is here so the enabled/disabled checkbox appears in the inspector.
        }
        
        #region EventRelays

        private void OnMouseEnter()
        {
            if (enabled)
                mouseEnter?.Invoke();
        }

        private void OnMouseDrag()
        {
            if (enabled)
                mouseDrag?.Invoke();
        }

        private void OnMouseOver()
        {
            if (enabled)
                mouseOver?.Invoke();
        }

        private void OnMouseDown()
        {
            if (enabled)
                mouseDown?.Invoke();
        }

        private void OnMouseExit()
        {
            if (enabled)
                mouseExit?.Invoke();
        }

        private void OnMouseUp()
        {
            if (enabled)
                mouseUp?.Invoke();
        }

        private void OnMouseUpAsButton()
        {
            if (enabled)
                mouseUpAsButton?.Invoke();
        }

        #endregion
    }
}