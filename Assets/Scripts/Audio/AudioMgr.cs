using UnityEngine;


public class AudioMgr : SingletonMono<AudioMgr> {
    [SerializeField] private AudioSource bgmSource, fxSource;

    [SerializeField]
    private AudioClip refreshTask,
                      onClose,
                      openLetter,
                      plantTree1,
                      plantTree2,
                      controlSand1,
                      controlSand2;


    public float BgmVolumn {
        get => bgmSource.volume;
        set => bgmSource.volume = Mathf.Clamp01(value);
    }

    public bool IsBgmPlaying => bgmSource.isPlaying;

    public void PlayBgm() => bgmSource.UnPause();

    public void PauseBgm() => bgmSource.Pause();


    protected override void OnInstanceAwake() {
        bgmSource.Play();
    }


    public float FxVolumn {
        get => fxSource.volume;
        set => fxSource.volume = Mathf.Clamp01(value);
    }


    public void PlayFx(AudioFxType fx) {
        fxSource.Stop();
        fxSource.clip = fx switch {
            AudioFxType.RefreshTask => refreshTask,
            AudioFxType.OnClose => onClose,
            AudioFxType.OpenLetter => openLetter,
            AudioFxType.PlantTree => Random.value < .5f ? plantTree1 : plantTree2,
            AudioFxType.ControlSand => Random.value < .5f ? controlSand1 : controlSand2,
            _ => null
        };
        fxSource.Play();
    }
}


public enum AudioFxType {
    RefreshTask,
    OnClose,
    OpenLetter,
    PlantTree,
    ControlSand,
}
