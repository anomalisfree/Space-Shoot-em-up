using System.Collections.Generic;
using UnityEngine;

namespace VR
{
    public class Grabber : MonoBehaviour
    {
        [SerializeField] private PlayerHandAnimationController playerHandAnimationController;
        [SerializeField] private Transform grabPivot;

        private const float HoldDetectedValue = 0.5f;

        private readonly List<GrabbableObject> grabbableObjects = new List<GrabbableObject>();
        private GrabbableObject currentGrabbableObject;
        private bool isHolding;

        private void Start()
        {
            playerHandAnimationController.FlexChanged += FlexChanged;
        }

        private void FlexChanged(float value)
        {
            isHolding = value > HoldDetectedValue;
        }

        private void Update()
        {
            if (isHolding)
            {
                if (currentGrabbableObject != null) return;
                if (grabbableObjects.Count <= 0) return;

                currentGrabbableObject = grabbableObjects[0];

                for (var i = 1; i < grabbableObjects.Count; i++)
                {
                    if (Vector3.Distance(grabbableObjects[i].transform.position, grabPivot.position) <
                        Vector3.Distance(currentGrabbableObject.transform.position, grabPivot.position))
                    {
                        currentGrabbableObject = grabbableObjects[i];
                    }
                }

                //TODO: Find the most versatile method for all types of collider. Make it possible to intercept objects with your hands. 
                var currentGrabbableObjectTransform = currentGrabbableObject.transform;
                grabPivot.position = currentGrabbableObjectTransform.position;
                grabPivot.rotation = currentGrabbableObjectTransform.rotation;

                var newCollider = grabPivot.gameObject.AddComponent<BoxCollider>();
                var oldCollider = currentGrabbableObject.Grab(grabPivot, gameObject.layer);
                newCollider.size = oldCollider.size;
                newCollider.center = oldCollider.center;
                
                playerHandAnimationController.SetPose(1);
            }
            else
            {
                if (currentGrabbableObject == null) return;
                
                currentGrabbableObject.Throw(playerHandAnimationController.GetState());
                currentGrabbableObject = null;
                Destroy(grabPivot.gameObject.GetComponent<BoxCollider>());
                playerHandAnimationController.SetPose(0);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var grabbableObject = other.GetComponent<GrabbableObject>();

            if (grabbableObject != null)
            {
                grabbableObjects.Add(grabbableObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var grabbableObject = other.GetComponent<GrabbableObject>();

            if (grabbableObject != null)
            {
                grabbableObjects.Remove(grabbableObject);
            }
        }
    }
}