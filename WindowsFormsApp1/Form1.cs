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
            //GraphicsPath linePath = new GraphicsPath();
            //linePath.AddLine(35.25f, 806.67f, 146.25f, 718.92f);
            //e.Graphics.DrawPath(Pens.Blue, linePath);

            //GraphicsPath rectPath = new GraphicsPath();
            //rectPath.AddRectangle(new RectangleF(36.75f, 599.67f, 116.25f, 93f));
            //e.Graphics.DrawPath(Pens.Blue, rectPath);

            //GraphicsPath ellipsePath = new GraphicsPath();
            //ellipsePath.AddBezier(39f, 510.8f, 39f, 542.07f, 66.367f, 567.42f, 100.12f, 567.42f);
            //ellipsePath.AddBezier(100.12f, 567.42f, 133.88f, 567.42f, 161.25f,542.07f, 161.25f, 510.8f);
            //ellipsePath.AddBezier(161.25f, 510.8f, 161.25f, 479.52f, 133.88f, 454.17f, 100.12f, 454.17f);
            //ellipsePath.AddBezier(100.12f, 454.17f, 66.367f, 454.17f, 39f, 479.52f, 39f, 510.8f);
            //e.Graphics.DrawPath(Pens.Blue, ellipsePath);

            if(_bitmap != null)
                e.Graphics.DrawImage(_bitmap, 12, 12);

            base.OnPaint(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PdfDocument document = new PdfDocument();
            document.Load(@"d:\Shapes.pdf", true);
            document.Close();

            PdfGDIRenderer renderer = new PdfGDIRenderer();
            PdfPageProcessor processsor = new PdfPageProcessor(document.Catalog.Pages[0], renderer);
            processsor.Process();
            _bitmap = renderer.Bitmap;

            Refresh();
        }

        private Bitmap _bitmap;
    }
}
