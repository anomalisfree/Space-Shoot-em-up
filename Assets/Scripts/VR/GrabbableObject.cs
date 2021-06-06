using System.Collections;
using UnityEngine;
using UnityEngine.XR;

namespace VR
{
    public class GrabbableObject : MonoBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private new BoxCollider collider;

        private const float WaitingForChangingLayer = 0.2f;

        private Transform defaultParent;
        private LayerMask defaultLayerMask;

        private void Start()
        {
            defaultParent = transform.parent;
            defaultLayerMask = gameObject.layer;
        }

        public BoxCollider Grab(Transform parent, LayerMask layerMask)
        {
            StopAllCoroutines();
            
            transform.parent = parent;
            gameObject.layer = layerMask;

            rigidbody.isKinematic = true;

            return collider;
        }

        public void Throw(XRNodeState state)
        {
            transform.parent = defaultParent;

            rigidbody.isKinematic = false;
            
            state.TryGetVelocity(out var velocity);
            rigidbody.velocity = velocity;

            state.TryGetAngularVelocity(out var angularVelocity);
            rigidbody.angularVelocity = angularVelocity;

            StartCoroutine(SetDefaultMask());
        }

        private IEnumerator SetDefaultMask()
        {
            yield return new WaitForSeconds(WaitingForChangingLayer);
            gameObject.layer = defaultLayerMask;
        }
    }
}
