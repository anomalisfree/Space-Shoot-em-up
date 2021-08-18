using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace VR
{
    public class Grabber : MonoBehaviour
    {
        [SerializeField] private PlayerHandAnimationController playerHandAnimationController;
        [SerializeField] private Transform grabPivot;
        [SerializeField] private SkinnedMeshRenderer handMesh;
        [SerializeField] private PhotonView photonView;
        

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
                
                var newCollider = grabPivot.gameObject.AddComponent<BoxCollider>();
                var oldCollider = currentGrabbableObject.Grab(grabPivot, gameObject.layer,
                    playerHandAnimationController.GetDeviceType(), playerHandAnimationController, out var isGun);
                newCollider.size = oldCollider.size;
                newCollider.center = oldCollider.center;
                handMesh.enabled = !isGun;

                if (photonView != null)
                {
                    photonView.RPC("EnableDisableHandMesh", RpcTarget.All, !isGun);
                }

                playerHandAnimationController.SetPose(1);
            }
            else
            {
                if (currentGrabbableObject == null) return;
                
                currentGrabbableObject.Throw(playerHandAnimationController.GetState());
                handMesh.enabled = true;

                if (photonView != null)
                {
                    photonView.RPC("EnableDisableHandMesh", RpcTarget.All, true);
                }

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
        
        [PunRPC]
        public void EnableDisableHandMesh(bool isActive){
            handMesh.enabled = isActive;
        }
    }
}