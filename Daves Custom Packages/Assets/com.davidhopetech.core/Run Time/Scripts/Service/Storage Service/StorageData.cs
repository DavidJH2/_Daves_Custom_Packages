using System;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable] 
public class StorageData
{
	private string         playerPrefsKey;
	public  Action<string> ChangeEvent = delegate { };
	
	[SerializeField] private string _value = "";
	
	
	public StorageData(string iPlayerPrefsKey)
	{
		playerPrefsKey = iPlayerPrefsKey;
	}
	
	
	
	public string value
	{
		get
		{
			if (playerPrefsKey != "")
			{
				var v = PlayerPrefs.GetString(playerPrefsKey, "");
				return v;
			}

			return _value;
		}
		set
		{
			if (playerPrefsKey != "")
			{
				PlayerPrefs.SetString(playerPrefsKey, value);
			}
			else
			{
				this._value = value;
			}

			ChangeEvent.Invoke(this._value);					
		}
	}
}