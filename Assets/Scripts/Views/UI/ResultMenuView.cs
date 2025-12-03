using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class ResultMenuView : MonoBehaviour
    {
        [SerializeField] public Button RestartButton;
        [SerializeField] public Button BackButton;
        [SerializeField] public Button ScoreButton;
        [SerializeField] public Button ExitButton;

        [SerializeField] private GameObject _scoreView;
        [SerializeField] private GameObject _menuView;

        [SerializeField] private TMP_Text _killedCount;
        [SerializeField] private TMP_Text _timeAlived;

        public void ShowScore()
        {
            _scoreView.gameObject.SetActive(true);
            _menuView.gameObject.SetActive(false);
        }

        public void HideScore()
        {
            _scoreView.gameObject.SetActive(false);
            _menuView.gameObject.SetActive(true);
        }

        public void InItScore(int killedCount, float timeAlived)
        {
            _killedCount.text = killedCount.ToString();
            SetTime(timeAlived);
        }

        private void SetTime(float totalSeconds)
        {
            totalSeconds = Mathf.Max(0f, totalSeconds);

            int minutes = (int)(totalSeconds / 60f);
            int seconds = (int)(totalSeconds % 60f);

            string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

            _timeAlived.text = formattedTime;
        }
    }
}