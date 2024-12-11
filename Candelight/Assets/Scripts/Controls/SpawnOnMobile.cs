using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Controls
{
    public class SpawnOnMobile : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(Application.isMobilePlatform);
        }
    }
}
