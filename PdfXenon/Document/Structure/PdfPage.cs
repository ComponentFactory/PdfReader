using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfPage : PdfPageInherit
    {
        private PdfContents _contents;

        public PdfPage(PdfDictionary dictionary)
            : base(dictionary.Parent, dictionary.ParseDictionary)
        {
        }

        public override void Visit(IPdfObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void FindLeafPages(List<PdfPage> pages)
        {
            pages.Add(this);
        }

        public PdfDateTime LastModified { get => OptionalDateTime("LastModified"); }
        public PdfDictionary Resources { get => InheritableMandatoryRefValue<PdfDictionary>("Resources"); }
        public PdfRectangle MediaBox { get => ArrayToRectangle(InheritableMandatoryValue<PdfArray>("MediaBox")); }
        public PdfRectangle CropBox { get => ArrayToRectangle(InheritableOptionalValue<PdfArray>("CropBox")); }
        public PdfRectangle BleedBox { get => ArrayToRectangle(OptionalValue<PdfArray>("BleedBox")); }
        public PdfRectangle TrimBox { get => ArrayToRectangle(OptionalValue<PdfArray>("TrimBox")); }
        public PdfRectangle ArtBox { get => ArrayToRectangle(OptionalValue<PdfArray>("ArtBox")); }
        public PdfDictionary BoxColorInfo { get => OptionalValue<PdfDictionary>("BoxColorInfo"); }

        public PdfContents Contents
        {
            get
            {
                if (_contents == null)
                {
                    PdfObject obj = InheritableMandatoryValue<PdfObject>("Contents");
                    _contents = new PdfContents(this, obj);
                }

                return _contents;
            }

        }

        public PdfInteger Rotate { get => InheritableOptionalValue<PdfInteger>("Rotate"); }
        public PdfDictionary Group { get => OptionalValue<PdfDictionary>("Group"); }
        public PdfStream Thumb { get => OptionalValue<PdfStream>("Thumb"); }
        public PdfArray B { get => OptionalValue<PdfArray>("B"); }
        public PdfInteger Dur { get => OptionalValue<PdfInteger>("Dur"); }
        public PdfDictionary Trans { get => OptionalValue<PdfDictionary>("Trans"); }
        public PdfArray Annots { get => OptionalValue<PdfArray>("Annots"); }
        public PdfDictionary AA { get => OptionalValue<PdfDictionary>("AA"); }
        public PdfStream Metadata { get => OptionalValue<PdfStream>("Metadata"); }
        public PdfDictionary PieceInfo { get => OptionalValue<PdfDictionary>("PieceInfo"); }
        public PdfInteger StructParents { get => OptionalValue<PdfInteger>("StructParents"); }
        public PdfString ID { get => OptionalValue<PdfString>("ID"); }
        public PdfInteger PZ { get => OptionalValue<PdfInteger>("PZ"); }
        public PdfDictionary SeparationInfo { get => OptionalValue<PdfDictionary>("SeparationInfo"); }
        public PdfName Tabs { get => OptionalValue<PdfName>("Tabs"); }
        public PdfName TemplateInstantiated { get => OptionalValue<PdfName>("TemplateInstantiated"); }
        public PdfDictionary PresSteps { get => OptionalValue<PdfDictionary>("PresSteps"); }
        public PdfInteger UserUnit { get => OptionalValue<PdfInteger>("UserUnit"); }
        public PdfDictionary VP { get => OptionalValue<PdfDictionary>("VP"); }
    }
}
