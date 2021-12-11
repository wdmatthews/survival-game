using UnityEngine;

namespace Project.Building
{
    [System.Serializable]
    public class CampfireData
    {
        public string ID = "";
        public string Name = "";
        public Vector3 Position = new Vector3();

        public CampfireData(string name, Vector3 position)
        {
            ID = System.Guid.NewGuid().ToString();
            Name = name;
            Position = position;
        }
    }
}
