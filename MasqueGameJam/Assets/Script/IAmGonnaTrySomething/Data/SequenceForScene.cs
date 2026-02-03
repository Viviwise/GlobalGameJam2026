using UnityEngine;
[CreateAssetMenu(fileName = "SequenceForScene", menuName = "Test/SequenceForScene")]
public class SequenceForScene : ScriptableObject
{
    public bool isSetUp;
    
    public string sequenceName;
    public Sprite giver;
    public EventForScene[] events;
    public SceneData nextScene;
    
    public Sprite CatharsysReaction;
    public SoundType catharsysReactionSound;
    public WheelScore wheelScore;
    public string reactionText;
}
