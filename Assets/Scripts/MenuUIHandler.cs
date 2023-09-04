using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    public InputField nameInputField;

    public Text BestScoreText;

    public void Start()
    {
        //save entered name
        nameInputField.onEndEdit.AddListener(UpdateName);
        MainManager.LoadBestScoreData();

        if (MainManager.BestScoreResult > 0) {
            BestScoreText.text = $"Best score : {MainManager.BestScoreUserName} : {MainManager.BestScoreResult}";
        }    
    }

    // we assign event onClick on Start button
    public void StartNew()
    {
        if (MainManager.CurrentUserName.Length == 0) {
            Debug.Log("Enter user name");
        } else {
            SceneManager.LoadScene(1);
        }
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

    // in start() method listener is added
    public void UpdateName(string name)
    {
        MainManager.CurrentUserName = name;
    }
}
