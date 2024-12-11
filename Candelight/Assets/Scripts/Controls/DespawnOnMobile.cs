using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controls
{
    public class DespawnOnMobile : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(!Application.isMobilePlatform);
        }
    }
}
