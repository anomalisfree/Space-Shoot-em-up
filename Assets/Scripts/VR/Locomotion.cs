using UnityEngine;

namespace VR
{
    public class Locomotion : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform headTransform;
        [SerializeField] private PlayerHandAnimationController playerHandAnimationController;
        [SerializeField] private float maxLocomotionDistance;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private LayerMask layerMask;

        private LocomotionPoint[] locomotionPoints;
        private LocomotionPoint currentLocomotionPoint;

        private void Start()
        {
            locomotionPoints = FindObjectsOfType<LocomotionPoint>();
        }

        private void Update()
        {
            if (playerHandAnimationController.GetLocomotionButtonPressed())
            {
                foreach (var locomotionPoint in locomotionPoints)
                {
                    if (Vector3.Distance(playerTransform.position, locomotionPoint.transform.position) <=
                        maxLocomotionDistance)
                    {
                        locomotionPoint.Show();
                    }
                    else
                    {
                        locomotionPoint.Hide();
                    }
                }

                var fwd = transform.TransformDirection(Vector3.forward);

                if (Physics.Raycast(transform.position, fwd, out var hit, 10, layerMask))
                {
                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, hit.point);

                    var lp = hit.collider.GetComponent<LocomotionPoint>();

                    if (currentLocomotionPoint != null && lp != currentLocomotionPoint)
                    {
                        currentLocomotionPoint.SetUnActive();
                    }

                    currentLocomotionPoint = lp;
                    currentLocomotionPoint.SetActive();
                }
                else
                {
                    lineRenderer.enabled = false;

                    if (currentLocomotionPoint != null)
                    {
                        currentLocomotionPoint.SetUnActive();
                        currentLocomotionPoint = null;
                    }
                }
            }
            else
            {
                foreach (var locomotionPoint in locomotionPoints)
                {
                    locomotionPoint.Hide();
                    locomotionPoint.SetUnActive();
                }

                lineRenderer.enabled = false;

                if (currentLocomotionPoint != null)
                {
                    var headLocalPosition = headTransform.localPosition;
                    playerTransform.position = currentLocomotionPoint.GetPosition() -
                                               new Vector3(headLocalPosition.x, 0,
                                                   headLocalPosition.z);
                }

                currentLocomotionPoint = null;
            }
        }
    }
}