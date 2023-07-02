using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using _Deliverence.Scripts.Player;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

public class GameEngine : MonoBehaviour
{
    private static           TextMeshProUGUI             debugTMP;
    [SerializeField] private Material                    skyBox;
    [SerializeField] private XROrigin                    _xrOrigin;
    [SerializeField] private DeliverancePlayerController _playerController;
    [SerializeField] private GameObject                  _menu;
    public                   GameObject                  _gameOver;
    [SerializeField] private GameObject                  _grenadeLauncher;
    [SerializeField] private Transform                   _playerStartOrien;
    [SerializeField] private GameObject                  _menuBackgroundMusic;
    [SerializeField] private GameObject                  _gameBackgroundMusic;
    

    public static bool            GamePlaying;
    private       ZombieSpawner[] _spawners;
    
    void Start()
    {
        var debugTextGO = FindObjectOfType<DebugText>();
        debugTMP = debugTextGO.GetComponent<TextMeshProUGUI>();

        GamePlaying = false;
        _spawners   = FindObjectsOfType<ZombieSpawner>();
        // RenderSettings.skybox = skyBox;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void StartGame()
    {
        GamePlaying = true;
        
        _menuBackgroundMusic.SetActive(false);
        _gameBackgroundMusic.SetActive(true);
        
        _playerController.ChangeToIdleState();
        _xrOrigin.transform.position = _playerStartOrien.position;
        _xrOrigin.transform.rotation = _playerStartOrien.rotation;
        
        _menu.SetActive(false);
        _grenadeLauncher.SetActive(true);
        
        foreach (var spawner in _spawners)
        {
            spawner.SpawnZombies();
        }
    }
    
    public static void QuitGame()
    {
        Application.Quit();
    }

    public void SetVRMode(bool vrSittingMode)
    {
        if (vrSittingMode)
        {
            _xrOrigin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
        }
        else
        {
            _xrOrigin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
        }
    }


    public static void AddDebugText(string text)
    {
        debugTMP.text += text;
    }


    public static void SetDebugText(string text)
    {
        debugTMP.text = text;
    }
}
