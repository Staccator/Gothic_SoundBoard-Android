using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Graphics;
using Android.Content;
using Android.Content.PM;
using Environment = Android.OS.Environment;

namespace Gothic_Soundboard
{
    public static class DialogDataManager
    {
        public static ObservableCollection<Dialog> Dialogs;
        public static Activity context;
        public static List<string> Characters;
        public static Typeface QuickSand;
        public static Typeface Witcher;
        public static Typeface AutourOne;
        public static void LoadDataFromAssets(Activity context)
        {
            DialogDataManager.context = context;
            Dialogs = new ObservableCollection<Dialog>();
            Characters = new List<string>();
            Stream DialogData = context.Resources.Assets.Open("DialogsInfo.txt");
            StreamReader sr = new StreamReader(DialogData);
            string text = sr.ReadToEnd();
            var dialogs_info = text.Split(new char[] { '\r', '\n' },StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < dialogs_info.Length; i++)
            {
                var parts = dialogs_info[i].Split(';');
                Dialog dialog = new Dialog(parts[0], parts[1], parts[2], parts[3]);
                Dialogs.Add(dialog);
                string character = parts[2];
                if (!Characters.Contains(character)) Characters.Add(character);
            }
            QuickSand = Typeface.CreateFromAsset(context.Assets, "fonts/Quicksand-Regular.otf");
            AutourOne = Typeface.CreateFromAsset(context.Assets, "fonts/AutourOne-Regular.otf");
            Witcher = Typeface.CreateFromAsset(context.Assets, "fonts/thewitcher.ttf");
            DeleteList = new List<string>();    
            LoadFavorites();
            Dialogs.CollectionChanged += (object1, args) => {SaveFavorites(); };
        }

        private static string file_name = "favorites.txt";
        private static string file_path;
        private static List<string> DeleteList;
        public static void LoadFavorites()
        {
            file_path = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryMusic);
            if (!Directory.Exists(file_path))
            {
                Directory.CreateDirectory(file_path);
            }
            file_path = System.IO.Path.Combine(file_path, file_name);
            if (!File.Exists(file_path)) return;
            var lines = System.IO.File.ReadAllLines(file_path);
            for (int i = 0; i < Dialogs.Count; i++)
            {
                var dialog = Dialogs[i];
                if (lines.Contains(dialog.file_name)) dialog.favorite = true;
            }
        }

        public static void SaveFavorites()
        {
            var tosave = Dialogs.Where(x => x.favorite).Select(x => x.file_name);
            
            System.IO.File.WriteAllLines(file_path, tosave);
            foreach (var file in DeleteList)
            {
                File.Delete(file);
            }
        }

        public static void SendFile(Dialog dialog, Activity activity)
        {
            string fileName = $"{dialog.file_name}";

            var localFolder = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            var MyFilePath = System.IO.Path.Combine(localFolder, "Music", fileName);

            using (var streamReader = new StreamReader(activity.Assets.Open($"Files/{dialog.file_name}")))
            {
                using (var memstream = new MemoryStream())
                {
                    streamReader.BaseStream.CopyTo(memstream);
                    var bytes = memstream.ToArray();
                    //write to local storage
                    System.IO.File.WriteAllBytes(MyFilePath, bytes);
                    // MyFilePath = $"file://{localFolder}/{fileName}";
                }
            }
            Java.IO.File file = new Java.IO.File(MyFilePath);
            string str = file.Parent;
            var fileUri = Android.Support.V4.Content.FileProvider.GetUriForFile(activity, "com.companyname.Gothic_Soundboard.fileprovider", file);
            //var fileUri = Android.Net.Uri.Parse(MyFilePath);

            var intent = new Intent();
            intent.SetFlags(ActivityFlags.ClearTop);
            intent.SetFlags(ActivityFlags.NewTask);
            intent.SetAction(Intent.ActionSend);
            intent.SetType("audio/*");
            intent.PutExtra(Intent.ExtraStream, fileUri);
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            intent.AddFlags(ActivityFlags.GrantWriteUriPermission);

            IList<ResolveInfo> activities = activity.PackageManager.QueryIntentActivities(intent, 0);
            foreach (var item in activities) Console.WriteLine(item.ActivityInfo.Name);

            activity.StartActivity(Intent.CreateChooser(intent, "OPEN"));
            DeleteList.Add(MyFilePath);
            
        }
    }
}