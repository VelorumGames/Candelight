using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToFirstPerson : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            player.ChangeToFirstPerson();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            player.ReturnToThirdPerson();
        }
    }
}
