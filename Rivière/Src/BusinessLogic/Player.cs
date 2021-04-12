using System.Linq;
using System.Collections.Generic;
using Rivière.Utils;
using System.Collections.ObjectModel;

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
        public string Name { get; internal set; }

        public CardColor? ChosenCardColor { get; internal set; }
        public LessEqualMore? ChosenLessEqualMore { get; internal set; }
        public InterEqualExter? ChosenInterEqualExter { get; internal set; }
        public CardSuit? ChosenCardSuit { get; internal set; }

        public ReadOnlyDictionary<Player, int> GivenSips => givenSips.ReadOnly();
        public ReadOnlyDictionary<Player, int> ReceivedSips => receiveSips.ReadOnly();
        public int TakenSipsFromDeckCount { get; private set; }

        public ReadOnlyCollection<Card> Cards => cards.AsReadOnly();

        readonly List<Card> cards = new List<Card>(4);

        readonly Dictionary<Player, int> givenSips = new Dictionary<Player, int>();
        readonly Dictionary<Player, int> receiveSips = new Dictionary<Player, int>();

        public Player(string name)
        {
            Name = name;

            ChosenCardColor = null;
            ChosenLessEqualMore = null;
            ChosenInterEqualExter = null;
            ChosenCardSuit = null;
            TakenSipsFromDeckCount = 0;
        }

        public void DrawCard(Card card, GameState gameState)
        {
            cards.Insert((int)gameState - 1, card);
        }

        public bool HasChosenGoodColor()
        {
            return cards[0].Color == ChosenCardColor;
        }

        public bool HasChosenGoodLessEqualMore()
        {
            LessEqualMore actualLessMoreEqual;

            if (cards[1].Value < cards[0].Value)
                actualLessMoreEqual = LessEqualMore.Less;
            else if (cards[1].Value > cards[0].Value)
                actualLessMoreEqual = LessEqualMore.More;
            else
                actualLessMoreEqual = LessEqualMore.Equal;

            return ChosenLessEqualMore == actualLessMoreEqual;
        }

        public bool HasChosenGoodInterEqualExter()
        {
            InterEqualExter actualInterEqualExter;

            Card smallestCard = (cards[0].Value <= cards[1].Value) ? cards[0] : cards[1];
            Card biggestCard = (cards[0].Value > cards[1].Value) ? cards[0] : cards[1];

            if (smallestCard.Value < cards[2].Value && cards[2].Value < biggestCard.Value)
                actualInterEqualExter = InterEqualExter.Inter;
            else if (smallestCard.Value > cards[2].Value || cards[2].Value > biggestCard.Value)
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

        public void TakeSips(int numberOfSips, Player player = null)
        {
            // The sips are from the draw
            if (player == null)
            {
                TakenSipsFromDeckCount += numberOfSips;
            }
            // The sips are from a player
            else
            {
                // Add the number of sips to the counter of receives sips
                if (!receiveSips.ContainsKey(player))
                    receiveSips.Add(player, 0);
                receiveSips[player] += numberOfSips;
                // Add the number of given sips of the other player
                player.GiveSipsTo(numberOfSips, this);
            }
        }

        private void GiveSipsTo(int numberOfSips, Player player)
        {
            if (!givenSips.ContainsKey(player))
                givenSips.Add(player, 0);
            givenSips[player] += numberOfSips;
        }

        public void Reset()
        {
            cards.Clear();
            ChosenCardColor = null;
            ChosenLessEqualMore = null;
            ChosenInterEqualExter = null;
            ChosenCardSuit = null;

            givenSips.Clear();
            receiveSips.Clear();
            TakenSipsFromDeckCount = 0;
        }
    }
}
