using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace NumberNotify_Demo
{
    [Activity(Label = "NumberNotify_Demo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        private uint _count = 0;
        private TextView _numTextView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            
            _numTextView = FindViewById<TextView>(Resource.Id.toolbar_cart_number);
            if (_count == 0)
            {
                _numTextView.Visibility = ViewStates.Gone;
            }

            var cart = FindViewById<ImageView>(Resource.Id.toolbar_cart);
            cart.Click += (sender, args) =>
            {
                _count += 1;
                if (_numTextView.Visibility == ViewStates.Gone)
                    _numTextView.Visibility = ViewStates.Visible;

                if (_count < 10)
                {
                    _numTextView.Text = _count.ToString();
                }
                else
                {
                    _numTextView.Text = 9 + "+";
                }
            };
        }
    }
}

