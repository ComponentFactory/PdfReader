using PdfXenon.GDI;
using PdfXenon.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if(_bitmap != null)
                e.Graphics.DrawImage(_bitmap, 12, 12);

            base.OnPaint(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_document == null)
            {
                _document = new PdfDocument();
                _document.Load(@"d:\Shapes.pdf", true);
                _document.Close();
            }

            if (_pageIndex < _document.Catalog.Pages.Count)
            {
                Console.WriteLine(_document.Catalog.Pages[_pageIndex].ToDebug());

                label1.Text = _pageIndex.ToString();
                PdfGDIRenderer renderer = new PdfGDIRenderer();
                PdfPageProcessor processsor = new PdfPageProcessor(_document.Catalog.Pages[_pageIndex++], renderer);
                processsor.Process();
                _bitmap = renderer.Bitmap;
                Refresh();
            }
        }

        private PdfDocument _document;
        private int _pageIndex = 0;
        private Bitmap _bitmap;
    }
}
