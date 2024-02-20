using System;
using System.Collections.Generic;
using com.davidhopetech.vr.Run_Time.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace com.davidhopetech.core.Run_Time.Scripts.Service
{
	[System.Serializable]
	public class DHTStorageServiceTemplate<T> : DHTService<DHTStorageServiceTemplate<T>>
	{
		private Dictionary<T, StorageData> allStorageDatas = new();

		public void AddData(T dataName)
		{
			var itemWasNotAdded = !allStorageDatas.TryAdd(dataName, new StorageData(""));
			if (itemWasNotAdded) throw new Exception("DataStroage allStorageDatas already added");
		}

		public StorageData GetData(T str, string defaultValue, string PlayerPrefsKey)
		{
			if (!allStorageDatas.TryGetValue(str, out var dataItem))
			{
				dataItem = (allStorageDatas[str] = new StorageData(PlayerPrefsKey));
				if (PlayerPrefsKey == "" && defaultValue != "")
				{
					dataItem.value = defaultValue;
				}
				else
				{
					var value = dataItem.value;
				}
			}

			return dataItem;
		}

		private void OnValidate()
		{
			foreach (var dataItemPair in allStorageDatas)
			{
				var dataItem = dataItemPair.Value;
				dataItem.ChangeEvent?.Invoke(dataItem.value);
			}
		}
	}
}
