using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Gothic_Soundboard
{
    [Activity(Label = "Filter")]
    public class Filter : Activity
    {
        ImageButton Button1, Button2, Button3;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.filter_layout);

            Button1 = FindViewById<ImageButton>(Resource.Id.Button1_filter);
            Button2 = FindViewById<ImageButton>(Resource.Id.Button2_filter);
            Button3 = FindViewById<ImageButton>(Resource.Id.Button3_filter);
            SetAnimations();
        }
        private void SetAnimations()
        {
            Button1.Touch += Button_Touch;
            Button2.Touch += Button_Touch;
            Button3.Touch += Button_Touch;
        }

        public static int duration = 180;
        private void Button_Touch(object sender, View.TouchEventArgs e)
        {
            ImageButton button = sender as ImageButton;
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    ObjectAnimator animatorX = ObjectAnimator.OfFloat(button, "scaleX", button.ScaleX, 1.25f);
                    animatorX.SetDuration(duration);
                    ObjectAnimator animatorY = ObjectAnimator.OfFloat(button, "scaleY", button.ScaleY, 1.25f);
                    animatorY.SetDuration(duration);
                    var animSet = new AnimatorSet();
                    animSet.PlayTogether(animatorX, animatorY);
                    animSet.Start();
                    break;
                case MotionEventActions.Move:
                    break;
                case MotionEventActions.Up:
                    ObjectAnimator animatorX1 = ObjectAnimator.OfFloat(button, "scaleX", button.ScaleX, 1f);
                    animatorX1.SetDuration(duration);
                    ObjectAnimator animatorY1 = ObjectAnimator.OfFloat(button, "scaleY", button.ScaleY, 1f);
                    animatorY1.SetDuration(duration);
                    var reverseanimSet = new AnimatorSet();
                    reverseanimSet.PlayTogether(animatorX1, animatorY1);
                    reverseanimSet.Start();
                    break;
            }
        }
    }

}