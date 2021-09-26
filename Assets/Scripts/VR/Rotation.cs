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

        private bool isTurned;

        private void Update()
        {
            if (!isTurned && playerHandAnimationController.GetStickPos().x > 0.85f)
            {
                isTurned = true;
                playerTransform.RotateAround(headTransform.position, Vector3.up, angle);
            }
            
            if (!isTurned && playerHandAnimationController.GetStickPos().x < -0.85f)
            {
                isTurned = true;
                playerTransform.RotateAround(headTransform.position, Vector3.up, -angle);
            }

            if (isTurned && Mathf.Abs(playerHandAnimationController.GetStickPos().x) < 0.1f)
            {
                isTurned = false;
            }
        }
    }
}
