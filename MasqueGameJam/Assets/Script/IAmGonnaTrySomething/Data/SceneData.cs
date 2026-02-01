using UnityEngine;

[CreateAssetMenu(fileName = "SceneData", menuName = "Test/SceneData")]
public class SceneData : ScriptableObject
{
    public string sceneName;
    
    public SequenceForScene[] sequences;
    
    public ObjectsID[] actors;
    public SequenceForScene setUpSequence;
}
