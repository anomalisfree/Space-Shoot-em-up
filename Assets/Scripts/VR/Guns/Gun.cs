using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

namespace VR.Guns
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private GrabbableObject grabObject;
        [SerializeField] private float pressForceDetected = 0.5f;
        [SerializeField] private GameObject muzzleFlash;
        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform muzzleFlashPoint;
        [SerializeField] private Animator animator;

        [Header("Vibration")] [SerializeField] [Range(0f, 1f)]
        private float vibrationAmplitude;
        [SerializeField] private float vibrationTime;

        private float oldPinchParam;
        private PlayerHandAnimationController playerController;
        private static readonly int ShootTrigger = Animator.StringToHash("shoot");

        private void Start()
        {
            grabObject.IsGrabbed += IsGrabbed;
            grabObject.IsThrown += IsThrown;
        }

        private void IsThrown(PlayerHandAnimationController playerHandAnimationController)
        {
            playerHandAnimationController.PinchChanged -= PinchChanged;
        }

        private void IsGrabbed(PlayerHandAnimationController playerHandAnimationController)
        {
            playerHandAnimationController.PinchChanged += PinchChanged;
            playerController = playerHandAnimationController;
        }

        private void PinchChanged(float param)
        {
            if (oldPinchParam < pressForceDetected && param > pressForceDetected)
            {
                Shoot();
            }

            oldPinchParam = param;
        }

        private void Shoot()
        {
            var position = muzzleFlashPoint.position;
            var rotation = muzzleFlashPoint.rotation;
            PhotonNetwork.Instantiate(muzzleFlash.name, position, rotation);
            PhotonNetwork.Instantiate(bullet.name, position, rotation);
            playerController.Vibrate(vibrationAmplitude, vibrationTime);
            animator.SetTrigger(ShootTrigger);
        }
    }
}