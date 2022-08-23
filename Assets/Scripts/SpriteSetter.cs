using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSetter : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;
    private SpriteRenderer spriteRenderer;
    private FreeCell game;
    private UInput userInput;



    // Start is called before the first frame update
    void Start()
    {
        // Create deck to hold all possible cards
        List<Card> deck = FreeCell.CreateDeck();
        game = FindObjectOfType<FreeCell>();
        userInput = FindObjectOfType<UInput>();

        // Compare card game object to each card in deck
        // Once match found, set card face to appropriate sprite
        int i = 0;
        foreach (Card card in deck)
        {
            if (this.name == card.getWholeName())
            {
                cardFace = game.cardFronts[i];
                break;
            }
            i++;
        }

        // assigns sprite renderer for selected card color changes
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // card face is rendered
        spriteRenderer.sprite = cardFace;

        // if selected, put a yellow highlight
        if (userInput.selected.Contains(gameObject))
        {
            spriteRenderer.color = Color.yellow;
        }
        // Otherwise, normal (white or uncolored) light on sprite
        else
        {
            spriteRenderer.color = Color.white;
        }
    }
}