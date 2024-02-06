using System;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable] 
public class StorageData
{
	[FormerlySerializedAs("NameChangeEvent")] public Action<string> ChangeEvent;

	[FormerlySerializedAs("_name")] [SerializeField] private string value = "";
	public string _value
	{
		get => value;
		set
		{
			this.value = value;
			ChangeEvent.Invoke(this.value);					
		}
	}
}