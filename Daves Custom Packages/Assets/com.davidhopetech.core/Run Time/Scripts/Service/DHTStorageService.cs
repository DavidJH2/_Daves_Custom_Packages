using System;
using UnityEngine;

namespace com.davidhopetech.core.Run_Time.Scripts.Service
{
	[System.Serializable] 
	public class DHTStorageService : DHTService<DHTStorageService>
	{
		[System.Serializable] 
		public class PlayerData
		{
			public Action<string> NameChangeEvent;
			
			private string _name;
			public string Name
			{
				get => _name;
				set
				{
					_name = value;
					NameChangeEvent.Invoke(_name);					
				}
			}
		}

			
		public PlayerData playerData;
	}
}