namespace Script.Choice.BonPasPropreMaisDemoTKT
{
    using UnityEngine;

    public enum CharacterID
    {
        William,
        Beatrice,
        Johann,
        Mephisto,
        Pandore,
        Tech,
        Decor
    }
    public class CharacterActor : MonoBehaviour
    {
        public CharacterID characterID;
        public string displayName;

        [HideInInspector] public Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
    }


}