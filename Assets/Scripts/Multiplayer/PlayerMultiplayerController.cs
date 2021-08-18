using System.Collections.Generic;
using Photon.Pun;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;
using UnityEngine.SpatialTracking;
using VR;

namespace Multiplayer
{
    public class PlayerMultiplayerController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject head;

        [SerializeField] private new Camera camera;
        [SerializeField] private AudioListener audioListener;

        [SerializeField] private CameraOffset cameraOffset;
        
        [SerializeField] private List<TrackedPoseDriver> trackedPoseDrivers = new List<TrackedPoseDriver>();

        [SerializeField]
        private List<PlayerHandAnimationController> playerHandAnimationControllers =
            new List<PlayerHandAnimationController>();

        [SerializeField] private List<Grabber> grabbers = new List<Grabber>();
        [SerializeField] private List<LeverGrabber> leverGrabbers = new List<LeverGrabber>();
        [SerializeField] private List<Rigidbody> rigidbodies = new List<Rigidbody>();
        
        
        private void Start()
        {
            if (photonView.IsMine)
            {
                head.SetActive(false);

                cameraOffset.enabled = true;

                camera.enabled = true;

                audioListener.enabled = true;

                foreach (var trackedPoseDriver in trackedPoseDrivers)
                {
                    trackedPoseDriver.enabled = true;
                }

                foreach (var playerHandAnimationController in playerHandAnimationControllers)
                {
                    playerHandAnimationController.enabled = true;
                }

                foreach (var grabber in grabbers)
                {
                    grabber.enabled = true;
                }

                foreach (var leverGrabber in leverGrabbers)
                {
                    leverGrabber.enabled = true;
                }

                foreach (var rb in rigidbodies)
                {
                    rb.isKinematic = false;
                }
            }
            else
            {
                head.SetActive(true);
                
                cameraOffset.enabled = false;
                
                camera.enabled = false;

                audioListener.enabled = false;
                
                foreach (var trackedPoseDriver in trackedPoseDrivers)
                {
                    trackedPoseDriver.enabled = false;
                }
                
                foreach (var playerHandAnimationController in playerHandAnimationControllers)
                {
                    playerHandAnimationController.enabled = false;
                }

                foreach (var grabber in grabbers)
                {
                    grabber.enabled = false;
                }

                foreach (var leverGrabber in leverGrabbers)
                {
                    leverGrabber.enabled = false;
                }
                
                foreach (var rb in rigidbodies)
                {
                    rb.isKinematic = true;
                }
            }
        }
    }
}