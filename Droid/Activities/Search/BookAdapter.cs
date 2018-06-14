namespace eBooks.Droid.Activities.Search
{
    using System;
    using System.Collections.Generic;
    using Android.App;
    using Android.Support.V7.Widget;
    using Android.Views;
    using eBooks.Models;
    using Square.Picasso;

    public class BookAdapter : RecyclerView.Adapter
    {
        #region Attributes
        readonly List<Book> book;
        readonly Activity activity;
        #endregion

        #region Constructor
        public BookAdapter(Activity activity, List<Book> book)
        {
            this.activity = activity;
            this.book = book;
        }
        #endregion

        #region Events
        public event EventHandler<int> ItemClick;
        #endregion

        #region Methods
        public override int ItemCount
        {
            get { return book.Count; }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View layout = activity.LayoutInflater.Inflate(Resource.Layout.ItemBook, parent, false);
            return new BookViewHolderItem(layout, OnItemClick);
        }

        private Book GetItem(int position)
        {
            return book[position];
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            Book currentItem = GetItem(position);
            BookViewHolderItem VHitem = (BookViewHolderItem)holder;

            VHitem.TitleBook.Text = currentItem.Title;
            VHitem.SubTitleBook.Text = currentItem.SubTitle;
            Picasso.With(activity).Load(currentItem.Image).Into(VHitem.PictureBook);
        }

        void OnItemClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }
        #endregion
    }
}
