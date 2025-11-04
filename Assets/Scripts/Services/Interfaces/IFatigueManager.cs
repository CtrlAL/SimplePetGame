namespace Services.Interfaces
{
    public interface IFatigueManager
    {
        public void MakeFatigueDamage(float damage);
        public float GetKnockbackMultiplier(float maxMultiplier = 2f);
    }
}
