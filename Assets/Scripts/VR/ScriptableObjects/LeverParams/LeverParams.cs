using UnityEngine;

namespace VR.ScriptableObjects.LeverParams
{
    [CreateAssetMenu(fileName = "LeverParams", menuName = "ScriptableObjects/LeverParams", order = 1)]
    public class LeverParams : ScriptableObject
    {
        public float minLeverPosition;
        public float maxLeverPosition;
        public float maxGrabDistance;
        public float returnSpeed;
    }
}
