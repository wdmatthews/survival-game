using UnityEngine;
using Project.UI;

namespace Project.Time
{
    [AddComponentMenu("Project/Time/Time Manager")]
    [DisallowMultipleComponent]
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private TimeManagerSO _timeManager = null;
        [SerializeField] private TimeHUD _timeHUD = null;

        private void Start()
        {
            _timeManager.TimeHUD = _timeHUD;
            _timeManager.StartFirstDay();
        }

        private void Update()
        {
            _timeManager.DecreaseMinutesLeft(UnityEngine.Time.deltaTime / 60);
        }
    }
}
