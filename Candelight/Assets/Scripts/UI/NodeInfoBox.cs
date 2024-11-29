using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using World;

namespace UI
{
    public class NodeInfoBox : MonoBehaviour
    {
        [SerializeField] GameObject[] _difficulties;
        [SerializeField] TextMeshProUGUI _name;
        [SerializeField] TextMeshProUGUI _description;
        [SerializeField] TextMeshProUGUI _biome;
        [SerializeField] TextMeshProUGUI _state;
        [SerializeField] TextMeshProUGUI _inputText;

        public void RegisterNode(string name, string description, EBiome biome, string state)
        {
            GetComponent<RectTransform>().localScale = 0f * Vector3.one;
            GetComponent<RectTransform>().DOScale(0.835f, 0.5f).SetUpdate(true).SetEase(Ease.OutBack).Play();

            _name.text = name;
            _description.text = description;
            _biome.text = biome.ToString();
            _state.text = state;

            _inputText.text = state == "Completado" ? "" : "E";

            for (int i = 0; i < (int)biome; i++)
            {
                _difficulties[i].SetActive(true);
            }
        }

        public void ShowBox(bool b)
        {
            gameObject.SetActive(b);

            if (!b)
            {
                foreach (var d in _difficulties) d.SetActive(false);
            }
        }
    }
}
