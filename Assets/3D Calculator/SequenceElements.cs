using System.Collections.Generic;

namespace _3D_Calculator
{
    public static class SequenceElements
    {
        public static SequenceElement DeadHang(float transitionTime, float holdTime)
        {
            return new SequenceElement
            {
                transitionTime = transitionTime, shoulderFlexionAngle = 180, anteriorHipFlexionAngle = 180,
                lateralHipFlexionAngle = 0, holdDuration = holdTime
            };
        }

        public static SequenceElement Inverted()
        {
            return new SequenceElement
            {
                transitionTime = 2, shoulderFlexionAngle = 0, anteriorHipFlexionAngle = 180,
                lateralHipFlexionAngle = 0, holdDuration = 1
            };
        }
        
        public static List<SequenceElement> SASLInvert()
        {
            return new List<SequenceElement>
            {
                new SequenceElement{
                    transitionTime = 1.5f, shoulderFlexionAngle = 180, anteriorHipFlexionAngle = 70,
                },
                new SequenceElement
                {
                    transitionTime = 2.5f, shoulderFlexionAngle = 35, anteriorHipFlexionAngle = 70,
                    holdDuration = .5f, name = "Straight Arm Straight Leg Invert"
                }
            };
        }
        
        public static SequenceElement SkinTheCat()
        {
            return new SequenceElement
            {
                transitionTime = 3f, shoulderFlexionAngle = -75, anteriorHipFlexionAngle = 70,
                lateralHipFlexionAngle = 0, holdDuration = 1, name = "Skin the Cat"
            };
        }

        public static SequenceElement BackPlanche(float straddleAngle)
        {
            return new SequenceElement
            {
                transitionTime = 1.5f, shoulderFlexionAngle = -45, anteriorHipFlexionAngle = 180,
                lateralHipFlexionAngle = straddleAngle, holdDuration = 2, name = "Back Planche ("+straddleAngle+")"
            };
        }

        public static SequenceElement FrontPlanche(float straddleAngle)
        {
            return new SequenceElement
            {
                transitionTime = 3, shoulderFlexionAngle = 55, anteriorHipFlexionAngle = 180,
                lateralHipFlexionAngle = straddleAngle, holdDuration = 2, name = "Front Planche ("+straddleAngle+")"
            };
        }

        
    }
}