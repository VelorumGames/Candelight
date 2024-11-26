using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    [CreateAssetMenu(menuName = "World Info")]
    public class WorldInfo : ScriptableObject
    {
        public int Seed;
        public GameObject World;
        public int MAX_NODES;
        int _nodes;
        public int CompletedNodes
        {
            get => _nodes;
            set
            {
                if (value != _nodes)
                {
                    _nodes = value;
                    if (OnCompletedNodesChanged != null) OnCompletedNodesChanged(value);
                }
            }
        }
        public float NodeCandleFactor = 1f;

        public float MAX_CANDLE = 100f;
        [SerializeField] float m_candle;
        public float Candle
        {
            get => m_candle;
            set
            {
                value = Mathf.Clamp(value, 0f, MAX_CANDLE);
                if (m_candle != value)
                {
                    m_candle = value;
                    if (OnCandleChanged != null) OnCandleChanged(m_candle);

                    if (m_candle <= 0 && OnPlayerDeath != null)
                    {
                        OnPlayerDeath();
                    }
                }
            }
        }

        public event Action OnPlayerDeath;
        public event Action<float> OnCandleChanged;
        public event Action<int> OnCompletedNodesChanged;


        //SAVE INFO
        public List<int> CompletedIds = new List<int>();
        public bool LoadedPreviousGame;
    }
}