using UnityEngine;

namespace UI.Utils
{
    public class UIHelper
    {
        public static string GetHMCounterDown(long remainTime)
        {
            if (remainTime < 0)
            {
                Debug.LogWarning("倒计时小于0");
                return null;
            }

            var hour = remainTime / 3600;
            var min = remainTime % 3600 / 60;
            
            return hour.ToString("00")+":"+min.ToString("00");
        }
        
    }
}