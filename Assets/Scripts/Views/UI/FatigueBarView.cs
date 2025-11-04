using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class FatigueBarView : MonoBehaviour
    {
        [SerializeField]
        private Image _fatigueBarImage;

        public void SetValue(float percent)
        {
            if (_fatigueBarImage != null)
            {
                _fatigueBarImage.fillAmount = percent;
            }
        }
    }
}


