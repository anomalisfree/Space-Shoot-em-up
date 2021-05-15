using UnityEngine;

namespace ScrollShooter
{
    public enum HelperPosition
    {
        Right,
        Left
    }
    
    public class AircraftHelper : MonoBehaviour
    {
        [SerializeField] private Transform pivot;
        [SerializeField] private HelperPosition helperPosition;
        [SerializeField] private float reaction = 40;
        [SerializeField] private float distanceToPivot = 1.5f;
        [SerializeField] private float rotationIntensity = 100;

        public void Initialize(Transform aircraftPivot, HelperPosition position)
        {
            pivot = aircraftPivot;
            helperPosition = position;
        }
        private void Update()
        {
            var pivotPosition = pivot.position;
            var thisTransform = transform;

            var positionTarget = helperPosition switch
            {
                HelperPosition.Right => pivotPosition + Vector3.right * distanceToPivot,
                HelperPosition.Left => pivotPosition + Vector3.left * distanceToPivot,
                _ => Vector3.zero
            };

            var thisPosition = thisTransform.position;
            transform.position = Vector3.Lerp(thisPosition, positionTarget,
                Time.deltaTime * reaction);

            transform.rotation =
                Quaternion.Euler(new Vector3(0, 0, (thisPosition.x - positionTarget.x) * rotationIntensity));
        }
    }
}