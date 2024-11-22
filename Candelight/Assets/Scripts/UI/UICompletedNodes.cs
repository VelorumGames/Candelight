using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using World;

namespace UI
{
    public class UICompletedNodes : MonoBehaviour
    {
        [SerializeField] WorldInfo _world;
        [SerializeField] TextMeshProUGUI _num;

        private void OnEnable()
        {
            _world.OnCompletedNodesChanged += UpdateNum;
        }

        private void Start()
        {
            UpdateNum(_world.CompletedNodes);
        }

        void UpdateNum(int n)
        {
            if (n <= 0) gameObject.SetActive(false);
            else _num.text = $"{n}";
        }

        private void OnDisable()
        {
            _world.OnCompletedNodesChanged += UpdateNum;
        }

    }
}