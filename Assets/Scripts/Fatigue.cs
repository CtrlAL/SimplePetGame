using UnityEngine;

public class Fatigue : MonoBehaviour
{
    [SerializeField]
    private float _fatigueThrashold = 100f;

    public float FatigueThrashold => _fatigueThrashold;

    public float CurrentFatigue { get; private set; } = 0f;

    public void MakeFatigueDamake(int damage)
    {
        if (CurrentFatigue + damage < _fatigueThrashold)
        {
            CurrentFatigue += damage;
        }
    }

    public void Reset(int damage)
    {
        CurrentFatigue = 0f;
    }
}
