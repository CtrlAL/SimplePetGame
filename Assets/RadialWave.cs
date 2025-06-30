using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class RadialWave : MonoBehaviour
    {
        public float maxRadius = 5f;
        public float duration = 2f;
        public bool loop = true;
        public bool destroyAfter = false;
        public float speedMultiplier = 1f;

        private Vector3 startScale;
        private Color startColor;
        private float elapsedTime;

        void Start()
        {
            startScale = transform.localScale;
            startColor = GetComponent<SpriteRenderer>().color;
            elapsedTime = 0f;
        }

        void Update()
        {
            elapsedTime += Time.deltaTime * speedMultiplier;
            float normalizedTime = Mathf.Clamp01(elapsedTime / duration);

            if (normalizedTime >= 1f)
            {
                if (destroyAfter)
                {
                    Destroy(gameObject);
                    return;
                }

                if (loop)
                {
                    elapsedTime = 0f;
                    transform.localScale = startScale;
                    GetComponent<SpriteRenderer>().color = startColor;
                    return;
                }
            }

            float scale = Mathf.Lerp(0, maxRadius, normalizedTime);
            Color newColor = new Color(startColor.r, startColor.g, startColor.b, 1 - normalizedTime);

            transform.localScale = Vector3.one * scale;
            GetComponent<SpriteRenderer>().color = newColor;
        }
    }
}