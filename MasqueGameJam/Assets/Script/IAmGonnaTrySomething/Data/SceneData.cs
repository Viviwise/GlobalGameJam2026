using UnityEngine;

[CreateAssetMenu(fileName = "SceneData", menuName = "Test/SceneData")]
public class SceneData : ScriptableObject
{
    public SequenceForScene[] sequences;
    
    public DramaObject[] actors;
    public SequenceForScene setUpSequence;
}
