using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioListener))]
public class AudioGaudioListener : MonoBehaviour
{
    private Graph graph;
    private AudioListener audioListener;
    public int channel;
    private SineWaveGenerator[] generators;

    void Start()
    {
        graph = this.GetComponentInChildren<Graph>();
        generators = Object.FindObjectsOfType<SineWaveGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (SineWaveGenerator gen in generators)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, gen.transform.position, NavMesh.AllAreas, path);
            if (!hasPath || path.status != NavMeshPathStatus.PathComplete)
            {
                gen.currentAmplitude = 0;
            }
            else
            {
                float quotient = gen.maxDistance - GetPathLength(path);
                float newAmplitude = 0;
                if (quotient < 0f)
                {
                    newAmplitude = 0;
                }
                else
                {
                    newAmplitude = gen.initialAmplitude * quotient / gen.maxDistance;
                }

                gen.currentAmplitude = newAmplitude;
            }
        }

        if (graph.showWindow0)
        {
            float[] farr = new float[VALUES.sampleSize];
            AudioListener.GetOutputData(farr, channel);
            graph.SetValues(farr);
        }
    }

    private float GetPathLength(NavMeshPath path)
    {
        float total = 0;
        if (path.corners.Length < 2) return total;
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return total;
    }
    public static void DebugDrawPath(Vector3[] corners)
    {
        if (corners.Length < 2) { return; }
        int i = 0;

        for (; i < corners.Length - 1; i++)
        {
            Debug.DrawLine(new Vector3(corners[i].x, corners[i].y, 0), new Vector3(corners[i + 1].x, corners[i + 1].y, 0), Color.blue);
        }

        Debug.DrawLine(new Vector3(corners[0].x, corners[0].y, 0), new Vector3(corners[1].x, corners[1].y, 0), Color.red);
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
