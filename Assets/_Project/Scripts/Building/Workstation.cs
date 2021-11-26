using UnityEngine;

namespace Project.Building
{
    [AddComponentMenu("Project/Building/Workstation")]
    [DisallowMultipleComponent]
    public class Workstation : Structure
    {
        public string Name => _data.name;
    }
}
