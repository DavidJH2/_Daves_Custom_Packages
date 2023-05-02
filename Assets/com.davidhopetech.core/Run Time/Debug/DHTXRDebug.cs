using UnityEngine;
using TMPro;


namespace com.davidhopetech.core.Run_Time.Debug
{
	public class DHTXRDebug : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI value1;

		private DHTUpdateDebugValue1Event _debugValue1;
	
		void Start()
		{
			UpdateValue1("Hello");
			_debugValue1 = FindObjectOfType<DHTEventContainer>().GetComponent<DHTEventContainer>().dhtUpdateDebugValue1Event;

			/*
			if (_debugValue1 == null)
				_debugValue1 = new DHTUpdateDebugValue1Event();
			*/

			_debugValue1.AddListener(UpdateValue1);
		}

		public void UpdateValue1(string text)
		{
			value1.text = text;
		}


		void Update()
		{
		
		}
	}
}
