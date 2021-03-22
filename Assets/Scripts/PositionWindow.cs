
using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

/// <summary>
/// A component to position/resize the current active OS window
/// Window width [-w], height [-h], x position [-x] and y-position [-y] are specified via command line arguments
/// The specified values are applied to the window with the current active focus on Start
/// </summary>
public class PositionWindow : MonoBehaviour
{

#if UNITY_STANDALONE_WIN || UNITY_EDITOR

    // Import required OS Window functions from user32.dll

    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int y, int w, int h, int wFlags);

    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    static extern IntPtr FindWindow(System.String className, System.String windowName);

    [DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", EntryPoint = "GetWindowText")]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("user32.dll", EntryPoint = "SetWindowText")]
    static extern int SetWindowText(IntPtr hWnd, System.String text);

    /// <summary>
    /// Get the title of the currently active OS window
    /// </summary>
    /// <returns>The title of the currently active OS window</returns>
    private string GetActiveWindowTitle()
    {
        const int nChars = 256;
        StringBuilder Buff = new StringBuilder(nChars);
        IntPtr handle = GetForegroundWindow();

        if (GetWindowText(handle, Buff, nChars) > 0)
        {
            return Buff.ToString();
        }
        return null;
    }

    /// <summary>
    /// Set the size and position of the current active window that has focus
    /// </summary>
    /// <param name="x">The new position of the left side of the window, in client coordinates</param>
    /// <param name="y">The new position of the top of the window, in client coordinates</param>
    /// <param name="w">The new width of the window, in pixels</param>
    /// <param name="h">The new height of the window, in pixels</param>
    public void SetPosition(int x, int y, int w = 0, int h = 0)
    {
        // Get the title of the currently focused window
        string name = GetActiveWindowTitle();

        // Update the window position and size
        SetWindowPos(FindWindow(null, name), 0, x, y, w, h, w * h == 0 ? 1 : 0);        
    }

#endif

    void Start()
    {
        try
        {
            int x = -1, y = -1;
            int w = 0, h = 0;

            // Retrieve command line arguments specified at launch
            // NOTE: This is an alternative/native approach to the CommandLineArguments helper class
            string arg;
            string[] args = Environment.GetCommandLineArgs();
          
            for(int i=0; i<args.Length; ++i)
            {
                arg = args[i];

                if (arg != null && args.Length > 0)
                {
                    switch(arg)
                    {
                        case "-x":
                            if(args.Length > i+1)
                                x = int.Parse(args[++i]);
                            break;

                        case "-y":
                            if (args.Length > i + 1)
                                y = int.Parse(args[++i]);
                            break;

                        case "-w":
                            if (args.Length > i + 1)
                                w = int.Parse(args[++i]);
                            break;

                        case "-h":
                            if (args.Length > i + 1)
                                h = int.Parse(args[++i]);
                            break;

                        default:
                            break;
                    }
                }
            }

            if(x != -1 && y != -1)
                SetPosition(x, y, w, h);
        }
        catch (Exception e)
        {
            Debug.LogWarningFormat("WARNING: Parsing arguments failed, using default values ({0})\n", e.Message);
        }
    }
}