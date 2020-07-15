using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace RssReaderAssignment.Model
{
    public class Repository
    {
        //using simple text documents for storing data
        public readonly string Path = AppDomain.CurrentDomain.BaseDirectory + "rsslist.txt";
        public readonly string FavPath = AppDomain.CurrentDomain.BaseDirectory + "rssfavlist.txt";

        public ObservableCollection<RssItem> RssItems;
        public ObservableCollection<RssListItem> RssList;
        //separate observable for favorite items
        public ObservableCollection<RssItem> RssFavList;
        public Repository()
        {
            CheckCreateFile();
            RssList = new ObservableCollection<RssListItem>();
            RssItems = new ObservableCollection<RssItem>();
            RssFavList = new ObservableCollection<RssItem>();
            RssList = GetRssList();

            RssFavList = GetRssFavList();

        }

        //started data if none exist
        private void CheckCreateFile()
        {
            if (!File.Exists(Path))
            {
                using (StreamWriter sw = File.CreateText(Path))
                {
                    sw.WriteLine("Next at Microsoft" + ";" + "http://blogs.microsoft.com/next/feed");
                }
            }
        }

        private ObservableCollection<RssListItem> GetRssList()
        {
            var listitems = new ObservableCollection<RssListItem>();

            List<string> lines = File.ReadAllLines(Path).ToList();

            foreach (var l in lines)
            {
                string[] split = l.Split(';');

                RssListItem listitem = new RssListItem()
                {
                    Name = split[0],
                    Uri = split[1],

                };
                listitems.Add(listitem);
            }


            return listitems;
        }

        private ObservableCollection<RssItem> GetRssFavList()
        {
            var listitems = new ObservableCollection<RssItem>();

            List<string> lines = File.ReadAllLines(FavPath).ToList();

            foreach (var l in lines)
            {
                string[] split = l.Split(';');

                RssItem listitem = new RssItem()
                {
                    Title = split[0],
                    UniqueId = split[1],
                    ParentUri = split[2]

                };
                listitems.Add(listitem);
            }


            return listitems;
        }

        public ObservableCollection<RssItem> GetRssFeeds(string url)
        {
            var items = new ObservableCollection<RssItem>();
            string imgPath = "";
            //iterate over the feeds from the parent uri
            //syndicationfeed - retrieving RSS / Atom data
            using (var reader = XmlReader.Create(url))
            {
                SyndicationFeed feed = SyndicationFeed.Load(reader);

                foreach (SyndicationItem item in feed.Items)
                {

                    Regex regx = new Regex("(http|https)://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?.(?:jpeg|bmp|gif|png|img)", RegexOptions.IgnoreCase);
                    MatchCollection matches = regx.Matches(item.Summary.Text);

                    if (matches.Count > 0)
                        imgPath = matches[0].ToString();

                    matches = null;

                    string text = Regex.Replace(item.Summary.Text, @"(?></?\w+)(?>(?:[^>'""]+|'[^']*'|""[^""]*"")*)>", "");
                    string newtext = Regex.Replace(text, @"\t|\n|\r", "");

                    RssItem rssitem = new RssItem()
                    {
                        UniqueId = item.Id,
                        Title = item.Title.Text,
                        Description = newtext,
                        ImagePath = imgPath,
                        Summary = item.Summary.Text,
                        Link = item.Links.FirstOrDefault().Uri.ToString(),
                        ParentUri = url
                    };

                    imgPath = null;
                    items.Add(rssitem);
                }
            }
            return items;
        }

        public ObservableCollection<RssItem> GetRssFavFeeds(ObservableCollection<RssItem> rssItemsCol)
        {
            var items = new ObservableCollection<RssItem>();
            //iterate over the parent uri and extract the child items from each rss/category (using key exist would be a better option)
            foreach (RssItem rssitem in rssItemsCol)
            {
                string imgPath = "";

                //XmlReaderSettings settings = new XmlReaderSettings();
                //settings.DtdProcessing = DtdProcessing.Parse;
                using (var reader = XmlReader.Create(rssitem.ParentUri))
                {


                    SyndicationFeed feed = SyndicationFeed.Load(reader);

                    foreach (SyndicationItem item in feed.Items)
                    {
                        if (item.Id == rssitem.UniqueId)
                        {
                            Regex regx = new Regex("(http|https)://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?.(?:jpeg|bmp|gif|png|img)", RegexOptions.IgnoreCase);
                            MatchCollection matches = regx.Matches(item.Summary.Text);

                            if (matches.Count > 0)
                                imgPath = matches[0].ToString();

                            matches = null;

                            string text = Regex.Replace(item.Summary.Text, @"(?></?\w+)(?>(?:[^>'""]+|'[^']*'|""[^""]*"")*)>", "");
                            string newtext = Regex.Replace(text, @"\t|\n|\r", "");


                            RssItem rssitemsingle = new RssItem()
                            {
                                UniqueId = item.Id,
                                Title = item.Title.Text,
                                Description = newtext,
                                ImagePath = imgPath,
                                Summary = item.Summary.Text,
                                Link = item.Links.FirstOrDefault().Uri.ToString(),
                                ParentUri = rssitem.ParentUri
                            };


                            imgPath = null;
                            items.Add(rssitemsingle);
                        }
                    }
                }
            }
            return items;
        }

        internal void RemoveLinkFromList(RssItem selectedListItem)
        {
            File.WriteAllText(FavPath, String.Empty);

            //using remove method with query since the object passed does not match the object in the obscollection
            RssFavList.Remove(RssFavList.Where(i => i.Title == selectedListItem.Title).Single());
            if (File.Exists(FavPath))
            {
                using (StreamWriter sw = File.CreateText(FavPath))
                {
                    foreach (var i in RssFavList)
                    {
                        sw.WriteLine(i.Title + ";" + i.UniqueId + ";" + i.ParentUri);
                    }
                }
            }
        }

        internal void AddLinkToList(RssItem newItem)
        {
            File.WriteAllText(FavPath, String.Empty);

            RssFavList.Add(newItem);
            if (File.Exists(FavPath))
            {
                using (StreamWriter sw = File.CreateText(FavPath))
                {
                    foreach (var i in RssFavList)
                    {
                        sw.WriteLine(i.Title + ";" + i.UniqueId + ";" + i.ParentUri);
                    }
                }
            }

        }
    }
}
