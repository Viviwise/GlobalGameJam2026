using UnityEngine;

public enum ActionType { PlayAnimation, Dialogue, Spawn, Wait, Sound }

[System.Serializable]
public class SequenceAction
{
    public ActionType type;
    public string character;        

    public AnimationClip animationClip; 

    [TextArea] public string dialogueText;
    public SoundType soundType;

    public GameObject prefabToSpawn;
    public float duration = 1f;      
    public bool blocking = true;     
}
