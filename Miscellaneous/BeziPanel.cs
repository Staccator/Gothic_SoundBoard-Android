using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Animation;

namespace Gothic_Soundboard
{
    [Activity(Label = "Activity1")]
    public class BeziPanel : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.bezi_panel);
            Button go_back = FindViewById<Button>(Resource.Id.go_back_button);
            go_back.Click += Go_back_Click;
        }

        

        private void Go_back_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}