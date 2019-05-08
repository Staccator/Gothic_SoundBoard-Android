using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using static Gothic_Soundboard.DialogDataManager;

namespace Gothic_Soundboard
{
    [Activity(Label = "CharacterList", Theme = "@android:style/Theme.Material.Light.NoActionBar")]
    public class CharacterList : Activity
    {
        ListView CharacterListView;
        CharacterListAdapter adapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.char_list_layout);
            CharacterListView = FindViewById<ListView>(Resource.Id.listView_characters);
            adapter = new CharacterListAdapter(this);
            CharacterListView.Adapter = adapter;
            CharacterListView.ItemClick += CharacterList_ItemClick;
        }

        private void CharacterList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Console.WriteLine(e.Position);
            string character_name = Characters[e.Position];
            Intent CharacterIntent = new Intent(this, typeof(DialogList));
            CharacterIntent.PutExtra("character_name", character_name);
            StartActivity(CharacterIntent);
        }
    }

    public class CharacterListAdapter : BaseAdapter<string>
    {
        private Activity context;

        public CharacterListAdapter(Activity context)
        {
            this.context = context;
        }

        public override string this[int position] { get { return Characters[position]; } }
        public override int Count { get { return Characters.Count; } }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view is null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.CharacterLayout, null);
            }
            view.FindViewById<TextView>(Resource.Id.textView_speaker).Text = Characters[position];
            view.FindViewById<TextView>(Resource.Id.textView_speaker).SetTypeface(QuickSand, TypefaceStyle.Normal);
            return view;
        }
    }
}