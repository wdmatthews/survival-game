using UnityEngine;
using Project.UI;

namespace Project.World
{
    [CreateAssetMenu(fileName = "Time Manager", menuName = "Project/World/Time Manager")]
    public class TimeManagerSO : ScriptableObject
    {
        public int MinutesPerDay = 1;
        public int MinutesPerNight = 1;
        public float NightLightIntensity = 0.5f;
        public LocationManagerSO LocationManager = null;

        [System.NonSerialized] public TimeHUD TimeHUD = null;
        [System.NonSerialized] public int Day = 0;
        [System.NonSerialized] public float MinutesLeftBeforeNextNight = 0;
        [System.NonSerialized] public float MinutesLeftBeforeNextDay = 0;
        [System.NonSerialized] public bool IsDay = true;
        [System.NonSerialized] private Light _light = null;

        public void StartFirstDay(Light light)
        {
            MinutesLeftBeforeNextNight = MinutesPerDay;
            MinutesLeftBeforeNextDay = MinutesPerNight;
            IsDay = true;
            _light = light;
            LocationManager.OnNextDay();
            TimeHUD.UpdateTime(true, Day, MinutesLeftBeforeNextNight);
        }

        public void DecreaseMinutesLeft(float amount)
        {
            if (IsDay)
            {
                if (Mathf.Approximately(MinutesLeftBeforeNextNight, 0)) GoToNextNight();
                else
                {
                    MinutesLeftBeforeNextNight = Mathf.Clamp(MinutesLeftBeforeNextNight - amount,
                        0, MinutesPerDay);
                    TimeHUD.UpdateTime(true, Day, MinutesLeftBeforeNextNight);
                }
            }
            else
            {
                if (Mathf.Approximately(MinutesLeftBeforeNextDay, 0)) GoToNextDay();
                else
                {
                    MinutesLeftBeforeNextDay = Mathf.Clamp(MinutesLeftBeforeNextDay - amount,
                        0, MinutesPerNight);
                    TimeHUD.UpdateTime(false, Day, MinutesLeftBeforeNextDay);
                }
            }
        }

        public void GoToNextDay()
        {
            IsDay = true;
            Day++;
            MinutesLeftBeforeNextNight = MinutesPerDay;
            MinutesLeftBeforeNextDay = MinutesPerNight;
            _light.intensity = 1;
            LocationManager.OnNextDay();
            TimeHUD.UpdateTime(true, Day, MinutesLeftBeforeNextNight);
        }

        public void GoToNextNight()
        {
            IsDay = false;
            _light.intensity = NightLightIntensity;
            TimeHUD.UpdateTime(false, Day, MinutesLeftBeforeNextDay);
        }
    }
}
