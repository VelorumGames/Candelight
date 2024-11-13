using Controls;
using Hechizos.DeForma;
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
        protected bool Activated = false;

        public static Dictionary<ESpellInstruction[], ARune> Spells = new Dictionary<ESpellInstruction[], ARune>();
        public static AElementalRune[] ActiveElements;
        public static Mage MageManager;

        public ARune(Mage m, int Complexity, float Difficulty)
        {
            MageManager = m;
            Instructions = CreateInstructionChain(Complexity, Difficulty);
            Spells.Add(Instructions, this);

            //Para debuggear:
            Activate();

            string instrs = "";

            foreach (ESpellInstruction i in Instructions) instrs += i.ToString();
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

            if (CheckRegisteredChain(chain)) return CreateInstructionChain(compl, dif);
            else return chain;
        }

        public static bool FindSpell(string name, out ARune spell)
        {
            spell = null;
            foreach (var rune in Spells.Values)
            {
                if (rune.Name == name)
                {
                    spell = rune;
                    return true;
                }
            }

            return false;
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
                    if (found == chain.Length && Spells[instrs].Activated)
                    {
                        spell = Spells[instrs];
                        return true;
                    }
                }
            }

            return false;
        }

        static bool FindUnactiveSpell(ESpellInstruction[] chain, out ARune spell)
        {
            int found = 0;
            spell = null;
            foreach (var instrs in Spells.Keys)
            {
                if (chain.Length == instrs.Length)
                {
                    found = 0;
                    for (int i = 0; i < chain.Length; i++)
                    {
                        if (chain[i] == instrs[i]) found++;
                    }

                    //Si todas han coincidido
                    if (found == chain.Length && !Spells[instrs].Activated)
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
            Debug.Log($"Deberia haber {num} elementos en esta cadena: " + InstructionsToString(chain));
            elements = new AElementalRune[num];

            if (chain.Length < 2) return false;
            if (chain.Length % 2 != 0) return false;

            for (int i = 0; i < num; i++)
            {
                ESpellInstruction[] subChain = new ESpellInstruction[2];
                try
                {
                    subChain[0] = chain[i * num];
                    subChain[1] = chain[i * num + 1];
                }
                catch (System.IndexOutOfRangeException e)
                {
                    Debug.Log("ERROR: Fallo al dividir subcadenas: " + e);
                    return false;
                }

                if (FindSpell(subChain, out ARune element))
                {
                    elements[i] = element as AElementalRune;
                    Debug.Log(elements[i].Name);
                    found++;
                }
            }

            //Comprobar si se trata del mismo elemento. En ese caso, simplemente se toma uno de ellos y no los dos
            try
            {
                if (num > 1 && elements[0].Name == elements[1].Name)
                {
                    AElementalRune el = elements[0];
                    elements = new AElementalRune[1];
                    elements[0] = el;
                }
            }
            catch (System.NullReferenceException e)
            {
                Debug.Log("ERROR: Problema a la hora de encontrar elementos. Se ignora la cadena. " + e);
                return false;
            }

            return found == num;
        }

        public static void CreateAllRunes(Mage m)
        {
            new CosmicRune(m);
            new ElectricRune(m);
            new PhantomRune(m);

            new MeleeRune(m);
            new ProjectileRune(m);
            new ExplosionRune(m);
            new BuffRune(m);

            new FireRune(m);
        }

        public static string InstructionsToString(ESpellInstruction[] chain)
        {
            string str = "{ ";
            foreach (var i in chain)
            {
                str += i.ToString();
                str += " ";
            }
            str += "}";
            return str;
        }

        public void Activate() => Activated = true;
        public bool IsActivated() => Activated;
        public static void Activate(ESpellInstruction[] chain)
        {
            Debug.Log("Se busca el hechizo con cadena: " + InstructionsToString(chain) + $"({Spells.Keys.Count})");
            if (FindUnactiveSpell(chain, out var rune))
            {
                Debug.Log("Se activa el hechizo: " + rune.Name);
                rune.Activate();
            }
        }

        public static void RegisterMage(Mage m)
        {
            MageManager = m;
        }

        public static bool CompareInstructions(ESpellInstruction[] chainA, ESpellInstruction[] chainB)
        {
            if (chainA.Length != chainB.Length) return false;

            for (int i = 0; i < chainA.Length; i++)
            {
                if (chainA[i] != chainB[i]) return false;
            }

            return true;
        }

        public static bool CheckRegisteredChain(ESpellInstruction[] chain)
        {
            foreach (var k in Spells.Keys)
            {
                if (CompareInstructions(chain, k)) return true;
            }
            return false;
        }
    }
}