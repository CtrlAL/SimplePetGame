namespace Services.Interfaces
{
    public interface IFatigue
    {
        public void MakeFatigueDamage(float damage);
        public float GetKnockbackMultiplier(float maxMultiplier = 2f);
    }
}
