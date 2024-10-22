using Cinemachine;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cameras
{
    public class FindPlayer : MonoBehaviour
    {
        CinemachineVirtualCamera _cam;
        Transform _player;

        private void Start()
        {
            _cam = GetComponent<CinemachineVirtualCamera>();
            _player = FindObjectOfType<PlayerController>().transform;

            _cam.Follow = _player;
            _cam.LookAt = _player;
        }
    }
}
