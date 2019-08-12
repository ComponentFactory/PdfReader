using System;
using System.Collections.Generic;
using System.Text;

namespace PdfReader
{
    public class PdfStructTreeRoot : PdfDictionary
    {
        private List<PdfStructTreeElement> _elements;
        private PdfNameTree _IdTree;
        private PdfNumberTree _parentTree;

        public PdfStructTreeRoot(PdfObject parent, ParseDictionary dictionary)
            : base(parent, dictionary)
        {
        }

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
                        foreach(PdfObject item in array.Objects)
                        {
                            dictionary = item as PdfDictionary;
                            if (dictionary == null)
                            {
                                if (item is PdfObjectReference reference)
                                    dictionary = Document.IndirectObjects.MandatoryValue<PdfDictionary>(reference);
                                else
                                    throw new ApplicationException($"PdfStructTreeRoot property K with array must contain dictionary or object reference and not '{item.GetType().Name}'.");
                            }

                            _elements.Add(new PdfStructTreeElement(dictionary));
                        }
                    }
                }

                return _elements;
            }
        }

        public PdfNameTree IDTree
        {
            get
            {
                if (_IdTree == null)
                    _IdTree = new PdfNameTree(MandatoryValueRef<PdfDictionary>("IDTree"));

                return _IdTree;
            }
        }

        public PdfNumberTree ParentTree
        {
            get
            {
                if (_parentTree == null)
                    _parentTree = new PdfNumberTree(MandatoryValueRef<PdfDictionary>("ParentTree"));

                return _parentTree;
            }
        }

        public PdfInteger ParentTreeNextKey { get => OptionalValue<PdfInteger>("ParentTreeNextKey"); }
        public PdfDictionary RoleMap { get => OptionalValueRef<PdfDictionary>("RoleMap"); }
        public PdfDictionary ClassMap { get => OptionalValueRef<PdfDictionary>("ClassMap"); }
    }
}
