using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(menuName = "Enemy/Enemy Info")]
    public class EnemyInfo : ScriptableObject
    {
        public string Name;
        public string Description;
        public float BaseHP;
        public float BaseDamage;
        public int MinFragments;
        public int MaxFragments;
    }

}