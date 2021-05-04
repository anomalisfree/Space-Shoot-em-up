using UnityEngine;

namespace ScrollShooter
{
    public class TrackFollowing : MonoBehaviour
    {
        [SerializeField] private Transform[] points;
        [SerializeField] private float speed;

        private int currentPointIndex;

        private void Start()
        {
            transform.position = points[0].position;
        }

        private void Update()
        {
            if (points.Length <= 1) return;
            
            if (transform.position != points[currentPointIndex].position)
            {
                transform.position = Vector3.MoveTowards(transform.position, points[currentPointIndex].position,
                    Time.deltaTime * speed);
            }
            else
            {
                if (currentPointIndex < points.Length - 1)
                {
                    currentPointIndex++;
                }
                else
                {
                    currentPointIndex = 0;
                }
            }
        }

        private void OnDrawGizmos()
        {
            var currentPoint = points[0].position;
            Gizmos.color = Color.blue;

            for (var x = 1; x < points.Length; x++)
            {
                Gizmos.DrawLine(currentPoint, points[x].position);
                currentPoint = points[x].position;
            }
            
            Gizmos.DrawLine(currentPoint, points[0].position);
        }
    }
}