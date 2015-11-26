using System;
using System.Drawing;
using System.Globalization;
using NetOffice.ExcelApi;
using NetOffice.OfficeApi.Enums;
using NetOffice.PowerPointApi;
using NetOffice.PowerPointApi.Enums;
using Application = NetOffice.PowerPointApi.Application;
using Axis = NetOffice.PowerPointApi.Axis;
using Chart = NetOffice.PowerPointApi.Chart;
using XlAxisGroup = NetOffice.PowerPointApi.Enums.XlAxisGroup;
using XlAxisType = NetOffice.PowerPointApi.Enums.XlAxisType;

namespace imperugo.spike.office.powerpoint
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var powerApplication = new Application();
			var presentation = powerApplication.Presentations.Add(MsoTriState.msoTrue);

			var slide1 = presentation.Slides.Add(1, PpSlideLayout.ppLayoutBlank);

			//Access the first slide of presentation
			slide1 = presentation.Slides[1];

			//Select firs slide and set its layout
			slide1.Select();
			slide1.Layout = PpSlideLayout.ppLayoutBlank;

			//Add a default chart in slide
			slide1.Shapes.AddChart(Microsoft.Office.Core.XlChartType.xl3DColumn, 20F, 30F, 400F, 300F);

			//Access the added chart
			Chart ppChart = slide1.Shapes[1].Chart;

			//Access the chart data
			ChartData chartData = ppChart.ChartData;

			//Create instance to Excel workbook to work with chart data
			Workbook dataWorkbook = (Workbook)chartData.Workbook;

			//Accessing the data worksheet for chart
			Worksheet dataSheet = (Worksheet)dataWorkbook.Worksheets[1];

			//Setting the range of chart
			Range tRange = dataSheet.Cells.get_Range("A1", "B5");

			//Applying the set range on chart data table
			ListObject tbl1 = dataSheet.ListObjects["Table1"];
			tbl1.Resize(tRange);

			// Setting values for categories and respective series data

			((Range)(dataSheet.Cells.get_Range("A2"))).FormulaR1C1 = "Bikes";
			((Range)(dataSheet.Cells.get_Range("A3"))).FormulaR1C1 = "Accessories";
			((Range)(dataSheet.Cells.get_Range("A4"))).FormulaR1C1 = "Repairs";
			((Range)(dataSheet.Cells.get_Range("A5"))).FormulaR1C1 = "Clothing";
			((Range)(dataSheet.Cells.get_Range("B2"))).FormulaR1C1 = "1000";
			((Range)(dataSheet.Cells.get_Range("B3"))).FormulaR1C1 = "2500";
			((Range)(dataSheet.Cells.get_Range("B4"))).FormulaR1C1 = "4000";
			((Range)(dataSheet.Cells.get_Range("B5"))).FormulaR1C1 = "3000";

			//Setting chart title
			ppChart.ChartTitle.Font.Italic = true;
			ppChart.ChartTitle.Text = "2007 Sales";
			ppChart.ChartTitle.Font.Size = 18;
			ppChart.ChartTitle.Font.Color = Color.Black.ToArgb();
			ppChart.ChartTitle.Format.Line.Visible = MsoTriState.msoTrue;
			ppChart.ChartTitle.Format.Line.ForeColor.RGB = Color.Black.ToArgb();


			//Accessing Chart value axis
			Axis valaxis = (Axis)ppChart.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary);

			//Setting values axis units
			valaxis.MajorUnit = 2000.0F;
			valaxis.MinorUnit = 1000.0F;
			valaxis.MinimumScale = 0.0F;
			valaxis.MaximumScale = 4000.0F;

			//Accessing Chart Depth axis
			Axis Depthaxis = (Axis)ppChart.Axes(XlAxisType.xlSeriesAxis, XlAxisGroup.xlPrimary);
			Depthaxis.Delete();

			//Setting chart rotation
			ppChart.Rotation = 20; //Y-Value
			ppChart.Elevation = 15; //X-Value
			ppChart.RightAngleAxes = false;


			string fileExtension = GetDefaultExtension(powerApplication);
			string documentFile = $"{AppDomain.CurrentDomain.BaseDirectory}\\Example04{fileExtension}";

			presentation.SaveAs(documentFile,
						PpSaveAsFileType.ppSaveAsDefault,
						MsoTriState.msoTrue);

			// close power point and dispose reference
			powerApplication.Quit();
			powerApplication.Dispose();

			Console.WriteLine("Fatto...");
			Console.ReadLine();
		}

		private static string GetDefaultExtension(Application application)
		{
			double version = Convert.ToDouble(application.Version, CultureInfo.InvariantCulture);

			return version >= 12.00 ? ".pptx" : ".ppt";
		}
	}
}