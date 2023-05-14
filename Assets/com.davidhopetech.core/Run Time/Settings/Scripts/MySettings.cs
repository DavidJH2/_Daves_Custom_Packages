using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MySettings", order = 1)]
public class MySettings : ScriptableObject
{
	public string prefabName;

	public float dammperCoeef;
	public float springCoeeff;
}