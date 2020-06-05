using AForge.Imaging;
using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace SWRunner
{
    public static class BitmapUtils
    {
        public static Bitmap CropImage(Bitmap source, Rectangle rec)
        {
            Bitmap target = new Bitmap(rec.Width, rec.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(source, new Rectangle(0, 0, target.Width, target.Height),
                                 rec, GraphicsUnit.Pixel);
            }

            // Test
            //target.Save($"F:\\Test\\meh", ImageFormat.Png);

            return target;
        }

        public static bool FindMatchImage(Bitmap source, Bitmap template, float percentMatch)
        {
            float scale = 0.45f;
            source = ConvertToFormat(source, PixelFormat.Format24bppRgb);
            source = new ResizeBicubic((int)(source.Width * scale), (int)(source.Height * scale)).Apply(source);


            template = ConvertToFormat(template, PixelFormat.Format24bppRgb);
            template = new ResizeBicubic((int)(template.Width * scale), (int)(template.Height * scale)).Apply(template);

            ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(percentMatch);

            TemplateMatch[] matchings = tm.ProcessImage(source, template);

            return matchings.Length > 0; // 1 to be exact
        }

        private static Bitmap ConvertToFormat(Bitmap image, PixelFormat format)
        {
            Bitmap copy = new Bitmap(image.Width, image.Height, format);
            using (Graphics gr = Graphics.FromImage(copy))
            {
                gr.DrawImage(image, new Rectangle(0, 0, copy.Width, copy.Height));
            }
            return copy;
        }

        public static Bitmap Resize(Bitmap source, int newWidth, int newHeight)
        {
            return new ResizeBicubic(newWidth, newHeight).Apply(source);
        }
    }
}
