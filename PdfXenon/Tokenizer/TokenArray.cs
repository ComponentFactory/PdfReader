using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class TokenArrayOpen : TokenBase
    {
        public override string ToString()
        {
            return "Array Open";
        }
    }

    public class TokenArrayClose : TokenBase
    {
        public override string ToString()
        {
            return "Array Close";
        }
    }
}
