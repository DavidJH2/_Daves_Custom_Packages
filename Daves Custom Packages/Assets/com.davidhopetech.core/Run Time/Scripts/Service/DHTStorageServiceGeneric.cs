

using System;
using System.Collections.Generic;


namespace com.davidhopetech.core.Run_Time.Scripts.Service
{
	[System.Serializable]
	public class DHTStorageServiceGeneric<T> : DHTService<DHTStorageServiceGeneric<T>> where T : IConvertible
	{
		private Dictionary<string, DHTStorageDataGeneric<T>> allStorageDatas = new();

		public void AddData(string dataName)
		{
			var itemWasNotAdded = !allStorageDatas.TryAdd(dataName, new DHTStorageDataGeneric<T>());
			if (itemWasNotAdded) throw new Exception("DataStroage allStorageDatas already added");
		}

		public DHTStorageDataGeneric<T> GetData(string dataItemName, T defaultValue = default(T))
		{
			var PlayerPrefsKey = $"{dataItemName}PF";
            
			if (!allStorageDatas.TryGetValue(dataItemName, out var dataItem))
			{
				dataItem = (allStorageDatas[dataItemName] = new DHTStorageDataGeneric<T>(PlayerPrefsKey, defaultValue));
				if (PlayerPrefsKey == "" && defaultValue.Equals(default(T)))
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
