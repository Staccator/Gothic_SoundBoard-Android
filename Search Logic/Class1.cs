using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.InputMethodServices;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Keycode = Android.Views.Keycode;

namespace Gothic_Soundboard
{
    [Activity(Label = "CharacterList", Theme = "@android:style/Theme.Material.Light.NoActionBar")]
    public class Test : Activity
    {
        List<Dialog> SmallTalks = new List<Dialog>();
        const int NumOfDifferent = 24;
        Queue<int> UsedNumbers = new Queue<int>();
        private LinearLayout Relative;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            //Relative = FindViewById<LinearLayout>(Resource.Id.relativeLayout1);
            //RandomLogic();
            SmallTalks = DialogDataManager.Dialogs.Where(d => d.character == "SMALLTALK").ToList();
            //ManageTexts();
            //PlaySmallTalkLogic();
        }

    }
}