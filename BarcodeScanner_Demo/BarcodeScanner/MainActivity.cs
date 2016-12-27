using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using Android.OS;
using ZXing;
using ZXing.Mobile;

namespace BarcodeScanner
{
    [Activity(Label = "BarcodeScanner", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private TextView _barcodeFormat, _barcodeData;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            MobileBarcodeScanner.Initialize(Application);

            _barcodeFormat = FindViewById<TextView>(Resource.Id.barcode_format);
            _barcodeData = FindViewById<TextView>(Resource.Id.barcode_data);

            var button = FindViewById<Button>(Resource.Id.button_scan);
            button.Click += async (sender, args) =>
            {
                var opts = new MobileBarcodeScanningOptions
                {
                    PossibleFormats = new List<BarcodeFormat>
                    {
                        BarcodeFormat.CODE_128,
                        BarcodeFormat.EAN_13,
                        BarcodeFormat.EAN_8,
                        BarcodeFormat.QR_CODE
                    }
                };
                var scanner = new MobileBarcodeScanner();
                var result = await scanner.Scan(opts);

                _barcodeFormat.Text = result?.BarcodeFormat.ToString() ?? string.Empty;
                _barcodeData.Text = result?.Text ?? string.Empty;
            };
        }
    }
}

