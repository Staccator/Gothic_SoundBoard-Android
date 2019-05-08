using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using static Gothic_Soundboard.DialogDataManager;

namespace Gothic_Soundboard
{
    [Activity(Label = "CharacterList", Theme = "@android:style/Theme.Material.Light.NoActionBar")]
    public class SearchList : Activity
    {
        ListView SearchListView;
        SearchListAdapter adapter;
        SearchView searchView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.search_list_layout);
            SearchListView = FindViewById<ListView>(Resource.Id.listView_Search);
            searchView = FindViewById<SearchView>(Resource.Id.searchView1);
            adapter = new SearchListAdapter(this);
            SearchListView.Adapter = adapter;
            SearchListView.ItemClick += SearchList_ItemClick;
            SearchListView.ItemLongClick += SearchListView_ItemLongClick;
            searchView.QueryTextChange += SearchView_QueryTextChange;
        }

        private void SearchView_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            string text = e.NewText;
            if (text.Length ==0 || String.IsNullOrWhiteSpace(text)) { adapter.Dialogs = new List<Dialog>(); adapter.NotifyDataSetChanged(); return; }

            adapter.SetProperDialogs(text);
        }

        MediaPlayer mp;
        private void SearchList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (mp != null)
            {
                mp.Release();
            }
            Dialog dialog = adapter.Dialogs[e.Position];
            mp = new MediaPlayer();
#pragma warning disable CS0618 // Typ lub składowa jest przestarzała
            mp.SetAudioStreamType(Android.Media.Stream.Music);
            var fd = Assets.OpenFd($"Files/{dialog.file_name}");
            mp.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
            fd.Close();
            mp.Prepare();
            mp.Start();

            
        }

        private void SearchListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            Dialog dialog = adapter.Dialogs[e.Position];
            if (!dialog.favorite)
            {
                AlertDialog.Builder alertFavorite = new AlertDialog.Builder(this);
                alertFavorite.SetCancelable(true);
                alertFavorite.SetTitle("Set as favorite?");
                alertFavorite.SetPositiveButton("Yes", delegate {
                    RunOnUiThread(delegate
                    {
                        dialog.favorite = true;
                        adapter.NotifyDataSetChanged();
                        DialogDataManager.SaveFavorites();

                        var toast = Toast.MakeText(this, "Success!", ToastLength.Short);
                        toast.SetGravity(GravityFlags.Bottom, 0, 0);
                        toast.Show();
                    });
                });
                alertFavorite.SetNeutralButton("Share", delegate {
                    RunOnUiThread(delegate{ DialogDataManager.SendFile(dialog, this); });
                });
                alertFavorite.SetNegativeButton("No", delegate { });
                alertFavorite.Show();
            }
            else
            {
                AlertDialog.Builder alertFavorite = new AlertDialog.Builder(this);
                alertFavorite.SetCancelable(true);
                alertFavorite.SetTitle("Delete from favorites?");
                alertFavorite.SetNeutralButton("Share", delegate {
                    RunOnUiThread(delegate { DialogDataManager.SendFile(dialog, this); });
                });
                alertFavorite.SetPositiveButton("Yes", delegate {
                    RunOnUiThread(delegate
                    {
                        dialog.favorite = false;
                        adapter.NotifyDataSetChanged();
                        DialogDataManager.SaveFavorites();

                        var toast = Toast.MakeText(this, "Success!", ToastLength.Short);
                        toast.SetGravity(GravityFlags.Bottom, 0, 0);
                        toast.Show();
                    });
                });
                alertFavorite.SetNegativeButton("No", delegate { });
                alertFavorite.Show();
            }
        }
        public override void OnBackPressed()
        {
            this.Finish();
            base.OnBackPressed();
            if (mp != null)
            {
                mp.Pause();
                mp.Release();
            }
        }
    }
}