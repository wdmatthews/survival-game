using UnityEngine;

namespace Project.Items
{
    public class Item : MonoBehaviour
    {
        [SerializeField] protected ItemSO _data = null;
        [SerializeField] protected int _amount = 1;

        public ItemSO Data => _data;
        public int Amount => _amount;
    }
}
