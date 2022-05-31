using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class AudioGaudioListener : MonoBehaviour
{
    public int numSamples = 4096;
    private Graph graph;
    void Start()
    {
        graph = this.GetComponentInChildren<Graph>();
    }

    // Update is called once per frame
    void Update()
    {
        if (graph.showWindow0) 
        {
            float[] farr = new float[numSamples];
            AudioListener.GetOutputData(farr, 1);
            graph.SetValues(farr);
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
}
