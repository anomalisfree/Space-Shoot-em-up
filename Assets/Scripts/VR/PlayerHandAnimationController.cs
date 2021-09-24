using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.Oculus;
using UnityEngine;
using UnityEngine.XR;

namespace VR
{
    public class PlayerHandAnimationController : MonoBehaviour
    {
        [SerializeField] private XRNode deviceType;
        [SerializeField] private Animator animator;
    
        [Header("Vibration when touched")]
        [SerializeField] private bool useTouchVibration;
        [SerializeField] private float vibrationMultiplier = 1;
        [SerializeField] [Range(0,1)] private float vibrationMax = 1;

        public Action<float> FlexChanged;
        public Action<float> PinchChanged;

        private const string AnimLayerNamePoint = "Point Layer";
        private const string AnimLayerNameThumb = "Thumb Layer";
        private const string AnimParamNameFlex = "Flex";
        private const string AnimParamNamePose = "Pose";
        private const string AnimParamNamePinch = "Pinch";

        private const float InputRateChange = 20.0f;

        private int animLayerIndexPoint;
        private int animLayerIndexThumb;
        private int animParamIndexFlex;
        private int animParamIndexPose;
        private int animParamIndexPinch;

        private float pointBlend;
        private float thumbsUpBlend;
        private int pose;

        private float flex;
        private float lastFlex;
        private float pinch;
        private float lastPinch;

        private InputDevice device;
        private Transform parent;
        private bool vibrateNow;

        private void Start()
        {
            animLayerIndexPoint = animator.GetLayerIndex(AnimLayerNamePoint);
            animLayerIndexThumb = animator.GetLayerIndex(AnimLayerNameThumb);
            animParamIndexFlex = Animator.StringToHash(AnimParamNameFlex);
            animParamIndexPose = Animator.StringToHash(AnimParamNamePose);
            animParamIndexPinch = Animator.StringToHash(AnimParamNamePinch);
            parent = transform.parent;
        }

        private void Update()
        {
            if (device == default)
            {
                var handDevices = new List<InputDevice>();
                InputDevices.GetDevicesAtXRNode(deviceType, handDevices);

                if (handDevices.Count == 1)
                {
                    device = handDevices[0];
                }
                else if (handDevices.Count > 1)
                {
                    Debug.Log($"Found more than one {deviceType}");
                }
            }
            else
            {
                UpdateAnimStates();

                if (useTouchVibration)
                {
                    UpdateVibration();
                }
            }
        }

        public void SetPose(int newPose)
        {
            pose = newPose;
            animator.SetInteger(animParamIndexPose, pose);
        }
        
        public bool GetLocomotionButtonPressed()
        {
            device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out var buttonPressed);
            return buttonPressed;
        }

        public Vector2 GetStickPos()
        {
            device.TryGetFeatureValue(CommonUsages.primary2DAxis, out var stickPose);
            return stickPose;
        }

        private void UpdateAnimStates()
        {
            device.TryGetFeatureValue(CommonUsages.grip, out flex);

            if (Math.Abs(flex - lastFlex) > 0.01f)
            {
                animator.SetFloat(animParamIndexFlex, flex);
                FlexChanged?.Invoke(flex);
                lastFlex = flex;
            }

            device.TryGetFeatureValue(CommonUsages.trigger, out pinch);

            if (Math.Abs(pinch - lastPinch) > 0.01f)
            {
                animator.SetFloat(animParamIndexPinch, pinch);
                PinchChanged?.Invoke(pinch);
                lastPinch = pinch;
            }

            if (pose == 0)
            {
                device.TryGetFeatureValue(OculusUsages.indexTouch, out var isPointing);
                pointBlend = InputValueRateChange(!isPointing, pointBlend);
                animator.SetLayerWeight(animLayerIndexPoint, pointBlend);

                device.TryGetFeatureValue(OculusUsages.thumbTouch, out var isGivingThumbsUp);
                thumbsUpBlend = InputValueRateChange(!isGivingThumbsUp, thumbsUpBlend);
                animator.SetLayerWeight(animLayerIndexThumb, thumbsUpBlend);
            }
            else
            {
                animator.SetLayerWeight(animLayerIndexPoint, 0);
                animator.SetLayerWeight(animLayerIndexThumb, 0);
            }
        }

        private void UpdateVibration()
        {
            if (vibrateNow) return;
            
            var vibrationForce = Vector3.Distance(transform.position, parent.position) * vibrationMultiplier;

            if (vibrationForce > vibrationMax)
                vibrationForce = vibrationMax;

            device.SendHapticImpulse(0, vibrationForce, Time.deltaTime);
        }

        private static float InputValueRateChange(bool isDown, float value)
        {
            var rateDelta = Time.deltaTime * InputRateChange;
            var sign = isDown ? 1.0f : -1.0f;
            return Mathf.Clamp01(value + rateDelta * sign);
        }

        public XRNodeState GetState()
        {
            var nodes = new List<XRNodeState>();
            InputTracking.GetNodeStates (nodes);

            foreach (var node in nodes.Where(node => node.nodeType == deviceType))
            {
                return node;
            }

            return default;
        }

        public Transform GetParent()
        {
            return parent;
        }

        public XRNode GetDeviceType()
        {
            return deviceType;
        }

        public void Vibrate(float amplitude, float time)
        {
            vibrateNow = true;
            device.SendHapticImpulse(0, amplitude, time);
            StopAllCoroutines();
            StartCoroutine(VibrateTimer(time));
        }

        private IEnumerator VibrateTimer(float time)
        {
            yield return new WaitForSeconds(time);
            vibrateNow = false;
        }
    }
}