using UnityEngine;

[CreateAssetMenu(fileName = "NewScene", menuName = "Theatre/Scene")]
public class SceneSO : ScriptableObject
{
    public string sceneName;
    public Sprite background;
    public GameObject[] characters;
    public Choice[] choices;
}