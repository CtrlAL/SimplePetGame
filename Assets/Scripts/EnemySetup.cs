using UnityEngine;

public class EnemySetup : MonoBehaviour
{
    void Awake()
    {
        var input = gameObject.GetComponent<IMoveInput>();
        var mover = gameObject.GetComponent<Mover>();
        mover.SetInput(input);
    }
}
