using UnityEngine;

namespace VR
{
    public class LocomotionPoint : MonoBehaviour
    {
        [SerializeField] private GameObject imageObject;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private Color activeColor;
        [SerializeField] private Color unActiveColor;

        private void Start()
        {
            Hide();
        }

        public void Show()
        {
            imageObject.SetActive(true);
            boxCollider.enabled = true;
        }

        public void Hide()
        {
            imageObject.SetActive(false);
            boxCollider.enabled = false;
        }

        public void SetActive()
        {
            sprite.color = activeColor;
        }

        public void SetUnActive()
        {
            sprite.color = unActiveColor;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}
