using Rivière.BusinessLogic;
using Rivière.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace Rivière.Translations
{
    public static class ExtensionTranslations
    {
        private static ReadOnlyDictionary<CardNumber, string> CardNumberName;

        private static ReadOnlyDictionary<CardColor, string> CardColorName;

        private static ReadOnlyDictionary<CardSuit, string> CardSuitName;

        private static ReadOnlyDictionary<CardSuit, string> CardSuitSymbol;

        private static ReadOnlyDictionary<LessEqualMore, string> LessEqualMoreText;

        private static ReadOnlyDictionary<InterEqualExter, string> InterEqualExterText;

        private static ReadOnlyDictionary<GameState, string> GameStateTitle;

        private static ReadOnlyDictionary<GameState, string> GameStateDescription;

        public static void LoadTranslations(ATranslations translations)
        {
            CardNumberName = translations.CardNumberName;
            CardColorName = translations.CardColorName;
            CardSuitName = translations.CardSuitName;
            CardSuitSymbol = translations.CardSuitSymbol;
            LessEqualMoreText = translations.LessEqualMoreText;
            InterEqualExterText = translations.InterEqualExterText;
            GameStateTitle = translations.GameStateTitle;
            GameStateDescription = translations.GameStateDescription;
        }

        public static string Name(this CardNumber number)
        {
            try
            {
                return CardNumberName[number];
            }
            catch (Exception e)
            {
                throw new TranslationsException("Cannot translate CardNumber.Name()", e);
            }
        }

        public static string Name(this CardColor color)
        {
            try
            {
                return CardColorName[color];
            }
            catch (Exception e)
            {
                throw new TranslationsException("Cannot translate CardColor.Name()", e);
            }
        }
        public static string Name(this CardSuit suit)
        {
            try
            {
                return CardSuitName[suit];
            }
            catch (Exception e)
            {
                throw new TranslationsException("Cannot translate CardSuit.Name()", e);
            }
        }

        public static string Symbol(this CardSuit suit)
        {
            try
            {
                return CardSuitSymbol[suit];
            }
            catch (Exception e)
            {
                throw new TranslationsException("Cannot translate CardSuit.Symbol()", e);
            }
        }

        public static string Text(this LessEqualMore lessEqualMore)
        {
            try
            {
                return LessEqualMoreText[lessEqualMore];
            }
            catch (Exception e)
            {
                throw new TranslationsException("Cannot translate LessEqualMore.Text()", e);
            }
        }

        public static string Text(this InterEqualExter interEqualExter)
        {
            try
            {
                return InterEqualExterText[interEqualExter];
            }
            catch (Exception e)
            {
                throw new TranslationsException("Cannot translate InterEqualExter.Text()", e);
            }
        }

        public static string Title(this GameState gameState)
        {
            try
            {
                return GameStateTitle[gameState];
            }
            catch (Exception e)
            {
                throw new TranslationsException("Cannot translate GameState.Title()", e);
            }
        }

        public static string Description(this GameState gameState)
        {
            try
            {
                return GameStateDescription[gameState];
            }
            catch (Exception e)
            {
                throw new TranslationsException("Cannot translate GameState.Description()", e);
            }
        }
    }
}
