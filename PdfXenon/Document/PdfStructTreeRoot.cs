using System;
using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class PdfStructTreeRoot : PdfDictionary
    {
        private PdfNameTree _IdTree;
        private PdfNumberTree _parentTree;

        public PdfStructTreeRoot(PdfObject parent, ParseDictionary dictionary)
            : base(parent, dictionary)
        {
        }

        public PdfNameTree IDTree
        {
            get
            {
                if (_IdTree == null)
                    _IdTree = new PdfNameTree(this, MandatoryValueRef<PdfDictionary>("IDTree"));

                return _IdTree;
            }
        }

        public PdfNumberTree ParentTree
        {
            get
            {
                if (_parentTree == null)
                    _parentTree = new PdfNumberTree(this, MandatoryValueRef<PdfDictionary>("ParentTree"));

                return _parentTree;
            }
        }

        public PdfInteger ParentTreeNextKey { get => OptionalValue<PdfInteger>("ParentTreeNextKey"); }
        public PdfDictionary RoleMap { get => OptionalValueRef<PdfDictionary>("RoleMap"); }
        public PdfDictionary ClassMap { get => OptionalValueRef<PdfDictionary>("ClassMap"); }
    }
}
