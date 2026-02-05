using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Menus
{
    public class MainMenuManager : MonoBehaviour
    {
        public event Action<string> ChangeScene;
        public void LaunchScene(string sceneName) => ChangeScene?.Invoke(sceneName);
        
        public void QuitGame() => Application.Quit();
    }
}
