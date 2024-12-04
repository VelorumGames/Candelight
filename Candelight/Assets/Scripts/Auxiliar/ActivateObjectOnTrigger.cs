using UnityEngine;

namespace Auxiliar
{
    public class ActivateObjectOnTrigger : MonoBehaviour
    {
        public GameObject[] Objs;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                foreach (var o in Objs) o.SetActive(true);
            }
        }
    }
}