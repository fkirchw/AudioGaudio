using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveGenerator : MonoBehaviour
{
    [Range(1, 20000)]  //Creates a slider in the inspector
    public float frequency1;

    //[Range(0.01f, 1)]
    public float initialAmplitude = 1;
    //[Range(1, 20000)]  //Creates a slider in the inspector
    //public float frequency2;
    private float waveLengthInSeconds = 1;
    internal float currentAmplitude;
    internal float maxDistance
    {
        get
        {
            if(audioSource == null)
            {
                return 0;
            }
            else
            {
                return audioSource.maxDistance;
            }
        }
    }

    AudioSource audioSource;
    Graph graph;
    float[] farr = new float[1024];
    private int ti = 0;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        graph = this.GetComponentInChildren<Graph>();
        currentAmplitude = initialAmplitude;
        for (int i = 0; i < farr.Length; i += 1)
        {
            farr[i] = CreateSine(ti, frequency1, VALUES.sampleSize);
            ti++;
            if (ti >= (VALUES.sampleSize * waveLengthInSeconds))
            {
                ti = 0;
            }
        }
    }

    void Update()
    {
        if (graph.showWindow0)
        {
            graph.SetValues(farr);
        }
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += 1)
        {
            farr[i] = data[i] = CreateSine(ti, frequency1, VALUES.sampleSize);
            ti++;
            if (ti >= (VALUES.sampleSize * waveLengthInSeconds))
            {
                ti = 0;
            }
        }
    }

    //Creates a sinewave
    public float CreateSine(int timeIndex, float frequency, float sampleRate)
    {
        return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate) * currentAmplitude;
    }
}
