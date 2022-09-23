using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Wrapper class for nested lists as Unity cannot serialize/deserialize nested lists otherwise
[Serializable]
public class CardHolder
{
    public List<Card> cardHolder = new List<Card>();

    // [] operator override so it can be treated as a list in some respects
    // Maybe worth doing the same for the basic functions of a list (Add, Remove, etc.)
    // But does not seem like there is a '.' overload which would shortcut to the cardHolder
    public Card this[int index]
    {
        get
        {
            return cardHolder[index];
        }
        set
        {
            cardHolder[index] = value;
        }
    }
}

// Serializable NewMove class
// Must track names as well as Transforms as Unity cannot serialize/deserialize Transforms
[Serializable]
public class NewMove
{
    public int topCardIndex;
    public Transform startingParent; 
    public Transform destinationParent;
    public string startingParentName;
    public string destinationParentName;

    public NewMove(int index, Transform start, Transform destination)
    {
        topCardIndex = index;
        startingParent = start; 
        destinationParent = destination;

        startingParentName = start.gameObject.name;
        destinationParentName = destinationParent.gameObject.name;
    }

    public NewMove(int index, string startName, string destinationName)
    {
        topCardIndex = index;
        startingParent = GameObject.Find(startName).transform;
        destinationParent = GameObject.Find(destinationName).transform;
    }

    // Used to rebuild each NewMove object after deserialization as the Transforms are basically null due to Unity's serialization process
    public void RebuildSelf()
    {
        startingParent = GameObject.Find(startingParentName).transform;
        destinationParent = GameObject.Find(destinationParentName).transform;
    }
}

// Serializable Game Board for saving/loading game status
// Only 1 game can be saved at a time which is sensible, in my opinion
[Serializable]
public class GameBoard
{
    public List<CardHolder> foundationBoard;
    private CardHolder foundation0 = new CardHolder();
    private CardHolder foundation1 = new CardHolder();
    private CardHolder foundation2 = new CardHolder();
    private CardHolder foundation3 = new CardHolder();

    public List<CardHolder> cellBoard;
    private CardHolder cell0 = new CardHolder();
    private CardHolder cell1 = new CardHolder();
    private CardHolder cell2 = new CardHolder();
    private CardHolder cell3 = new CardHolder();

    public List<CardHolder> cascadeBoard;
    private CardHolder cascade0 = new CardHolder();
    private CardHolder cascade1 = new CardHolder();
    private CardHolder cascade2 = new CardHolder();
    private CardHolder cascade3 = new CardHolder();
    private CardHolder cascade4 = new CardHolder();
    private CardHolder cascade5 = new CardHolder();
    private CardHolder cascade6 = new CardHolder();
    private CardHolder cascade7 = new CardHolder();

    // Was originally a stack, functions as a stack
    // Has to be a list because Unity cannot serialize Stacks
    // Maybe worth making my own wrapper class/stack implementation? probably not
    public List<NewMove> moveList;

    public GameBoard()
    {
        cascadeBoard = new List<CardHolder> {cascade0, cascade1, cascade2, cascade3, cascade4, cascade5, cascade6, cascade7};
        foundationBoard = new List<CardHolder> {foundation0, foundation1, foundation2, foundation3};
        cellBoard = new List<CardHolder> {cell0, cell1, cell2, cell3};
        moveList = new List<NewMove>();
    }

    public void Clear()
    {
        foreach (CardHolder foundation in foundationBoard)
        {
            foundation.cardHolder.Clear();
        }
        foreach (CardHolder cell in cellBoard)
        {
            cell.cardHolder.Clear();
        }
        foreach (CardHolder cascade in cascadeBoard)
        {
            cascade.cardHolder.Clear();
        }
    }
}