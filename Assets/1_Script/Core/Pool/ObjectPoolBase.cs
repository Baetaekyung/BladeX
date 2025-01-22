using System.Collections.Generic;

namespace Swift_Blade.Pool
{
    internal static class ObjectPoolBase
    {
        internal static bool collisionCheck = false;
        static ObjectPoolBase()
        {
#if UNITY_EDITOR
            collisionCheck = true;
#endif  
        }

    }

    public abstract class ObjectPoolBase<T>
        where T : class
    {
        protected readonly List<T> poolList;
        private readonly int maxCapacity;
        public ObjectPoolBase(int initialPoolCapacity = 10, int maxCapacity = 1000, int preCreate = 10)
        {
            poolList = new List<T>(initialPoolCapacity);
            this.maxCapacity = maxCapacity;
            //for (int i = 0; i < preCreate; i++)
            //{
            //    Push(Create());
            //}
        }

        public int Dbg_Cnt()
        {
            return poolList.Count;
        }
        public T Pop()
        {
            T result;
            int lastIndex = poolList.Count - 1;
            if (lastIndex == -1)
                result = Create();
            else
            {
                result = poolList[lastIndex];
                poolList.RemoveAt(lastIndex);
            }
            OnPop(result);
            return result;
        }
        public void Push(T instance)
        {
            if (poolList.Count < maxCapacity)
            {
                poolList.Add(instance);
                OnPush(instance);
            }
            else
                OnDestroy(instance);
        }
        protected abstract T Create();
        protected virtual void OnDestroy(T instance)
        {
        }
        protected virtual void OnPop(T instance)
        {
        }
        protected virtual void OnPush(T instance)
        {
        }
        public virtual void Clear()
        {
            poolList.Clear();
        }
    }
}
