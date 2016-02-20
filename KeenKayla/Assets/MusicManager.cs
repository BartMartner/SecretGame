using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{

    public static MusicManager instance;
    public AudioSource _audioSource;
    private float _maxVolume = 0.2f;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        instance = this;
    }

    public void SetSong(AudioClip clip)
    {
        if(_audioSource.clip != clip)
        {
            StartCoroutine(ChangeSongs(clip));
        }
    }    

    private IEnumerator ChangeSongs(AudioClip clip)
    {
        if (_audioSource.isPlaying)
        {
            yield return StartCoroutine(FadeOut(1));
        }

        _audioSource.clip = clip;
        _audioSource.Play();
        yield return StartCoroutine(FadeIn(1));
    }

    private IEnumerator FadeOut(float time)
    {
        var timer = 0f;
        var _origVolume = _audioSource.volume;
        while (timer < time)
        {
            timer += Time.unscaledDeltaTime;
            _audioSource.volume = Mathf.Lerp(_origVolume, 0, timer / timer);
            yield return null;
        }
    }

    private IEnumerator FadeIn(float time)
    {
        var timer = 0f;
        var _origVolume = _audioSource.volume;
        while (timer < time)
        {
            timer += Time.unscaledDeltaTime;
            _audioSource.volume = Mathf.Lerp(_origVolume, _maxVolume, timer / timer);
            yield return null;
        }
    }
}
