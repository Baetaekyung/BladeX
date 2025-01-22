using UnityEngine;

namespace Swift_Blade.Pool.CSharp
{
    internal class ObjectPool<T> : ObjectPoolBase<T> 
        where T : class, new()
    {
        public ObjectPool(int initialPoolCapacity = 100, int maxCapacity = 1000, int preCreate = 10) : base(initialPoolCapacity, maxCapacity, preCreate)
        {
        }

        protected override T Create()
        {
            return new T();
        }
    }
}
