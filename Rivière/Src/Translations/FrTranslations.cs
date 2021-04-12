using Rivière.BusinessLogic;
using Rivière.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rivière.Translations
{
    public class FrTranslations : ATranslations
    {
        private ReadOnlyDictionary<CardNumber, string> cardNumberName = new Dictionary<CardNumber, string>()
        {
            { CardNumber.Two, "2" },
            { CardNumber.Three, "3" },
            { CardNumber.Four, "4" },
            { CardNumber.Five, "5" },
            { CardNumber.Six, "6" },
            { CardNumber.Seven, "7" },
            { CardNumber.Eight, "8" },
            { CardNumber.Nine, "9" },
            { CardNumber.Ten, "10" },
            { CardNumber.Jack, "Valet" },
            { CardNumber.Queen, "Reine" },
            { CardNumber.King, "Roi" },
            { CardNumber.Ace, "As" },
        }.ReadOnly();

        private ReadOnlyDictionary<CardColor, string> cardColorName = new Dictionary<CardColor, string>()
        {
            { CardColor.Red, "Rouge" },
            { CardColor.Black, "Noir" }
        }.ReadOnly();

        private ReadOnlyDictionary<CardSuit, string> cardSuitName = new Dictionary<CardSuit, string>()
        {
            { CardSuit.Heart, "Coeur" },
            { CardSuit.Diamond, "Carreau" },
            { CardSuit.Club, "Trèfle" },
            { CardSuit.Spade, "Pique" }
        }.ReadOnly();

        private ReadOnlyDictionary<CardSuit, string> cardSuitSymbol = new Dictionary<CardSuit, string>()
        {
            { CardSuit.Heart, "♥" },
            { CardSuit.Diamond, "♦" },
            { CardSuit.Club, "♣" },
            { CardSuit.Spade, "♠" }
        }.ReadOnly();

        private ReadOnlyDictionary<LessEqualMore, string> lessEqualMoreText = new Dictionary<LessEqualMore, string>()
        {
            { LessEqualMore.Less, "Moins" },
            { LessEqualMore.Equal, "Egal" },
            { LessEqualMore.More, "Plus" }
        }.ReadOnly();

        private ReadOnlyDictionary<InterEqualExter, string> interEqualExterText = new Dictionary<InterEqualExter, string>()
        {
            { InterEqualExter.Inter, "Interieur" },
            { InterEqualExter.Equal, "Egal" },
            { InterEqualExter.Exter, "Exterieur" }
        }.ReadOnly();

        private ReadOnlyDictionary<GameState, string> gameStateTitle = new Dictionary<GameState, string>()
        {
            { GameState.ASKING_FOR_COLOR, "Couleur" },
            { GameState.ASKING_FOR_LESS_EQUAL_MORE, "Plus ou Moins" },
            { GameState.ASKING_FOR_INTER_EQUAL_EXTER, "Interieur ou Exterieur" },
            { GameState.ASKING_FOR_SUIT, "Suite" },
            { GameState.GIVING_OR_RECEIVING_DRINKS, "Maintenant on rigole" },
            { GameState.FINISHED, "Partie terminée" }
        }.ReadOnly();

        private ReadOnlyDictionary<GameState, string> gameStateDescription = new Dictionary<GameState, string>()
        {
            { GameState.ASKING_FOR_COLOR, "Quelle est la couleur de la prochaine carte ?" },
            { GameState.ASKING_FOR_LESS_EQUAL_MORE, "La prochaine carte est-elle plus grande, égale ou plus petite que ta precedente carte ?" },
            { GameState.ASKING_FOR_INTER_EQUAL_EXTER, "Tes deux premieres incluent-elles la prochaine carte ?" },
            { GameState.ASKING_FOR_SUIT, "A quelle suite appartient la prochaine carte ?" },
            { GameState.GIVING_OR_RECEIVING_DRINKS, "" },
            { GameState.FINISHED, "Déjà ?" }
        }.ReadOnly();

        public override ReadOnlyDictionary<CardNumber, string> CardNumberName => cardNumberName;

        public override ReadOnlyDictionary<CardColor, string> CardColorName => cardColorName;

        public override ReadOnlyDictionary<CardSuit, string> CardSuitName => cardSuitName;

        public override ReadOnlyDictionary<CardSuit, string> CardSuitSymbol => cardSuitSymbol;

        public override ReadOnlyDictionary<LessEqualMore, string> LessEqualMoreText => lessEqualMoreText;

        public override ReadOnlyDictionary<InterEqualExter, string> InterEqualExterText => interEqualExterText;

        public override ReadOnlyDictionary<GameState, string> GameStateTitle => gameStateTitle;

        public override ReadOnlyDictionary<GameState, string> GameStateDescription => gameStateDescription;
    }
}
