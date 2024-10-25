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

        int m_candle;
        public int Candle
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

        public event Action<int> OnCandleChanged;
    }
}