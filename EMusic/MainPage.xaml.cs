using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace EMusic
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                var state = "VisualState000";
                size = 0;
                if(on==1)
                    state = "VisualState001";
                if (e.NewSize.Width > 600 && e.NewSize.Width <= 800)
                {
                    state = "VisualState600";
                    size = 600;
                }
                else if (e.NewSize.Width > 800)
                {
                    state = "VisualState800";
                    size = 800;
                }
                VisualStateManager.GoToState(this, state, true);
            };
            var view = ApplicationView.GetForCurrentView();
            view.TitleBar.BackgroundColor = Color.FromArgb(255, 88, 88, 88);
            view.TitleBar.ButtonBackgroundColor = Color.FromArgb(255, 88, 88, 88);
            view.TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(255, 88, 88, 88);
            view.TitleBar.ButtonPressedBackgroundColor = Color.FromArgb(255, 66, 66, 66);
        }
        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    if (e.NavigationMode == NavigationMode.New)
        //    {
        //        searchFrame.Navigate(typeof(SearchPage));
        //    }
        //    base.OnNavigatedTo(e);
        //}
        public string topid = "23";
        public int page = 0;
        public int choose = 0;
        public int next = 0;
        public int size = 0;
        public int on = 0;
        public class Album
        {
            public string albumid { get; set; }
            public string albumpic_big { get; set; }
            public string albumpic_small { get; set; }
            public string downUrl { get; set; }
            public string singername { get; set; }
            public string songname { get; set; }
            public string url { get; set; }
            public string songid { get; set; }
            public string m4a { get; set; }
            public Album()
            {
                albumid = "";
                albumpic_big = "";
                albumpic_small = "";
                downUrl = "";
                singername = "";
                songname = "";
                url = "";
                songid = "";
                m4a = "";
            }
        }
        public Album chooseitem = new Album();
        ObservableCollection<Album> Like = new ObservableCollection<Album>();
        //private static String DB_NAME = "Love.db";
        //private static String TABLE_NAME = "Song";
        //private static String SQL_CREATE_TABLE = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (id text,song text);";
        //private static String SQL_QUERY_VALUE = "SELECT Value FROM " + TABLE_NAME + " WHERE Key = (?);";
        //private static String SQL_INSERT = "INSERT INTO " + TABLE_NAME + " VALUES(?,?);";
        //private static String SQL_UPDATE = "UPDATE " + TABLE_NAME + " SET Value = ? WHERE Key = ?";
        //private static String SQL_DELETE = "DELETE FROM " + TABLE_NAME + " WHERE Key = ?";
        //SQLiteConnection _connection = new SQLiteConnection(DB_NAME);

        //private void CREATETable()
        //{
        //    using (var statement = _connection.Prepare(SQL_CREATE_TABLE))
        //    {
        //        statement.Step();
        //    }
        //}
        //private void INSERTTable()
        //{
        //    using (var statement = _connection.Prepare(SQL_INSERT))
        //    {
        //        statement.Bind(1, chooseitem.songid);
        //        statement.Bind(2, chooseitem.songname);
        //        statement.Step();
        //    }
        //}
        //private void DELETETable()
        //{
        //    using (var statement = _connection.Prepare(SQL_DELETE))
        //    {
        //        statement.Bind(1, chooseitem.songid);
        //        statement.Step();
        //    }
        //}
        //private void UPDATETable()
        //{
        //    using (var statement = _connection.Prepare(SQL_UPDATE))
        //    {
        //        statement.Bind(1, chooseitem.songname);
        //        statement.Bind(2, chooseitem.songid);
        //        statement.Step();
        //    }
        //}
        //private void QUERYTable()
        //{
        //    using (var statement = _connection.Prepare(SQL_QUERY_VALUE))
        //    {
        //        statement.Bind(1, chooseitem.songname);
        //        SQLiteResult result = statement.Step();
        //        if (SQLiteResult.ROW == result)
        //        {
        //            chooseitem.songname = statement[0] as String;
        //        }
        //    }
        //}
        private async Task<string> listHttpClient(string uri)
        {
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("showapi_appid", "19000"));
            paramList.Add(new KeyValuePair<string, string>("showapi_sign", "ed7f09d9f9b84cb7af9f70155f7418f6"));
            if (choose == 0)
                paramList.Add(new KeyValuePair<string, string>("topid", topid));
            else if (choose == 1)
            {
                paramList.Add(new KeyValuePair<string, string>("keyword", input.Text));
                paramList.Add(new KeyValuePair<string, string>("page", next.ToString()));
            }
            else if (choose == 2)
            {
                paramList.Add(new KeyValuePair<string, string>("musicid", chooseitem.songid));
            }
            string content = "";
            return await Task.Run(() =>
            {
                HttpClient httpClient = new HttpClient();
                System.Net.Http.HttpResponseMessage response;
                response = httpClient.PostAsync(new Uri(uri), new FormUrlEncodedContent(paramList)).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    content = response.Content.ReadAsStringAsync().Result;
                return content;
            });
        }

        ObservableCollection<Album> list = new ObservableCollection<Album>();
        private async void PivotItem_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                choose = 0;
                string content = await listHttpClient("http://route.showapi.com/213-4");
                JObject jsonobj = JObject.Parse(content);
                string json = jsonobj["showapi_res_code"].ToString();
                if (json == "0")
                {
                    string json1 = jsonobj["showapi_res_body"].ToString();
                    JObject result = JObject.Parse(json1);
                    string json2 = result["pagebean"].ToString();
                    JObject pa = JObject.Parse(json2);
                    string json3 = pa["songlist"].ToString();
                    list = JsonConvert.DeserializeObject<ObservableCollection<Album>>(json3);
                    bestListView.ItemsSource = list;
                }
            }
            catch
            {
                state1.Text = "亲，貌似网络有点小故障";
            }
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            on = 1;
            chooseitem = (Album)e.ClickedItem;
            if (chooseitem.url == "")
                mediaElement1.Source = new Uri(chooseitem.m4a);
            else
                mediaElement1.Source = new Uri(chooseitem.url);
            mediactrol.IsEnabled = true;
            Image bigimg = new Image();
            if (chooseitem.albumpic_big != "")
            {
                HttpClient client = new HttpClient();
                byte[] bytes = await client.GetByteArrayAsync(chooseitem.albumpic_big);
                bigimg.Source = await trans.SaveToImageSource(bytes);
                Bigimg.Children.Add(bigimg);
                none.Visibility = Visibility.Collapsed;
            }
            else
            {
                none.Visibility = Visibility.Visible;
            }
            songName.Text = chooseitem.songname;
            singerName.Text = chooseitem.singername;
            lyric.Text = "";
            getLyric();
            detailGrid.Visibility = Visibility.Visible;
            detail.Visibility = Visibility.Visible;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
        }


        private async void PivotItem_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                choose = 0;
                topid = "26";
                string content = await listHttpClient("http://route.showapi.com/213-4");
                JObject jsonobj = JObject.Parse(content);
                string json = jsonobj["showapi_res_code"].ToString();
                if (json == "0")
                {
                    string json1 = jsonobj["showapi_res_body"].ToString();
                    JObject result = JObject.Parse(json1);
                    string json2 = result["pagebean"].ToString();
                    JObject pa = JObject.Parse(json2);
                    string json3 = pa["songlist"].ToString();
                    list = JsonConvert.DeserializeObject<ObservableCollection<Album>>(json3);
                    hotListView.ItemsSource = list;
                }
            }
            catch
            {
                state2.Text = "亲，貌似网络有点小故障";
            }
        }

        private async void PivotItem_Loaded_2(object sender, RoutedEventArgs e)
        {
            try
            {
                choose = 0;
                topid = "5";
                string content = await listHttpClient("http://route.showapi.com/213-4");
                JObject jsonobj = JObject.Parse(content);
                string json = jsonobj["showapi_res_code"].ToString();
                if (json == "0")
                {
                    string json1 = jsonobj["showapi_res_body"].ToString();
                    JObject result = JObject.Parse(json1);
                    string json2 = result["pagebean"].ToString();
                    JObject pa = JObject.Parse(json2);
                    string json3 = pa["songlist"].ToString();
                    list = JsonConvert.DeserializeObject<ObservableCollection<Album>>(json3);
                    chListView.ItemsSource = list;
                }
            }
            catch
            {
                state3.Text = "亲，貌似网络有点小故障";
            }
        }

        private async void PivotItem_Loaded_3(object sender, RoutedEventArgs e)
        {
            try
            {
                choose = 0;
                topid = "6";
                string content = await listHttpClient("http://route.showapi.com/213-4");
                JObject jsonobj = JObject.Parse(content);
                string json = jsonobj["showapi_res_code"].ToString();
                if (json == "0")
                {
                    string json1 = jsonobj["showapi_res_body"].ToString();
                    JObject result = JObject.Parse(json1);
                    string json2 = result["pagebean"].ToString();
                    JObject pa = JObject.Parse(json2);
                    string json3 = pa["songlist"].ToString();
                    list = JsonConvert.DeserializeObject<ObservableCollection<Album>>(json3);
                    hkListView.ItemsSource = list;
                }
            }
            catch
            {
                state4.Text = "亲，貌似网络有点小故障";
            }
        }

        private async void PivotItem_Loaded_4(object sender, RoutedEventArgs e)
        {
            try
            {
                choose = 0;
                topid = "3";
                string content = await listHttpClient("http://route.showapi.com/213-4");
                JObject jsonobj = JObject.Parse(content);
                string json = jsonobj["showapi_res_code"].ToString();
                if (json == "0")
                {
                    string json1 = jsonobj["showapi_res_body"].ToString();
                    JObject result = JObject.Parse(json1);
                    string json2 = result["pagebean"].ToString();
                    JObject pa = JObject.Parse(json2);
                    string json3 = pa["songlist"].ToString();
                    list = JsonConvert.DeserializeObject<ObservableCollection<Album>>(json3);
                    amListView.ItemsSource = list;
                }
            }
            catch
            {
                state5.Text = "亲，貌似网络有点小故障";
            }
        }

        private async void PivotItem_Loaded_5(object sender, RoutedEventArgs e)
        {
            try
            {
                choose = 0;
                topid = "16";
                string content = await listHttpClient("http://route.showapi.com/213-4");
                JObject jsonobj = JObject.Parse(content);
                string json = jsonobj["showapi_res_code"].ToString();
                if (json == "0")
                {
                    string json1 = jsonobj["showapi_res_body"].ToString();
                    JObject result = JObject.Parse(json1);
                    string json2 = result["pagebean"].ToString();
                    JObject pa = JObject.Parse(json2);
                    string json3 = pa["songlist"].ToString();
                    list = JsonConvert.DeserializeObject<ObservableCollection<Album>>(json3);
                    hgListView.ItemsSource = list;
                }
            }
            catch
            {
                state6.Text = "亲，貌似网络有点小故障";
            }
        }

        private async void PivotItem_Loaded_6(object sender, RoutedEventArgs e)
        {
            try
            {
                choose = 0;
                topid = "17";
                string content = await listHttpClient("http://route.showapi.com/213-4");
                JObject jsonobj = JObject.Parse(content);
                string json = jsonobj["showapi_res_code"].ToString();
                if (json == "0")
                {
                    string json1 = jsonobj["showapi_res_body"].ToString();
                    JObject result = JObject.Parse(json1);
                    string json2 = result["pagebean"].ToString();
                    JObject pa = JObject.Parse(json2);
                    string json3 = pa["songlist"].ToString();
                    list = JsonConvert.DeserializeObject<ObservableCollection<Album>>(json3);
                    jpListView.ItemsSource = list;
                }
            }
            catch
            {
                state7.Text = "亲，貌似网络有点小故障";
            }
        }

        public void clickbase()
        {
            if (page != 3)
                loveGrid.Visibility = Visibility.Collapsed;
            else
                loveGrid.Visibility = Visibility.Visible;
            if (page != 2)
                searchGrid.Visibility = Visibility.Collapsed;
            else
                searchGrid.Visibility = Visibility.Visible;
            if (page != 1)
                listGrid.Visibility = Visibility.Collapsed;
            else
                listGrid.Visibility = Visibility.Visible;
            if (page != 0)
                firstGrid.Visibility = Visibility.Collapsed;
            else
                firstGrid.Visibility = Visibility.Visible;
        }

        private void toHome_Click(object sender, RoutedEventArgs e)
        {
            if (page != 0)
            {
                page = 0;
                clickbase();
            }
        }

        private void toList_Click(object sender, RoutedEventArgs e)
        {
            if (page != 1)
            {
                page = 1;
                clickbase();
            }
        }

        private void toSearch_Click(object sender, RoutedEventArgs e)
        {
            if (page != 2)
            {
                page = 2;
                clickbase();
            }
        }
        private void toLove_Click(object sender, RoutedEventArgs e)
        {
            if (page != 3)
            {
                page = 3;
                clickbase();
            }
        }

        private async void searchBtm_Click(object sender, RoutedEventArgs e)
        {
            if (input.Text != "")
            {
                next = 1;
                try
                {
                    choose = 1;
                    string content = await listHttpClient("http://route.showapi.com/213-1");
                    JObject jsonobj = JObject.Parse(content);
                    string json = jsonobj["showapi_res_code"].ToString();
                    if (json == "0")
                    {
                        string json1 = jsonobj["showapi_res_body"].ToString();
                        JObject result = JObject.Parse(json1);
                        string json2 = result["pagebean"].ToString();
                        JObject pa = JObject.Parse(json2);
                        string json3 = pa["contentlist"].ToString();
                        list = JsonConvert.DeserializeObject<ObservableCollection<Album>>(json3);
                        searchListView.ItemsSource = list;
                    }
                }
                catch
                {
                    state8.Text = "亲，貌似网络有点小故障";
                }
            }
        }
        private async void saveMusic(object sender, RoutedEventArgs e)
        {
            if (chooseitem.downUrl != "")
            {
                HttpClient client = new HttpClient();
                var savePicker = new Windows.Storage.Pickers.FileSavePicker();
                savePicker.SuggestedStartLocation =
                    Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                savePicker.FileTypeChoices.Add("可保存的格式", new List<string>() { ".mp3" });
                savePicker.SuggestedFileName = chooseitem.songname;
                Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    byte[] bytes = await client.GetByteArrayAsync(chooseitem.downUrl);
                    Windows.Storage.CachedFileManager.DeferUpdates(file);
                    await Windows.Storage.FileIO.WriteBytesAsync(file, bytes);
                    Windows.Storage.Provider.FileUpdateStatus status =
                        await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                    if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                    {
                        XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
                        XmlNodeList elements = toastXml.GetElementsByTagName("text");
                        elements[0].AppendChild(toastXml.CreateTextNode("《" + file.Name + "》已经下载好啦！"));
                        ToastNotification toast = new ToastNotification(toastXml);
                        ToastNotificationManager.CreateToastNotifier().Show(toast);
                    }
                    else
                    {
                        XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
                        XmlNodeList elements = toastXml.GetElementsByTagName("text");
                        elements[0].AppendChild(toastXml.CreateTextNode("《" + file.Name + "》并没有下载呢"));
                        ToastNotification toast = new ToastNotification(toastXml);
                        ToastNotificationManager.CreateToastNotifier().Show(toast);
                    }
                }
                else
                {
                    XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
                    XmlNodeList elements = toastXml.GetElementsByTagName("text");
                    elements[0].AppendChild(toastXml.CreateTextNode("下载取消了哦！"));
                    ToastNotification toast = new ToastNotification(toastXml);
                    ToastNotificationManager.CreateToastNotifier().Show(toast);
                }
            }
            else
            {
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
                XmlNodeList elements = toastXml.GetElementsByTagName("text");
                elements[0].AppendChild(toastXml.CreateTextNode("请选择要下载的音乐哦！"));
                ToastNotification toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
        }

        private async void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (next != 0)
            {
                next += 1;
                try
                {
                    choose = 1;
                    string content = await listHttpClient("http://route.showapi.com/213-1");
                    JObject jsonobj = JObject.Parse(content);
                    string json = jsonobj["showapi_res_code"].ToString();
                    if (json == "0")
                    {
                        string json1 = jsonobj["showapi_res_body"].ToString();
                        JObject result = JObject.Parse(json1);
                        string json2 = result["pagebean"].ToString();
                        JObject pa = JObject.Parse(json2);
                        string json3 = pa["contentlist"].ToString();
                        //var newli = new List<Album>();
                        //if (next == 2)
                        //    newli = list.Concat(JsonConvert.DeserializeObject<ObservableCollection<Album>>(json3)).ToList<Album>();
                        //else
                        //    newli.AddRange(JsonConvert.DeserializeObject<ObservableCollection<Album>>(json3));
                        list = JsonConvert.DeserializeObject<ObservableCollection<Album>>(json3);
                        searchListView.ItemsSource = list;
                    }
                }
                catch
                {
                    state8.Text = "亲，貌似网络有点小故障";
                }
            }
        }
        private async void getLyric()
        {
            try
            {
                choose = 2;
                string content = await listHttpClient("http://route.showapi.com/213-2");
                JObject jsonobj = JObject.Parse(content);
                string json = jsonobj["showapi_res_code"].ToString();
                if (json == "0")
                {
                    string json1 = jsonobj["showapi_res_body"].ToString();
                    JObject result = JObject.Parse(json1);
                    string json2 = result["lyric_txt"].ToString();
                    json2 = json2.Replace(" ", "\n");
                    for(int i=0;i<12; i++)
                    json2 = json2.Replace("\n\n", "\n");
                    lyric.Text = json2;
                }
            }
            catch
            {
                lyric.Text = "啊哦，出了点小问题";
            }
        }
        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            detail.Visibility = Visibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            on = 0;
            if (size == 0)
                detailGrid.Visibility = Visibility.Collapsed;
            chooseitem = new Album();
            e.Handled = true;
        }

        private void addLove_Click(object sender, RoutedEventArgs e)
        {
            if (chooseitem.songid != "")
            {
                //CREATETable();
                //INSERTTable();
                if (Like.Where(x => x == chooseitem).Count() == 0)
                {
                    Like.Add(chooseitem);
                    loveListView.ItemsSource = Like;
                    XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
                    XmlNodeList elements = toastXml.GetElementsByTagName("text");
                    elements[0].AppendChild(toastXml.CreateTextNode("添加《" + chooseitem.songname + "》成功！"));
                    ToastNotification toast = new ToastNotification(toastXml);
                    ToastNotificationManager.CreateToastNotifier().Show(toast);
                }
                else
                {
                    XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
                    XmlNodeList elements = toastXml.GetElementsByTagName("text");
                    elements[0].AppendChild(toastXml.CreateTextNode(chooseitem.songname + "已经添加过了！"));
                    ToastNotification toast = new ToastNotification(toastXml);
                    ToastNotificationManager.CreateToastNotifier().Show(toast);
                }
            }
            else
            {
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
                XmlNodeList elements = toastXml.GetElementsByTagName("text");
                elements[0].AppendChild(toastXml.CreateTextNode("请选择要添加的音乐哦！"));
                ToastNotification toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            if (Like.Count() == 0)
            {
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
                XmlNodeList elements = toastXml.GetElementsByTagName("text");
                elements[0].AppendChild(toastXml.CreateTextNode("噢嚯！本来就是空的"));
                ToastNotification toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
            else
            {
                Like.Clear();
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
                XmlNodeList elements = toastXml.GetElementsByTagName("text");
                elements[0].AppendChild(toastXml.CreateTextNode("清空成功！"));
                ToastNotification toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
        }
    }
}
public static class trans
{
    public static async Task<ImageSource> SaveToImageSource(this byte[] imageBuffer)
    {
        ImageSource imageSource = null;
        using (MemoryStream stream = new MemoryStream(imageBuffer))
        {
            var ras = stream.AsRandomAccessStream();
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(BitmapDecoder.JpegDecoderId, ras);
            var provider = await decoder.GetPixelDataAsync();
            byte[] buffer = provider.DetachPixelData();
            WriteableBitmap bitmap = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
            await bitmap.PixelBuffer.AsStream().WriteAsync(buffer, 0, buffer.Length);
            imageSource = bitmap;
        }
        return imageSource;
    }

}
