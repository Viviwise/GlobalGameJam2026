using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Menus
{
    public class MainMenuManager : MonoBehaviour
    {
        public event Action<SceneAsset> ChangeScene;

        public void LaunchScene(SceneAsset scene)
        {
            ChangeScene?.Invoke(scene);
        }
    }
}
