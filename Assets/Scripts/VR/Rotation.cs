using System;
using UnityEngine;

namespace VR
{
    public class Rotation : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private PlayerHandAnimationController playerHandAnimationController;
        [SerializeField] private float angle;
        [SerializeField] private Transform headTransform;
        [SerializeField] private WallDetector wallDetector;

        private bool isTurned;

        private void Update()
        {
            if (!isTurned && playerHandAnimationController.GetStickPos().x > 0.85f)
            {
                isTurned = true;
                playerTransform.RotateAround(headTransform.position, Vector3.up, angle);
                wallDetector.SetScreenDark();
            }
            
            if (!isTurned && playerHandAnimationController.GetStickPos().x < -0.85f)
            {
                isTurned = true;
                playerTransform.RotateAround(headTransform.position, Vector3.up, -angle);
                wallDetector.SetScreenDark();
            }

            if (isTurned && Mathf.Abs(playerHandAnimationController.GetStickPos().x) < 0.1f)
            {
                isTurned = false;
            }
        }
    }
}
