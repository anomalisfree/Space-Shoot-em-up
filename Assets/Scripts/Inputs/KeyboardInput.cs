using ScrollShooter;
using UnityEngine;

namespace Inputs
{
    public class KeyboardInput : MonoBehaviour
    {
        [SerializeField] private AircraftMovement aircraftMovement;
        [SerializeField] private float blindArea;
        [SerializeField] private AircraftShooter aircraftShooter;

        private void Update()
        {
            aircraftMovement.HorizontalMoving(Input.GetAxis("Horizontal"), blindArea);
            aircraftMovement.VerticalMoving(Input.GetAxis("Vertical"), blindArea);

            if (Input.GetButtonDown("Fire1"))
            {
                aircraftShooter.Shoot();
            }
        }
    }
}
