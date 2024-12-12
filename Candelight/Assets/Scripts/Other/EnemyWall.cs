using Enemy;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class EnemyWall : MonoBehaviour
{
    public EnemyController TutEnemy;

    private void OnEnable()
    {
        TutEnemy.OnDamage += CheckEnemyHealth;
    }

    void CheckEnemyHealth(float dam, float rem)
    {
        if (rem <= 0.5f)
        {
            FindObjectOfType<UIManager>().ShowTutorial("El murciélago destruyó la barrera.");
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        TutEnemy.OnDamage -= CheckEnemyHealth;
    }
}
