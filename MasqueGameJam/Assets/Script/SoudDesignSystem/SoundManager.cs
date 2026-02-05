using System;
using UnityEngine;
public enum SoundType
{
    IntroductionAct,
    CalmAmbiance,
    Applause,
    CatharsysLaugh,
    CharacterExplosion,
    Clic,
    JohanTalk,
    Spotlight,
    Walking,
    Woosh,
    WheelDrums,
    CatharsysMad,
    CatharsysBored,
    SisypheCry,
    SisypheLaugh,
    SisypheSurprised,
    BeatriceShortLine,
    BeatriceNormalLine,
    BeatriceCryLine,
    JohannShortLine,
    JohannNormalLine,
    MephistoShortLine,
    MephistoNormalLine,
    PandoreShortLine,
    PandoreNormalLine,
    WilliamShortLine,
    WilliamNormalLine,
    TensionMusic,
    Poof,
    Rope,
    GroundImpact,
    Orchestral,
    Dramatic,
    Fire,
}
[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundsList;
    public static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundsList[(int)sound].Sound;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(randomClip, volume);
    }
#if UNITY_EDITOR

    private void OnEnable()
    {
        string[] names =  Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundsList,  names.Length);
        for (int i = 0; i < soundsList.Length; i++)
            soundsList[i].name = names[i];
    }
#endif
}
[Serializable]
public struct SoundList
{
    public AudioClip[] Sound {get => sounds; }
    [SerializeField] public string name;
    [SerializeField] private AudioClip[] sounds;
}