using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OverviewRkiData.Components.WpfRender
{
    class WpfControlToBitmap
    {
        public static bool SaveControlImage(FrameworkElement control, string filename, bool setByActual = true)
        {
            // Get the size of the Visual and its descendants.
            var rect = VisualTreeHelper.GetDescendantBounds(control);

            if (rect.IsEmpty)
            {
                return false;
            }

            // Make a DrawingVisual to make a screen
            // representation of the control.
            var dv = new DrawingVisual();

            // Fill a rectangle the same size as the control
            // with a brush containing images of the control.
            var brush = new VisualBrush(control);
            using (var ctx = dv.RenderOpen())
            {
                ctx.DrawRectangle(brush, null, new Rect(rect.Size));
            }

            // Make a bitmap and draw on it.
            int width = (int)control.ActualWidth;
            int height = (int)control.ActualHeight;

            if (!setByActual)
            {
                width = (int)control.Width;
                height = (int)control.Height;
            }

            var rtb = new RenderTargetBitmap(
                width, 
                height, 
                96, 
                96, 
                PixelFormats.Pbgra32);
            rtb.Render(dv);

            // Make a PNG encoder.
            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            // Save the file.
            using var fs = new FileStream(
                filename,
                FileMode.Create, 
                FileAccess.Write, 
                FileShare.None);

            encoder.Save(fs);

            return true;
        }
    }
}
