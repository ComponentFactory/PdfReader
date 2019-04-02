using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PdfXenon.Standard
{
    public enum PdfKeyword
    {
        [Description("endobj")]     EndObj,
        [Description("endstream")]  EndStream,
        [Description("false")]      False,
        [Description("null")]       Null,
        [Description("obj")]        Obj,
        [Description("R")]          R,
        [Description("startxref")]  StartXRef,
        [Description("stream")]     Stream,
        [Description("trailer")]    Trailer,
        [Description("true")]       True,
        [Description("xref")]       XRef
    }
}
