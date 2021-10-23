using UnityEngine;

namespace Project.Combat
{
    [CreateAssetMenu(fileName = "New Damageable Reference", menuName = "Project/Combat/Damageable Reference")]
    public class DamageableReferenceSO : ScriptableObject
    {
        [System.NonSerialized] public Damageable Value = null;
    }
}
