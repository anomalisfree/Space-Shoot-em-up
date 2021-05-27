using ScrollShooter.ScriptableObjects;
using UnityEngine;

namespace ScrollShooter
{
    public class AircraftMovement : MonoBehaviour
    {
        private float speed;
        private float slopeScale;
        private float slopeSpeed;
        private Vector2 fieldSize;

        private float xMoving;
        private float yMoving;

        private Quaternion slope;

        public void Initialize(AircraftParams aircraftParams)
        {
            speed = aircraftParams.speed;
            slopeScale = aircraftParams.slopeScale;
            slopeSpeed = aircraftParams.slopeSpeed;
            fieldSize = aircraftParams.fieldSize;
            transform.localPosition = aircraftParams.startPose;
        }
        
        public void HorizontalMoving(float value, float blindArea = 0)
        {
            ChangeTargetSlope(value);
            CheckMovingValue(value, blindArea, out xMoving);
        }

        public void VerticalMoving(float value, float blindArea = 0)
        {
            CheckMovingValue(value, blindArea, out yMoving);
        }

        private static void CheckMovingValue(float value, float blindArea, out float movingValue)
        {
            if (value <= blindArea && value >= -blindArea) value = 0;
            if (value > 1) value = 1;
            if (value < -1) value = -1;
            movingValue = value;
        }

        private static Vector3 ReturnToField(Vector3 position, Vector2 fieldSize)
        {
            return new Vector3(ReturnToValues(position.x, fieldSize.x),
                0, ReturnToValues(position.z, fieldSize.y));
        }

        private static float ReturnToValues(float value, float edge)
        {
            if (value > edge) return edge;
            if (value < -edge) return -edge;
            return value;
        }

        private void ChangeTargetSlope(float rotationValue)
        {
            slope = Quaternion.Euler(-Vector3.forward * (rotationValue * slopeScale));
        }

        private void Update()
        {
            MovementUpdate();
            SlopeUpdate();
        }

        private void MovementUpdate()
        {
            var position = transform.localPosition;
            var localPosition = position;
            localPosition += new Vector3(xMoving, 0, yMoving) * (Time.deltaTime * speed);
            localPosition = ReturnToField(localPosition, fieldSize);
            position = Vector3.MoveTowards(position, localPosition, Time.deltaTime * 100);
            transform.localPosition = position;
        }

        private void SlopeUpdate()
        {
            if (Mathf.Abs(transform.localPosition.x) >= fieldSize.x - fieldSize.x * 0.1f)
                slope = Quaternion.identity;

            transform.localRotation = Quaternion.Lerp(transform.localRotation, slope, Time.deltaTime * slopeSpeed);
        }
    }
}