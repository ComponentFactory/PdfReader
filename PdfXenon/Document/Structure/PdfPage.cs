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

        public override int ToDebug(StringBuilder sb, int indent)
        {
            string blank = new string(' ', indent);

            if (LastModified != null) sb.Append($"LastModified: {LastModified.ToDebug()}\n{blank}");
            if (Resources != null) sb.Append($"Resources: {Resources.ToDebug()}\n{blank}");
            if (MediaBox != null) sb.Append($"MediaBox: {MediaBox.ToDebug()}\n{blank}");
            if (CropBox != null) sb.Append($"CropBox: {CropBox.ToDebug()}\n{blank}");
            if (BleedBox != null) sb.Append($"BleedBox: {BleedBox.ToDebug()}\n{blank}");
            if (TrimBox != null) sb.Append($"TrimBox: {TrimBox.ToDebug()}\n{blank}");
            if (ArtBox != null) sb.Append($"ArtBox: {ArtBox.ToDebug()}\n{blank}");
            if (BoxColorInfo != null) sb.Append($"BoxColorInfo: {BoxColorInfo.ToDebug()}\n{blank}");
            if (Contents != null) sb.Append($"Contents: {Contents.ToDebug()}\n{blank}");
            if (Rotate != null) sb.Append($"Rotate: {Rotate.ToDebug()}\n{blank}");
            if (Group != null) sb.Append($"Group: {Group.ToDebug()}\n{blank}");
            if (Thumb != null) sb.Append($"Thumb: {Thumb.ToDebug()}\n{blank}");
            if (B != null) sb.Append($"B: {B.ToDebug()}\n{blank}");
            if (Metadata != null) sb.Append($"Metadata: {Metadata.ToDebug()}\n{blank}");
            if (PieceInfo != null) sb.Append($"PieceInfo: {PieceInfo.ToDebug()}\n{blank}");
            if (StructParents != null) sb.Append($"StructParents: {StructParents.ToDebug()}\n{blank}");
            if (ID != null) sb.Append($"ID: {ID.ToDebug()}\n{blank}");
            if (PZ != null) sb.Append($"PZ: {PZ.ToDebug()}\n{blank}");
            if (SeparationInfo != null) sb.Append($"SeparationInfo: {SeparationInfo.ToDebug()}\n{blank}");
            if (Tabs != null) sb.Append($"Tabs: {Tabs.ToDebug()}\n{blank}");
            if (TemplateInstantiated != null) sb.Append($"TemplateInstantiated: {TemplateInstantiated.ToDebug()}\n{blank}");
            if (PresSteps != null) sb.Append($"PresSteps: {PresSteps.ToDebug()}\n{blank}");
            if (UserUnit != null) sb.Append($"UserUnit: {UserUnit.ToDebug()}\n{blank}");
            if (VP != null) sb.Append($"VP: {VP.ToDebug()}\n{blank}");

            return indent;
        }

        public PdfPageProcessor CreateProcessor()
        {
            return new PdfPageProcessor(this);
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
