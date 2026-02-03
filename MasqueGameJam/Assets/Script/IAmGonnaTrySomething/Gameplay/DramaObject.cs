using System.Collections;
using UnityEngine;

public class DramaObject : MonoBehaviour
{
    public ObjectsID id;
    [SerializeField] public string actorName;
    [SerializeField] public bool killable;
    public bool dead;
    public Sprite deadSprite;
    private ParticleSystem explosionEffect;

    public void Explode(ParticleSystem effect)
    {
        explosionEffect=effect;
        explosionEffect.gameObject.SetActive(true);
        explosionEffect.gameObject.transform.position = transform.position;
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        SoundManager.PlaySound(SoundType.CharacterExplosion,0.6f);
        yield return new WaitForSeconds(3f);
        explosionEffect.Play();
        dead = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = deadSprite;
    }
}

public enum ObjectsID
{
    Beatrice,
    William, 
    Johann,
    Pandore,
    Mephisto,
    MissSisyphe,
    LightOperator,
    RopeOperator,
    SoundOperator,
    TowerObject,
    ChandelierObject,
    
}
