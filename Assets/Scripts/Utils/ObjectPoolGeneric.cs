using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ObjectPoolGeneric<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private T prefab;                // Prefab to instantiate for the pool
        [SerializeField] private int initialPoolSize = 10; // Initial number of objects in the pool
        [SerializeField] private bool expandable = true;   // Allow expanding the pool if necessary

        private Queue<T> pool = new Queue<T>();

        private void Awake()
        {
            InitializePool();
        }

        // Initializes the pool with the specified number of objects
        private void InitializePool()
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateNewObject();
            }
        }

        // Gets an object from the pool, activating it and returning it
        public T Get()
        {
            if (pool.Count == 0 && expandable)
            {
                CreateNewObject();
            }

            if (pool.Count > 0)
            {
                T obj = pool.Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }

            return null;
        }

        // Returns an object to the pool, deactivating it
        public void ReturnToPool(T obj)
        {
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }

        // Creates a new object, adds it to the pool, and deactivates it
        private T CreateNewObject()
        {
            T newObj = Instantiate(prefab, transform);
            newObj.gameObject.SetActive(false);
            pool.Enqueue(newObj);
            return newObj;
        }
    }

}
