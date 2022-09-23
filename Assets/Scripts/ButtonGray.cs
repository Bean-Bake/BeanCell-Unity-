using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

// This simply grays out (makes uninteractable) the continue button in main menu if there isn't actually a previously started game
public class ButtonGray : MonoBehaviour
{
    Button continueButton;
    // Start is called before the first frame update
    void Start()
    {
        continueButton = this.gameObject.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (File.Exists(Application.dataPath + "/savedGame.txt"))
        {
            continueButton.interactable = true;
        }
        else
        {
            continueButton.interactable = false;
        }
    }
}
