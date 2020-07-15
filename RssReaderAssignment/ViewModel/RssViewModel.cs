using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RssReaderAssignment.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RssReaderAssignment.ViewModel
{
    public class RssViewModel :ViewModelBase
    {
        //using MVVMlight nuget
        public ICommand AddLink { get; private set; }

        public ICommand RemoveLink { get; private set; }

        public ICommand OpenLink { get; private set; }

        public string SearchString
        {
            get { return searchString; }
            set
            {
                if (searchString != value)
                {
                    //searches for matching string in the rssitems collection - NOTE: selected listbox item will be in bold when selected /found search
                    searchString = value;
                    SelectedRss = RssItems.FirstOrDefault(_ => !string.IsNullOrEmpty(_.Title) && _.Title.IndexOf(searchString, StringComparison.CurrentCultureIgnoreCase) >= 0);
                    RaisePropertyChanged("SearchString");
                }
            }
        }
        private string searchString;

        private Repository repo;
        private ObservableCollection<RssItem> rssItems;
        private ObservableCollection<RssListItem> rssListItems;
        private ObservableCollection<RssItem> rssFavList;
        private RssListItem selectedListItem;
        private string name;
        private string uri;
        private RssItem selectedrss;

        public RssItem SelectedRss
        {
            get { return this.selectedrss; }
            set { this.selectedrss = value; RaisePropertyChanged("SelectedRss"); }
        }


        public string Name
        {
            get { return this.name; }
            set { this.name = value; RaisePropertyChanged("Name"); }
        }

        public string Uri
        {
            get { return this.uri; }
            set { this.uri = value; RaisePropertyChanged("Uri"); }
        }

        public RssListItem SelectedListItem
        {
            get { return this.selectedListItem; }
            set { this.selectedListItem = value; RssItems = GetSelectedRssFeed(); }
        }

        public RssItem SelectedRssListItem
        {
            get { return this.selectedrss; }
            set { this.selectedrss = value; GetSelectedRssFavFeed(this.RssFavList); }
        }

        public ObservableCollection<RssItem> RssItems
        {
            get { return this.rssItems; }
            set { this.rssItems = value; RaisePropertyChanged("RssItems"); }
        }

        public ObservableCollection<RssListItem> RssListItems
        {
            get { return this.rssListItems; }
            set { this.rssListItems = value; RaisePropertyChanged("RssListItems"); }
        }

        public ObservableCollection<RssItem> RssFavList
        {
            get { return this.rssFavList; }
            set { this.rssFavList = value; RaisePropertyChanged("RssFavList"); }
        }

        public RssViewModel()
        {
            //initialize objects 
            repo = new Repository();
            RssListItems = repo.RssList;
            rssFavList = repo.RssFavList;

            AddLink = new RelayCommand(AddLinkToList);
            RemoveLink = new RelayCommand(RemoveLinkFromList);
            OpenLink = new RelayCommand(OpenLinkArticle);
          
        }

        public ObservableCollection<RssItem> GetSelectedRssFeed()
        {
            if (SelectedListItem == null)
            {
                RssItems.Clear();
                return RssItems;
            }

            return repo.GetRssFeeds(SelectedListItem.Uri);
        }

        public ObservableCollection<RssItem> GetSelectedRssFavFeed(ObservableCollection<RssItem> rssFavList)
        {
            if (rssFavList == null)
            {
                RssItems.Clear();
                return RssItems;
            }
            RssItems = repo.GetRssFavFeeds(rssFavList);
            return RssItems;
        }

        public void AddLinkToList()
        {
            repo.AddLinkToList(SelectedRss);
        }

        private void RemoveLinkFromList()
        {
            repo.RemoveLinkFromList(SelectedRss);
        }

        public void OpenLinkArticle()
        {
            System.Diagnostics.Process.Start(SelectedRss.Link.ToString());
        }
    }
}
