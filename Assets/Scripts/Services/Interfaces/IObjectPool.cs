namespace Services.Interfaces
{
    public interface IObjectPool<T>
    {
        public T SpawnObject();
        public void ReturnToPool(T gameObject);
    }
}
