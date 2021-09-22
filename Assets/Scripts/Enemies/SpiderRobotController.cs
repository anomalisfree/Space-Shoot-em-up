using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class SpiderRobotController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform shootDirection;
        [SerializeField] private Transform gunDirection;
        [SerializeField] private Transform targetIndicator;

        private Vector3 moveTarget;
        private Transform shootTarget;
        private Vector3 previousPosition;
        private static readonly int Walk = Animator.StringToHash("walk");


        public void SetMoveTarget(Vector3 target)
        {
            moveTarget = target;
            navMeshAgent.SetDestination(moveTarget);
        }

        public void SetShootTarget(Transform target)
        {
            shootTarget = target;
        }

        private void Update()
        {
            var position = transform.position;
            var currentMove = position - previousPosition;
            previousPosition = position;

            UpdateAnimation(currentMove.magnitude / Time.deltaTime);

            targetIndicator.LookAt(shootTarget);
            shootDirection.localRotation = Quaternion.Euler(0,
                Mathf.MoveTowards(shootDirection.localRotation.eulerAngles.y, targetIndicator.localRotation.eulerAngles.y,
                    Time.deltaTime * 500), 0);
            // gunDirection.localRotation =
            //     Quaternion.Euler(
            //         Mathf.MoveTowards(gunDirection.localRotation.eulerAngles.x, targetIndicator.localRotation.eulerAngles.x,
            //             Time.deltaTime * 500), 0, 0);
        }

        private void UpdateAnimation(float speed)
        {
            if (speed > 0)
            {
                animator.SetBool(Walk, true);
                animator.speed = speed;
            }
            else
            {
                animator.SetBool(Walk, false);
                animator.speed = 1;
            }
        }
    }
}