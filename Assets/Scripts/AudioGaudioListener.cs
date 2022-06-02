using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class AudioGaudioListener : MonoBehaviour
{
    private int numSamples = 4096;
    private Graph graph;
    private float max = 0f;
    private float min = 0f;
    public SpriteRenderer listener;
    public SpriteRenderer source; 
    [Range(1, 50)] 
    public int frequenze = 2;
    
    
    private int timeIndex;
    private float expFreq = 2;
    private float expAmp = 1;
    private float[] inData = new float[2048];
    public float waveLengthInSeconds = 2.0f;

    


    private void Awake()
    {
        timeIndex = 0;
        for (int i = 0; i < inData.Length; i++)
        {
            inData[i] = CreateSine(timeIndex, expFreq, numSamples);
            timeIndex++;
            if (timeIndex >= (numSamples * waveLengthInSeconds))
            {
                timeIndex = 0;
            }
        }
    }

    void Start()
    {
        source = GetComponent<SpriteRenderer>();
        listener = GetComponent<SpriteRenderer>();
        graph = this.GetComponentInChildren<Graph>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (frequenze % 2 != 0)
        {
            frequenze--;
        }

        if (graph.showWindow0) 
        {
            float[] inFreq = new float[numSamples];
            AudioListener.GetOutputData(inFreq, 1);
            if (max == 0.0f)
            {
                setMinMax(inFreq);
            }
            else
            {
                CompareFreq(inFreq);
            }
            graph.SetValues(inFreq);
        }
    }

    private void setMinMax(float[] inFreq)
    {
        for (int i = 0; i < inFreq.Length; i++)
        {
            if (min > inFreq[i])
            {
                min = inFreq[i];
            }else if (max < inFreq[i])
            {
                max = inFreq[i];
            }
        }

        source.color = Color.black;
    }

    private void CompareFreq(float[] inFreq)
    {
        int count = 0;
        for (int i = 0; i < inFreq.Length; i++)
        {
            if (Equals(max, inFreq[i], 0.000001f))
            {
                count++;
            }
            
        }
        Debug.Log(count);
        
        if (count == 8)
        {
            listener.color = Color.green;
        }
        else
        {
            listener.color = Color.red;
        }
    }

    private void OnMouseOver()
    {
        graph.showWindow0 = true;
    }

    private void OnMouseExit()
    {
        graph.showWindow0 = false;
    }
    
    
    //Creates a sinewave
    public float CreateSine(int timeIndex, float frequency, float sampleRate)
    {
        return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate) * expAmp;
    }
    
    bool Equals(double x, double y, double tolerance)
    {
        var diff = Math.Abs(x - y);
        return diff <= tolerance ||
               diff <= Math.Max(Math.Abs(x), Math.Abs(y)) * tolerance;
    }
    
}
