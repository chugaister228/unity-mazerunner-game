using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers.SoundControllers
{
    public class MainSoundtrackPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource soundtrack;
        
        private static MainSoundtrackPlayer _instance;
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            soundtrack.Play();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "GameScene")
            {
                if (!soundtrack.isPlaying) soundtrack.Play();
            }
            else
            {
                FadeOutMusic(soundtrack, 0.5f);
            }
        }
        
        private void FadeOutMusic(AudioSource audioSource, float fadeDuration)
        {
            StartCoroutine(FadeOutCoroutine(audioSource, fadeDuration));
        }

        private IEnumerator FadeOutCoroutine(AudioSource audioSource, float fadeDuration)
        {
            var startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }
    }
}
