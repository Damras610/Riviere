using System.Linq;

namespace Rivière.BusinessLogic
{
    public enum LessEqualMore
    {
        Less,
        Equal,
        More
    }

    public enum InterEqualExter
    {
        Inter,
        Equal,
        Exter
    }

    public class Player
    {
        public string Name { get; private set; }

        public Card[] Cards
        { 
            get
            {
                return (Card[])cards.Clone();
            }
        }

        public CardColor? ChosenCardColor { get; private set; }
        public LessEqualMore? ChosenLessEqualMore { get; private set; }
        public InterEqualExter? ChosenInterEqualExter { get; private set; }
        public CardSuit? ChosenCardSuit { get; private set; }

        readonly Card[] cards;

        public Player(string name)
        {
            this.Name = name;

            cards = new Card[4];
            ChosenCardColor = null;
            ChosenLessEqualMore = null;
            ChosenInterEqualExter = null;
            ChosenCardSuit = null;
        }

        public void SetChoiceCardColor(CardColor choiceCardColor) => ChosenCardColor = choiceCardColor;

        public void SetChoiceLessEqualMore(LessEqualMore choiceLessEqualMore) => ChosenLessEqualMore = choiceLessEqualMore;

        public void SetChoiceInterEqualExter(InterEqualExter choiceInterEqualExter) => ChosenInterEqualExter = choiceInterEqualExter;

        public void SetChoiceCardSuit(CardSuit choiceCardSuit) => ChosenCardSuit = choiceCardSuit;

        public void DrawCard(Card card, GameState gameState)
        {
            cards[(int)gameState - 1] = card;
        }

        public bool HasChosenGoodColor()
        {
            return cards[0].Color == ChosenCardColor;
        }

        public bool HasChosenGoodLessEqualMore()
        {
            LessEqualMore actualLessMoreEqual;

            if (cards[1].Number < cards[0].Number)
                actualLessMoreEqual = LessEqualMore.Less;
            else if (cards[1].Number > cards[0].Number)
                actualLessMoreEqual = LessEqualMore.More;
            else
                actualLessMoreEqual = LessEqualMore.Equal;

            return ChosenLessEqualMore == actualLessMoreEqual;
        }

        public bool HasChosenGoodInterEqualExter()
        {
            InterEqualExter actualInterEqualExter;

            Card smallestCard = (cards[0].Number <= cards[1].Number) ? cards[0] : cards[1];
            Card biggestCard = (cards[0].Number > cards[1].Number) ? cards[0] : cards[1];

            if (smallestCard.Number < cards[2].Number && cards[2].Number < biggestCard.Number)
                actualInterEqualExter = InterEqualExter.Inter;
            else if (smallestCard.Number > cards[2].Number || cards[2].Number > biggestCard.Number)
                actualInterEqualExter = InterEqualExter.Exter;
            else
                actualInterEqualExter = InterEqualExter.Equal;

            return ChosenInterEqualExter == actualInterEqualExter;

        }

        public bool HasChosenGoodCardSuit()
        {
            return cards[3].Suit == ChosenCardSuit;
        }

        public int HowManyOccurencesNumber(Card card)
        {
            return cards.Where(c => c.Number == card.Number).Count();
        }

    }
}
