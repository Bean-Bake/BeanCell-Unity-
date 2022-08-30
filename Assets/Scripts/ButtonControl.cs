using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   
using UnityEngine.SceneManagement;
using System;

public class ButtonControl : MonoBehaviour
{
    GameObject freeCell;

    void Start()
    {
        freeCell = GameObject.Find("FreeCell Game");
    }

    public void StartMainGame()
    {
        GlobalVariables.SetGameSeed(Environment.TickCount);
        SceneManager.LoadScene("BeancellGame");
    }

    public void RestartCurrentGame()
    {
        SceneManager.LoadScene("BeancellGame");
    }
    
    public void Undo()
    {
        freeCell.GetComponent<UInput>().Undo();
    }
}
