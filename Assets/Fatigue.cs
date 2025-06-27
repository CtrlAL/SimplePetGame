using UnityEngine;

public class Fatigue : MonoBehaviour
{
    [SerializeField]
    public float CurrentFatigue = 0f;

    [SerializeField]
    public float FatigueThrashold = 100f;

    public void MakeFatigueDamake(int damage)
    {
        if (CurrentFatigue + damage < FatigueThrashold)
        {
            CurrentFatigue += damage;
        }
    }
}
