using System.Collections.Generic;
using UnityEngine;

namespace Project.Items
{
    public abstract class ItemSO : CraftingIngredientSO
    {
        public Transform PhysicalItem = null;

        public virtual void Use() { }
        public virtual void Use(MonoBehaviour monoBehaviour) { }
        public virtual void Use(List<MonoBehaviour> monoBehaviours) { }
        public virtual void Use(Resource resource, InventorySO inventory) { }
    }
}
