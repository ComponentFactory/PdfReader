using System;
using System.IO;
using System.Text;

namespace PdfXenon.Standard
{
    public class Parser
    {
        public Parser(Stream stream)
        {
            Tokenizer = new Tokenizer(stream);
        }

        public void TestParse()
        {
            try
            {
                ParseHeader(out int major, out int minor);
                ParseIndirectObjects();
            }
            catch(ApplicationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ParseHeader(out int major, out int minor)
        {
            // Should start with a comment that is the pdf header
            Tokenizer.IgnoreComments = false;
            TokenBase t = Tokenizer.GetToken();
            if (!(t is TokenComment))
                throw new ApplicationException("Missing PDF header.");

            TokenComment c = (TokenComment)t;
            if (!c.Comment.StartsWith("%PDF"))
                throw new ApplicationException("Header must start with %PDF text.");

            string[] splits = c.Comment.Substring(5).Split('.');
            if (splits.Length != 2)
                throw new ApplicationException("Header must have a major.minor version number.");

            if (!int.TryParse(splits[0].Trim(), out major))
                throw new ApplicationException("Cannot parse the header major version number.");


            if (!int.TryParse(splits[1].Trim(), out minor))
                throw new ApplicationException("Cannot parse the header minor version number.");
        }

        private void ParseIndirectObjects()
        {
            // Ignore any comments before the next real content
            Tokenizer.IgnoreComments = true;
            TokenBase t = Tokenizer.GetToken();
            if (t is TokenNumeric)
            {

            }
            else if (t is TokenKeyword)
            {

            }
            else if (t is TokenError)
                throw new ApplicationException(t.ToString());
            else
                throw new ApplicationException("Unrecognized pdf content");
        }

        private Tokenizer Tokenizer { get; set; }
    }
}
