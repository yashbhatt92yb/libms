using ABCLibrary.Application.Interfaces;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;

namespace ABCLibrary.Infrastructure.Services;

public sealed class BarcodeService : IBarcodeService
{
    public string BuildNextBarcode(long sequence) => $"LIB-{sequence:D7}";

    public byte[] GenerateCode128Png(string value, int width = 300, int height = 80)
    {
        var writer = new BarcodeWriter<byte[]>
        {
            Format = BarcodeFormat.CODE_128,
            Options = new EncodingOptions
            {
                Width = width,
                Height = height,
                Margin = 2,
                PureBarcode = false
            },
            Renderer = new ByteArrayBitmapRenderer()
        };

        return writer.Write(value);
    }
}
