using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioListener))]
public class AudioGaudioListener : MonoBehaviour
{
    private Graph graph;
    private SineWaveGenerator[] generators;
    private SpriteRenderer spriteRenderer;
    private float[] winArray = new float[1024];
    public string WinstateName;
    public string NextLevelName;
    public float delta = 0.001f;

    void Start()
    {
        graph = this.GetComponentInChildren<Graph>();
        generators = Object.FindObjectsOfType<SineWaveGenerator>();
        try
        {
            winArray = JsonUtility.FromJson<Winstate>(File.ReadAllText(WinstateName)).farr;
            if(winArray == null)
            {
                winArray = new float[] {float.MaxValue, float.MinValue};
            }
            else
            {
                graph.SetWinValues(winArray);
                Debug.Log("Write");
            }

        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (SineWaveGenerator gen in generators)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, gen.transform.parent.position, NavMesh.AllAreas, path);
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


        float[] currentFarr = new float[VALUES.sampleSize];
        AudioListener.GetOutputData(currentFarr, 0);

        if (graph.showWindow0)
        {
            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                File.WriteAllText(WinstateName, JsonUtility.ToJson(new Winstate { farr = currentFarr }));
                Debug.Log("Write");
            }
            graph.SetValues(currentFarr);
        }

        bool won = true;
        for (int i = 0; i < winArray.Length; i += 10)
        {
            if (Mathf.Abs(winArray[i] - currentFarr[i]) > delta)
            {
                won = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.R) || won)
        {
            spriteRenderer.color = Color.green;
            SceneManager.LoadScene(NextLevelName, LoadSceneMode.Single);
        }
        else
        {
            spriteRenderer.color = Color.red;
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

    void OnMouseDown()
    {
        graph.showWindow0 = !graph.showWindow0;
    }
}
