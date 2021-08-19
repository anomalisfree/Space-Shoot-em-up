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
        private LayerMask defaultLayerMask;

        public Action<PlayerHandAnimationController> IsThrown;
        public Action<PlayerHandAnimationController> IsGrabbed;


        private void Start()
        {
            defaultParent = transform.parent;
            defaultLayerMask = gameObject.layer;
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

            transform.parent = parent;
            gameObject.layer = layerMask;

            rigidbody.isKinematic = true;
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

            transform.parent = defaultParent;

            if (Equals(photonView.Owner, PhotonNetwork.LocalPlayer))
                rigidbody.isKinematic = false;

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
                (transformThis = transform).localPosition =
                    Vector3.MoveTowards(transformThis.localPosition, Vector3.up * 0.045f, Time.deltaTime * 50);

                transform.localRotation = Quaternion.Lerp(transformThis.localRotation,
                    Quaternion.Euler(Vector3.forward * 90), Time.deltaTime * 100);
            }

            if (leftHand != null && leftHand.activeSelf && Equals(photonView.Owner, PhotonNetwork.LocalPlayer))
            {
                Transform transformThis;
                (transformThis = transform).localPosition =
                    Vector3.MoveTowards(transformThis.localPosition, Vector3.up * 0.045f, Time.deltaTime * 50);

                transform.localRotation = Quaternion.Lerp(transformThis.localRotation,
                    Quaternion.Euler(Vector3.back * 90), Time.deltaTime * 100);
            }
        }

        [PunRPC]
        public void ShowHideLeftHand(bool isActive)
        {
            leftHand.SetActive(isActive);
        }

        [PunRPC]
        public void ShowHideRightHand(bool isActive)
        {
            rightHand.SetActive(isActive);
        }
    }
}