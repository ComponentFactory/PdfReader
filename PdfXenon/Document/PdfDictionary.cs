namespace PdfXenon.Standard
{
    public class PdfDictionary
    {
        private ParseDictionary _dictionary;

        public PdfDictionary(ParseDictionary dictionary)
        {
            _dictionary = dictionary;
        }

        public override string ToString()
        {
            return _dictionary.ToString();
        }

        public T OptionalValue<T>(string name) where T : ParseObject
        {
            return _dictionary.OptionalValue<T>(name);
        }

        public T MandatoryValue<T>(string name) where T : ParseObject
        {
            return _dictionary.MandatoryValue<T>(name);
        }
    }
}
