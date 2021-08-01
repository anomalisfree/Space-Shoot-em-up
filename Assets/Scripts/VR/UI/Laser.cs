using UnityEngine;

namespace VR.UI
{
    public class Laser : MonoBehaviour
    {
        [SerializeField] private PlayerHandAnimationController playerHandAnimationController;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float maxDistance;
        [SerializeField] private GameObject ball;

        private InteractableUI currentUI;

        private void Start()
        {
            lineRenderer.positionCount = 2;
        }

        private void FixedUpdate()
        {
            var fwd = transform.TransformDirection(Vector3.forward);

            if (Physics.Raycast(transform.position, fwd, out var hit, maxDistance, layerMask))
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hit.point);
                ball.transform.position = hit.point;
                ball.SetActive(true);

                var newCurrentUI = hit.collider.GetComponent<InteractableUI>();
                
                
                if (newCurrentUI != null)
                {
                    if (currentUI != null && currentUI != newCurrentUI)
                    {
                        HighlightOffCurrentUI();
                    }

                    currentUI = newCurrentUI;
                    currentUI.HighlightOn();
                    playerHandAnimationController.PinchChanged += PressUI;
                }
                else
                {
                    if (currentUI != null)
                    {
                        HighlightOffCurrentUI();
                    }
                }
            }
            else
            {
                lineRenderer.enabled = false;
                // var position = transform.position;
                // lineRenderer.SetPosition(0, position);
                // lineRenderer.SetPosition(1, position + fwd * maxDistance);
                ball.SetActive(false);

                if (currentUI != null)
                {
                    HighlightOffCurrentUI();
                }
            }
        }

        private void HighlightOffCurrentUI()
        {
            currentUI.HighlightOff();
            playerHandAnimationController.PinchChanged -= PressUI;
            currentUI = null;
        }
        
        private void PressUI(float value)
        {
            if (currentUI != null)
                currentUI.isPressed = value > 0.5f;
        }
    }
}