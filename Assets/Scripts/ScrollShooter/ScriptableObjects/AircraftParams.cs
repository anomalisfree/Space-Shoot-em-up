using ScrollShooter.Data;
using UnityEngine;

namespace ScrollShooter.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AircraftParams", menuName = "ScriptableObjects/AircraftParams", order = 1)]
    public class AircraftParams : ScriptableObject
    {
        public int healthOnStart;
        public Vector3 startPose;
        public AircraftBody aircraftBody;
        
        public float speed;
        public float slopeScale;
        public float slopeSpeed;
        public Vector2 fieldSize;
        
        public float shieldTime;
    }
}
