using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectGateway.Core
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }
        public event Action OnSceneLoadStart;
        public event Action OnSceneLoadComplete;
        [SerializeField]
        private List<GameObject> gameplayObjects;

        // List of scene names where the player should be disabled
        private readonly string[] nonGameplayScenes = { "MainMenu", "PersistentScene", "LoadingScreen" };

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
            LoadScene("MainMenu");
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneRoutine(sceneName));
        }

        private IEnumerator LoadSceneRoutine(string newScene) 
        {
            OnSceneLoadStart?.Invoke(); // Notify UI that loading has started

            // Load the loading screen if it's not already active
            if (!SceneManager.GetSceneByName("LoadingScreen").isLoaded)
            {
                yield return SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);
            }

            // Get the active scene (to unload it later)
            Scene activeScene = SceneManager.GetActiveScene();

            // Load the new scene asynchronously
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);
            if (loadOperation is not null)
            {
                loadOperation.allowSceneActivation = false;
            }
            else
            {
                Debug.LogError($"Scene {newScene} failed to load.");
                throw new NullReferenceException("loadOperation was null, scene failed to load.");
            }

            // Wait until the scene is fully loaded
            while (loadOperation.progress < 0.9f)
            {
                yield return null;
            }

            // Artificial delay
            yield return new WaitForSeconds(1f);

            // Check if the current scene is in the list of non-gameplay scenes
            bool newSceneIsGameplayScene = nonGameplayScenes.All(s => s != newScene);
            
            gameplayObjects.ForEach(m => m.SetActive(newSceneIsGameplayScene));

            // Activate the new scene
            loadOperation.allowSceneActivation = true;

            // Unload the old scene if it's not the persistent scene
            if (activeScene.name != "PersistentScene")
            {
                SceneManager.UnloadSceneAsync(activeScene);
            }
            
            
            yield return new WaitForSeconds(0.1f);
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(newScene));
            
            yield return new WaitForSeconds(0.1f);
            
            // Rescan the AI if it's a gameplay level
            if (newSceneIsGameplayScene)
            {
                //AstarPath.active.Scan();
            }
            
            OnSceneLoadComplete?.Invoke(); // Notify UI that loading is done
            // Remove the loading screen
            SceneManager.UnloadSceneAsync("LoadingScreen");
            
            Debug.Log("Active Scene: " + SceneManager.GetActiveScene().name);
        }
    }
}
