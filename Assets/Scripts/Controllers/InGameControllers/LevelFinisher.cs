using System;
using System.Collections;
using AnimationsScripts;
using Models;
using Models.Enemies;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.InGameControllers
{
    public class LevelFinisher : MonoBehaviour
    {
        [SerializeField] private InfoText gameOverText;
        [SerializeField] private ScoresCounter scoresCounter;
        [SerializeField] private Text pressAnyKeyText;

        public static event Action OnLevelFinished;
        public static event Action OnReadyToStartNewLevel;
        public static event Action OnGameFinished;

        private void Awake()
        {
            pressAnyKeyText.gameObject.SetActive(false);
        }

        private void Update()
        {
            EnableGameStopping();
        }

        private void OnEnable()
        {
            ScoresCounter.OnPinkScoreUpdated += FinishLevel;
            Player.OnDestroyedByEnemy += FinishGame;
            AfkDetector.OnPlayerIsAfk += FinishGame;
        }

        private void OnDisable()
        {
            ScoresCounter.OnPinkScoreUpdated -= FinishLevel;
            Player.OnDestroyedByEnemy -= FinishGame;
            AfkDetector.OnPlayerIsAfk -= FinishGame;
        }

        private void EnableGameStopping()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                FinishGame();
            }
        }

        private void FinishLevel()
        {
            StartCoroutine(FinishLevelCoroutine());
        }

        private IEnumerator FinishLevelCoroutine()
        {
            CircleAnimation.Instance.Increase(0.2f);
            yield return new WaitForSeconds(0.2f);
            OnLevelFinished?.Invoke();
            yield return new WaitForSeconds(1.5f);
            OnReadyToStartNewLevel?.Invoke();
        }

        private void FinishGame()
        {
            StartCoroutine(FinishGameCoroutine());
        }

        private IEnumerator FinishGameCoroutine()
        {
            CircleAnimation.Instance.Increase();
            yield return new WaitForSeconds(1f);
            OnGameFinished?.Invoke();
            gameOverText.SetText(scoresCounter.GetCurrentGameScore());
            scoresCounter.CalculateTotalTimeSpent();
            scoresCounter.UpdateOnlineBestScore();
            StartCoroutine(ActivateExitButtonCoroutine());
        }

        private IEnumerator ActivateExitButtonCoroutine()
        {
            yield return new WaitForSeconds(3f);
            pressAnyKeyText.gameObject.SetActive(true);
        }
    }
}
