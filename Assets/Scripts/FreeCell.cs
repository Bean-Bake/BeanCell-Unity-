using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class FreeCell : MonoBehaviour
{
    // Refers to own object
    GameObject freeCell;

    // bools to avoid errors during starting coroutines
    bool gameOverHappening;
    bool gameStarted;

    // Game board serializable object and deck for starting cards
    public GameBoard gameBoard;
    public List<Card> deck;
    
    // Sprite Arrays and Lists of Game Object Holders (Foundations, Cells, Cascades)
    public Sprite[] cardFronts;
    public GameObject[] cascadesList;
    public GameObject[] foundationsList;
    public GameObject[] cellsList;

    // Prefab for Card Game Objects
    public GameObject cardPrefab;

    public int cellsFilled = 0;

    // Start is called before the first frame update
    void Start()
    {
        freeCell = GameObject.Find("FreeCell Game");
        gameBoard = new GameBoard();

        if (File.Exists(Application.dataPath + "/savedGame.txt"))
        {
            LoadGame();
        }
        else
        {
            deck = CreateDeck();
            ShuffleDeck(GlobalVariables.GetGameSeed());
            SortIntoCascades();
        }

        StartCoroutine(DealDeck());
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOverHappening && gameStarted)
        {
            GameOverShortcut();
        }

        if (GameOver())
        {
            File.Delete(Application.dataPath + "/savedGame.txt");
            File.Delete(Application.dataPath + "/savedGame.txt.meta");
            SceneManager.LoadScene("Game Won");
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
        int seed = Environment.TickCount;
        GlobalVariables.SetGameSeed(seed);
        System.Random shuffleIndex = new System.Random(seed);

        for (int i = 0; i < 400; i++)
			{
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

    // Shuffle cards
    void ShuffleDeck(int seed)
    {
        GlobalVariables.SetGameSeed(seed);
        System.Random shuffleIndex = new System.Random(seed);
        
        for (int i = 0; i < 400; i++)
        {
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
                gameBoard.cascadeBoard[i].cardHolder.Add(deck.Last());
                deck.RemoveAt(deck.Count - 1);
            }
        }

        for (int i = 4; i < 8; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                gameBoard.cascadeBoard[i].cardHolder.Add(deck.Last());
                deck.RemoveAt(deck.Count - 1);
            }
        }
    }

    // Deal Card Game Objects into proper places in game screen
    // This deals the cascades, the foundations, and the cells in case it's a continued (loaded) game
    IEnumerator DealDeck()
    {
        for (int i = 0; i < 4; i++)
        {
            float yLocation = 0;
            float zLocation = 0.01f;
            
            foreach (Card card in gameBoard.cellBoard[i].cardHolder)
            {
                Vector3 cardLocation = new Vector3(cellsList[i].transform.position.x, 
                                                    cellsList[i].transform.position.y - yLocation, 
                                                    cellsList[i].transform.position.z - zLocation);

                GameObject newCard = Instantiate(cardPrefab, cardLocation, Quaternion.identity, cellsList[i].transform);
                newCard.name = card.getWholeName();
                zLocation += .01f;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            float yLocation = 0;
            float zLocation = 0.01f;

            foreach (Card card in gameBoard.foundationBoard[i].cardHolder)
            {
                Vector3 cardLocation = new Vector3(foundationsList[i].transform.position.x, 
                                                    foundationsList[i].transform.position.y - yLocation, 
                                                    foundationsList[i].transform.position.z - zLocation);

                GameObject newCard = Instantiate(cardPrefab, cardLocation, Quaternion.identity, foundationsList[i].transform);
                newCard.name = card.getWholeName();
                zLocation += .01f;
            }
        }
        
        
        for (int i = 0; i < 8; i++)
        {

            float yLocation = 0;
            float zLocation = 0.01f;

            foreach (Card card in gameBoard.cascadeBoard[i].cardHolder)
            {
                Vector3 cardLocation = new Vector3(cascadesList[i].transform.position.x, 
                                                    cascadesList[i].transform.position.y - yLocation, 
                                                    cascadesList[i].transform.position.z - zLocation);

                GameObject newCard = Instantiate(cardPrefab, cardLocation, Quaternion.identity, cascadesList[i].transform);
                newCard.name = card.getWholeName();

                yLocation += .5f;
                zLocation += .01f;
                yield return new WaitForSeconds(0.01f);
            }
        }

        gameStarted = true;
        SaveGame();
    }


    // EXTRA PRINTING/DEBUG FUNCTIONS

    // Prints the deck of cards
    void PrintDeck()
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
        foreach (CardHolder cascade in gameBoard.cascadeBoard)
        {
            string countS = "Cascade" + count;
            print(countS);
            count++;
            foreach (Card card in cascade.cardHolder)
            {
                string cardString = "" + card.getWholeName();
                print(cardString);
            }
        }
    }

    // Checks if all the foundations are full
    // If so, game is over
    bool GameOver()
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

    // Checks if the game is essentially over
    // Once all the cascades are in order or empty, we can ascertain the rest of the work to finish the game is tedious/unnecessary
    // The game is basically won
    void GameOverShortcut()
    {
        foreach (GameObject cascade in cascadesList)
        {
            if (cascade.transform.childCount < 2)
            {
                continue;
            }
            else
            {
                for (int i = cascade.transform.childCount - 1; i > 0; i--)
                {
                    if (gameObject.GetComponent<UInput>().ValidStack(cascade.transform.GetChild(i).gameObject, cascade.transform.GetChild(i - 1).gameObject))
                    {
                        continue;
                    }
                    return;
                }
            }
        }

        gameOverHappening = true;
        StartCoroutine(SendNextToFoundation());
    }

    // Coroutine used to finish game once clearly one
    // The idea is the player doesn't have to manually move cards as it's tedious
    IEnumerator SendNextToFoundation()
    {
        while (!GameOver())
        {
            foreach (GameObject cascade in cascadesList)
            {
                if (cascade.transform.childCount > 0)
                {
                    foreach (GameObject foundation in foundationsList)
                    {
                        if (gameObject.GetComponent<UInput>().ValidStack(cascade.transform.GetChild(cascade.transform.childCount - 1).gameObject, foundation.transform.GetChild(foundation.transform.childCount - 1).gameObject))
                        {                           
                            yield return new WaitForSeconds(0.05f);
                            gameObject.GetComponent<UInput>().Shortcut(cascade.transform.GetChild(cascade.transform.childCount - 1).gameObject);
                            break;
                        }
                    }
                }
            }

            foreach (GameObject cell in cellsList)
            {
                if (cell.transform.childCount > 0)
                {
                    foreach (GameObject foundation in foundationsList)
                    {
                        if (gameObject.GetComponent<UInput>().ValidStack(cell.transform.GetChild(cell.transform.childCount - 1).gameObject, foundation.transform.GetChild(foundation.transform.childCount - 1).gameObject))
                        {
                            yield return new WaitForSeconds(0.05f);
                            gameObject.GetComponent<UInput>().Shortcut(cell.transform.GetChild(cell.transform.childCount - 1).gameObject);
                            break;
                        }
                    }
                }
            }
        }
    }

    // Saves game via JSON serialization
    // Saves using card classes (not game objects)
    // Saves game board and undo "stack"
    public void SaveGame()
    {
        gameBoard.Clear();

        for (int i = 0; i < foundationsList.Length; i++)
        {
            for (int j = 0; j < foundationsList[i].transform.childCount; j++)
            {
                Card card = new Card(foundationsList[i].transform.GetChild(j).GetComponent<CardFields>().suitValue, foundationsList[i].transform.GetChild(j).GetComponent<CardFields>().value);
                gameBoard.foundationBoard[i].cardHolder.Add(card);
            }
        }

        for (int i = 0; i < cellsList.Length; i++)
        {
            for (int j = 0; j < cellsList[i].transform.childCount; j++)
            {
                Card card = new Card(cellsList[i].transform.GetChild(j).GetComponent<CardFields>().suitValue, cellsList[i].transform.GetChild(j).GetComponent<CardFields>().value);
                gameBoard.cellBoard[i].cardHolder.Add(card);
            }
        }

        for (int i = 0; i < cascadesList.Length; i++)
        {
            for (int j = 0; j < cascadesList[i].transform.childCount; j++)
            {
                Card card = new Card(cascadesList[i].transform.GetChild(j).GetComponent<CardFields>().suitValue, cascadesList[i].transform.GetChild(j).GetComponent<CardFields>().value);
                gameBoard.cascadeBoard[i].cardHolder.Add(card);
            }
        }

        string json = JsonUtility.ToJson(gameBoard, true);
        File.WriteAllText(Application.dataPath + "/savedGame.txt", json);
    }

    //  Loads game via JSON deserialization
    //  Loads game board and undo "stack"
    public void LoadGame()
    {
        if (File.Exists(Application.dataPath + "/savedGame.txt"))
        {
            string save = File.ReadAllText(Application.dataPath + "/savedGame.txt");
            GameBoard newBoard = JsonUtility.FromJson<GameBoard>(save);
            gameBoard = newBoard;
            foreach (NewMove move in gameBoard.moveList)
            {
                move.RebuildSelf();
            }
        }
    }
}