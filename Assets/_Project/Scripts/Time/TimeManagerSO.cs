using UnityEngine;

namespace Project.Time
{
    [CreateAssetMenu(fileName = "Time Manager", menuName = "Project/Time/Time Manager")]
    public class TimeManagerSO : ScriptableObject
    {
        public int MinutesPerDay = 1;
        public int MinutesPerNight = 1;

        [System.NonSerialized] public int Day = 0;
        [System.NonSerialized] public float MinutesLeftBeforeNextNight = 0;
        [System.NonSerialized] public float MinutesLeftBeforeNextDay = 0;
        [System.NonSerialized] public bool IsDay = true;

        public void StartFirstDay()
        {
            MinutesLeftBeforeNextNight = MinutesPerDay;
            MinutesLeftBeforeNextDay = MinutesPerNight;
            IsDay = true;
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
                }
            }
            else
            {
                if (Mathf.Approximately(MinutesLeftBeforeNextDay, 0)) GoToNextDay();
                else
                {
                    MinutesLeftBeforeNextDay = Mathf.Clamp(MinutesLeftBeforeNextDay - amount,
                        0, MinutesPerNight);
                }
            }

            
        }

        public void GoToNextDay()
        {
            IsDay = true;
            Day++;
            MinutesLeftBeforeNextNight = MinutesPerDay;
            MinutesLeftBeforeNextDay = MinutesPerNight;
        }

        public void GoToNextNight()
        {
            IsDay = false;
        }
    }
}
