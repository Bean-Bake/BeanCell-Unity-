using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{
    GameObject freeCell;

    void Start()
    {
        freeCell = GameObject.Find("FreeCell Game");
    }
    public void StartMainGame()
    {
        SceneManager.LoadScene("BeancellGame");
    }
    public void Undo()
    {
        freeCell.GetComponent<UInput>().Undo();
    }
}
