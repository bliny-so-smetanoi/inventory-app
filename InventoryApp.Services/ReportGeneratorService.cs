using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using QuestPDF.Helpers;
using QuestPDF.Fluent;
using InventoryApp.DataAccess.Providers.Interfaces;
using System.Runtime.CompilerServices;

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
                var tempName = Guid.NewGuid().ToString() + ".pdf";
                
                if (classroomInformation is null)
                {
                    throw new ArgumentException("Classroom does not exist!");
                }

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
                                x.Cell().Text("Name of category");
                                x.Cell().Text("Quantity of item per category");
                                foreach (var item in categoryReport)
                                {
                                    x.Cell().Text(item.Name);
                                    x.Cell().Text(item.Count);
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
                                x.Cell().Text("Name of item");
                                x.Cell().Text("Item number");
                                x.Cell().Text("Condition");
                                x.Cell().Text("Description");
                                foreach (var item in itemReport)
                                {
                                    x.Cell().Text(item.Name);
                                    x.Cell().Text(item.ItemNumber);
                                    x.Cell().Text(item.Condition);
                                    x.Cell().Text(item.Description);
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
                var url = await _uploadService.UploadReport(memoryStream, System.IO.Directory.GetCurrentDirectory()+ "\\" + tempName);

                await _reportProvider.Add(new Models.Reports { ReportUrl = url, UserId = Guid.Parse(userId) });
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogInformation(ex.Message);
            }
            
        }

    }
}
