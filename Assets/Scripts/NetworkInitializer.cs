using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class NetworkInitializer : MonoBehaviour
{
    public bool editorIsHost;

    void Start()
    {
#if !UNITY_EDITOR
        if (QUT.CommandLineArguments.HasArgument("server")) StartServer();
        if (QUT.CommandLineArguments.HasArgument("host")) StartHost();
        if (QUT.CommandLineArguments.HasArgument("client")) StartClient();
#else 
        if(editorIsHost) {
            StartHost();
        }
#endif
    }

    private void StartHost()
    {
        NetworkingManager.Singleton.StartHost();
    }

    private void StartServer()
    {
        NetworkingManager.Singleton.StartServer();
    }

    private void StartClient()
    {
        NetworkingManager.Singleton.StartClient();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
