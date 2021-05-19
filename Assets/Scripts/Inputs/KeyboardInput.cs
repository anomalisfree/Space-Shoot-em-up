using ScrollShooter;
using UnityEngine;

namespace Inputs
{
    public class KeyboardInput : MonoBehaviour
    {
        [SerializeField] private float blindArea;
        [SerializeField] private AircraftPlayerController aircraftPlayerController;

        private void Update()
        {
            aircraftPlayerController.Movement(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")),
                blindArea);

            if (Input.GetButtonDown("Fire1"))
            {
                aircraftPlayerController.Shoot();
            }
        }
    }
}