using System;
using UnityEngine;



[System.Serializable]
public class DHTStorageDataGeneric<T> where T : IConvertible
{
	private                   string    playerPrefsKey;
	public                    Action<T> ChangeEvent = delegate { };
	[SerializeField] internal T         _value;

	
	
	public DHTStorageDataGeneric() {}
	
	public DHTStorageDataGeneric(string iPlayerPrefsKey)
	{
		playerPrefsKey = iPlayerPrefsKey;
	}

	
	public T value
	{
		get
		{
			if (playerPrefsKey != "")
			{
				if (typeof(T) == typeof(string))
				{
					var value = PlayerPrefs.GetString(playerPrefsKey, "");
					_value  = (T) (object) value;

					return _value;
				}
				else if (typeof(T) == typeof(int))
				{
					var value = PlayerPrefs.GetInt(playerPrefsKey, 0);
					_value = (T) (object) value;

					return _value;
				}
				else if (typeof(T) == typeof(float))
				{
					var value = PlayerPrefs.GetFloat(playerPrefsKey, 0);
					_value = (T) (object) value;

					return _value;
				}
				else
				{
					throw new Exception(">>>>>>>>>>  Unsupported Type  <<<<<<<<<<");
				}
			}

			return _value;
		}
		set
		{
			if (playerPrefsKey != "")
			{
				if (typeof(T) == typeof(string))
				{
					string val = (string)Convert.ChangeType(value, typeof(T));
					PlayerPrefs.SetString(playerPrefsKey, val);
				}
				else if (typeof(T) == typeof(int))
				{
					int val = (int)Convert.ChangeType(value, typeof(T));
					PlayerPrefs.SetInt(playerPrefsKey, val);
				}
				else if (typeof(T) == typeof(float))
				{
					float val = (float)Convert.ChangeType(value, typeof(T));
					PlayerPrefs.SetFloat(playerPrefsKey, 0);
				}
				else
				{
					throw new Exception(">>>>>>>>>>  Unsupported Type  <<<<<<<<<<");
				}


				ChangeEvent.Invoke(this.value);
			}
			
			_value = value;
		}
	}
}