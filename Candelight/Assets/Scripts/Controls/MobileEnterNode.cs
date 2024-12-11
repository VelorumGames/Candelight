using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Controls
{
    public class MobileEnterNode : MonoBehaviour
    {
        public void EnterNode()
        {
            GetComponentInParent<NodeInfoBox>().EnterNodeThroughButton();
        }
    }
}