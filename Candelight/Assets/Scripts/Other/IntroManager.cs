using Controls;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [SerializeField] Transform _startPoint;
    PlayerController _player;
    float _zDist;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        FindObjectOfType<InputManager>().LoadControls(EControlMap.Intro);

        _player.transform.position = _startPoint.position;
        _zDist = Mathf.Abs(_player.transform.position.z - transform.position.z);
    }

    private void Update()
    {
        RenderSettings.fogEndDistance = Mathf.Lerp(10f, 250f, 1 - Mathf.Abs(_player.transform.position.z - transform.position.z) / _zDist);
    }
}
