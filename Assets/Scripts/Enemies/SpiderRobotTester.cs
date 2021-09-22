using UnityEngine;

namespace Enemies
{
    public class SpiderRobotTester : MonoBehaviour
    {
        [SerializeField] private SpiderRobotController spiderRobotController;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Clicked(0);
            }

            if (Input.GetMouseButtonDown(1))
            {
                Clicked(1);
            }
        }

        private void Clicked(int type)
        {
            if (Camera.main is null) return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit)) return;

            Debug.Log(hit.point);

            switch (type)
            {
                case 0:
                    spiderRobotController.SetMoveTarget(hit.point);
                    break;
                case 1:
                    spiderRobotController.SetShootTarget(hit.transform);
                    break;
            }
        }
    }
}