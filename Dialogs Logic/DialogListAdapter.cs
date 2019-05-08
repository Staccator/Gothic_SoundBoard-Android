using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Gothic_Soundboard
{
    public class DialogListAdapter : BaseAdapter<Dialog>
    {
        private Activity context;
        public List<Dialog> Dialogs;

        public DialogListAdapter(Activity context, string character = null, string actor = null, bool favorites = false)
        {
            if (favorites) Dialogs = DialogDataManager.Dialogs.Where(d => d.favorite).ToList();
            else if (character != null) Dialogs = DialogDataManager.Dialogs.Where(d => d.character == character).ToList();
            this.context = context;
        }

        public override Dialog this[int position] { get { return Dialogs[position]; } }
        public override int Count { get { return Dialogs.Count; } }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if(view is null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.DialogLayout,null);
            }

            Dialog dialog = Dialogs[position];
            TextView Speaker = view.FindViewById<TextView>(Resource.Id.textView_character);
            TextView Speech = view.FindViewById<TextView>(Resource.Id.textView_dialog);
            Speech.Text = dialog.text;
            Speaker.SetTypeface(DialogDataManager.QuickSand, TypefaceStyle.Normal);
            Speech.SetTypeface(DialogDataManager.AutourOne, TypefaceStyle.Normal);
            Speaker.Text = dialog.character;
            if (dialog.favorite) Speaker.SetTextColor(Color.DarkGoldenrod);
            else Speaker.SetTextColor(Color.White);

            return view;
        }
    }


    public class Dialog
    {
        public string text;
        public string file_name;
        public string character;
        public string actor;
        public bool favorite = false;

        public Dialog(string file_name,string text, string character, string actor)
        {
            this.file_name = file_name;
            this.text = text;
            this.character = character;
            this.actor = actor;
        }
    }
}