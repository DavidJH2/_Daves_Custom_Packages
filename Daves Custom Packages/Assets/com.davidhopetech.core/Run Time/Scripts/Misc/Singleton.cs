using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T      _instance;
	private static object _lock          = new object();
	private static bool   _isInitialized = false;

	public static T Instance
	{
		get
		{
			lock (_lock)
			{
				if (!_isInitialized)
				{
#if UNITY_2022_1_OR_NEWER && !UNITY_2022
					T[] instances = DHTFindObjectsByType<T>(FindObjectsSortMode.None);
#else					
					T[] instances = FindObjectsOfType<T>();
#endif
					if (instances.Length > 0)
					{
						if (instances.Length > 1)
						{
							Debug.LogError("[Singleton] There should never be more than 1 singleton!");
						}

						_instance      = instances[0];
						_isInitialized = true;
						DontDestroyOnLoad(_instance.gameObject);
					}
					else
					{
						GameObject singletonObject = new GameObject();
						_instance            = singletonObject.AddComponent<T>();
						singletonObject.name = typeof(T).ToString() + " (Singleton)";

						DontDestroyOnLoad(singletonObject);
						_isInitialized = true;
					}
				}

				return _instance;
			}
		}
	}

	protected virtual void Awake()
	{
		if (_instance == null)
		{
			_instance = this as T;
		}
		else if (_instance != this)
		{
			Destroy(gameObject);
		}
	}
}