using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Draws a basic oscilloscope type graph in a GUI.Window()
/// Michael Hutton May 2020
/// This is just a basic 'as is' do as you wish...
/// Let me know if you use it as I'd be interested if people find it useful.
/// I'm going to keep experimenting wih the GL calls...eg GL.LINES etc 
/// </summary>
public class Graph : MonoBehaviour
{

    Material mat;
    private Rect windowRect = new Rect(20, 20, 1024, 256);
    private static int nextWindowId = 0;
    private int WindowId { get; set; }

    // A list of random values to draw
    private List<float> values = new List<float>();

    // The list the drawing function uses...
    private List<float> drawValues = new List<float>();

    // List of Windows
    public bool showWindow0 = false;

    public int sourceAmount = 1;
    public bool listener;
    // Start is called before the first frame update
    void Start()
    {
        WindowId = nextWindowId;
        nextWindowId++;
        mat = new Material(Shader.Find("Hidden/Internal-Colored"));
    }

    public void SetValues(float[] newValues)
    {
        values.Clear();
        values.AddRange(newValues);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnGUI()
    {
        if (showWindow0)
        {
            // Set out drawValue list equal to the values list 
            drawValues = values;
            windowRect = GUI.Window(WindowId, windowRect, DrawGraph, "");
        }
    }


    void DrawGraph(int windowID)
    {
        // Make Window Draggable
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));

        // Draw the graph in the repaint cycle
        if (Event.current.type == EventType.Repaint)
        {
            GL.PushMatrix();

            GL.Clear(true, false, Color.black);
            mat.SetPass(0);

            // Draw a black back ground Quad 
            GL.Begin(GL.QUADS);
            GL.Color(Color.black);
            GL.Vertex3(4, 4, 0);
            GL.Vertex3(windowRect.width - 4, 4, 0);
            GL.Vertex3(windowRect.width - 4, windowRect.height - 4, 0);
            GL.Vertex3(4, windowRect.height - 4, 0);
            GL.End();

            // Draw the lines of the graph
            GL.Begin(GL.LINES);
            GL.Color(Color.green);

            int valueIndex = VALUES.sampleSize - 1;
            for (int i = (int)windowRect.width - 4; i > 3; i--)
            {
                float y1 = 0;
                float y2 = 0;
                if (valueIndex > 0)
                {
                    y2 = (drawValues[valueIndex] * windowRect.height / 2);
                    y1 = (drawValues[valueIndex - 1] * windowRect.height / 2);
                }

                GL.Vertex3(i, windowRect.height / 2 + y2, 0);
                GL.Vertex3((i - 1), windowRect.height / 2 + y1, 0);
                valueIndex -= 1;
            }
            GL.End();

            GL.PopMatrix();
        }
    }
}