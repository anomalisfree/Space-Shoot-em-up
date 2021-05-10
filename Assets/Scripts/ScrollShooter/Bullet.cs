using UnityEngine;

namespace ScrollShooter
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private int damage = 10;
        [SerializeField] private float destroyDistanceLong = 10;
        [SerializeField] private float destroyDistanceWidth = 5;
        [SerializeField] private GameObject explosion;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private new ParticleSystem particleSystem;
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        public void Initialize(Color color)
        {
            meshRenderer.material.color = color;
            meshRenderer.material.SetColor(EmissionColor, color);
            var particleSystemMain = particleSystem.main;
            particleSystemMain.startColor = color;
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));

            if (Mathf.Abs(transform.localPosition.z) > destroyDistanceLong ||
                Mathf.Abs(transform.localPosition.x) > destroyDistanceWidth)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.GetComponent<Health>() != null)
            {
                other.gameObject.GetComponent<Health>().Decrease(damage);
            }

            var transformThis = transform;
            var mainModule = Instantiate(explosion, transformThis.position, transformThis.rotation)
                .GetComponent<ParticleSystem>().main;
            mainModule.startColor = meshRenderer.material.color;
            Destroy(gameObject);
        }
    }
}