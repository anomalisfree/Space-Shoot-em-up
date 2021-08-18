
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;

namespace VR
{
    public class GrabbableObject : MonoBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private new BoxCollider collider;
        [Header("use if gun")]
        [SerializeField] private GameObject leftHand;
        [SerializeField] private GameObject rightHand;

        private const float WaitingForChangingLayer = 0.2f;

        private Transform defaultParent;
        private LayerMask defaultLayerMask;
        
        public Action IsThrown;
        public Action IsGrabbed;
        
        private void Start()
        {
            defaultParent = transform.parent;
            defaultLayerMask = gameObject.layer;
        }
        
        public BoxCollider Grab(Transform parent, LayerMask layerMask, XRNode device, out bool isGun)
        {
            StopAllCoroutines();
            isGun = false;

            switch (device)
            {
                case XRNode.LeftHand:
                {
                    if (leftHand != null)
                    {
                        leftHand.SetActive(true);
                        isGun = true;
                    }

                    break;
                }
                case XRNode.RightHand:
                {
                    if (rightHand != null)
                    {
                        rightHand.SetActive(true);
                        isGun = true;
                    }

                    break;
                }
            }
            
            transform.parent = parent;
            gameObject.layer = layerMask;

            rigidbody.isKinematic = true;
            IsGrabbed?.Invoke();
            return collider;
        }

        public void Throw(XRNodeState state)
        {
            if (leftHand != null)
            {
                leftHand.SetActive(false);
            }
            
            if (rightHand != null)
            {
                rightHand.SetActive(false);
            }
            
            transform.parent = defaultParent;

            rigidbody.isKinematic = false;
            
            state.TryGetVelocity(out var velocity);
            rigidbody.velocity = velocity;

            state.TryGetAngularVelocity(out var angularVelocity);
            rigidbody.angularVelocity = angularVelocity;
            IsThrown?.Invoke();

            StartCoroutine(SetDefaultMask());
        }

        private IEnumerator SetDefaultMask()
        {
            yield return new WaitForSeconds(WaitingForChangingLayer);
            gameObject.layer = defaultLayerMask;
        }
        
        private void Update()
        {
            if (rightHand != null && rightHand.activeSelf)
            {
                Transform transformThis;
                (transformThis = transform).localPosition =
                    Vector3.MoveTowards(transformThis.localPosition, Vector3.up*0.045f, Time.deltaTime * 100);
                
                transform.localRotation = Quaternion.Lerp( transformThis.localRotation, Quaternion.Euler(Vector3.forward*90), Time.deltaTime * 100);
            }
            
            if (leftHand != null && leftHand.activeSelf)
            {
                Transform transformThis;
                (transformThis = transform).localPosition =
                    Vector3.MoveTowards(transformThis.localPosition, Vector3.up*0.045f, Time.deltaTime * 100);
                
                transform.localRotation = Quaternion.Lerp( transformThis.localRotation, Quaternion.Euler(Vector3.back*90), Time.deltaTime * 100);
            }
        }
    }
}
