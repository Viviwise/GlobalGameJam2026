using UnityEngine;
[CreateAssetMenu(fileName = "SequenceForScene", menuName = "Test/SequenceForScene")]
public class SequenceForScene : ScriptableObject
{
    public bool isSetUp;
    
    public string sequenceName;
    public EventForScene[] events;
    public SceneData nextScene;
}
