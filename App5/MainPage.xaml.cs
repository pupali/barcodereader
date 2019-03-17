using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using App5.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Windows.Storage.AccessCache;
using System.Collections.ObjectModel;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App5
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Windows.UI.Xaml.Controls.Page
    {
        public string mruToken = "";
        ObservableCollection<Product> products = new ObservableCollection<Product>();
        FileImportContext Context = new FileImportContext();
        Windows.UI.Xaml.Media.Brush TextBoxBrush = null;
        private Frame windowFrame;
        public MainPage()
        {
            this.InitializeComponent();
            windowFrame = Window.Current.Content as Frame;
            this.DataContext = Context;
            listview.ItemsSource = products;
            errors_List.ItemsSource = Context.Errors;
        }

        // Picking A File On Import Button
        private async void PickFile(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            picker.FileTypeFilter.Add(".xlsx");
            picker.FileTypeFilter.Add(".xls");
            Windows.Storage.StorageFile pickedFile = await picker.PickSingleFileAsync();
            if (pickedFile == null) {
                return;
            }

            var stream = await pickedFile.OpenAsync(Windows.Storage.FileAccessMode.Read);
            List<Product> prs = ReadExcelFile(stream.AsStream()); 

            //List<Product> prs = new List<Product>();
            //using (SpreadsheetDocument spDoc = SpreadsheetDocument.Open(stream.AsStream(), false))
            //{
            //    WorkbookPart wrPart = spDoc.WorkbookPart;
            //    WorksheetPart wsPart = wrPart.WorksheetParts.First();
            //    SheetData sheet = wsPart.Worksheet.Elements<SheetData>().First();
            //    foreach (Row r in sheet.Elements<Row>())
            //    {
            //        List<Cell> lst = r.Elements<Cell>().ToList();
            //        if (Context.BarcodeIdx != 0)
            //        {
            //            Debug.Write("Here");
            //            Product product = new Product(ReadExcelCell(lst[Context.ProductNameIdx], wrPart), ReadExcelCell(lst[Context.BarcodeIdx], wrPart), ReadExcelCell(lst[Context.PriceIdx], wrPart));
            //            prs.Add(product);
            //        }
            //        else
            //        {
            //            Product product = new Product(ReadExcelCell(lst[0], wrPart), ReadExcelCell(lst[1], wrPart), ReadExcelCell(lst[2], wrPart));
            //            prs.Add(product);
            //        }
            //    }
            //}
            foreach (Product product in prs)
            {
                products.Add(product);
            }
        }

        //Reading Excel File
        private List<Product> ReadExcelFile(Stream stream)
        {
            List<Product> prs = new List<Product>();
            using (SpreadsheetDocument spDoc = SpreadsheetDocument.Open(stream, false))
            {
                WorkbookPart wrPart = spDoc.WorkbookPart;
                WorksheetPart wsPart = wrPart.WorksheetParts.First();
                SheetData sheet = wsPart.Worksheet.Elements<SheetData>().First();
                foreach (Row r in sheet.Elements<Row>())
                {
                    List<Cell> lst = r.Elements<Cell>().ToList();
                    if (Context.BarcodeIdx != 0)
                    {
                        Debug.Write("Here");
                        Product product = new Product(ReadExcelCell(lst[Context.ProductNameIdx], wrPart), ReadExcelCell(lst[Context.BarcodeIdx], wrPart), ReadExcelCell(lst[Context.PriceIdx], wrPart));
                        prs.Add(product);
                    }
                    else
                    {
                        Product product = new Product(ReadExcelCell(lst[0], wrPart), ReadExcelCell(lst[1], wrPart), ReadExcelCell(lst[2], wrPart));
                        prs.Add(product);
                    }

                }
            }
            return prs;
        }
        //Reading Excel Cell Value
        private string ReadExcelCell(Cell cell, WorkbookPart workbookPart)
        {
            var cellValue = cell.CellValue;
            var text = (cellValue == null) ? cell.InnerText : cellValue.Text;
            if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
            {
                text = workbookPart.SharedStringTablePart.SharedStringTable
                    .Elements<SharedStringItem>().ElementAt(
                        Convert.ToInt32(cell.CellValue.Text)).InnerText;
            }
            return (text ?? string.Empty).Trim();
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Context.Validate();
            if (!Context.Properties[nameof(Context.ProductNameIdx)].IsValid)
            {
                if (TextBoxBrush == null)  TextBoxBrush = (sender as TextBox).BorderBrush;
                (sender as TextBox).BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
            }else {
                if(TextBoxBrush != null) (sender as TextBox).BorderBrush = TextBoxBrush; ;
            }
            import_Button.IsEnabled = Context.IsSet;
            
        }

        private void TextBox_LostFocus_1(object sender, RoutedEventArgs e)
        {
            Context.Validate();
            if (!Context.Properties[nameof(Context.PriceIdx)].IsValid)
            {
                if (TextBoxBrush == null) TextBoxBrush = (sender as TextBox).BorderBrush;
                (sender as TextBox).BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
            }
            else
            {
                if (TextBoxBrush != null) (sender as TextBox).BorderBrush = TextBoxBrush; ;
            }
            import_Button.IsEnabled = Context.IsSet;
        }

        private void TextBox_LostFocus_2(object sender, RoutedEventArgs e)
        {
            Context.Validate();
            if (!Context.Properties[nameof(Context.BarcodeIdx)].IsValid)
            {
                if (TextBoxBrush == null) TextBoxBrush = (sender as TextBox).BorderBrush;
                (sender as TextBox).BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
            }
            else
            {
                if (TextBoxBrush != null) (sender as TextBox).BorderBrush = TextBoxBrush; ;
            }
            import_Button.IsEnabled = Context.IsSet;
        }

        //Handle multiple saves **IMPORTANT**

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using(ProductContext db = new ProductContext())
            {
                await db.Products.AddRangeAsync(products);
                db.SaveChanges();
                var product = await db.Products.FirstAsync();
                Debug.WriteLine(product.ProductBarcode);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if(windowFrame == null)
            {
                return;
            }
            if (windowFrame.CanGoBack)
            {
                windowFrame.GoBack();
            }

        }
    }
}
