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
using System.Diagnostics;
using App5.Models;
using Windows.Media.SpeechSynthesis;
using Windows.Devices.PointOfService;
using System.Threading.Tasks;
using Windows.Security.Cryptography;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App5
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SResultPage : Page
    {

        private Product ResultProduct = new Product() { ProductPrice = "12,000", ProductName = "some name" };
        private MediaElement mediaElement;
        private VoiceInformation voiceInfo;
        private BarcodeScanner barcodeScanner;
        private ClaimedBarcodeScanner claimedScanner;
        private Frame windowFrame;

        public SResultPage()
        {
            this.InitializeComponent();
            windowFrame = Window.Current.Content as Frame;
            mediaElement = new MediaElement();
            this.DataContext = ResultProduct;
            var prCtx = new ProductContext();
            ResultProduct.ProductPrice = "34562346";
            voiceInfo = SpeechSynthesizer.AllVoices.Where(vI => vI.DisplayName == "Microsoft Naayf").First();
            Task.Run(() => getDefaultBarcodeScanner());
        }

        private async Task getDefaultBarcodeScanner()
        {
            barcodeScanner = await BarcodeScanner.GetDefaultAsync();
            if(barcodeScanner == null)
            {
                Debug.WriteLine("No BarcodeScanner Found");
                return;     
            }
            claimedScanner = await barcodeScanner.ClaimScannerAsync();
            claimedScanner.IsDecodeDataEnabled = true;
            claimedScanner.DataReceived += scanner_DataRecieved;
            // Add Failing claim logic **Important**
        }

        private Product getProduct(string barCode)
        {
            Product resultProduct;
            using ( var db = new ProductContext())
            {
                resultProduct = db.Products.DefaultIfEmpty(new Product()).FirstOrDefault((pr) => pr.ProductBarcode == barCode);
                return resultProduct;
            }
        }

        private async void scanner_DataRecieved(ClaimedBarcodeScanner sender, BarcodeScannerDataReceivedEventArgs args)
        {
            var reportedData = args.Report;
            string barcodeLabel = getDataLabel(reportedData);
            await updateBarcodeData(barcodeLabel);
        }

        private string getDataLabel(BarcodeScannerReport data)
        {
            uint scanDataType = data.ScanDataType;
            if (data.ScanDataLabel == null)
            {
                return "1";
            }
            else if (scanDataType == BarcodeSymbologies.Upca ||
                scanDataType == BarcodeSymbologies.UpcaAdd2 ||
                scanDataType == BarcodeSymbologies.UpcaAdd5 ||
                scanDataType == BarcodeSymbologies.Upce ||
                scanDataType == BarcodeSymbologies.UpceAdd2 ||
                scanDataType == BarcodeSymbologies.UpceAdd5 ||
                scanDataType == BarcodeSymbologies.Ean8 ||
                scanDataType == BarcodeSymbologies.TfStd)
            {
                return CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, data.ScanDataLabel);
            }
            else return "2";
        }

        private async Task updateBarcodeData(string barcodeDate)
        {
            string barcodeToSearch = barcodeDate;
            var product = getProduct(barcodeToSearch);
            if (product != null)
            {
                ResultProduct.ProductName = product.ProductName;
                ResultProduct.ProductPrice = product.ProductPrice;
                var synth = new SpeechSynthesizer();
                SpeechSynthesisStream streamAudio = await synth.SynthesizeTextToStreamAsync(ResultProduct.ProductPrice);
                Debug.WriteLine(streamAudio.ContentType);
                mediaElement.SetSource(streamAudio, streamAudio.ContentType);
                if (voiceInfo != null)
                {
                    synth.Voice = voiceInfo;
                }
                else
                {
                    synth.Voice = SpeechSynthesizer.DefaultVoice;
                }
                mediaElement.Play();
                Debug.WriteLine(synth.Voice.Language);
                //Debug.WriteLine(ResultProduct.ProductName);
            }
            else
            {
                return;
            }
        }

        //Test Button Handler **Delete**
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string barcodeToSearch = barCodeText.Text;
            var product = getProduct(barcodeToSearch);
            if (product != null)
            {
                ResultProduct.ProductName = product.ProductName;
                ResultProduct.ProductPrice = product.ProductPrice;
                var synth = new SpeechSynthesizer();
                SpeechSynthesisStream streamAudio = await synth.SynthesizeTextToStreamAsync(ResultProduct.ProductPrice);
                Debug.WriteLine(streamAudio.ContentType);
                mediaElement.SetSource(streamAudio, streamAudio.ContentType);
                if (voiceInfo != null)
                {
                    synth.Voice = voiceInfo;
                }
                else
                {
                    synth.Voice = SpeechSynthesizer.DefaultVoice;
                }
                mediaElement.Play();
                Debug.WriteLine(synth.Voice.Language);
                //Debug.WriteLine(ResultProduct.ProductName);
            }
            else
            {
                return;
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
