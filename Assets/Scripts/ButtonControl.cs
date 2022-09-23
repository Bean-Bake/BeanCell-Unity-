using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class ButtonControl : MonoBehaviour
{
    GameObject freeCell;

    void Start()
    {
        freeCell = GameObject.Find("FreeCell Game");
    }

    // START A NEW GAME
    public void StartMainGame()
    {
        if (File.Exists(Application.dataPath + "/savedGame.txt"))
        {
            File.Delete(Application.dataPath + "/savedGame.txt");
            File.Delete(Application.dataPath + "/savedGame.txt.meta");
        }

        GlobalVariables.SetGameSeed(Environment.TickCount);
        SceneManager.LoadScene("Beancell Game");
    }

    // CONTINUE A STARTED GAME
    public void ContinueMainGame()
    {
        SceneManager.LoadScene("Beancell Game");
    }

    // RESTART THE CURRENT GAME INSTEAD OF STARTING A NEW GAME
    public void RestartCurrentGame()
    {
        if (File.Exists(Application.dataPath + "/savedGame.txt"))
        {
            File.Delete(Application.dataPath + "/savedGame.txt");
            File.Delete(Application.dataPath + "/savedGame.txt.meta");
        }
        SceneManager.LoadScene("Beancell Game");
    }
    
    // UNDO LAST MOVE
    public void Undo()
    {
        freeCell.GetComponent<UInput>().Undo();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    // SAVE GAME BOARD AND UNDO "STACK"
    public void Save()
    {
        freeCell.GetComponent<FreeCell>().SaveGame();
    }
    
    // LOAD PREVIOUSLY STARTED GAME
    public void Load()
    {
        freeCell.GetComponent<FreeCell>().LoadGame();
    }
}
