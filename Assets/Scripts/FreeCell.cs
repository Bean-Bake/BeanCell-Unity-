using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class FreeCell : MonoBehaviour
{
    // Sprite Arrays and Lists of Game Object Holders (Foundations, Cells, Cascades)
    public Sprite[] cardFronts;
    public GameObject[] cascadesList;
    public GameObject[] foundationsList;
    public GameObject[] cellsList;

    // Prefab for Card Game Objects
    public GameObject cardPrefab;
    
    public List<Card> deck;

    // BACKGROUND ARRAYS OF CARDS
    // NOT GAME OBJECTS
    public List<Card>[] cascadeBoard;
    private List<Card> cascade0 = new List<Card>();
    private List<Card> cascade1 = new List<Card>();
    private List<Card> cascade2 = new List<Card>();
    private List<Card> cascade3 = new List<Card>();
    private List<Card> cascade4 = new List<Card>();
    private List<Card> cascade5 = new List<Card>();
    private List<Card> cascade6 = new List<Card>();
    private List<Card> cascade7 = new List<Card>();
    public int cellsFilled = 0;

    // Start is called before the first frame update
    void Start()
    {
        deck = CreateDeck();
        ShuffleDeck();

        cascadeBoard = new List<Card>[] {cascade0, cascade1, cascade2, cascade3, cascade4, cascade5, cascade6, cascade7};

        SortIntoCascades();
        DealDeck();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver())
        {
             SceneManager.LoadScene("Main Menu");
        }
    }

    // Create deck of cards
    public static List<Card> CreateDeck()
    {
        List<Card> newDeck = new List<Card>();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                Card newCard = new Card(i, j);
                newDeck.Add(newCard);
            }
        }
        return newDeck;
    }

    // Shuffle cards
    void ShuffleDeck()
    {
        for (int i = 0; i < 300; i++)
			{
                System.Random shuffleIndex = new System.Random();
				// find two random indices of the array and create a temporary placeholder
				int randOne = shuffleIndex.Next(52);
				int randTwo = shuffleIndex.Next(52);
				Card temp = null;
				
				// swap cards at those two indices using that temporary placeholder
				temp = deck[randOne];
				deck[randOne] = deck[randTwo];
				deck[randTwo] = temp;
			}
    }


    // Deal cards into their proper places in background arrays
    void SortIntoCascades()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                cascadeBoard[i].Add(deck.Last());
                deck.RemoveAt(deck.Count - 1);
            }
        }

        for (int i = 4; i < 8; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                cascadeBoard[i].Add(deck.Last());
                deck.RemoveAt(deck.Count - 1);
            }
        }
    }

    // Deal Card Game Objects into proper places in game screen
    void DealDeck()
    {
        for (int i = 0; i < 8; i++)
        {

            float yLocation = 0;
            float zLocation = 0.01f;

            foreach (Card card in cascadeBoard[i])
            {
                
                Vector3 cardLocation = new Vector3(cascadesList[i].transform.position.x, 
                                                    cascadesList[i].transform.position.y - yLocation, 
                                                    cascadesList[i].transform.position.z - zLocation);

                GameObject newCard = Instantiate(cardPrefab, cardLocation, Quaternion.identity, cascadesList[i].transform);
                newCard.name = card.getWholeName();

                yLocation += .5f;
                zLocation += .01f;
            }
        }
    }


    // EXTRA PRINTING/DEBUG FUNCTIONS

    // Prints the deck of cards
    void printDeck()
    {
        foreach (Card card in deck)
        {
            string cardString = "" + card.getSuit() + " " + card.getRank();
            print(cardString);
        }
    }

    //  Prints the Cascades after cards have been dealt
    public void PrintCascades()
    {
        int count = 0;
        foreach (List<Card> cascade in cascadeBoard)
        {
            string countS = "Cascade" + count;
            print(countS);
            count++;
            foreach (Card card in cascade)
            {
                string cardString = "" + card.getWholeName();
                print(cardString);
            }
        }
    }

    bool gameOver()
    {
        foreach (GameObject foundation in foundationsList)
        {
            if (foundation.transform.childCount < 13)
            {
                return false;
            }
        }
        return true;
    } 
}