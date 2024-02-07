using System;
using System.Collections.Generic;
using com.davidhopetech.vr.Run_Time.Scripts;
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
			var itemWasNotAdded = !data.TryAdd(dataName, new StorageData());
			if (itemWasNotAdded) throw new Exception("DataStroage data already added");
		}

		public StorageData GetData(string str, string defaultValue)
		{
			if (!data.TryGetValue(str, out var dataItem))
			{
				dataItem = (data[str] = new StorageData());
				if (defaultValue != "")
					dataItem.value = defaultValue;
			}

			return dataItem;
		}

		private void OnValidate()
		{
			foreach (var dataItemPair in data)
			{
				var dataItem = dataItemPair.Value;
				dataItem.ChangeEvent?.Invoke(dataItem.value);
			}
		}
	}
}
