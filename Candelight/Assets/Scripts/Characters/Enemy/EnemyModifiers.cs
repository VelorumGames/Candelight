using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(menuName = "Enemy/Enemy Modifiers")]
    public class EnemyModifiers : ScriptableObject
    {
        public float SpeedMod = 1f;
        public float DamageMod = 1f;
        public float FragDropMod = 1f;
    }
}