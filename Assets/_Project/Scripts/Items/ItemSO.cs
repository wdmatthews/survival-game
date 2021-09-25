using UnityEngine;

namespace Project.Items
{
    public abstract class ItemSO : ScriptableObject
    {
        public virtual void Use() { }
        public virtual void Use(Resource resource) { }
    }
}
