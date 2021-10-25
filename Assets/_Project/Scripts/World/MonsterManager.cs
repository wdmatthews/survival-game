using UnityEngine;

namespace Project.World
{
    [AddComponentMenu("Project/World/Monster Manager")]
    [DisallowMultipleComponent]
    public class MonsterManager : MonoBehaviour
    {
        [SerializeField] private MonsterManagerSO _manager = null;

        private void Update()
        {
            _manager.OnUpdate();
        }
    }
}
