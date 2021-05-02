using UnityEngine;

namespace ScrollShooter
{
    public class AircraftMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 10;
        [SerializeField] private float slopeScale = 20;
        [SerializeField] private float slopeSpeed = 10;
        [SerializeField] private Vector2 fieldSize = new Vector2(4, 8);

        private float _xMoving;
        private float _yMoving;

        private Quaternion _slope;

        public void HorizontalMoving(float value, float blindArea = 0)
        {
            ChangeTargetSlope(value);
            CheckMovingValue(value, blindArea, out _xMoving);
        }

        public void VerticalMoving(float value, float blindArea = 0)
        {
            CheckMovingValue(value, blindArea, out _yMoving);
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
            _slope = Quaternion.Euler(-Vector3.forward * (rotationValue * slopeScale));
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
            localPosition += new Vector3(_xMoving, 0, _yMoving) * (Time.deltaTime * speed);
            localPosition = ReturnToField(localPosition, fieldSize);
            position = Vector3.MoveTowards(position, localPosition, Time.deltaTime * 100);
            transform.localPosition = position;
        }

        private void SlopeUpdate()
        {
            if (Mathf.Abs(transform.localPosition.x) >= fieldSize.x - fieldSize.x * 0.1f)
                _slope = Quaternion.identity;

            transform.localRotation = Quaternion.Lerp(transform.localRotation, _slope, Time.deltaTime * slopeSpeed);
        }
    }
}