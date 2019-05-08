using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Gothic_Soundboard
{
    [Activity(Label = "DialogList", Theme = "@android:style/Theme.Material.Light.NoActionBar")]
    public class DialogList : Activity
    {
        ListView DialogsListView;
        DialogListAdapter adapter;
        string character_name = "THORUS";
        bool isFavoritesPanel= false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.dialog_list_layout);
            DialogsListView = FindViewById<ListView>(Resource.Id.listView_dialog);
            if (Intent.HasExtra("character_name"))
            {
                character_name = Intent.GetStringExtra("character_name");
                adapter = new DialogListAdapter(this, character_name);
            }
            else if (Intent.HasExtra("favorites"))
            {
                isFavoritesPanel = true;
                adapter = new DialogListAdapter(this, null,null,true);
            }
            else
            {
                adapter = new DialogListAdapter(this, character_name);
            }

            
            DialogsListView.Adapter = adapter;
            DialogsListView.ItemClick += DialogsListView_ItemClick;
            DialogsListView.ItemLongClick += DialogsListView_ItemLongClick;
        }

        private void DialogsListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            Dialog dialog = adapter.Dialogs[e.Position];
            if (!dialog.favorite)
            {
            AlertDialog.Builder alertFavorite = new AlertDialog.Builder(this);
            alertFavorite.SetCancelable(true);
            alertFavorite.SetTitle("Set as favorite?" ) ;
            alertFavorite.SetNeutralButton("Share", delegate {
                RunOnUiThread(delegate { DialogDataManager.SendFile(dialog, this); });
            });
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
            alertFavorite.SetNegativeButton("No", delegate { });
            alertFavorite.Show();
            }
            else
            {
            AlertDialog.Builder alertFavorite = new AlertDialog.Builder(this);
            alertFavorite.SetCancelable(true);
            alertFavorite.SetTitle("Delete from favorites?" ) ;
            alertFavorite.SetNeutralButton("Share", delegate {
                RunOnUiThread(delegate { DialogDataManager.SendFile(dialog, this); });
            });
            alertFavorite.SetPositiveButton("Yes", delegate { 
            RunOnUiThread(delegate 
            {
                dialog.favorite = false;
                DialogDataManager.SaveFavorites();
                if (isFavoritesPanel) adapter.Dialogs.Remove(dialog);
                adapter.NotifyDataSetChanged();

                var toast = Toast.MakeText(this, "Success!", ToastLength.Short);
                toast.SetGravity(GravityFlags.Bottom, 0, 0);
                toast.Show();
            });  } );
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
        MediaPlayer mp;
        private void DialogsListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if(mp!= null)
            {
                mp.Release();
            }
            Dialog dialog = adapter.Dialogs[e.Position];
            mp = new MediaPlayer();
#pragma warning disable CS0618 // Typ lub składowa jest przestarzała
            mp.SetAudioStreamType(Stream.Music);
            var fd = Assets.OpenFd($"Files/{dialog.file_name}");
            mp.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
            fd.Close();
            mp.Prepare();
            mp.Start();
        }
        
    }
}