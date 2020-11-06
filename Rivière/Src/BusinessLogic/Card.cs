using System;

namespace Rivière.BusinessLogic
{
    public enum CardInfo
    {
        Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King
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
        public CardInfo Number { get; private set; }
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

        public Card(CardInfo number, CardSuit suit)
        {
            this.Number = number;
            this.Suit = suit;
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
