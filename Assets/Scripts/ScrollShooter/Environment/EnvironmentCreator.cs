using UnityEngine;

namespace ScrollShooter.Environment
{
    public class EnvironmentCreator : MonoBehaviour
    {
        [SerializeField] private Vector2 envSize = new Vector2(8, 12);
        [SerializeField] private Vector2 cubeScaleRange = new Vector2(0.1f, 2f);
        [SerializeField] private float movingSpeed = 0.01f;
        [SerializeField] private GameObject environmentCubePrefab;
        [SerializeField] private Color randomColorMin;
        [SerializeField] private Color randomColorMax;

        private void Start()
        {
            for (var i = -envSize.x; i <= envSize.x; i++)
            {
                for (var j = -envSize.y + 1; j <= envSize.y; j++)
                {
                    Instantiate(environmentCubePrefab, transform).GetComponent<EnvironmentCube>()
                        .Initialize(new Vector3(i, 0, j), cubeScaleRange, envSize.y,
                            randomColorMin, randomColorMax, movingSpeed);
                }
            }
        }
    }
}