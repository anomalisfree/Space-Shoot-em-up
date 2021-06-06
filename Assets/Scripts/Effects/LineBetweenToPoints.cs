using UnityEngine;

namespace Effects
{
    public class LineBetweenToPoints : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;
        [SerializeField] private float maxDistance;


        private void Start()
        {
            lineRenderer.positionCount = 2;
        }

        private void Update()
        {
            var startPosition = startPoint.position;
            lineRenderer.SetPosition(0, startPosition);
            var endPosition = endPoint.position;
            lineRenderer.SetPosition(1, endPosition);

            var greenBlueValue = 1 - Vector3.Distance(startPosition, endPosition) / maxDistance;
            
            lineRenderer.startColor = lineRenderer.endColor = new Color(1, greenBlueValue, greenBlueValue);
        }
    }
}
