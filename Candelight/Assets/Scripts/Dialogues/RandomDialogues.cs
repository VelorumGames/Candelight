using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Dialogues
{
    public class RandomDialogues : MonoBehaviour
    {
        [SerializeField] NodeInfo _currentNodeInfo;
        [Space(10)]
        public Dialogue[] DurniaLoreDialogues;
        public Dialogue[] DurniaRandDialogues;
        public Dialogue[] DurniaItemDialogues;
        [Space(10)]
        public Dialogue[] TemeriaLoreDialogues;
        public Dialogue[] TemeriaRandDialogues;
        public Dialogue[] TemeriaItemDialogues;
        [Space(10)]
        public Dialogue[] IdriaLoreDialogues;
        public Dialogue[] IdriaRandDialogues;
        public Dialogue[] IdriaItemDialogues;

        Dialogue[] LoreDialogues;
        Dialogue[] RandDialogues;
        Dialogue[] ItemDialogues;

        static bool[] _takenDurniaLoreDialogues;
        static bool[] _takenDurniaRandDialogues;
        static bool[] _takenDurniaItemDialogues;
        static bool[] _takenTemeriaLoreDialogues;
        static bool[] _takenTemeriaRandDialogues;
        static bool[] _takenTemeriaItemDialogues;
        static bool[] _takenIdriaLoreDialogues;
        static bool[] _takenIdriaRandDialogues;
        static bool[] _takenIdriaItemDialogues;

        static bool[] _takenLore;
        static bool[] _takenRand;
        static bool[] _takenItem;

        private void Awake()
        {
            if (_takenDurniaLoreDialogues == null)
            {
                _takenDurniaLoreDialogues = new bool[DurniaLoreDialogues.Length];
                _takenDurniaRandDialogues = new bool[DurniaRandDialogues.Length];
                _takenDurniaItemDialogues = new bool[DurniaItemDialogues.Length];
                _takenTemeriaLoreDialogues = new bool[TemeriaLoreDialogues.Length];
                _takenTemeriaRandDialogues = new bool[TemeriaRandDialogues.Length];
                _takenTemeriaItemDialogues = new bool[TemeriaItemDialogues.Length];
                _takenIdriaLoreDialogues = new bool[IdriaLoreDialogues.Length];
                _takenIdriaRandDialogues = new bool[IdriaRandDialogues.Length];
                _takenIdriaItemDialogues = new bool[IdriaItemDialogues.Length];
            }

            switch(_currentNodeInfo.Biome)
            {
                case EBiome.Durnia:
                    LoreDialogues = DurniaLoreDialogues;
                    RandDialogues = DurniaRandDialogues;
                    ItemDialogues = DurniaItemDialogues;

                    _takenLore = _takenDurniaLoreDialogues;
                    _takenRand = _takenDurniaRandDialogues;
                    _takenItem = _takenDurniaItemDialogues;
                    break;
                case EBiome.Temeria:
                    LoreDialogues = TemeriaLoreDialogues;
                    RandDialogues = TemeriaRandDialogues;
                    ItemDialogues = TemeriaItemDialogues;

                    _takenLore = _takenTemeriaLoreDialogues;
                    _takenRand = _takenTemeriaRandDialogues;
                    _takenItem = _takenTemeriaItemDialogues;
                    break;
                case EBiome.Idria:
                    LoreDialogues = IdriaLoreDialogues;
                    RandDialogues = IdriaRandDialogues;
                    ItemDialogues = IdriaItemDialogues;

                    _takenLore = _takenIdriaLoreDialogues;
                    _takenRand = _takenIdriaRandDialogues;
                    _takenItem = _takenIdriaItemDialogues;
                    break;
            }
        }

        public Dialogue GetRandomDialogue()
        {
            if (LoreDialogues.Length > 0)
            {
                Dialogue d = null;

                if (Random.value > 0.6f)
                {
                    
                    d = TryGetRandomDialogue(LoreDialogues, _takenLore);
                    //LoreDialogues.Remove(d);
                }
                else
                {
                    d = TryGetRandomDialogue(RandDialogues, _takenRand);
                    //RandDialogues.Remove(d);
                }

                return d;
            }

            return null;
        }

        public Dialogue GetRewardDialogue()
        {
            if (ItemDialogues.Length > 0)
            {
                Dialogue d = TryGetRandomDialogue(ItemDialogues, _takenItem);
                //ItemDialogues.Remove(d);

                return d;
            }

            return null;
        }

        static bool CheckForAvailableDialogue(bool[] taken)
        {
            foreach (var b in taken)
            {
                if (!b) return true;
            }
            return false;
        }

        static Dialogue TryGetRandomDialogue(Dialogue[] dialogues, bool[] taken)
        {
            if (CheckForAvailableDialogue(taken))
            {
                int id = Random.Range(0, dialogues.Length);

                while (taken[id])
                {
                    if (++id >= taken.Length) id = 0;
                }

                return dialogues[id];
            }
            else
            {
                taken = new bool[dialogues.Length];
                return dialogues[Random.Range(0, dialogues.Length)];
            }
        }
    }
}
