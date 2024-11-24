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

        private void Awake()
        {
            switch(_currentNodeInfo.Biome)
            {
                case EBiome.Durnia:
                    LoreDialogues = DurniaLoreDialogues;
                    RandDialogues = DurniaRandDialogues;
                    ItemDialogues = DurniaItemDialogues;
                    break;
                case EBiome.Temeria:
                    LoreDialogues = TemeriaLoreDialogues;
                    RandDialogues = TemeriaRandDialogues;
                    ItemDialogues = TemeriaItemDialogues;
                    break;
                case EBiome.Idria:
                    LoreDialogues = IdriaLoreDialogues;
                    RandDialogues = IdriaRandDialogues;
                    ItemDialogues = IdriaItemDialogues;
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
                    d = LoreDialogues[Random.Range(0, LoreDialogues.Length)];
                    //LoreDialogues.Remove(d);
                }
                else
                {
                    d = RandDialogues[Random.Range(0, RandDialogues.Length)];
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
                Dialogue d = ItemDialogues[Random.Range(0, ItemDialogues.Length)];
                //ItemDialogues.Remove(d);

                return d;
            }

            return null;
        }
    }
}
