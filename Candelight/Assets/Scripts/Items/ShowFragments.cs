using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Items
{
    public class ShowFragments : MonoBehaviour
    {
        Inventory _inv;
        TextMeshProUGUI _text;

        private void Awake()
        {
            _inv = FindObjectOfType<Inventory>();
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            if (_inv != null) _text.text = $"{_inv.GetFragments()}";
        }

        private void OnEnable()
        {
            if (_inv != null) _inv.OnFragmentsChange += UpdateFragments;
        }

        void UpdateFragments(int prev, int num)
        {
            _text.text = $"{num}";
        }

        private void OnDisable()
        {
            if (_inv != null) _inv.OnFragmentsChange += UpdateFragments;
        }
    }
}