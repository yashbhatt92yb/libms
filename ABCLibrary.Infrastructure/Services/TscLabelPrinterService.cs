using System.Diagnostics;
using System.Text;
using ABCLibrary.Application.Interfaces;

namespace ABCLibrary.Infrastructure.Services;

public sealed class TscLabelPrinterService : ILabelPrinterService
{
    public Task PrintBarcodeLabelAsync(string printerName, string barcode, CancellationToken cancellationToken = default)
    {
        var command = BuildTsplCommand(barcode);
        return SendToPrinterAsync(printerName, command, cancellationToken);
    }

    public async Task PrintBatchAsync(string printerName, IEnumerable<string> barcodes, CancellationToken cancellationToken = default)
    {
        foreach (var barcode in barcodes)
        {
            await PrintBarcodeLabelAsync(printerName, barcode, cancellationToken);
        }
    }

    private static string BuildTsplCommand(string barcode)
    {
        var sb = new StringBuilder();
        sb.AppendLine("SIZE 50 mm,25 mm");
        sb.AppendLine("GAP 2 mm,0 mm");
        sb.AppendLine("DENSITY 8");
        sb.AppendLine("DIRECTION 1");
        sb.AppendLine("CLS");
        sb.AppendLine("TEXT 20,20,\"0\",0,1,1,\"PROPERTY OF ABC COLLEGE\"");
        sb.AppendLine($"TEXT 20,50,\"0\",0,1,1,\"{barcode}\"");
        sb.AppendLine($"BARCODE 20,80,\"128\",50,1,0,2,2,\"{barcode}\"");
        sb.AppendLine("PRINT 1,1");
        return sb.ToString();
    }

    private static async Task SendToPrinterAsync(string printerName, string command, CancellationToken cancellationToken)
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"abclib-tsc-{Guid.NewGuid():N}.txt");
        await File.WriteAllTextAsync(tempFile, command, cancellationToken);

        var process = Process.Start(new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/C copy /B \"{tempFile}\" \\\\localhost\\{printerName}",
            CreateNoWindow = true,
            UseShellExecute = false
        });

        if (process is not null)
        {
            await process.WaitForExitAsync(cancellationToken);
        }

        File.Delete(tempFile);
    }
}
