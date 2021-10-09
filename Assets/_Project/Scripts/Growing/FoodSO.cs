using UnityEngine;
using Project.Combat;
using Project.Items;

namespace Project.Growing
{
    [CreateAssetMenu(fileName = "New Food", menuName = "Project/Growing/Food")]
    public class FoodSO : ItemSO
    {
        public float HealthRegainedFromEating = 1;

        public override void Use(MonoBehaviour monoBehaviour)
        {
            Damageable damageable = (Damageable)monoBehaviour;
            damageable.Heal(HealthRegainedFromEating);
        }
    }
}
