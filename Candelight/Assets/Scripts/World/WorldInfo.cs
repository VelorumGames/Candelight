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
        public int CompletedNodes;
        public float NodeCandleFactor = 1f;

        [SerializeField] float m_candle;
        public float Candle
        {
            get => m_candle;
            set
            {
                if (m_candle != value)
                {
                    m_candle = value;
                    if (OnCandleChanged != null) OnCandleChanged(m_candle);
                }
            }
        }

        public event Action<float> OnCandleChanged;
    }
}