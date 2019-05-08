using System;
using System.Collections.Generic;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Gothic_Soundboard
{
    public class GenericFileProvider : Android.Support.V4.Content.FileProvider { }

    [Activity(Label = "@string/app_name", MainLauncher = true, Theme ="@android:style/Theme.Material.Light.NoActionBar")]
    public class MainActivity : Activity
    {
        Button button1, button2, button3, button4;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            InitializeAssets();
            SetButtons();
            //WavTest();
           
        }

        protected override void OnPause()
        {
            base.OnPause();
            //DialogDataManager.SaveFavorites();
        }

        MediaPlayer mp;
        private void WavTest()
        {
            mp = new MediaPlayer();
            #pragma warning disable CS0618 // Typ lub składowa jest przestarzała
            mp.SetAudioStreamType(Stream.Music);
            var fd = Assets.OpenFd("Dialogs/DIA_LESTER_CAMPINFO_SLEEPER_05_05.wav");
            mp.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
            fd.Close();
            mp.Prepare();
        }

        private void InitializeAssets()
        {
            DialogDataManager.LoadDataFromAssets(this);
        }

        Typeface tf1, tf2;
        private void SetButtons()
        {
            tf1 = Typeface.CreateFromAsset(Assets, "fonts/thewitcher.ttf");   
            tf2 = Typeface.CreateFromAsset(Assets, "stibold.ttf");

            button1 = FindViewById<Button>(Resource.Id.main_button_1);
            button2 = FindViewById<Button>(Resource.Id.main_button_2);
            button3 = FindViewById<Button>(Resource.Id.main_button_3);
            button4 = FindViewById<Button>(Resource.Id.main_button_4);
            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
            button3.Click += Button3_Click;
            button4.Click += Button4_Click;

            SetButton(button1);
            SetButton(button2);
            SetButton(button3);
            SetButton(button4);

        }

        private void SetButton(Button button)
        {
            button.SetTypeface(tf1, TypefaceStyle.Normal);
            button.SetTextSize(Android.Util.ComplexUnitType.Dip, 30);
            button.SetTextColor(Color.Black);
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            //DialogDataManager.LoadDataFromAssets(this);
            StartActivity(typeof(SearchList));
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(CharacterList));
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(SmallTalk));
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Intent CharacterIntent = new Intent(this, typeof(DialogList));
            CharacterIntent.PutExtra("favorites", true);
            StartActivity(CharacterIntent);
        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}

