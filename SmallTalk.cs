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
    

    [Activity(Label = "SmallTalk", Theme = "@android:style/Theme.Material.Light.NoActionBar",ScreenOrientation = ScreenOrientation.Portrait)]
    public class SmallTalk : Activity
    {
        List<Dialog> SmallTalks = new List<Dialog>();
        const int NumOfDifferent = 24;
        Queue<int> UsedNumbers = new Queue<int>();
        private LinearLayout Relative;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SmallTalkLayout);
            Relative = FindViewById<LinearLayout>(Resource.Id.relativeLayout1);
            RandomLogic();
            SmallTalks = DialogDataManager.Dialogs.Where(d => d.character == "SMALLTALK").ToList();
            ManageTexts();
            PlaySmallTalkLogic();
        }

        private MediaPlayer mp;
        private bool top;
        private bool slept;
        private void PlaySmallTalkLogic()
        {
            if(!slept)
            {
                mp = new MediaPlayer();
                #pragma warning disable CS0618 // Typ lub składowa jest przestarzała
                mp.SetAudioStreamType(Stream.Music);
                var descriptor = Assets.OpenFd("silence.mp3");
                mp.SetDataSource(descriptor.FileDescriptor, descriptor.StartOffset, descriptor.Length);
                descriptor.Close();
                mp.Prepare();
                mp.Completion += (sender, args) =>
                {
                    mp.Stop();
                    mp.Release();
                    slept = true;
                    PlaySmallTalkLogic(); 
                };
                mp.Start();
                return;
            }
            slept = false;
            
            top = !top;

            Dialog dialog = GetNextSmallTalk();

            switch (top)
            {
                case true:
                    RunOnUiThread(() =>
                    {
                        SetFadeAnimation(TopText, 1600);
                        TopText.Text = dialog.text;
                    });
                    break;
                case false:
                    RunOnUiThread(() => 
                    {
                        SetFadeAnimation(BotText, 1600);
                        BotText.Text = dialog.text;
                    });
                    break;
            }

            mp = new MediaPlayer();
#pragma warning disable CS0618 // Typ lub składowa jest przestarzała
            mp.SetAudioStreamType(Stream.Music);
            var fd = Assets.OpenFd("Files/" + dialog.file_name);
            mp.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
            fd.Close();
            mp.Prepare();
            mp.Completion += Mp_Completion;
            mp.Start();
        }

        private void Mp_Completion(object sender, EventArgs e)
        {
            mp.Stop();
            mp.Release();
            if (top)
            {
                SetFadeAnimation(TopText, 800, false);
            }
            else
            {
                SetFadeAnimation(BotText, 800, false);
            }

            PlaySmallTalkLogic();
        }

        private void SetFadeAnimation(TextView text, long duration, bool show = true)
        {
            if (show)
            {
                ObjectAnimator colorAnimator = ObjectAnimator.OfArgb(text, "TextColor", Color.Argb(0, 255, 255, 255), Color.Argb(255, 255, 255, 255));
                colorAnimator.SetDuration(duration);
                colorAnimator.Start();
            }
            else
            {
                ObjectAnimator colorAnimator = ObjectAnimator.OfArgb(text, "TextColor", text.CurrentTextColor, Color.Argb(0, 255, 255, 255));
                colorAnimator.SetDuration(duration);
                colorAnimator.Start();
            }
        }

        TextView TopText, BotText;
        private void ManageTexts()
        {
            TopText = FindViewById<TextView>(Resource.Id.SmallTalkText1);
            BotText = FindViewById<TextView>(Resource.Id.SmallTalkText2);
            TopText.Text = "";
            BotText.Text = "";
        }

        private void RandomLogic()
        {
            var shuffled = Enumerable.Range(1, NumOfDifferent).ToArray().Shuffle().Take(NumOfDifferent/2);
            UsedNumbers = new Queue<int>(shuffled);
        }
        private Dialog GetNextSmallTalk()
        {
            List<int> availableNumbers = Enumerable.Range(1, NumOfDifferent).Except(UsedNumbers).ToList();

            Dialog result = SmallTalks.Where(d => availableNumbers.Contains(d.file_name.NumberOfSmalltalk())).GetRandomElement();
            UsedNumbers.Dequeue();
            UsedNumbers.Enqueue(result.file_name.NumberOfSmalltalk());
            return result;
        }

        int alpha = 255;
        public void UpgradeBackgroundColor()
        {
            alpha -= 2;if (alpha < 0) alpha =0;
            Relative.SetBackgroundColor(Color.Argb(alpha,150,150,150));
            TopText.SetLineSpacing(3,1);

            TopText.SetTextColor(Color.Argb(alpha, 120, 200, 150));
        }
        public override void OnBackPressed()
        {
            FinishAndRemoveTask();
            base.OnBackPressed();
            if (mp != null)
            {
                mp.Pause();
                mp.Release();
            }
              
        }
        
    }

    public static class Extender
    {
        public static T GetRandomElement<T>(this IEnumerable<T> enumerable)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            return enumerable.ElementAt(rnd.Next() % enumerable.Count());
        }
        public static T[] Shuffle<T>(this T[] array)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);

            for (int n = array.Length; n > 1;)
            {
                int k = rnd.Next(n);
                --n;
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
            return array;
        }
        public static int NumberOfSmalltalk(this string str)
        {
            string num = str.Substring(str.IndexOf('.') - 2, 2);
            return int.Parse(num[0] == '0' ? num[1].ToString() : num);
        }
    }
    
}