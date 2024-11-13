using UnityEngine;
using System.Collections;

namespace Utils
{
    public class MonoSingleton<T> : MonoBehaviour where T: MonoBehaviour 
    {

        static T m_instance;

        public static T Instance
        {
            get 
            {
                if (m_instance == null) 
                {
                    m_instance = GameObject.FindObjectOfType<T> ();

                    if (m_instance == null) 
                    {
                        GameObject singleton = new GameObject (typeof(T).Name);
                        m_instance = singleton.AddComponent<T> ();
                    }
                }
                return m_instance;
            }
        }

        public virtual void Awake()
        {
            if (m_instance == null) 
            {
                m_instance = this as T;
                transform.parent = null;
                //DontDestroyOnLoad (this.gameObject);
            } 
            else 
            {
                Destroy (gameObject);
            }
        }
        
        private void OnApplicationQuit()
        {
            m_instance = null;
        }

        private void OnDestroy()
        {
            if (m_instance == this)
            {
                m_instance = null;
            }
        }
    }
}
