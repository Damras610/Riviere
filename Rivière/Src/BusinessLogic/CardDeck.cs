using Rivière.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rivière.BusinessLogic
{
    public class CardDeck
    {
        readonly List<Card> drawPile = new List<Card>();
        readonly List<Card> discardPile = new List<Card>();

        public CardDeck()
        {
            // Fill the pile with the cards
            foreach (CardSuit cardSuitValue in Enum.GetValues(typeof(CardSuit)))
                foreach (CardInfo cardNumberValue in Enum.GetValues(typeof(CardInfo)))
                    drawPile.Add(new Card(cardNumberValue, cardSuitValue));
        }

        public void SortDrawPile() => drawPile.Sort();

        public void ShuffleDrawPile() => drawPile.Shuffle();

        public Card DrawCard()
        {
            Card card = drawPile.First();
            drawPile.Remove(card);
            discardPile.Add(card);
            return card;
        }

        public void ResetPiles()
        {
            while (discardPile.Count != 0)
            {
                Card card = discardPile.First();
                discardPile.Remove(card);
                drawPile.Add(card);
            }
        }

        public int DrawPileCount() => drawPile.Count();
        public int DiscardPileCount() => discardPile.Count();
    }
}
