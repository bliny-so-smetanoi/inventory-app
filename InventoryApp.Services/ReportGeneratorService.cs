using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using QuestPDF.Helpers;
using QuestPDF.Fluent;
using InventoryApp.DataAccess.Providers.Interfaces;
using System.Runtime.CompilerServices;
using Spire.Xls;
using Spire.Xls.Core;
using System.Drawing;
namespace InventoryApp.Services
{
    public class ReportGeneratorService
    {
        private readonly IClassroomProvider _classroomProvider;
        private readonly ItemProvider _itemProvider;
        private readonly AwsS3FileUploadService _uploadService;
        private readonly IReportProvider _reportProvider;
        private readonly ILogger<ReportGeneratorService> _logger;
        public ReportGeneratorService(IClassroomProvider classroomProvider,
            ItemProvider itemProvider,
            AwsS3FileUploadService uploadService,
            IReportProvider reportProvider,
            ILogger<ReportGeneratorService> logger)
        {
            _classroomProvider = classroomProvider;
            _itemProvider = itemProvider;
            _uploadService = uploadService;
            _reportProvider = reportProvider;
            _logger = logger;
        }

        public async Task GenerateReport(string classroom, string userId)
        {
            try
            {
                var classroomInformation = await _classroomProvider.GetById(Guid.Parse(classroom));
                var categoryReport = await _classroomProvider.StatisticsPerClassCategory(classroom);
                var itemReport = await _itemProvider.Get(x => x.ClassroomId.Equals(Guid.Parse(classroom)));
                var newId = Guid.NewGuid().ToString();
                var tempName = newId + ".pdf";
                var tempNameXls = newId + ".xls";

                if (classroomInformation is null)
                {
                    throw new ArgumentException("Classroom does not exist!");
                }
                // Xls generation.
                Workbook workbook = new Workbook();
                Worksheet sheet = workbook.Worksheets[0];
                workbook.Worksheets.Remove("Sheet2");
                workbook.Worksheets.Remove("Sheet3");
                
                for (int i = 1; i <= 8; i++)
                {
                    sheet.SetColumnWidth(i, 20);
                    
                }
                
                sheet.Range["A1"].Text = "Report: ";
                sheet.Range["A2"].Text = "Report time: ";
                sheet.Range["A3"].Text = "Classroom number/name: ";
                sheet.Range["A4"].Text = "Classroom description: ";
                sheet.Range["B2"].Text = DateTime.UtcNow.ToString();
                sheet.Range["B3"].Text = classroomInformation.ClassroomName;
                sheet.Range["B4"].Text = classroomInformation.Description;
                
                
                
                sheet.Range["C1"].Text = "Category name";
                sheet.Range["D1"].Text = "Item per category";

                if(categoryReport.Count > 0)
                {
                    int count = 1;
                    for (int i = 0; i < categoryReport.Count; i++)
                    {
                        sheet.Range["C" + (i + 2)].Text = categoryReport[i].Name;
                        sheet.Range["D" + (i + 2)].Text = categoryReport[i].Count.ToString();
                        count++;
                    }
                    sheet.Range["C1:D" + count.ToString()].BorderInside(LineStyleType.MediumDashed, Color.Black);
                    sheet.Range["C1:D" + count.ToString()].BorderAround(LineStyleType.Medium, Color.Black);
                }

                sheet.Range["E1"].Text = "Name";
                sheet.Range["F1"].Text = "Barcode number";
                sheet.Range["G1"].Text = "Condition";
                sheet.Range["H1"].Text = "Description";

                if (itemReport.Count > 0)
                {
                    int count = 1;
                    for (int i = 0; i< itemReport.Count; i++)
                    {
                        sheet.Range["E" + (i + 2)].Text = itemReport[i].Name;
                        sheet.Range["F" + (i + 2)].Text = itemReport[i].ItemNumber;
                        sheet.Range["G" + (i + 2)].Text = itemReport[i].Condition;
                        sheet.Range["H" + (i + 2)].Text = itemReport[i].Description;
                        count++;
                    }
                    sheet.Range["E1:H" + count.ToString()].BorderInside(LineStyleType.MediumDashed, Color.Blue);
                    sheet.Range["E1:H" + count.ToString()].BorderAround(LineStyleType.Medium, Color.Blue);
                }

                workbook.SaveToFile(tempNameXls);

                // PDF Generation.

                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(1, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(15).FontFamily(Fonts.TimesNewRoman));
                        Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
                        page.Header().Row(x =>
                        {
                            _logger.LogInformation(System.IO.Directory.GetCurrentDirectory());
                            x.ConstantItem(150).Width(150).Height(80).Image(System.IO.Directory.GetCurrentDirectory() + "\\wwwroot\\aitu.png", ImageScaling.Resize);
                            x.RelativeItem().AlignRight().Text("Report").SemiBold().FontSize(30);
                        });


                        page.Content().Column(x =>
                        {
                            x.Item().Text("Report on (date): " + DateTime.UtcNow.ToString());
                            x.Item().Text("Classroom number: " + classroomInformation.ClassroomName);
                            x.Item().Text("Description: " + classroomInformation.Description + "\n");

                            x.Item().Table(x =>
                            {
                                x.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(150);
                                    columns.ConstantColumn(200);

                                });

                                x.Cell().ColumnSpan(2).Text("Report for categories");
                                x.Cell().AlignCenter().AlignMiddle().Text("Name of category");
                                x.Cell().AlignCenter().AlignMiddle().Text("Quantity of item per category");
                                foreach (var item in categoryReport)
                                {
                                    x.Cell().Border(1).Background(Colors.Grey.Lighten3).AlignCenter().AlignMiddle().Text(item.Name);
                                    x.Cell().Border(1).Background(Colors.Grey.Lighten3).AlignCenter().AlignMiddle().Text(item.Count);
                                }

                            });
                            x.Item().Text("\n");

                            x.Item().Table(x =>
                            {
                                x.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(100);
                                    columns.ConstantColumn(100);
                                    columns.ConstantColumn(100);
                                    columns.ConstantColumn(100);
                                });

                                x.Cell().ColumnSpan(4).Text("Report for items");
                                x.Cell().AlignCenter().AlignMiddle().Text("Name of item");
                                x.Cell().AlignCenter().AlignMiddle().Text("Barcode number");
                                x.Cell().AlignCenter().AlignMiddle().Text("Condition");
                                x.Cell().AlignCenter().AlignMiddle().Text("Description");
                                foreach (var item in itemReport)
                                {
                                    x.Cell().Border(1).Background(Colors.Grey.Lighten3).AlignCenter().AlignMiddle().Text(item.Name);
                                    x.Cell().Border(1).Background(Colors.Grey.Lighten3).AlignCenter().AlignMiddle().Text(item.ItemNumber);
                                    x.Cell().Border(1).Background(Colors.Grey.Lighten3).AlignCenter().AlignMiddle().Text(item.Condition);
                                    x.Cell().Border(1).Background(Colors.Grey.Lighten3).AlignCenter().AlignMiddle().Text(item.Description);
                                }
                            });
                        });


                        page.Footer()
                           .AlignCenter()
                           .Text(x =>
                           {
                               x.Span("Page ");
                               x.CurrentPageNumber();
                               x.Span("\n");
                               x.Span("Astana IT University, " + DateTime.UtcNow.Year.ToString());
                           });
                    });
                }).GeneratePdf(tempName);


                var memoryStream = new MemoryStream(File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\" + tempName));
                var memoryStreamXls = new MemoryStream(File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\" + tempNameXls));

                var url = await _uploadService.UploadReport(memoryStream, System.IO.Directory.GetCurrentDirectory()+ "\\" + tempName);
                var xlsUrl = await _uploadService.UploadXls(memoryStreamXls, System.IO.Directory.GetCurrentDirectory() + "\\" + tempNameXls);

                await _reportProvider.Add(new Models.Reports { XlsUrl = xlsUrl, ReportUrl = url, UserId = Guid.Parse(userId) });
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogInformation(ex.Message);
            }
            
        }

    }
}
