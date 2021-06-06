using System.Collections.Generic;
using UnityEngine;

namespace VR
{
    public class Button : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Transform centerOfButton;
        
        private readonly List<Collider> colliders = new List<Collider>();

        private void Update()
        {
            var centerOfButtonLocalPosition = centerOfButton.localPosition;
            centerOfButtonLocalPosition = colliders.Count > 0
                ? Vector3.MoveTowards(centerOfButtonLocalPosition, Vector3.down * 0.02f, Time.deltaTime)
                : Vector3.MoveTowards(centerOfButtonLocalPosition, Vector3.zero, Time.deltaTime);
            centerOfButton.localPosition = centerOfButtonLocalPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((layerMask.value & (1 << other.gameObject.layer)) > 0) 
            {
                if (!colliders.Contains(other))
                {
                    colliders.Add(other);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if ((layerMask.value & (1 << other.gameObject.layer)) > 0) 
            {
                if (colliders.Contains(other))
                {
                    colliders.Remove(other);
                }
            }
        }
    }
}