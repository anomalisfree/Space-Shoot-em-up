using UnityEngine;

namespace VR
{
    public class LeverGrabber : MonoBehaviour
    {
        [SerializeField] private PlayerHandAnimationController playerHandAnimationController;

        private const float HoldDetectedValue = 0.5f;
        
        private Lever currentLever;
        private bool isTaken;
        private float canGrabTimer;

        private Quaternion startLocalRotation;
        private Transform parent;


        private void Start()
        {
            playerHandAnimationController.FlexChanged += FlexChanged;
            var transformThis = transform;
            startLocalRotation = transformThis.localRotation;
            parent = transformThis.parent;
        }

        private void FlexChanged(float value)
        {
            isTaken = value > HoldDetectedValue;
        }

        private void Update()
        {
            if (canGrabTimer <= 0)
            {
                if (currentLever != null)
                {
                    if (isTaken)
                    {
                        Grab();
                    }
                    else
                    {
                        UnGrab();
                    }
                }
            }
            else
            {
                canGrabTimer -= Time.deltaTime;
            }
        }

        private void Grab()
        {
            playerHandAnimationController.SetPose(4);
            GetComponent<Rigidbody>().isKinematic = true;
            var transformThis = transform;
            transformThis.parent = null;
            transformThis.position = currentLever.handPivot.position;
            transformThis.rotation = currentLever.handPivot.rotation;
            currentLever.handParentPivot.position = playerHandAnimationController.GetParent().position;
            currentLever.IsBreakDown += BreakDown;
        }

        private void UnGrab()
        {
            playerHandAnimationController.SetPose(0);
            var transformThis = transform;
            transformThis.parent = parent;
            transformThis.localRotation = startLocalRotation;
            GetComponent<Rigidbody>().isKinematic = false;
            currentLever.handParentPivot.localPosition = Vector3.zero;
            currentLever.IsBreakDown -= BreakDown;
        }
        
        private void BreakDown()
        {
            UnGrab();
            canGrabTimer = 0.3f;
        }

        private void OnTriggerEnter(Collider other)
        {
            var leverDetector = other.gameObject.GetComponent<LeverHandDetector>();
            
            if (leverDetector != null)
            {
                currentLever = leverDetector.lever;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var leverDetector = other.gameObject.GetComponent<LeverHandDetector>();
            
            if (leverDetector != null)
            {
                currentLever = null;
            }
        }
    }
}
