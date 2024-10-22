using Controls;
using Hechizos;
using Hechizos.DeForma;
using Hechizos.Elementales;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        public string NextNodeName;
        public string ActualNodeName;
        public string Chains;

        ShowInstructions _showInstr;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            _showInstr = FindObjectOfType<ShowInstructions>();
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 50), $"FPS: {1.0f / Time.deltaTime}\nCurrent Node: {ActualNodeName}\nNext Node: {NextNodeName}");
            if (GUI.Button(new Rect(200, 10, 150, 20), "WORLD SCENE")) SceneManager.LoadScene("WorldScene");
            if (GUI.Button(new Rect(350, 10, 150, 20), "LEVEL SCENE")) SceneManager.LoadScene("LevelScene");
            if (GUI.Button(new Rect(500, 10, 150, 20), "CALM SCENE")) SceneManager.LoadScene("CalmScene");
            if (GUI.Button(new Rect(10, 60, 200, 20), "CREATE RUNES")) ARune.CreateAllRunes(FindObjectOfType<Mage>());
            GUI.Label(new Rect(10, 80, 200, 500), Chains);
        }

        private void Update()
        {
            Chains = "";
            foreach(var rune in ARune.Spells.Keys)
            {
                Chains += $"{ARune.Spells[rune].Name}: {ARune.InstructionsToString(rune)}\n";
            }
        }

        public void ShowNewInstruction(ESpellInstruction instr)
        {
            _showInstr.ShowInstruction(instr);
        }

        public void ShowValidSpell(AShapeRune spell)
        {
            Debug.Log("Se mostrara: " + spell);
            if (spell != null) StartCoroutine(_showInstr.ShowValidInstructions());
            else _showInstr.ResetSprites();
        }
        public void ShowValidElements(AElementalRune[] elements)
        {
            Debug.Log("Se mostrara: " + elements);
            if (elements != null) StartCoroutine(_showInstr.ShowValidInstructions());
            else _showInstr.ResetSprites();
        }
    }
}