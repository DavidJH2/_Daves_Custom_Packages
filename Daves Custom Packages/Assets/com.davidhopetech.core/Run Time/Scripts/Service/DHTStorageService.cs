using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace com.davidhopetech.core.Run_Time.Scripts.Service
{
	[System.Serializable]
	public class DHTStorageService : DHTService<DHTStorageService>
	{
		private Dictionary<string, StorageData> data = new();

		public void AddData(string dataName)
		{
			var item = data.TryAdd(dataName, new StorageData());
			if (item == null) throw new Exception("DataStroage data already added");
		}

		public StorageData this[string str]
		{
			get
			{
				if (!data.TryGetValue(str, out var dataItem))
				{
					dataItem = (data[str] = new StorageData());
				}

				return dataItem;
			}
		}

		private void OnValidate()
		{
			foreach (var dataItemPair in data)
			{
				var dataItem = dataItemPair.Value;
				dataItem.ChangeEvent?.Invoke(dataItem._value);
			}
		}
	}
}
