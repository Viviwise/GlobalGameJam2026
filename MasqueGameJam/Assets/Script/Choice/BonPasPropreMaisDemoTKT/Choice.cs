using UnityEngine;

[CreateAssetMenu(fileName = "NewChoice", menuName = "Theatre/Choice")]
public class Choice : ScriptableObject
{
    public string label; // texte du bouton
    public Sequence sequence;
    public SceneSO targetScene; // scene à charger après
}