using ScrollShooter.Data;
using UnityEngine;

namespace ScrollShooter.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AircraftParams", menuName = "ScriptableObjects/AircraftParams", order = 1)]
    public class AircraftParams : ScriptableObject
    {
        [Header("Aircraft Settings")]
        public int healthOnStart;
        public Vector3 startPose;
        public AircraftBody aircraftBody;
        
        [Header("Moving Settings")]
        public float speed;
        public float slopeScale;
        public float slopeSpeed;
        public Vector2 fieldSize;
        
        [Header("Shield")][Header("PowerUp Settings")]
        public float shieldTime;

        [Header("Helpers")] 
        public float helperReaction;
        public float helperDistanceToPivot;
        public float helperRotationIntensity;
    }
}
