using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class DebugBuilder : IPdfObjectVisitor
    {
        private StringBuilder _sb = new StringBuilder();
        private Stack<int> _indents = new Stack<int>();
        private int _index;

        public DebugBuilder(PdfObject obj)
        {
            _index = 0;
            _indents.Push(_index);
            obj.Visit(this);
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
   
        public void Visit(PdfArray array)
        {
            Append("[");

            bool first = true;
            foreach (PdfObject child in array.Objects)
            {
                if (!first)
                    Append(" ");
                else
                    first = false;

                child.Visit(this);
            }

            Append("]");
        }

        public void Visit(PdfBoolean boolean)
        {
            Append(boolean);
        }

        public void Visit(PdfCatalog catalog)
        {
            PushNextLevel();
            Append("Catalog");

            CurrentLevelNewLine();
            catalog.RootPage.Visit(this);

            VisitNotNull(catalog.PageLabels, "PageLabels");
            VisitNotNull(catalog.Names, "Names");
            VisitNotNull(catalog.Dests, "Dests");
            VisitNotNull(catalog.ViewerPreferences, "ViewerPreferences");
            VisitNotNull(catalog.PageLayout, "PageLayout");
            VisitNotNull(catalog.PageMode, "PageMode");
            VisitNotNull(catalog.Outlines, "Outlines");
            VisitNotNull(catalog.Threads, "Threads");
            VisitNotNull(catalog.OpenAction, "OpenAction");
            VisitNotNull(catalog.AA, "AA");
            VisitNotNull(catalog.URI, "URI");
            VisitNotNull(catalog.AcroForm, "AcroForm");
            VisitNotNull(catalog.Metadata, "Metadata");
            VisitNotNull(catalog.StructTreeRoot, "StructTreeRoot");
            VisitNotNull(catalog.MarkInfo, "MarkInfo");
            VisitNotNull(catalog.Lang, "Lang");
            VisitNotNull(catalog.SpiderInfo, "SpiderInfo");
            VisitNotNull(catalog.OutputIntents, "OutputIntents");
            VisitNotNull(catalog.PieceInfo, "PieceInfo");
            VisitNotNull(catalog.OCProperties, "OCProperties");
            VisitNotNull(catalog.Perms, "Perms");
            VisitNotNull(catalog.Legal, "Legal");
            VisitNotNull(catalog.Requirements, "Requirements");
            VisitNotNull(catalog.Collection, "Collection");
            VisitNotNull(catalog.NeedsRendering, "NeedsRendering");
            VisitNotNull(catalog.Version, "Version");

            PopLevel();
        }

        public void Visit(PdfContents contents)
        {
 //           throw new NotImplementedException();
        }

        public void Visit(PdfDateTime dateTime)
        {
            Append(dateTime);
        }

        public void Visit(PdfDictionary dictionary)
        {
            PushNextLevel();
            Append("<<");

            int index = 0;
            bool newLine = false;
            int count = dictionary.Count;
            foreach (var entry in dictionary)
            {
                if ((index == 1) && (count == 2))
                    Append(" ");

                VisitNotNull(entry.Value, $"/{entry.Key}", newLine);

                if (count > 2)
                    newLine = true;

                index++;
            }

            Append(">>");
            PopLevel();
        }

        public void Visit(PdfDocument document)
        {
            PushNextLevel();
            Append("Document");

            VisitNotNull(document.Catalog);
            VisitNotNull(document.Info);
            VisitNotNull(document.Version, "Version");

            PopLevel();
            CurrentLevelNewLine();
        }

        public void Visit(PdfIdentifier identifier)
        {
            Append(identifier);
        }

        public void Visit(PdfInfo info)
        {
            PushNextLevel();
            Append("Info");

            VisitNotNull(info.Title, "Title");
            VisitNotNull(info.Author, "Author");
            VisitNotNull(info.Subject, "Subject");
            VisitNotNull(info.Keywords, "Keywords");
            VisitNotNull(info.Creator, "Creator");
            VisitNotNull(info.Producer, "Producer");
            VisitNotNull(info.CreationDate, "CreationDate");
            VisitNotNull(info.ModDate, "ModDate");
            VisitNotNull(info.Trapped, "Trapped");

            PopLevel();
        }

        public void Visit(PdfInteger integer)
        {
            Append(integer);
        }

        public void Visit(PdfName name)
        {
            Append($"/{name}");
        }

        public void Visit(PdfNameTree nameTree)
        {
            throw new NotImplementedException();
        }

        public void Visit(PdfNull nul)
        {
            Append(nul);
        }

        public void Visit(PdfNumberTree numberTree)
        {
            throw new NotImplementedException();
        }

        public void Visit(PdfObject obj)
        {
            Append(obj.ToString());
        }

        public void Visit(PdfObjectReference reference)
        {
            Append($"{reference.Id} {reference.Gen} R");
        }

        public void Visit(PdfOutlineItem outlineItem)
        {
            PushNextLevel();
            Append("Item");

            VisitNotNull(outlineItem.Title, "Title");
            VisitNotNull(outlineItem.Dest, "Dest");
            VisitNotNull(outlineItem.A, "A");
            VisitNotNull(outlineItem.SE, "SE");
            VisitNotNull(outlineItem.C, "C");
            VisitNotNull(outlineItem.F, "F");

            foreach (PdfOutlineItem item in outlineItem.Items)
            {
                CurrentLevelNewLine();
                item.Visit(this);
            }

            PopLevel();
        }

        public void Visit(PdfOutlineLevel outlineLevel)
        {
            PushNextLevel();
            Append("Level");

            foreach (PdfOutlineItem item in outlineLevel.Items)
            {
                CurrentLevelNewLine();
                item.Visit(this);
            }

            PopLevel();
        }

        public void Visit(PdfPage page)
        {
            PushNextLevel();
            Append("Page");

            VisitNotNull(page.LastModified, "LastModified");
            VisitNotNull(page.Resources, "Resources");
            VisitNotNull(page.MediaBox, "MediaBox");
            VisitNotNull(page.CropBox, "CropBox");
            VisitNotNull(page.BleedBox, "BleedBox");
            VisitNotNull(page.TrimBox, "TrimBox");
            VisitNotNull(page.ArtBox, "ArtBox");
            VisitNotNull(page.BoxColorInfo, "BoxColorInfo");
            VisitNotNull(page.Contents, "Contents");
            VisitNotNull(page.Rotate, "Rotate");
            VisitNotNull(page.Group, "Group");
            VisitNotNull(page.Thumb, "Thumb");
            VisitNotNull(page.B, "B");
            VisitNotNull(page.Dur, "Dur");
            VisitNotNull(page.Trans, "Trans");
            VisitNotNull(page.Annots, "Annots");
            VisitNotNull(page.AA, "AA");
            VisitNotNull(page.Metadata, "Metadata");
            VisitNotNull(page.PieceInfo, "PieceInfo");
            VisitNotNull(page.StructParents, "StructParents");
            VisitNotNull(page.ID, "ID");
            VisitNotNull(page.PZ, "PZ");
            VisitNotNull(page.SeparationInfo, "SeparationInfo");
            VisitNotNull(page.Tabs, "Tabs");
            VisitNotNull(page.TemplateInstantiated, "TemplateInstantiated");
            VisitNotNull(page.PresSteps, "PresSteps");
            VisitNotNull(page.UserUnit, "UserUnit");
            VisitNotNull(page.VP, "VP");

            PopLevel();
        }

        public void Visit(PdfPages pages)
        {
            PushNextLevel();
            Append("Pages");

            foreach (PdfObject child in pages.Children)
            {
                CurrentLevelNewLine();
                child.Visit(this);
            }

            PopLevel();
        }

        public void Visit(PdfReal real)
        {
            Append(real);
        }

        public void Visit(PdfRectangle rectangle)
        {
            Append(rectangle);
        }

        public void Visit(PdfStream stream)
        {
            stream.Dictionary.Visit(this);
        }

        public void Visit(PdfString str)
        {
            Append($"'{str}'");
        }

        public void Visit(PdfVersion version)
        {
            Append($"{version}");
        }

        private void VisitNotNull(PdfObject obj, bool newLine = true)
        {
            if (obj != null)
            {
                if (newLine)
                    CurrentLevelNewLine();

                obj.Visit(this);
            }
        }

        private void VisitNotNull(PdfObject obj, string name, bool newLine = true)
        {
            if (obj != null)
            {
                if (newLine)
                    CurrentLevelNewLine();

                Append($"{name} ");
                obj.Visit(this);
            }
        }

        private void PushNextLevel()
        {
            _indents.Push(_index + 2);
        }

        private void PopLevel()
        {
            _index = _indents.Pop();
        }

        private void Append(object obj)
        {
            string str = obj.ToString();
            _sb.Append(str);
            _index += str.Length;
        }

        private void CurrentLevelNewLine()
        {
            int indent = _indents.Peek();
            _index = indent;
            _sb.Append($"\n{new string(' ', _index)}");
        }
    }
}