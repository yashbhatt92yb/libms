using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ABCLibrary.Application.Interfaces;
using ZXing;
using ZXing.Common;

namespace ABCLibrary.Infrastructure.Services;

public sealed class BarcodeService : IBarcodeService
{
    public string BuildNextBarcode(long sequence) => $"LIB-{sequence:D7}";

    public byte[] GenerateCode128Png(string value, int width = 300, int height = 80)
    {
        var writer = new BarcodeWriterPixelData
        {
            Format = BarcodeFormat.CODE_128,
            Options = new EncodingOptions
            {
                Width = width,
                Height = height,
                Margin = 2,
                PureBarcode = false
            }
        };

        var pixelData = writer.Write(value);

        using var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
        var rect = new Rectangle(0, 0, pixelData.Width, pixelData.Height);
        var bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
        try
        {
            Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
        }
        finally
        {
            bitmap.UnlockBits(bitmapData);
        }

        using var ms = new MemoryStream();
        bitmap.Save(ms, ImageFormat.Png);
        return ms.ToArray();
    }
}
