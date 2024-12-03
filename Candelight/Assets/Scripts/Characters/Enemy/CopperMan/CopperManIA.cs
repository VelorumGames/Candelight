using Enemy;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CopperManIA : EnemyController
{
    private bool angry;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _playerNearDistance;
    [SerializeField] private float _canFleeDistance;
    [SerializeField] private float _playerAtRangeDistance;

    private bool PlayerNear { get; set; }
    private bool CanFlee { get; set; }
    private bool PlayerAtRange { get; set; }

    private delegate void OnPerceptionsChangedHandler();

    private event OnPerceptionsChangedHandler _onPerceptionsChanged;

    new void Awake()
    {
        angry = false;
        OnDamage += CheckElectricDamage;
        _onPerceptionsChanged += UpdatePerceptions;
        StartCoroutine(RefreshPerceptions());
    }

    private void UpdatePerceptions()
    {

    }

    private void CheckElectricDamage(float _, float _1)
    {

    }

    private IEnumerator RefreshPerceptions()
    {
        if (Physics2D.OverlapCircle(transform.position, _playerNearDistance, 6) != null)
        {
            PlayerNear = true;
            Vector2 fleeDirection = transform.position - Player.transform.position;
            CanFlee = Physics2D.Raycast(transform.position, fleeDirection);
        }

        PlayerAtRange = Physics2D.OverlapCircle(transform.position, _playerAtRangeDistance, 6) != null;
        yield return new WaitForSecondsRealtime(0.5f);
    }
}
