using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remapper : MonoBehaviour, TouchScript.InputSources.ICoordinatesRemapper
{
    public TouchScript.InputSources.TuioInput[] tuioInputs;
    private int numberOfScreens;

    public void Awake()
    {
        int commandTUIOInput = QUT.CommandLineArguments.CommandLineArgument<int>("tuio");
        if (commandTUIOInput != 0) tuioInputs[0].TuioPort = commandTUIOInput;

        numberOfScreens = tuioInputs.Length;
#if UNITY_EDITOR
        for (int i = 0; i < numberOfScreens; i++)
        {
            tuioInputs[i].TuioPort += 10;
        }
#endif
    }

    public void Start()
    {
        for (int i = 0; i < numberOfScreens; i++)
        {
            tuioInputs[i].panelIndex = i;
            tuioInputs[i].numPanels = numberOfScreens;
            tuioInputs[i].CoordinatesRemapper = this;
        }
    }

    public Vector2 Remap(Vector2 input, int index, int size)
    {

        Vector2 output = Vector2.zero;
        output.x = input.y;
        output.y = input.x;

        output.x = 1 - output.x;
        output.y = 1 - output.y;

        output.x = 1080 * output.x + (1080 * index); // Screen.width * output.x  / numOfScreens + (Screen.width * index  / numOfScreens);
        output.y = Screen.height * output.y;

        //Debug.LogError("Remapped " + input.x + " , " + input.y + " to " + output.x + " , " + output.y);
        return output;
    }

    public Vector2 Remap(Vector2 input)
    {

        Vector2 output = Vector2.zero;
        output.x = Screen.width * input.x;
        output.y = Screen.height * input.y;

        //Debug.LogError("Remapped " + input.x + " , " + input.y + " to " + output.x + " , " + output.y);
        return output;
    }
}
