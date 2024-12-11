using Controls;
using Hechizos.DeForma;
using Hechizos.Elementales;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        static bool _extraElement;

        static bool _runesCreated;

        public ARune(Mage m, int Complexity, float Difficulty)
        {
            MageManager = m;
            Instructions = CreateInstructionChain(Complexity, Difficulty);
            Spells.Add(Instructions, this);

            //Debug. Deberia estar desactivado
            //Activate(true);

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

        /// <summary>
        /// Encuentra cualquier hechizo en base a su nombre
        /// </summary>
        /// <param name="name"></param>
        /// <param name="spell"></param>
        /// <returns></returns>
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
        /// Encuentra un hechizo (previamente activado) en base a una cadena de instrucciones
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
            int elementalComplexity = 2;
            int num = System.Math.Clamp(chain.Length / 2, 0, _extraElement ? 3 : 2);  //2 Es la complejidad de cada elemento
            Debug.Log($"Deberia haber {num} elementos en esta cadena: " + InstructionsToString(chain));
            elements = new AElementalRune[num];

            //Filtro
            if (chain.Length < elementalComplexity || chain.Length % elementalComplexity != 0 || chain.Length / elementalComplexity > num) return false;

            for (int i = 0; i < num; i++)
            {
                ESpellInstruction[] subChain = new ESpellInstruction[elementalComplexity];
                try
                {
                    subChain[0] = chain[i * elementalComplexity];
                    subChain[1] = chain[i * elementalComplexity + 1];
                }
                catch (System.IndexOutOfRangeException e)
                {
                    Debug.LogWarning("ERROR: Fallo al dividir subcadenas: " + e);
                    return false;
                }

                if (FindSpell(subChain, out ARune element) && element != null)
                {
                    elements[i] = element as AElementalRune;
                    if (elements[i] != null)
                    {
                        Debug.Log(elements[i].Name);
                        found++;
                    }
                }
                else
                {
                    Debug.LogWarning("Hay un elemento que no se ha encontrado");
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
            if (!_runesCreated) //Si no se han creado previamente
            {
                new CosmicRune(m);
                new ElectricRune(m);
                new PhantomRune(m);

                new MeleeRune(m);
                new ProjectileRune(m);
                new ExplosionRune(m);
                new BuffRune(m);

                new FireRune(m);

                _runesCreated = true;
            }
            else //Si se han creado ya antes, resetearlas
            {
                foreach(var rune in Spells.Values)
                {
                    if (rune.Name != "Fire") rune.Activate(false);
                }
            }
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

        public void Activate(bool b)
        {
            Activated = b;
            if (b && MageManager != null) MageManager.RuneActivation(this);
        }
        public bool IsActivated() => Activated;
        public static void Activate(ESpellInstruction[] chain, out ARune rune)
        {
            rune = null;

            Debug.Log("Se busca el hechizo con cadena: " + InstructionsToString(chain) + $"({Spells.Keys.Count})");
            if (FindUnactiveSpell(chain, out rune))
            {
                Debug.Log("Se activa el hechizo: " + rune.Name);
                rune.Activate(true);
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

        public static void SetExtraElement(bool b) => _extraElement = b;

        public string GetInstructionsToString()
        {
            string instrs = "";
            foreach(var i in Instructions)
            {
                instrs += i.ToString();
            }
            return instrs;
        }

        public string GetInstructionsToArrows()
        {
            string instrs = "";
            foreach (var i in Instructions)
            {
                switch(i.ToString())
                {
                    case "Up":
                        instrs += "W";
                        break;
                    case "Down":
                        instrs += "S";
                        break;
                    case "Right":
                        instrs += "D";
                        break;
                    case "Left":
                        instrs += "A";
                        break;
                }
            }
            return instrs;
        }

        public ESpellInstruction[] GetInstructions() => Instructions.ToArray();
    }
}