using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UInput : MonoBehaviour
{
    public List<GameObject> selected = new List<GameObject>();
    private FreeCell game;

    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<FreeCell>();
    }

    // Update is called once per frame
    void Update()
    {
        GetClick();
    }

    // Defines mouse click controls
    void GetClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit)
            {
                if (hit.collider.CompareTag("Card"))
                {
                    if (selected.Count == 0)
                    {
                        SelectCard(hit.collider.gameObject);
                    }
                    else
                    {
                        MoveStack(hit.collider.gameObject);
                    }
                }
                else if (hit.collider.CompareTag("Cell"))
                {
                    if (selected.Count == 1)
                    {
                        MoveStack(hit.collider.gameObject);
                    }
                }
                else if (hit.collider.CompareTag("Cascade"))
                {
                    if (selected.Count > 0)
                    {
                        MoveStack(hit.collider.gameObject);
                    }
                }
                else if (hit.collider.CompareTag("Foundation"))
                {
                    if (selected.Count == 1)
                    {
                        MoveStack(hit.collider.gameObject);
                    }
                }
            }
            else
            {
                selected.Clear();
                print("Stack Deselected");
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit)
            {
                if (hit.collider.CompareTag("Card"))
                {
                    print("Card");
                    Shortcut(hit.collider.gameObject);
                }
            }
        }

        // FOR DEBUGGING / GOD MODE GAMEPLAY
        else if (Input.GetMouseButtonDown(2))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit)
            {
                if (hit.collider.CompareTag("Card"))
                {
                    if (selected.Count == 0)
                    {
                        FreeSelect(hit.collider.gameObject);
                    }
                    else
                    {
                        FreeMove(hit.collider.gameObject);
                    }
                }
            }
        }
    }

    // Selects cards according to game logic
    void SelectCard(GameObject card)
    {
        // If the card to be selected is either in a cell or a foundation, select it
        if (card.transform.parent.gameObject.CompareTag("Cell") || card.transform.parent.gameObject.CompareTag("Foundation"))
        {
            selected.Add(card);
            return;
        }
        // Otherwise, it must be in the Cascades
        else
        {
            int index = card.transform.GetSiblingIndex();

            int numSelecting = card.transform.parent.childCount - index;
            int allowedToSelect = (5 - game.cellsFilled);

            // Check if number of cards selected is less than cards allowed based on filled cells
            if (numSelecting > allowedToSelect)
            {
                print("Too many cards selected");
                return;
            } 

            // if it's not the last card in the Cascade
            if (index != card.transform.parent.childCount - 1)
            {
                GameObject card1 = card.transform.parent.GetChild(index).gameObject;
                GameObject card2 = card.transform.parent.GetChild(index + 1).gameObject;

                if (ValidStack(card2, card))
                {
                    selected.Add(card);
                    GameObject nextCard = card2;
                    SelectCard(nextCard);
                }
                else
                {
                    print("Invalid selection");
                    selected.Clear();
                    return;
                }
            }
            else
            {
                selected.Add(card);
                print("Stack selected");
            }
        }
    }

    // Checks if the two cards (or a card and a cascade/cell/foundation) are actually legally stackable
    public bool ValidStack(GameObject card, GameObject spotToStack)
    {
        if (spotToStack.CompareTag("Cascade"))
        {
            return true;
        }
        else if (spotToStack.CompareTag("Cell"))
        {
            return true;
        }
        else if (spotToStack.CompareTag("Foundation"))
        {
            if (card.GetComponent<CardFields>().value == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // If it's a card
        else
        {
            if (spotToStack.transform.parent.gameObject.CompareTag("Cell"))
            {
                return false;
            }
            else if (spotToStack.transform.parent.gameObject.CompareTag("Foundation"))
            {
                if (card.GetComponent<CardFields>().suit == spotToStack.GetComponent<CardFields>().suit
                    && card.GetComponent<CardFields>().value == spotToStack.GetComponent<CardFields>().value + 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else    // In Cascade
            {
                if (card.GetComponent<CardFields>().color != spotToStack.GetComponent<CardFields>().color
                    && card.GetComponent<CardFields>().value == spotToStack.GetComponent<CardFields>().value - 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    // Starts the move routine within the game logic, calls StackCards for the actual movement
    void MoveStack(GameObject spotToStack)
    {  
        // Start by identifying the list holding the selected card(s)
        Transform startingParent = selected[0].transform.parent;

        // Then identify the destination list
        Transform destinationParent;

        // If destination is a Card
        if (spotToStack.CompareTag("Card"))
        {
            destinationParent = spotToStack.transform.parent;
        }
        // Otherwise, it must be an empty Cell, Foundation, or Cascade
        else
        {
            destinationParent = spotToStack.transform;
        }
        StackCards(spotToStack, startingParent, destinationParent);
    }
    
    // Moves the cards with an offset depending on where, adds move to moveList for undo function
    void StackCards(GameObject spotToStack, Transform startingParent, Transform destinationParent)
    {
        // offset of Card Game Object(s) to whatever they are stacked on
        // If the selected are moving to a Cell, Foundation, a Card in a Foundation, or an empty Cascade, no offset
        float yOffset = 0.0f;
        float zOffset = 0.01f;

        // If being moved to a card that's not in a foundation (IE in a Cascade), stack it like cards (offset slightly)
        if (spotToStack.gameObject.CompareTag("Card") && !spotToStack.transform.parent.gameObject.CompareTag("Foundation"))
        {
            yOffset = 0.5f;
        }

        if (ValidStack(selected[0], spotToStack))
        {
            int topCardIndex = destinationParent.transform.childCount;

            for (int i = 0; i < selected.Count; i++)
            {
                if (startingParent.gameObject.CompareTag("Cell") && !spotToStack.CompareTag("Cell"))
                {
                    game.cellsFilled--;
                }
                else if (!startingParent.gameObject.CompareTag("Cell") && spotToStack.CompareTag("Cell"))
                {
                    game.cellsFilled++;
                }
                selected[i].transform.position = 
                new Vector3(spotToStack.transform.position.x, 
                            spotToStack.transform.position.y - yOffset, 
                            spotToStack.transform.position.z - zOffset);
                selected[i].transform.parent = destinationParent; // this makes the children move with the parents

                yOffset += 0.5f;
                zOffset += .01f;
            }

            selected.Clear();

            NewMove newMove = new NewMove(topCardIndex, startingParent, destinationParent);
            game.gameBoard.moveList.Add(newMove);
            game.SaveGame();
        }
    }

    // Right click shortcut, checks for a valid move to foundation first. if not, sends to cell if there is cell space
    public void Shortcut(GameObject card)
    {
        selected.Clear();

        bool sentToFoundation = ShortcutToFoundation(card);

        if (!sentToFoundation)
        {
            ShortcutToCell(card);
        }
    }

    // Checks each foundation to see if the card can be sent there. returns true or false and moves if true
    bool ShortcutToFoundation(GameObject card)
    {
        if (card.CompareTag("Card") && card.transform.GetSiblingIndex() == card.transform.parent.childCount - 1)
        {
            for (int i = 0; i < game.foundationsList.Length; i++)
            {
                if (game.foundationsList[i].transform.childCount > 0)
                {
                    if (ValidStack(card, game.foundationsList[i].transform.GetChild(game.foundationsList[i].transform.childCount - 1).gameObject))
                    {
                        SelectCard(card);
                        MoveStack(game.foundationsList[i].transform.GetChild(game.foundationsList[i].transform.childCount - 1).gameObject);
                        return true;
                    }
                }
                else
                {
                    if (ValidStack(card, game.foundationsList[i].gameObject))
                    {
                        SelectCard(card);
                        MoveStack(game.foundationsList[i].gameObject);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // moves to cell if possible.
    void ShortcutToCell(GameObject card)
    {
        if (card.CompareTag("Card") && card.transform.GetSiblingIndex() == card.transform.parent.childCount - 1)
        {
            for (int i = 0; i < game.cellsList.Length; i++)
            {
                if (game.cellsList[i].transform.childCount == 0)
                {
                    if (ValidStack(card, game.cellsList[i].gameObject))
                    {
                        SelectCard(card);
                        MoveStack(game.cellsList[i].gameObject);
                        return;
                    }
                }
            }
        }
    }

    /*
        WHAT FOLLOWS IS GODMODE/ILLEGAL MOVEMENT FOR
        1) Debugging
        2) Logic to make the undo function possible
        THERE ARE SOME BUGGY ELEMENTS TO THESE, BUT THEY'RE ONLY MEANS TO AN END
        WILL LIKELY REMAIN UNTOUCHED FOR A WHILE
    */

    // Illegal stack
    void FreeStackCards(GameObject spotToStack, Transform startingParent, Transform destinationParent)
    {
        // offset of Card Game Object(s) to whatever they are stacked on
        // If the selected are moving to a Cell, Foundation, a Card in a Foundation, or an empty Cascade, no offset
        float yOffset = 0.0f;
        float zOffset = 0.01f;

        // If being moved to a card that's not in a foundation (IE in a Cascade), stack it like cards (offset slightly)
        if (spotToStack.gameObject.CompareTag("Card") && !spotToStack.transform.parent.gameObject.CompareTag("Foundation"))
        {
            yOffset = 0.5f;
        }

        for (int i = 0; i < selected.Count; i++)
        {
            if (startingParent.gameObject.CompareTag("Cell") && !spotToStack.CompareTag("Cell"))
            {
                game.cellsFilled--;
            }
            else if (!startingParent.gameObject.CompareTag("Cell") && spotToStack.CompareTag("Cell"))
            {
                game.cellsFilled++;
            }
            selected[i].transform.position = 
            new Vector3(spotToStack.transform.position.x, 
                        spotToStack.transform.position.y - yOffset, 
                        spotToStack.transform.position.z - zOffset);
            selected[i].transform.parent = destinationParent; // this makes the children move with the parents

            yOffset += 0.5f;
            zOffset += .01f;
        }

        selected.Clear();
        game.gameBoard.moveList.RemoveAt(game.gameBoard.moveList.Count - 1);
    }

    // Illegal Select
    void FreeSelect(GameObject card)
    {
        if (!card.CompareTag("Card"))
        {
            return;
        }
        else
        {
            selected.Clear();

            int cardIndex = card.transform.GetSiblingIndex();
            int containerCount = card.transform.parent.childCount;

            for (int i = cardIndex; i < containerCount; i++)
            {
                selected.Add(card.transform.parent.GetChild(i).gameObject);
            }
        }
    }

    // Illegal move
    void FreeMove(GameObject spotToStack)
    {
        // Start by identifying the list holding the selected card(s)
        Transform startingParent = selected[0].transform.parent;

        // Then identify the destination list
        Transform destinationParent;

        // If destination is a Card
        if (spotToStack.CompareTag("Card"))
        {
            destinationParent = spotToStack.transform.parent;
        }
        // Otherwise, it must be an empty Cell, Foundation, or Cascade
        else
        {
            destinationParent = spotToStack.transform;
        }

        FreeStackCards(spotToStack, startingParent, destinationParent);
    }

    // Uses the illegal select and move in order to undo moves since they likely would not be allowed with regular moves
    public void Undo()
    {
        if (game.gameBoard.moveList.Count > 0)
        {
            FreeSelect(game.gameBoard.moveList[game.gameBoard.moveList.Count - 1].destinationParent.GetChild(game.gameBoard.moveList[game.gameBoard.moveList.Count - 1].topCardIndex).gameObject);

            if (game.gameBoard.moveList[game.gameBoard.moveList.Count - 1].startingParent.childCount == 0)
            {
                FreeMove(game.gameBoard.moveList[game.gameBoard.moveList.Count - 1].startingParent.gameObject);
            }
            else
            {
                FreeMove(game.gameBoard.moveList[game.gameBoard.moveList.Count - 1].startingParent.GetChild(game.gameBoard.moveList[game.gameBoard.moveList.Count - 1].startingParent.childCount - 1).gameObject);
            }
        }
        game.SaveGame();
    }
}