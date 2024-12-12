using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class ShowTutorialOnTrigger : MonoBehaviour
{
    public string TutorialText;
    public string TutorialMobileText;
    bool _activated = true;

    private void OnTriggerEnter(Collider other)
    {
        if (_activated && other.CompareTag("Player"))
        {
            _activated = false;
            FindObjectOfType<UIManager>().ShowTutorial(Application.isMobilePlatform ? TutorialMobileText :  TutorialText, 8f);
        }
    }
}
