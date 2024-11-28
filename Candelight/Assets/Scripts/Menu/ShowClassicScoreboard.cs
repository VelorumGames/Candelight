using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ShowClassicScoreboard : MonoBehaviour
{
    bool _active;
    [SerializeField] GameObject _scoreboard;

    public void ShowScoreboard()
    {
        _active = !_active;
        _scoreboard.SetActive(_active);
    }
}
