using UnityEngine;

namespace Project.Building
{
    [AddComponentMenu("Project/Building/Structure")]
    [DisallowMultipleComponent]
    public class Structure : MonoBehaviour
    {
        [SerializeField] protected StructureSO _data = null;

        public void Place(Vector3 position, int angleIndex)
        {
            transform.position = position;
            transform.localEulerAngles = new Vector3(0, angleIndex * 90, 0);
        }
    }
}
