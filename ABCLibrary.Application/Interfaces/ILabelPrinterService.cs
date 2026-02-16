namespace ABCLibrary.Application.Interfaces;

public interface ILabelPrinterService
{
    Task PrintBarcodeLabelAsync(string printerName, string barcode, CancellationToken cancellationToken = default);
    Task PrintBatchAsync(string printerName, IEnumerable<string> barcodes, CancellationToken cancellationToken = default);
}
