using System;

namespace Rivière.BusinessLogic
{
    public enum CardNumber
    {
        Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace,
    }

    public enum CardSuit
    {
        Heart, Diamond, Club, Spade,
    }

    public enum CardColor
    {
        Red, Black
    }

    public class Card : IEquatable<Card>, IComparable<Card>
    {
        public static bool isAceWorseThanTwo = false;

        public CardNumber Number { get; private set; }
        public CardSuit Suit { get; private set; }
        public CardColor Color
        {
            get
            {
                if (Suit == CardSuit.Club || Suit == CardSuit.Spade)
                    return CardColor.Black;
                else
                    return CardColor.Red;
            }
        }

        public int Value
        {
            get
            {
                // If the card is an Ace and the Ace is worse than two, return 1
                if (Number == CardNumber.Ace && isAceWorseThanTwo)
                    return 1;
                // Otherwise, return the enum value
                return (int)Number;
            }
        }

        public Card(CardNumber number, CardSuit suit)
        {
            Number = number;
            Suit = suit;
        }

        public bool Equals(Card other)
        {
            if (other == null)
                return false;
            return Number == other.Number && Suit == other.Suit;
        }

        public int CompareTo(Card other)
        {
            if (other == null)
                return 1;

            // Sort by suit
            if (Suit < other.Suit)
                return -1;
            else if (Suit > other.Suit)
                return 1;
            // and if the suit are the same, sort by number
            else if (Number < other.Number)
                return -1;
            else if (Number > other.Number)
                return 1;
            else
                return 0;
        }
    }
}
