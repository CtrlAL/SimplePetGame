using System.Collections;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField] 
    private GameObject _stunnedIcon;

    [SerializeField]
    private int _maxHitCount = 0;

    private int _currentHitCount = 0;

    private bool _isStuned = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isStuned)
        {
            if (collision.gameObject.CompareTag("Environment"))
            {
                StartCoroutine(ObjectStuned());
            }

            else if (collision.gameObject.CompareTag("Throwable"))
            {
                _currentHitCount++;

                if (_currentHitCount == _maxHitCount)
                {
                    StartCoroutine(ObjectStuned());
                }
            }
        }
    }

    private IEnumerator ObjectStuned()
    {
        _isStuned = true;
        _currentHitCount = 0;

        var rb = gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            _stunnedIcon.SetActive(true);
            gameObject.tag = "Throwable";
        }

        yield return new WaitForSeconds(10f);

        if (rb != null)
        {
            rb.isKinematic = false;
        }

        gameObject.tag = "Enemy";
        _stunnedIcon.SetActive(false);
        _isStuned = false;
    }
}
