using System;

namespace MovingScrewdriver.Web.ViewModels
{
    public class PagingViewModel
    {
        public bool HasNextPage
        {
            get { return CurrentPage * PageSize < PostsCount; }
        }

        public bool HasPrevPage
        {
            get { return CurrentPage * PageSize > PageSize * 1; }
        }

        public int CurrentPage { get; set; }
        public int PostsCount { get; set; }
        public int LastPage { get { return (int)Math.Ceiling(PostsCount / (decimal)PageSize); } }
        public bool IsFirstPage { get { return CurrentPage == 1; } }

        public virtual int PageSize 
        {
            get { return 25; }
        }
    }
}