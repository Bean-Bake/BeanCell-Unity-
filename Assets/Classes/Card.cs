public class Card
   {
   // Card properties
   private int suit;
   private int rank;
   private string suitName;
   private string rankName;
   private string suitColor;

   // Constructor with suit and rank values
   public Card(int suit, int rank)
   {
      this.suit = suit;
      this.rank = rank;
      setRankName();
      setSuitName();
   }

   // Gets the suit
   public int getSuit()
   {
      return suit;
   }
   
   // Gets the rank
   public int getRank()
   {
      return rank;
   }

   // Sets the rank
   private void setRankName()
   {
      switch (rank)
      {
         case 0:
            rankName = "Ace";
            break;
         case 1:
            rankName = "Two";
            break;
         case 2:
            rankName = "Three";
            break;
         case 3:
            rankName = "Four";
            break;
         case 4:
            rankName = "Five";
            break;
         case 5:
            rankName = "Six";
            break;
         case 6:
            rankName = "Seven";
            break;
         case 7:
            rankName = "Eight";
            break;
         case 8:
            rankName = "Nine";
            break;
         case 9:
            rankName = "Ten";
            break;
         case 10:
            rankName = "Jack";
            break;
         case 11:
            rankName = "Queen";
            break;
         case 12:
            rankName = "King";
            break;
      }
   }

   //-----------------------------------------------------------------
   //  Sets the string representation of the suit using its stored
   //  numeric value.
   //-----------------------------------------------------------------
   private void setSuitName()
   {
      switch (suit)
      {
         case 0:
            suitName = "Clubs";
            suitColor = "Black";
            break;
         case 1:
            suitName = "Diamonds";
            suitColor = "Red";
            break;
         case 2:
            suitName = "Spades";
            suitColor = "Black";
            break;
         case 3:
            suitName = "Hearts";
            suitColor = "Red";
            break;
      }
   }

   //-----------------------------------------------------------------
   //  Returns the face (string value) of this card.
   //-----------------------------------------------------------------
   public string getRankName()
   {
      return rankName;
   }

   //-----------------------------------------------------------------
   //  Returns the suit (string value) of this card.
   //-----------------------------------------------------------------
   public string getSuitName()
   {
      return suitName;
   }

   // Gets the plain English string name of the card (rank of suit)
   public string getWholeName()
   {
         string wholeName = "" + this.getRankName() + " of " + this.getSuitName();
         return wholeName;
   }

   // Gets the color
   public string getColor()
   {
         return suitColor;
   }

   // Gets the integer value of the card (rank of suit)
   public string getWholeID()
   {
         string wholeID = "" + this.getRank() + " of " + this.getSuit();
         return wholeID;
   }

   //-----------------------------------------------------------------
   //  Checks whether this card's face value is exactly 1 higher
   //  than another card's.
   //-----------------------------------------------------------------
   public bool isNextInLine (Card card)
   {
      if (rank - card.rank == 1)
      {
         return true;
      }
      else
      {
         return false;
      }
   }

      //-----------------------------------------------------------------
   //  Checks whether this card's suit color is the same
   //  as another card's.
   //-----------------------------------------------------------------
   public bool isSameColor (Card card)
   {
      if (suitColor == card.suitColor)
         return true;
      else
         return false;
   }

}