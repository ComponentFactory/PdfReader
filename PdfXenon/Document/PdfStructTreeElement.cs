using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfStructTreeElement : PdfDictionary
    {
        private List<PdfStructTreeElement> _elements;

        public PdfStructTreeElement(PdfDictionary dictionary)
            : base(dictionary.Parent, dictionary.ParseDictionary)
        {
        }

        public override int Output(StringBuilder sb, int indent)
        {
            string blank = new string(' ', indent);
            sb.Append($"S = {S.Value} ");

            if (ID != null)
                sb.Append($"ID = {ID.Value} ");

            if (Pg != null)
                sb.Append($"Pg = {Pg} ");

            if (A != null)
                sb.Append($"A = {A} ");

            if (C != null)
                sb.Append($"C = {C} ");

            if (R != null)
                sb.Append($"R = {R} ");

            if (T != null)
                sb.Append($"T = {T} ");

            if (Lang != null)
                sb.Append($"Lang = {Lang} ");

            if (ActualText != null)
                sb.Append($"ActualText = {ActualText} ");

            sb.AppendLine("");
            sb.Append(blank);

            foreach (PdfStructTreeElement element in K)
                element.Output(sb, indent + 2);

            return indent;
        }

        public PdfName S { get => MandatoryValue<PdfName>("S"); }
        public PdfString ID { get => OptionalValue<PdfString>("ID"); }
        public PdfObjectReference Pg { get => OptionalValue<PdfObjectReference>("Pg"); }

        public List<PdfStructTreeElement> K
        {
            get
            {
                if (_elements == null)
                {
                    _elements = new List<PdfStructTreeElement>();

                    PdfObject k = OptionalValueRef<PdfObject>("K");
                    if (k is PdfDictionary dictionary)
                        _elements.Add(new PdfStructTreeElement(dictionary));
                    else if (k is PdfArray array)
                    {
                        foreach (PdfObject item in array.Objects)
                        {
                            dictionary = item as PdfDictionary;
                            if (dictionary == null)
                            {
                                if (item is PdfObjectReference reference)
                                    dictionary = Document.IndirectObjects.MandatoryValue<PdfDictionary>(reference);
                                else
                                    throw new ApplicationException($"PdfStructTreeElement property K has unrecognized content of type '{item.GetType().Name}'.");
                            }

                            _elements.Add(new PdfStructTreeElement(dictionary));
                        }
                    }
                }

                return _elements;
            }
        }

        public PdfObject A { get => OptionalValue<PdfObject>("A"); }
        public PdfObject C { get => OptionalValue<PdfObject>("C"); }
        public PdfInteger R { get => OptionalValue<PdfInteger>("R"); }
        public PdfString T { get => OptionalValue<PdfString>("T"); }
        public PdfString Lang { get => OptionalValue<PdfString>("Lang"); }
        public PdfString Alt { get => OptionalValue<PdfString>("Alt"); }
        public PdfString E { get => OptionalValue<PdfString>("E"); }
        public PdfString ActualText { get => OptionalValue<PdfString>("ActualText"); }
    }
}
