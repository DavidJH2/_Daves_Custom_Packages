using System;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable] 
public class StorageData
{
	public Action<string> ChangeEvent = delegate { };
	
	[SerializeField] private string _value = "";
	
	public string value
	{
		get => _value;
		set
		{
			this._value = value;
			ChangeEvent.Invoke(this._value);					
		}
	}
}