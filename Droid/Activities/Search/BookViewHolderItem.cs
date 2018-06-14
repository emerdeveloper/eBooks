namespace eBooks.Droid.Activities.Search
{
    using System;
    using Android.Support.V7.Widget;
    using Android.Views;
    using Android.Widget;

    public class BookViewHolderItem : RecyclerView.ViewHolder
    {
        #region Attributes
        public readonly TextView TitleBook;
        public readonly TextView SubTitleBook;
        public readonly ImageView PictureBook;
        #endregion

        #region Events
        Action<int> listener;
        #endregion

        #region Methods
        public BookViewHolderItem(View itemView, Action<int> listener) : base(itemView)
        {
            TitleBook = itemView.FindViewById<TextView>(Resource.Id.TitleBook);
            SubTitleBook = itemView.FindViewById<TextView>(Resource.Id.SubtitleBook);
            PictureBook = itemView.FindViewById<ImageView>(Resource.Id.PictureBook);

            this.listener = listener;

            itemView.Click += OnClick;
        }

        private void OnClick(object sender, EventArgs e)
        {
            int position = base.AdapterPosition;

            if (position == RecyclerView.NoPosition)
                return;

            listener(position);
        }
        #endregion
    }
}
