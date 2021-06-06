using System;
using UnityEngine;
using VR.ScriptableObjects.LeverParams;

namespace VR
{
    public class Lever : MonoBehaviour
    {
        [SerializeField] private LeverParams leverParams;
        [SerializeField] private Transform rotationPivot;

        public Transform handPivot;
        public Transform handParentPivot;

        public Action IsBreakDown;

        private void Update()
        {
            if (Vector3.Distance(handPivot.position, handParentPivot.position) > leverParams.maxGrabDistance)
            {
                IsBreakDown?.Invoke();
            }
            else
            {
                SetLeverDuoRotation();
            }
        }

        private void SetLeverDuoRotation()
        {
            var localPivotRotation = rotationPivot.localRotation;
            var localHandParentPivotPosition = handParentPivot.localPosition;

            var rotY = localPivotRotation.y + localHandParentPivotPosition.z;
            var rotZ = localPivotRotation.z - localHandParentPivotPosition.x;

            if (rotY < leverParams.minLeverPosition)
                rotY = leverParams.minLeverPosition;

            if (rotY > leverParams.maxLeverPosition)
                rotY = leverParams.maxLeverPosition;

            if (rotZ < leverParams.minLeverPosition)
                rotZ = leverParams.minLeverPosition;

            if (rotZ > leverParams.maxLeverPosition)
                rotZ = leverParams.maxLeverPosition;

            rotationPivot.localRotation =
                new Quaternion(0, rotY, rotZ, 1);

            if (handParentPivot.localPosition == Vector3.zero)
            {
                rotationPivot.localRotation =
                    Quaternion.Lerp(rotationPivot.localRotation, Quaternion.identity,
                        Time.deltaTime * leverParams.returnSpeed);
            }
        }
    }
}