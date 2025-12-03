using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class ResultMenuView : MonoBehaviour
    {
        [SerializeField] public Button ScoreButton;
        [SerializeField] public Button MainMenuButton;
        [SerializeField] public Button RestartButton;

        [SerializeField] private GameObject _scoreView;

        [SerializeField] private TMP_Text _killedCount;
        [SerializeField] private TMP_Text _timeAlived;

        public void ShowScore()
        {
            _scoreView.gameObject.SetActive(true);
        }

        public void HideScore()
        {
            _scoreView.gameObject.SetActive(false);
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