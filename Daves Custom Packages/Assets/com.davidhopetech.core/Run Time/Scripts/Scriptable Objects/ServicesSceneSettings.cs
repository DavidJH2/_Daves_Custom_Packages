using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Boot Strapper Scene", menuName = "Davids VR Core/Bootstrapper Scene Settings", order = 1)]
public class ServicesSceneSettings : ScriptableObject
{
	[FormerlySerializedAs("bootstrapperSceneName")] public string ServicesSceneName;
}