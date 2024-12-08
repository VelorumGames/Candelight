using BehaviourAPI.Core.Actions;
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
        [Space(10)]
        [SerializeField] TextMeshProUGUI _owlText;

        UISoundManager _sound;

        private void Awake()
        {
            _sound = FindObjectOfType<UISoundManager>();
        }

        public void RegisterNode(string name, string description, EBiome biome, string state, ELevel[] levels)
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

            //Tipos de niveles
            if (GameSettings.Owl)
            {
                string s = "Niveles: ";
                int count = 0;
                foreach (var level in levels)
                {
                    if (count++ < levels.Length - 1)
                    {
                        switch (level)
                        {
                            case ELevel.Exploration:
                                s += "Exploración, ";
                                break;
                            case ELevel.Calm:
                                s += "Aldea, ";
                                break;
                            case ELevel.Challenge:
                                s += "Desafío, ";
                                break;
                        }
                    }
                    else
                    {
                        switch (level)
                        {
                            case ELevel.Exploration:
                                s += "Exploración.";
                                break;
                            case ELevel.Calm:
                                s += "Aldea.";
                                break;
                            case ELevel.Challenge:
                                s += "Desafío.";
                                break;
                        }
                    }
                }
                _owlText.text = s;
            }
        }

        public void HideBox()
        {
            GetComponent<RectTransform>().DOScale(0f, 0.5f).SetUpdate(true).SetEase(Ease.InBack).Play().OnComplete(() => gameObject.SetActive(false));
        }

        public void ShowBox(bool b)
        {
            if (!b)
            {
                foreach (var d in _difficulties) d.SetActive(false);
                HideBox();
            }
            else
            {
                gameObject.SetActive(true);
                _sound.PlayShowNodeInfo();
            }
        }
    }
}
