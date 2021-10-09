using UnityEngine;
using Project.Items;

namespace Project.Combat
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Project/Combat/Weapon")]
    public class WeaponSO : UpgradableItemSO
    {
        public float Damage = 1;
    }
}
