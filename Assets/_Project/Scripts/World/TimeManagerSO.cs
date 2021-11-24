using UnityEngine;
using Project.UI;

namespace Project.World
{
    [CreateAssetMenu(fileName = "Time Manager", menuName = "Project/World/Time Manager")]
    public class TimeManagerSO : ScriptableObject
    {
        public int MinutesPerDay = 1;
        public int MinutesPerNight = 1;
        public LocationManagerSO LocationManager = null;

        [System.NonSerialized] public TimeHUD TimeHUD = null;
        [System.NonSerialized] public int Day = 0;
        [System.NonSerialized] public float MinutesLeftBeforeNextNight = 0;
        [System.NonSerialized] public float MinutesLeftBeforeNextDay = 0;
        [System.NonSerialized] public bool IsDay = true;

        public void StartFirstDay()
        {
            MinutesLeftBeforeNextNight = MinutesPerDay;
            MinutesLeftBeforeNextDay = MinutesPerNight;
            IsDay = true;
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
            LocationManager.OnNextDay();
            TimeHUD.UpdateTime(true, Day, MinutesLeftBeforeNextNight);
        }

        public void GoToNextNight()
        {
            IsDay = false;
            TimeHUD.UpdateTime(false, Day, MinutesLeftBeforeNextDay);
        }
    }
}
