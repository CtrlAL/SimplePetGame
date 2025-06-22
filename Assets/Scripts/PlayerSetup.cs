using UnityEngine;


namespace Assets.Scripts
{
    public class PlayerSetup : MonoBehaviour
    {
        void Awake()
        {
            var input = FindObjectOfType<PlayerInput>();
            var mover = gameObject.GetComponent<Mover>();
            mover.SetInput(input);
        }
    }
}

