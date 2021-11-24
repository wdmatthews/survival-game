using UnityEngine;
using Project.UI;

namespace Project.World
{
    [AddComponentMenu("Project/World/Time Manager")]
    [DisallowMultipleComponent]
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private TimeManagerSO _timeManager = null;
        [SerializeField] private TimeHUD _timeHUD = null;

        private bool _hasStarted = false;
        private int _startTimer = 2;

        private void Start()
        {
            _timeManager.TimeHUD = _timeHUD;
        }

        private void Update()
        {
            if (!_hasStarted)
            {
                _startTimer--;

                if (_startTimer == 0)
                {
                    _hasStarted = true;
                    _timeManager.StartFirstDay();
                }

                return;
            }

            _timeManager.DecreaseMinutesLeft(Time.deltaTime / 60);
        }
    }
}
