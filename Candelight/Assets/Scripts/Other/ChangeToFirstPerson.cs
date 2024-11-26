using Player;
using System.Collections;
using UnityEngine;

public class ChangeToFirstPerson : MonoBehaviour
{ 
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null && other.transform.parent.TryGetComponent(out PlayerController player))
        {
            player.ChangeToFirstPerson();
        }
        else if (other.TryGetComponent(out player))
        {
            player.ChangeToFirstPerson();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent != null && other.transform.parent.TryGetComponent(out PlayerController player))
        {
            player.ReturnToThirdPerson();
        }
        else if (other.TryGetComponent(out player))
        {
            player.ReturnToThirdPerson();
        }
    }
}
