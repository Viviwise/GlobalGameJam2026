using UnityEngine;
[CreateAssetMenu(fileName = "SequenceForScene", menuName = "Test/SequenceForScene")]
public class SequenceForScene : ScriptableObject
{
    public bool isSetUp;
    public bool isCleanUp;
    
    public string sequenceName;
    public EventForScene[] events;
    public SequenceForScene cleanUpSequence;
    public SceneData nextScene;
    
    public Sprite CatharsysReaction;
    public WheelScore wheelScore;
    public string reactionText;
}
