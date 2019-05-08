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
    public class SearchListAdapter : BaseAdapter<Dialog>
    {
        private Activity context;
        public List<Dialog> Dialogs;

        public SearchListAdapter(Activity context)
        {
            this.Dialogs = new List<Dialog>();
            this.context = context;
        }

        public override Dialog this[int position] { get { return Dialogs[position]; } }
        public override int Count { get { return Dialogs.Count; } }
        public override long GetItemId(int position) { return position; }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view is null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.DialogLayout, null);
            }

            Dialog dialog = Dialogs[position];
            TextView Speaker = view.FindViewById<TextView>(Resource.Id.textView_character);
            TextView Speech = view.FindViewById<TextView>(Resource.Id.textView_dialog);
            Speaker.SetTypeface(DialogDataManager.QuickSand, TypefaceStyle.Normal);
            Speech.SetTypeface(DialogDataManager.AutourOne, TypefaceStyle.Normal);
            Speech.Text = dialog.text;
            Speaker.Text = dialog.character;
            if (dialog.favorite) Speaker.SetTextColor(Color.DarkGoldenrod);
            else Speaker.SetTextColor(Color.White);

            return view;
        }

        public void SetProperDialogs(string part)
        {
            part = part.ToLower();
            Dialogs = (from dialog in DialogDataManager.Dialogs
                       let text = dialog.text.ToLower()
                       where text.Contains(part)
                       select dialog).ToList();

            this.NotifyDataSetChanged();
        }

    }

}