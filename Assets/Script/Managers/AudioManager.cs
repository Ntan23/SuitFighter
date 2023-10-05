using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance {get; private set;}
    void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    #endregion
    [SerializeField] private Sound[] sounds;
    private GameManager gm;

    private void Play(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);

        if(s == null) return;

        s.source.Play();
    }

    private void Stop(string name)
    { 
        Sound s = System.Array.Find(sounds, sound => sound.name == name);

        if(s == null) return;

        s.source.Stop();
    }

    void Start() 
    {
        gm = GameManager.instance;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.audioMixer;
        }

        Play("BGM");
    } 
    
    public void PlayHoverRockCardSFX() 
    {
        if(!gm.isEnded && gm.canPlay) Play("HoverRockCard");
    }

    public void PlayHoverPaperCardSFX()
    {
        if(!gm.isEnded && gm.canPlay) Play("HoverPaperCard");
    }

    public void PlayHoverScissorsCardSFX() 
    {
        if(!gm.isEnded && gm.canPlay) Play("HoverScissorsCard");
    }
    
    public void PlayPlayerAttackSFX() => Play("PlayerAttack");

    public void PlayGulpSFX() => Play("Gulp");
    public void PlayHintSFX() => Play("Hint");
}

