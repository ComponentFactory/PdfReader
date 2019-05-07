using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;
using UnitTesting;
using System.Collections.Generic;
using System.Globalization;

namespace DocumentUnitTesting
{
    public class PdfMatrixTests : HelperMethods
    {
        [Fact]
        public void Identity()
        {
            PdfMatrix m = new PdfMatrix();
            PdfPoint p = new PdfPoint(100, 100);
            m.Transform(p);
            Assert.Equal(100.0, p.X);
            Assert.Equal(100.0, p.Y);
        }

        [Fact]
        public void TranslateXPos()
        {
            PdfMatrix m = new PdfMatrix();
            m.Translate(100, 0);

            PdfPoint p = new PdfPoint(100, 100);
            m.Transform(p);
            Assert.Equal(200.0, p.X);
            Assert.Equal(100.0, p.Y);
        }

        [Fact]
        public void TranslateXNeg()
        {
            PdfMatrix m = new PdfMatrix();
            m.Translate(-100, 0);

            PdfPoint p = new PdfPoint(100, 100);
            m.Transform(p);
            Assert.Equal(0.0, p.X);
            Assert.Equal(100.0, p.Y);
        }

        [Fact]
        public void TranslateYPos()
        {
            PdfMatrix m = new PdfMatrix();
            m.Translate(0, 100);

            PdfPoint p = new PdfPoint(100, 100);
            m.Transform(p);
            Assert.Equal(100.0, p.X);
            Assert.Equal(200.0, p.Y);
        }

        [Fact]
        public void TranslateYNeg()
        {
            PdfMatrix m = new PdfMatrix();
            m.Translate(0, -100);

            PdfPoint p = new PdfPoint(100, 100);
            m.Transform(p);
            Assert.Equal(100.0, p.X);
            Assert.Equal(0.0, p.Y);
        }

        [Fact]
        public void TranslateBothPos()
        {
            PdfMatrix m = new PdfMatrix();
            m.Translate(50, 80);

            PdfPoint p = new PdfPoint(100, 100);
            m.Transform(p);
            Assert.Equal(150.0, p.X);
            Assert.Equal(180.0, p.Y);
        }

        [Fact]
        public void TranslateBothNeg()
        {
            PdfMatrix m = new PdfMatrix();
            m.Translate(-20, -50);

            PdfPoint p = new PdfPoint(100, 100);
            m.Transform(p);
            Assert.Equal(80.0, p.X);
            Assert.Equal(50.0, p.Y);
        }

        [Fact]
        public void TranslateTwice()
        {
            PdfMatrix m = new PdfMatrix();
            m.Translate(10, 20);
            m.Translate(30, 40);

            PdfPoint p = new PdfPoint(100, 100);
            m.Transform(p);
            Assert.Equal(140.0, p.X);
            Assert.Equal(160.0, p.Y);
        }

        [Fact]
        public void TranslateTwiceNull()
        {
            PdfMatrix m = new PdfMatrix();
            m.Translate(10, 20);
            m.Translate(-10, -20);

            PdfPoint p = new PdfPoint(100, 100);
            m.Transform(p);
            Assert.Equal(100.0, p.X);
            Assert.Equal(100.0, p.Y);
        }

        [Fact]
        public void ScaleXPos()
        {
            PdfMatrix m = new PdfMatrix();
            m.Scale(2, 1);

            PdfPoint p = new PdfPoint(100, 100);
            m.Transform(p);
            Assert.Equal(200.0, p.X);
            Assert.Equal(100.0, p.Y);
        }

        [Fact]
        public void ScaleXNeg()
        {
            PdfMatrix m = new PdfMatrix();
            m.Scale(0.5f, 1);

            PdfPoint p = new PdfPoint(100, 100);
            m.Transform(p);
            Assert.Equal(50.0, p.X);
            Assert.Equal(100.0, p.Y);
        }

        [Fact]
        public void ScaleYPos()
        {
            PdfMatrix m = new PdfMatrix();
            m.Scale(1, 2);

            PdfPoint p = new PdfPoint(100, 100);
            m.Transform(p);
            Assert.Equal(100.0, p.X);
            Assert.Equal(200.0, p.Y);
        }

        [Fact]
        public void ScaleYNeg()
        {
            PdfMatrix m = new PdfMatrix();
            m.Scale(1, 0.5F);

            PdfPoint p = new PdfPoint(100, 100);
            m.Transform(p);
            Assert.Equal(100.0, p.X);
            Assert.Equal(50.0, p.Y);
        }

        [Fact]
        public void ScaleBoth()
        {
            PdfMatrix m = new PdfMatrix();
            m.Scale(2, 0.5f);

            PdfPoint p = new PdfPoint(100, 100);
            m.Transform(p);
            Assert.Equal(200.0, p.X);
            Assert.Equal(50.0, p.Y);
        }

        [Fact]
        public void ScaleTwice()
        {
            PdfMatrix m = new PdfMatrix();
            m.Scale(2, 2);
            m.Scale(2, 2);

            PdfPoint p = new PdfPoint(100, 100);
            m.Transform(p);
            Assert.Equal(400.0, p.X);
            Assert.Equal(400.0, p.Y);
        }

        [Fact]
        public void ScaleTwiceNull()
        {
            PdfMatrix m = new PdfMatrix();
            m.Scale(2, 2);
            m.Scale(0.5f, 0.5f);

            PdfPoint p = new PdfPoint(100, 100);
            m.Transform(p);
            Assert.Equal(100.0, p.X);
            Assert.Equal(100.0, p.Y);
        }


        [Fact]
        public void Rotate90()
        {
            PdfMatrix m = new PdfMatrix();
            m.RotateAt(90, 0, 0);

            PdfPoint p = new PdfPoint(20, 40);
            m.Transform(p);
            Assert.Equal(-40, Math.Round(p.X, 1));
            Assert.Equal(20, Math.Round(p.Y, 1));
        }

        [Fact]
        public void Rotate180()
        {
            PdfMatrix m = new PdfMatrix();
            m.RotateAt(180, 0, 0);

            PdfPoint p = new PdfPoint(20, 40);
            m.Transform(p);
            Assert.Equal(-20, Math.Round(p.X, 1));
            Assert.Equal(-40, Math.Round(p.Y, 1));
        }

        [Fact]
        public void Rotate270()
        {
            PdfMatrix m = new PdfMatrix();
            m.RotateAt(270, 0, 0);

            PdfPoint p = new PdfPoint(20, 40);
            m.Transform(p);
            Assert.Equal(40, Math.Round(p.X, 1));
            Assert.Equal(-20, Math.Round(p.Y, 1));
        }

        [Fact]
        public void Rotate360()
        {
            PdfMatrix m = new PdfMatrix();
            m.RotateAt(360, 0, 0);

            PdfPoint p = new PdfPoint(20, 40);
            m.Transform(p);
            Assert.Equal(20, Math.Round(p.X, 1));
            Assert.Equal(40, Math.Round(p.Y, 1));
        }
    }
}
