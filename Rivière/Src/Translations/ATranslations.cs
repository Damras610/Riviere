using Rivière.BusinessLogic;
using Rivière.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rivière.Translations
{
    public abstract class ATranslations
    {
        public abstract ReadOnlyDictionary<CardNumber, string> CardNumberName { get; }

        public abstract ReadOnlyDictionary<CardColor, string> CardColorName { get; }

        public abstract ReadOnlyDictionary<CardSuit, string> CardSuitName { get; }

        public abstract ReadOnlyDictionary<CardSuit, string> CardSuitSymbol { get; }

        public abstract ReadOnlyDictionary<LessEqualMore, string> LessEqualMoreText { get; }

        public abstract ReadOnlyDictionary<InterEqualExter, string> InterEqualExterText { get; }

        public abstract ReadOnlyDictionary<GameState, string> GameStateTitle { get; }

        public abstract ReadOnlyDictionary<GameState, string> GameStateDescription { get; }

    }
}
