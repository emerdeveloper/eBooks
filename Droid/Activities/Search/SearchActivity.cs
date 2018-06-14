namespace eBooks.Droid.Activities.Search
{
    using Android.App;
    using Android.OS;
    using Android.Runtime;
    using Android.Support.V7.App;
    using SearchView = Android.Support.V7.Widget.SearchView;
    using Android.Views;
    using Android.Widget;
    using System;
    using eBooks.Services;
    using eBooks.Models;
    using System.Collections.Generic;
    using Android.Support.V7.Widget;
    using Android.Views.InputMethods;
    using Android.Content;
    using eBooks.Utilities.Enums;
    using eBooks.Droid.Activities.Detail;
    using Newtonsoft.Json;

    [Activity(Label = Constants.Activity.Books, MainLauncher = true, Icon = "@drawable/ic_logo", Theme = "@style/Theme.AppCompat.Light")]
    public class SearchActivity : AppCompatActivity
    {
        #region Properties
        private ApiConsumer ApiService;
        private CheckConnection CheckConnection;
        private List<Book> books;
        #endregion

        #region Widgets
        private SearchView searchView;
        private RecyclerView bookListView;
        private LinearLayout contentMessageLayout;
        private ImageView statusImageView;
        private TextView messagetextView;
        private LinearLayout progressBar;
        #endregion

        #region Constructor
        public SearchActivity()
        {
            ApiService = new ApiConsumer();
            CheckConnection = new CheckConnection();
        }
        #endregion


        #region Methods
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            InitControls();
            CreateStatusInitial();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.main, menu);
            var item = menu.FindItem(Resource.Id.action_search);
            var searchItem = item.ActionView as SearchView;
            searchView = searchItem.JavaCast<SearchView>();
            searchView.QueryTextSubmit += OnSearch;
            return true;
        }

        private void OnSearch(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            searchView.ClearFocus();
            GetBooks(e.Query);
            e.Handled = true;
        }

        private async void GetBooks(string book)
        {
            CreateStatusInitSearch();

            var Connection = await CheckConnection.Check();
            if(!Connection.IsSuccess)
            {
                CreateStatusTurnNoternetConnection(Connection.Message);
                return;
            }

            var response = await ApiService.SearchBooks(book);

            if(!response.IsSuccess)
            {
                CreateStatusNoResponse(response.Message);
                return;
            }

            books = (List<Book>) response.Result;

            if(books == null || books.Count == 0)
            {
                CreateStatusBookNotFound(book);
                return;
            }

            ResponseSuccess();
        }

        private void CreateStatusInitial()
        {
            progressBar.Visibility = Android.Views.ViewStates.Gone;
            bookListView.Visibility = Android.Views.ViewStates.Gone;
            contentMessageLayout.Visibility = Android.Views.ViewStates.Visible;
            statusImageView.SetImageResource(Resource.Drawable.ic_logo);
            messagetextView.Text = Constants.Messages.Initial;
        }

        private void CreateStatusTurnNoternetConnection(string message)
        {
            progressBar.Visibility = Android.Views.ViewStates.Gone;
            bookListView.Visibility = Android.Views.ViewStates.Gone;
            contentMessageLayout.Visibility = Android.Views.ViewStates.Visible;
            statusImageView.SetImageResource(Resource.Drawable.ic_offline);
            messagetextView.Text = message;
        }

        private void CreateStatusBookNotFound(string book)
        {
            progressBar.Visibility = Android.Views.ViewStates.Gone;
            bookListView.Visibility = Android.Views.ViewStates.Gone;
            contentMessageLayout.Visibility = Android.Views.ViewStates.Visible;
            statusImageView.SetImageResource(Resource.Drawable.ic_NoData);
            messagetextView.Text = string.Format(Constants.Messages.NotFound, book);
        }

        private void CreateStatusNoResponse(string message)
        {
            progressBar.Visibility = Android.Views.ViewStates.Gone;
            bookListView.Visibility = Android.Views.ViewStates.Gone;
            contentMessageLayout.Visibility = Android.Views.ViewStates.Visible;
            statusImageView.SetImageResource(Resource.Drawable.ic_noResponse);
            messagetextView.Text = message;
        }

        private void CreateStatusInitSearch()
        {
            progressBar.Visibility = Android.Views.ViewStates.Visible;
            contentMessageLayout.Visibility = Android.Views.ViewStates.Gone;
            bookListView.Visibility = Android.Views.ViewStates.Gone;
        }

        private void ResponseSuccess()
        {
            progressBar.Visibility = Android.Views.ViewStates.Gone;
            contentMessageLayout.Visibility = Android.Views.ViewStates.Gone;
            bookListView.Visibility = Android.Views.ViewStates.Visible;
            bookListView.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Vertical, false));
            var adapter = new BookAdapter(this, books);
            adapter.ItemClick += OnItemClick;
            bookListView.SetAdapter(adapter);
        }

        private void OnItemClick(object sender, int position)
        {
            var intent = new Intent(this, typeof(DetailActivity));
            intent.PutExtra(Constants.Activity.Books, JsonConvert.SerializeObject(books[position]));
            StartActivity(intent);
        }

        private void InitControls()
        {
            bookListView = FindViewById<RecyclerView>(Resource.Id.booksReciclerView);
            contentMessageLayout = FindViewById<LinearLayout>(Resource.Id.ContentMessageLayout);
            statusImageView = FindViewById<ImageView>(Resource.Id.StatusImageView);
            messagetextView = FindViewById<TextView>(Resource.Id.MessagetextView);
            progressBar = FindViewById<LinearLayout>(Resource.Id.ProgressBar);
            bookListView.Visibility = Android.Views.ViewStates.Invisible;
            progressBar.Visibility = Android.Views.ViewStates.Gone;
        }
        #endregion
    }
}
