using UnityEngine;
using Random = UnityEngine.Random;

namespace ScrollShooter.Environment
{
    public class EnvironmentCube : MonoBehaviour
    {
        private Vector2 scaleRange;
        private float fieldLength;
        private Color minColorRange;
        private Color maxColorRange;
        private MeshRenderer meshRenderer;
        private float movingSpeed;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public void Initialize(Vector3 position, Vector2 range, float length, Color minColor, Color maxColor, float speed)
        {
            scaleRange = range;
            fieldLength = length;
            movingSpeed = speed;
            transform.localPosition = position;

            SetScale(scaleRange);

            minColorRange = minColor;
            maxColorRange = maxColor;
            SetColor(minColorRange, maxColorRange);
        }

        private void Update()
        {
            if (transform.localPosition.z < -fieldLength)
            {
                MoveToPosition(fieldLength);
                SetScale(scaleRange);
                SetColor(minColorRange, maxColorRange);
            }
            else
            {
                transform.Translate(-transform.forward * movingSpeed);
            }
        }

        private void MoveToPosition(float zPosition)
        {
            var transformThis = transform;
            var localPosition = transformThis.localPosition;
            localPosition = new Vector3(localPosition.x, localPosition.y,  zPosition);
            transformThis.localPosition = localPosition;
        }
        
        private void SetScale(Vector2 range)
        {
            transform.localScale = new Vector3(1, Random.Range(range.x, range.y), 1);
        }

        private void SetColor(Color minColor, Color maxColor)
        {
            Color.RGBToHSV(minColor, out var minH, out var minS, out var minV);
            Color.RGBToHSV(maxColor, out var maxH, out var maxS, out var maxV);

            meshRenderer.material.color = Random.ColorHSV(minH, maxH, minS, maxS, minV, maxV);
        }
    }
}