using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowUIRuneSprites : MonoBehaviour
{
    [SerializeField] Sprite[] _instrSprites;
    [SerializeField] Image[] _sprites;

    private void Start()
    {
        ShowSprites("Fire");

        foreach (var s in _sprites) s.color = new Color(1f, 1f, 1f, 0f);
    }

    public void ShowSprites(string name)
    {
        if (ARune.FindSpell(name, out var spell))
        {
            if (spell.GetInstructions().Length == _sprites.Length)
            {
                int count = 0;
                foreach (var instr in spell.GetInstructions())
                {
                    _sprites[count++].sprite = _instrSprites[(int)instr];
                }
            }
        }
    }
}
