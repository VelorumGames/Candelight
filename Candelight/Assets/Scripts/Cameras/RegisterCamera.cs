using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cameras
{
    public class RegisterCamera : MonoBehaviour
    {
        private void Start()
        {
            CameraManager.Instance.AddCamera(GetComponent<CinemachineVirtualCamera>());
        }
    }
}
