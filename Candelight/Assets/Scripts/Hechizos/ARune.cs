using Controls;
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

        public ARune(int Complexity, float Difficulty)
        {
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
                Debug.Log(v + " > " + dif);
                if (i > 0 && v > dif) chain[i] = chain[i - 1];
                else chain[i] = (ESpellInstruction)Random.Range(0, 4);
            }

            if (Spells.ContainsKey(chain)) return CreateInstructionChain(compl, dif);
            else return chain;
        }

        public static bool FindSpell(ESpellInstruction[] chain, out ARune spell)
        {
            int found = 0;
            spell = null;
            foreach(var instrs in Spells.Keys)
            {
                if (chain.Length == instrs.Length)
                {
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

        // Método abstracto para aplicar efectos a los glifos, que será implementado en las clases derivadas
        public abstract void ApplyEffect();

    }

}