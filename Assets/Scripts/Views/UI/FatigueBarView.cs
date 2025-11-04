using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class FatigueBarView : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;

        public void SetValue(float percent)
        {
            if (_slider != null)
            {
                _slider.value = percent;
            }
        }
    }
}

