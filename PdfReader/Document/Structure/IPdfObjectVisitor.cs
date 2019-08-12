using System;
using System.Collections.Generic;
using System.Text;

namespace PdfReader
{
    public interface IPdfObjectVisitor
    {
        void Visit(PdfArray array);
        void Visit(PdfBoolean boolean);
        void Visit(PdfCatalog contents);
        void Visit(PdfContents contents);
        void Visit(PdfDateTime dateTime);
        void Visit(PdfDictionary dateTime);
        void Visit(PdfDocument document);
        void Visit(PdfIdentifier identifier);
        void Visit(PdfInteger integer);
        void Visit(PdfInfo info);
        void Visit(PdfIndirectObject indirectObject);
        void Visit(PdfName name);
        void Visit(PdfNameTree nameTree);
        void Visit(PdfNull nul);
        void Visit(PdfNumberTree numberTree);
        void Visit(PdfObject obj);
        void Visit(PdfObjectReference reference);
        void Visit(PdfOutlineItem outlineItem);
        void Visit(PdfOutlineLevel outlineLevel);
        void Visit(PdfPage page);
        void Visit(PdfPages pages);
        void Visit(PdfReal real);
        void Visit(PdfRectangle rectangle);
        void Visit(PdfStream stream);
        void Visit(PdfString str);
        void Visit(PdfVersion version);
    }
}
