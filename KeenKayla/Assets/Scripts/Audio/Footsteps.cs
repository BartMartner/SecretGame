using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Footsteps : MonoBehaviour
{
    public SurfaceType currentSurface;
    public List<AudioClip> sand;
    public float activeLayer;
    private AudioSource _audioSource;

    private Dictionary<SurfaceType, List<int>> _stepIndices = new Dictionary<SurfaceType, List<int>>();
    private List<AudioClip> _currentClips;
    private List<int> _currentIndices;
    private int _currentIndex;

    private void Start ()
    {
        _audioSource = GetComponent<AudioSource>();

        int randomIndex;
        List<int> tempIndices, indices;
        int i;
        int j;
        int clipCount;

        //create a randomized, non-repeating list of ints for each surface type
        foreach (SurfaceType surface in Enum.GetValues(typeof(SurfaceType)))
        {
            clipCount = GetSurfaceList(surface).Count;
            indices = new List<int>();
            _stepIndices.Add(surface, indices);

            for (i = 0; i < 4; i++)
            {
                tempIndices = new List<int>();
                for (j = 0; j < clipCount; j++)
                {
                    tempIndices.Add(j);
                }

                while (tempIndices.Count > 0)
                {
                    randomIndex = Random.Range(0, tempIndices.Count); //Choose a random object in the list
                    _stepIndices[surface].Add(tempIndices[randomIndex]); //add it to the new, random list
                    tempIndices.RemoveAt(randomIndex); //remove to avoid duplicates
                }
            }
        }

        SetSurface(currentSurface);
	}

    private List<AudioClip> GetSurfaceList(SurfaceType surface)
    {
        switch(surface)
        {
            default:
            case SurfaceType.Sand:
                return sand;
        }
    }

    public void SetSurface(SurfaceType surface)
    {
        _currentIndices = _stepIndices[surface];
        _currentIndex = Random.Range(0, _currentIndices.Count);
        _currentClips = GetSurfaceList(surface);
    }

    public void PlayFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.floatParameter == activeLayer && animationEvent.animatorClipInfo.weight > 0.5f)
        {
            _audioSource.PlayOneShot(_currentClips[_currentIndices[_currentIndex]]);
            _currentIndex++;
            if (_currentIndex >= _currentIndices.Count)
            {
                _currentIndex = Random.Range(0, _currentIndices.Count);
            }
        }       
    }
}
