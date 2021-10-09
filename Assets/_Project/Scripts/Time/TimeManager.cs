using UnityEngine;

namespace Project.Time
{
    [AddComponentMenu("Project/Time/Time Manager")]
    [DisallowMultipleComponent]
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private TimeManagerSO _timeManager = null;

        private void Awake()
        {
            _timeManager.StartFirstDay();
        }

        private void Update()
        {
            _timeManager.DecreaseMinutesLeft(UnityEngine.Time.deltaTime / 60);
        }
    }
}
