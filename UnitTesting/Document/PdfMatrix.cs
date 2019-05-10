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
            RenderMatrix m = new RenderMatrix();
            RenderPoint p = new RenderPoint(100, 100);
            m.Transform(p);
            Assert.Equal(100.0, p.X);
            Assert.Equal(100.0, p.Y);
        }

        [Fact]
        public void TranslateXPos()
        {
            RenderMatrix m = new RenderMatrix();
            m.Translate(100, 0);

            RenderPoint p = new RenderPoint(100, 100);
            m.Transform(p);
            Assert.Equal(200.0, p.X);
            Assert.Equal(100.0, p.Y);
        }

        [Fact]
        public void TranslateXNeg()
        {
            RenderMatrix m = new RenderMatrix();
            m.Translate(-100, 0);

            RenderPoint p = new RenderPoint(100, 100);
            m.Transform(p);
            Assert.Equal(0.0, p.X);
            Assert.Equal(100.0, p.Y);
        }

        [Fact]
        public void TranslateYPos()
        {
            RenderMatrix m = new RenderMatrix();
            m.Translate(0, 100);

            RenderPoint p = new RenderPoint(100, 100);
            m.Transform(p);
            Assert.Equal(100.0, p.X);
            Assert.Equal(200.0, p.Y);
        }

        [Fact]
        public void TranslateYNeg()
        {
            RenderMatrix m = new RenderMatrix();
            m.Translate(0, -100);

            RenderPoint p = new RenderPoint(100, 100);
            m.Transform(p);
            Assert.Equal(100.0, p.X);
            Assert.Equal(0.0, p.Y);
        }

        [Fact]
        public void TranslateBothPos()
        {
            RenderMatrix m = new RenderMatrix();
            m.Translate(50, 80);

            RenderPoint p = new RenderPoint(100, 100);
            m.Transform(p);
            Assert.Equal(150.0, p.X);
            Assert.Equal(180.0, p.Y);
        }

        [Fact]
        public void TranslateBothNeg()
        {
            RenderMatrix m = new RenderMatrix();
            m.Translate(-20, -50);

            RenderPoint p = new RenderPoint(100, 100);
            m.Transform(p);
            Assert.Equal(80.0, p.X);
            Assert.Equal(50.0, p.Y);
        }

        [Fact]
        public void TranslateTwice()
        {
            RenderMatrix m = new RenderMatrix();
            m.Translate(10, 20);
            m.Translate(30, 40);

            RenderPoint p = new RenderPoint(100, 100);
            m.Transform(p);
            Assert.Equal(140.0, p.X);
            Assert.Equal(160.0, p.Y);
        }

        [Fact]
        public void TranslateTwiceNull()
        {
            RenderMatrix m = new RenderMatrix();
            m.Translate(10, 20);
            m.Translate(-10, -20);

            RenderPoint p = new RenderPoint(100, 100);
            m.Transform(p);
            Assert.Equal(100.0, p.X);
            Assert.Equal(100.0, p.Y);
        }

        [Fact]
        public void ScaleXPos()
        {
            RenderMatrix m = new RenderMatrix();
            m.Scale(2, 1);

            RenderPoint p = new RenderPoint(100, 100);
            m.Transform(p);
            Assert.Equal(200.0, p.X);
            Assert.Equal(100.0, p.Y);
        }

        [Fact]
        public void ScaleXNeg()
        {
            RenderMatrix m = new RenderMatrix();
            m.Scale(0.5f, 1);

            RenderPoint p = new RenderPoint(100, 100);
            m.Transform(p);
            Assert.Equal(50.0, p.X);
            Assert.Equal(100.0, p.Y);
        }

        [Fact]
        public void ScaleYPos()
        {
            RenderMatrix m = new RenderMatrix();
            m.Scale(1, 2);

            RenderPoint p = new RenderPoint(100, 100);
            m.Transform(p);
            Assert.Equal(100.0, p.X);
            Assert.Equal(200.0, p.Y);
        }

        [Fact]
        public void ScaleYNeg()
        {
            RenderMatrix m = new RenderMatrix();
            m.Scale(1, 0.5F);

            RenderPoint p = new RenderPoint(100, 100);
            m.Transform(p);
            Assert.Equal(100.0, p.X);
            Assert.Equal(50.0, p.Y);
        }

        [Fact]
        public void ScaleBoth()
        {
            RenderMatrix m = new RenderMatrix();
            m.Scale(2, 0.5f);

            RenderPoint p = new RenderPoint(100, 100);
            m.Transform(p);
            Assert.Equal(200.0, p.X);
            Assert.Equal(50.0, p.Y);
        }

        [Fact]
        public void ScaleTwice()
        {
            RenderMatrix m = new RenderMatrix();
            m.Scale(2, 2);
            m.Scale(2, 2);

            RenderPoint p = new RenderPoint(100, 100);
            m.Transform(p);
            Assert.Equal(400.0, p.X);
            Assert.Equal(400.0, p.Y);
        }

        [Fact]
        public void ScaleTwiceNull()
        {
            RenderMatrix m = new RenderMatrix();
            m.Scale(2, 2);
            m.Scale(0.5f, 0.5f);

            RenderPoint p = new RenderPoint(100, 100);
            m.Transform(p);
            Assert.Equal(100.0, p.X);
            Assert.Equal(100.0, p.Y);
        }


        [Fact]
        public void Rotate90()
        {
            RenderMatrix m = new RenderMatrix();
            m.RotateAt(90, 0, 0);

            RenderPoint p = new RenderPoint(20, 40);
            m.Transform(p);
            Assert.Equal(-40, Math.Round(p.X, 1));
            Assert.Equal(20, Math.Round(p.Y, 1));
        }

        [Fact]
        public void Rotate180()
        {
            RenderMatrix m = new RenderMatrix();
            m.RotateAt(180, 0, 0);

            RenderPoint p = new RenderPoint(20, 40);
            m.Transform(p);
            Assert.Equal(-20, Math.Round(p.X, 1));
            Assert.Equal(-40, Math.Round(p.Y, 1));
        }

        [Fact]
        public void Rotate270()
        {
            RenderMatrix m = new RenderMatrix();
            m.RotateAt(270, 0, 0);

            RenderPoint p = new RenderPoint(20, 40);
            m.Transform(p);
            Assert.Equal(40, Math.Round(p.X, 1));
            Assert.Equal(-20, Math.Round(p.Y, 1));
        }

        [Fact]
        public void Rotate360()
        {
            RenderMatrix m = new RenderMatrix();
            m.RotateAt(360, 0, 0);

            RenderPoint p = new RenderPoint(20, 40);
            m.Transform(p);
            Assert.Equal(20, Math.Round(p.X, 1));
            Assert.Equal(40, Math.Round(p.Y, 1));
        }
    }
}
