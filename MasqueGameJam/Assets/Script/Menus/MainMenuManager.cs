using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Script.Menus
{
    public class MainMenuManager : MonoBehaviour
    {
        private SceneManager sceneManager = null;
        public event Action<SceneAsset,SceneManager> ChangeScene;
    
        private void Start()
        {
            sceneManager = FindObjectOfType<SceneManager>();
        }

        public void LaunchScene(SceneAsset scene)
        {
            ChangeScene?.Invoke(scene, sceneManager);
            //sceneManager.LoadScene();
        }
    }
}
