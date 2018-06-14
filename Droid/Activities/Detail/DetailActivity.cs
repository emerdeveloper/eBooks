namespace eBooks.Droid.Activities.Detail
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Runtime;
    using Android.Support.V7.App;
    using Android.Views;
    using Android.Widget;
    using eBooks.Models;
    using eBooks.Utilities.Enums;
    using Newtonsoft.Json;
    using Square.Picasso;
    using Toolbar = Android.Support.V7.Widget.Toolbar;

    [Activity(Label = Constants.Activity.Books, MainLauncher = false, Icon = "@drawable/ic_logo", Theme = "@style/MyTheme")]
    public class DetailActivity : AppCompatActivity
    {
        #region Properties
        public Book Book { get; set; }
        #endregion

        #region Widget
        private Toolbar Toolbar;
        public TextView CardTitleBook;
        public TextView CardSubTitleBook;
        public TextView CardDescription;
        public ImageView CardPictureBook;
        #endregion

        #region Methods
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Detail);
            Book = JsonConvert.DeserializeObject<Book>(this.Intent.GetStringExtra(Constants.Activity.Books));


            InitControls();

            SetSupportActionBar(Toolbar);
            SupportActionBar.Title = Constants.Activity.Books;
            //SupportActionBar.SetIcon(Resource.Drawable.ic_logo);

            SetData();
        }

		public override void OnBackPressed()
		{
            this.Finish();
		}

		private void SetData()
        {
            CardTitleBook.Text = Book.Title;
            CardSubTitleBook.Text = Book.SubTitle;
            CardDescription.Text = Book.Description;
            Picasso.With(this).Load(Book.Image).Into(CardPictureBook);
        }

        private void InitControls()
        {
            Toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            CardTitleBook = FindViewById<TextView>(Resource.Id.CardTitleBook);
            CardSubTitleBook = FindViewById<TextView>(Resource.Id.CardSubtitleBook);
            CardDescription = FindViewById<TextView>(Resource.Id.CardDescription);
            CardPictureBook = FindViewById<ImageView>(Resource.Id.CardImage);
        }
        #endregion
    }
}
