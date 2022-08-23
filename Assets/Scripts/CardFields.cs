using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFields : MonoBehaviour
{
    // Suit, color, and value (rank) for Card Game Objects
    public string suit;
    public string color;
    public int value;

    // Start is called before the first frame update
    void Start()
    {
        // splits card name into separate strings to assign card game object fields
        string[] cardProperties = transform.name.Split(" of ");
        if (cardProperties.Length > 1)
        {
            // suit is simply the name of the suit on the card
            suit = cardProperties[1];

            // color depends on the given suit
            if (suit == "Spades" || suit == "Clubs")
            {
                color = "Black";
            }
            else
            {
                color = "Red";
            }

            // card rank/value set to int value based on string value
            switch (cardProperties[0])
            {
            case "Ace":
                value = 0;
                break;
            case "Two":
                value = 1;
                break;
            case "Three":
                value = 2;
                break;
            case "Four":
                value = 3;
                break;
            case "Five":
                value = 4;
                break;
            case "Six":
                value = 5;
                break;
            case "Seven":
                value = 6;
                break;
            case "Eight":
                value = 7;
                break;
            case "Nine":
                value = 8;
                break;
            case "Ten":
                value = 9;
                break;
            case "Jack":
                value = 10;
                break;
            case "Queen":
                value = 11;
                break;
            case "King":
                value = 12;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}