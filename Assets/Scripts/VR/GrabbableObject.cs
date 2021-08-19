using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

namespace VR
{
    public class GrabbableObject : MonoBehaviour
    {
#pragma warning disable 108,114
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private BoxCollider collider;
#pragma warning restore 108,114

        [Header("use if gun")] [SerializeField]
        private GameObject leftHand;

        [SerializeField] private GameObject rightHand;
        [SerializeField] private PhotonView photonView;

        private PlayerHandAnimationController playerHandController;

        private const float WaitingForChangingLayer = 0.2f;

        private Transform defaultParent;
        private Transform currentParent;
        private LayerMask defaultLayerMask;

        public Action<PlayerHandAnimationController> IsThrown;
        public Action<PlayerHandAnimationController> IsGrabbed;


        private void Start()
        {
            defaultParent = currentParent = transform.parent;
            defaultLayerMask = gameObject.layer;
            rigidbody.isKinematic = !Equals(photonView.Owner, PhotonNetwork.LocalPlayer);
        }

        public BoxCollider Grab(Transform parent, LayerMask layerMask, XRNode device,
            PlayerHandAnimationController playerHandAnimationController, out bool isGun)
        {
            StopAllCoroutines();
            isGun = false;
            playerHandController = playerHandAnimationController;

            switch (device)
            {
                case XRNode.LeftHand:
                {
                    if (leftHand != null)
                    {
                        photonView.RPC("ShowHideLeftHand", RpcTarget.AllBuffered, true);
                        isGun = true;
                    }

                    break;
                }
                case XRNode.RightHand:
                {
                    if (rightHand != null)
                    {
                        photonView.RPC("ShowHideRightHand", RpcTarget.AllBuffered, true);
                        isGun = true;
                    }

                    break;
                }
            }

            if (photonView != null)
            {
                if (!Equals(photonView.Owner, PhotonNetwork.LocalPlayer))
                    photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            }

            currentParent = transform.parent = parent;
            gameObject.layer = layerMask;
            
            IsGrabbed?.Invoke(playerHandController);
            return collider;
        }

        public void Throw(XRNodeState state)
        {
            if (leftHand != null)
            {
                photonView.RPC("ShowHideLeftHand", RpcTarget.AllBuffered, false);
            }

            if (rightHand != null)
            {
                photonView.RPC("ShowHideRightHand", RpcTarget.AllBuffered, false);
            }

            currentParent = transform.parent = defaultParent;

            state.TryGetVelocity(out var velocity);
            rigidbody.velocity = velocity;

            state.TryGetAngularVelocity(out var angularVelocity);
            rigidbody.angularVelocity = angularVelocity;
            IsThrown?.Invoke(playerHandController);

            StartCoroutine(SetDefaultMask());
        }

        private IEnumerator SetDefaultMask()
        {
            yield return new WaitForSeconds(WaitingForChangingLayer);
            gameObject.layer = defaultLayerMask;
        }

        private void Update()
        {
            if (rightHand != null && rightHand.activeSelf && Equals(photonView.Owner, PhotonNetwork.LocalPlayer))
            {
                Transform transformThis;
                (transformThis = transform).position =
                    Vector3.MoveTowards(transformThis.position, currentParent.position + Vector3.up * 0.045f,
                        Time.deltaTime * 50);

                transform.rotation = Quaternion.Lerp(transformThis.rotation,
                    Quaternion.Euler(currentParent.rotation.eulerAngles + Vector3.forward * 90), Time.deltaTime * 100);
            }

            if (leftHand != null && leftHand.activeSelf && Equals(photonView.Owner, PhotonNetwork.LocalPlayer))
            {
                Transform transformThis;
                (transformThis = transform).position =
                    Vector3.MoveTowards(transformThis.position, currentParent.position + Vector3.up * 0.045f,
                        Time.deltaTime * 50);

                transform.rotation = Quaternion.Lerp(transformThis.rotation,
                    Quaternion.Euler(currentParent.rotation.eulerAngles + Vector3.back * 90), Time.deltaTime * 100);
            }
        }

        [PunRPC]
        public void ShowHideLeftHand(bool isActive)
        {
            leftHand.SetActive(isActive);

            if (!isActive)
                rigidbody.isKinematic = !Equals(photonView.Owner, PhotonNetwork.LocalPlayer);
            else
                rigidbody.isKinematic = true;
        }

        [PunRPC]
        public void ShowHideRightHand(bool isActive)
        {
            rightHand.SetActive(isActive);

            if (!isActive)
                rigidbody.isKinematic = !Equals(photonView.Owner, PhotonNetwork.LocalPlayer);
            else
                rigidbody.isKinematic = true;
        }
    }
}