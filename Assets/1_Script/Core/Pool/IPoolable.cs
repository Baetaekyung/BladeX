namespace Swift_Blade.Pool
{
    public interface IPoolable
    {
        public void OnPopInitialize();
    }

    //public interface IPoolable<T> : IPoolable
    //    where T : MonoBehaviour, IPoolable<T>
    //{
    //    public void Die()
    //    {
    //        MonoGenericPool<T>.Push(this as T);
    //    }
    //}
}
