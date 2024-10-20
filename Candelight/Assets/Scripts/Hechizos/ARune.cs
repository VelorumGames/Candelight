using Controls;
using Hechizos.Elementales;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos
{
    public abstract class ARune
    {
        public string Name { get; protected set; } // Nombre del glifo

        protected int Complexity; //Numero de instrucciones que requerira el glifo
        protected float Difficulty; //Valor entre 0 y 1 que dictara la dificultad de la cadena de instrucciones
        protected ESpellInstruction[] Instructions;

        public static Dictionary<ESpellInstruction[], ARune> Spells = new Dictionary<ESpellInstruction[], ARune>();
        protected Mage MageManager;

        public ARune(Mage m, int Complexity, float Difficulty)
        {
            MageManager = m;
            Instructions = CreateInstructionChain(Complexity, Difficulty);
            Spells.Add(Instructions, this);

            string instrs = "";

            foreach (ESpellInstruction i in Instructions) instrs += i.ToString();

            Debug.Log($"Se genera la cadena {instrs} para {Name}");
        }

        ESpellInstruction[] CreateInstructionChain(int compl, float dif)
        {
            ESpellInstruction[] chain = new ESpellInstruction[compl];

            for (int i = 0; i < compl; i++)
            {
                float v = Random.value;
                //Debug.Log(v + " > " + dif);
                if (i > 0 && v > dif) chain[i] = chain[i - 1];
                else chain[i] = (ESpellInstruction)Random.Range(0, 4);
            }

            if (Spells.ContainsKey(chain)) return CreateInstructionChain(compl, dif);
            else return chain;
        }

        /// <summary>
        /// Encuentra un hechizo en base a una cadena de instrucciones
        /// </summary>
        /// <param name="chain"></param>
        /// <param name="spell"></param>
        /// <returns></returns>
        public static bool FindSpell(ESpellInstruction[] chain, out ARune spell)
        {
            int found = 0;
            spell = null;
            foreach(var instrs in Spells.Keys)
            {
                if (chain.Length == instrs.Length)
                {
                    found = 0;
                    for (int i = 0; i < chain.Length; i++)
                    {
                        if (chain[i] == instrs[i]) found++;
                    }

                    //Si todas han coincidido
                    if (found == chain.Length)
                    {
                        spell = Spells[instrs];
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Encuentra los elementos en una cadena de instrucciones, asumiendo que los elementos tienen una complejidad de 2
        /// </summary>
        /// <param name="chain"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static bool FindElements(ESpellInstruction[] chain, out AElementalRune[] elements)
        {
            int found = 0;
            int num = chain.Length / 2;  //2 Es la complejidad de cada elemento
            Debug.Log($"Deberia haber {num} elementos en esta cadena");
            elements = new AElementalRune[num];

            for (int i = 0; i < num; i++)
            {
                ESpellInstruction[] subChain = new ESpellInstruction[2];
                subChain[0] = chain[i * num];
                subChain[1] = chain[i * num + 1];
               if (FindSpell(subChain, out ARune element))
                {
                    elements[i] = element as AElementalRune;
                    Debug.Log(elements[i].Name);
                    found++;
                }
            }

            return found == num;
        }

        // M�todo abstracto para aplicar efectos a los glifos, que ser� implementado en las clases derivadas
        //public abstract void ApplyEffect();

    }

}