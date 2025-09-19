using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using ProjectGateway.DataPersistence;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectGateway.Core
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }
        [SerializeField]
        private List<GameObject> gameplayObjects;
        
        public CanvasGroup loadingScreen;
        public Scene? ActiveWorldScene;
        private const float FadeDuration = 0.5f;

        // List of scene names where the player should be disabled
        private readonly string[] nonGameplayScenes = { "MainMenu", "PersistentScene" };

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            StartCoroutine(LoadMainMenu());
        }

        private IEnumerator FadeLoadingScreen(float targetAlpha)
        {
            loadingScreen.gameObject.SetActive(true);
            float startAlpha = loadingScreen.alpha;
            float elapsedTime = 0;

            while (elapsedTime < FadeDuration)
            {
                elapsedTime += Time.deltaTime;
                loadingScreen.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / FadeDuration);
                yield return null;
            }

            loadingScreen.alpha = targetAlpha;
            if (Mathf.Approximately(targetAlpha, 0f))
            {
                loadingScreen.gameObject.SetActive(false);
            }
        }

        public void LoadScenesForProfile(string profileName)
        {
            StartCoroutine(LoadWorldSceneForSave(profileName));
        }

        private IEnumerator LoadMainMenu(GameData mainMenuGameData = null) 
        {
            yield return StartCoroutine(FadeLoadingScreen(1f));
            
            if (ActiveWorldScene is not null)
            {
                SceneManager.UnloadSceneAsync(ActiveWorldScene.Value);
            }
            
            // Load the new scene asynchronously
            AsyncOperation worldLoadOperation = SceneManager.LoadSceneAsync("World", LoadSceneMode.Additive);
            AsyncOperation mainMenuLoadOperation = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
            if (worldLoadOperation is not null)
            {
                worldLoadOperation.allowSceneActivation = false;
            }
            else
            {
                Debug.LogError($"World scene failed to load.");
                throw new NullReferenceException("loadOperation was null, scene failed to load.");
            }
            
            if (mainMenuLoadOperation is not null)
            {
                mainMenuLoadOperation.allowSceneActivation = false;
            }
            else
            {
                Debug.LogError($"Main Menu scene failed to load.");
                throw new NullReferenceException("loadOperation was null, scene failed to load.");
            }
            
            // Wait until the scene is fully loaded
            while (worldLoadOperation.progress < 0.9f && mainMenuLoadOperation.progress < 0.9f)
            {
                yield return null;
            }

            // Artificial delay
            yield return new WaitForSeconds(1f);
            
            // Activate the new scenes
            worldLoadOperation.allowSceneActivation = true;
            mainMenuLoadOperation.completed += (operation) =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu"));
            };

            mainMenuLoadOperation.allowSceneActivation = true;
            mainMenuLoadOperation.completed += (operation) =>
            {
                ActiveWorldScene = SceneManager.GetSceneByName("World");
            };
            
            gameplayObjects.ForEach(m => m.SetActive(false));
            

            yield return new WaitForSeconds(0.1f);
            
            DataPersistenceManager.Instance.LoadRecentProfile();
            
            
            yield return new WaitForSeconds(0.1f);
            yield return StartCoroutine(FadeLoadingScreen(0f));
        }
        
        private IEnumerator LoadWorldSceneForSave(string profileName)
        {
            yield return StartCoroutine(FadeLoadingScreen(1f));
            // Unload the old scene if it's not the persistent scene
            if (ActiveWorldScene is not null)
            {
                SceneManager.UnloadSceneAsync(ActiveWorldScene.Value);
            }
            
            // Load the new scene asynchronously
            AsyncOperation worldLoadOperation = SceneManager.LoadSceneAsync("World", LoadSceneMode.Additive);
            AsyncOperation mainMenuLoadOperation = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
            if (worldLoadOperation is not null)
            {
                worldLoadOperation.allowSceneActivation = false;
            }
            else
            {
                Debug.LogError($"World scene failed to load.");
                throw new NullReferenceException("loadOperation was null, scene failed to load.");
            }
            
            if (mainMenuLoadOperation is not null)
            {
                mainMenuLoadOperation.allowSceneActivation = false;
            }
            else
            {
                Debug.LogError($"World scene failed to load.");
                throw new NullReferenceException("loadOperation was null, scene failed to load.");
            }

            // Wait until the scene is fully loaded
            while (worldLoadOperation.progress < 0.9f && mainMenuLoadOperation.progress < 0.9f)
            {
                yield return null;
            }

            // Artificial delay
            yield return new WaitForSeconds(1f);
            
            gameplayObjects.ForEach(m => m.SetActive(false));
            
            // Activate the new scene
            worldLoadOperation.allowSceneActivation = true;

            
            
            yield return new WaitForSeconds(0.1f);
            
            ActiveWorldScene = SceneManager.GetSceneByName("World");
            SceneManager.SetActiveScene(ActiveWorldScene.Value);
            DataPersistenceManager.Instance.LoadProfile(profileName);
            
            yield return new WaitForSeconds(0.1f);
            yield return StartCoroutine(FadeLoadingScreen(0f));
        }
    }
}
