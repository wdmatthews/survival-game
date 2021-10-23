using System.Collections.Generic;
using UnityEngine;
using Project.Items;

namespace Project.Combat
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Project/Combat/Weapon")]
    public class WeaponSO : UpgradableItemSO
    {
        public float Damage = 1;

        public override void Use(List<MonoBehaviour> monoBehaviours)
        {
            DamageMonsters(monoBehaviours);
        }

        public void DamageMonsters(List<MonoBehaviour> monoBehaviours)
        {
            for (int i = monoBehaviours.Count - 1; i >= 0; i--)
            {
                Damageable damageable = (Damageable)monoBehaviours[i];
                damageable.TakeDamage(Damage);
            }
        }
    }
}
