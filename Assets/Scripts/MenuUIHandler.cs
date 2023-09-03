using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(500)]
public class MenuUIHandler : MonoBehaviour
{

    // we assign event onClick on Start button
    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }
    
    public void Exit()
    {
        //it is actually instructions for the compiler. (lines starts with # , such lines will not builded)
        // inside unity it will compile EditorApplication.ExitPlaymode() but during buid Application.Quit()
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit(); // original code to quit Unity player
        #endif
    }
}
