using System;
using System.Collections.Generic;
using System.Text;

namespace Rivière.Translations
{
    class TranslationsException : Exception
    {

        public TranslationsException()
        {
        }

        public TranslationsException(string message)
            : base(message)
        {
        }

        public TranslationsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
