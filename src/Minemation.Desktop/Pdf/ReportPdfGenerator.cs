using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Minemation.Desktop.Pdf;

public static class ReportPdfGenerator
{
    static ReportPdfGenerator()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public static string GenerateReportPdf(ReportPdfModel model)
    {
        var reportsDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "MinemationReports");

        Directory.CreateDirectory(reportsDirectory);

        var safeFileName = MakeSafeFileName($"{model.ReportName}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        var filePath = Path.Combine(reportsDirectory, safeFileName);

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(35);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Column(column =>
                {
                    column.Item().Text("MINEMATION")
                        .FontSize(22)
                        .Bold()
                        .FontColor(Colors.Green.Darken2);

                    column.Item().Text(model.ReportType)
                        .FontSize(15)
                        .SemiBold();

                    column.Item().Text($"Olusturma Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}")
                        .FontSize(9)
                        .FontColor(Colors.Grey.Darken1);

                    column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                });

                page.Content().PaddingVertical(20).Column(column =>
                {
                    column.Spacing(12);

                    column.Item().Text(model.ReportName)
                        .FontSize(18)
                        .Bold();

                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(130);
                            columns.RelativeColumn();
                        });

                        AddInfoRow(table, "Rapor Turu", model.ReportType);
                        AddInfoRow(table, "Zaman Araligi", model.DateRange);
                        AddInfoRow(table, "Personel ID", model.PersonnelId?.ToString() ?? "-");
                        AddInfoRow(table, "Ekipman ID", model.EquipmentId?.ToString() ?? "-");
                    });

                    column.Item().Text("Rapor Icerigi")
                        .FontSize(14)
                        .Bold();

                    column.Item()
                        .Border(1)
                        .BorderColor(Colors.Grey.Lighten2)
                        .Padding(12)
                        .Text(model.Description)
                        .FontSize(10)
                        .LineHeight(1.35f);
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Sayfa ");
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
            });
        }).GeneratePdf(filePath);

        return filePath;
    }

    private static void AddInfoRow(TableDescriptor table, string label, string value)
    {
        table.Cell().Element(CellStyle).Text(label).SemiBold();
        table.Cell().Element(CellStyle).Text(value);

        static IContainer CellStyle(IContainer container)
        {
            return container
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten3)
                .PaddingVertical(5)
                .PaddingHorizontal(4);
        }
    }

    private static string MakeSafeFileName(string fileName)
    {
        foreach (var invalidChar in Path.GetInvalidFileNameChars())
            fileName = fileName.Replace(invalidChar, '_');

        return fileName;
    }
}

public class ReportPdfModel
{
    public string ReportName { get; set; } = string.Empty;
    public string ReportType { get; set; } = string.Empty;
    public string DateRange { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? PersonnelId { get; set; }
    public int? EquipmentId { get; set; }
}
