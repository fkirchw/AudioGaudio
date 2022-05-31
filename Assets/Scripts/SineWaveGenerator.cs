using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveGenerator : MonoBehaviour
{
    [Range(1, 20000)]  //Creates a slider in the inspector
    public float frequency1;

    [Range(0.01f, 1)]
    public float amplitude;
    //[Range(1, 20000)]  //Creates a slider in the inspector
    //public float frequency2;

    public float sampleRate = 44100;
    public float waveLengthInSeconds = 2.0f;

    AudioSource audioSource;
    int timeIndex = 0;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1; //force 2D sound
        AnimationCurve curve = audioSource.GetCustomCurve(AudioSourceCurveType.SpatialBlend);
        audioSource.Stop(); //avoids audiosource from starting to play automatically
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!audioSource.isPlaying)
            {
                timeIndex = 0;  //resets timer before playing sound
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }
    }



    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += 1)
        {
            data[i] = CreateSine(timeIndex, frequency1, sampleRate);
            timeIndex++;
            if (timeIndex >= (sampleRate * waveLengthInSeconds))
            {
                timeIndex = 0;
            }
        }
    }

    //Creates a sinewave
    public float CreateSine(int timeIndex, float frequency, float sampleRate)
    {
        return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate) * amplitude;
    }
}
