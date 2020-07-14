using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading;

namespace FGOSBIAReloaded
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            DirectoryInfo gamedata = new DirectoryInfo(path + @"\Android\masterdata\");
            DirectoryInfo folder = new DirectoryInfo(path + @"\Android\");
            string output = "";
            if (!System.IO.Directory.Exists(gamedata.FullName))
            {
                MessageBox.Show("没有游戏数据,请先下载游戏数据(位于\"关于\"菜单栏中).", "温馨提示:", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                if (!File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSvt") ||
                    !File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSvtLimit") ||
                    !File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstCv") ||
                    !File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstIllustrator") ||
                    !File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSvtCard") ||
                    !File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDevice") ||
                    !File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSvtTreasureDevice") ||
                    !File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDeviceDetail") ||
                    !File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSkill") ||
                    !File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSkillDetail") ||
                    !File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSvtSkill"))
                {
                    MessageBox.Show("游戏数据损坏,请先下载游戏数据(位于\"关于\"菜单栏中).", "温馨提示:", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            string mstSvt = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSvt");
            JArray mstSvtArray = (JArray)JsonConvert.DeserializeObject(mstSvt);
            foreach (var svtIDtmp in mstSvtArray) //查找某个字段与值
            {
                output = output + "ID: " + ((JObject)svtIDtmp)["id"].ToString() + "    " + "名称: " + ((JObject)svtIDtmp)["name"].ToString() + "\r\n";
            }
            File.WriteAllText(path + "/SearchIDList.txt", output);
            MessageBox.Show("导出成功,文件名为 SearchIDList.txt", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
            System.Diagnostics.Process.Start(path + "/SearchIDList.txt");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Grid g = this.Content as Grid;
            UIElementCollection childrens = g.Children;
            foreach (UIElement ui in childrens)
            {
                if (ui is TextBox)
                {
                    (ui as TextBox).Text = "";
                }
            }
        }
        private void Hyperlink_Click(object sender, RoutedEventArgs e)

        {

            // open URL

            Hyperlink source = sender as Hyperlink;

            if (source != null)

            {

                System.Diagnostics.Process.Start(source.NavigateUri.ToString());

            }

        }
        private void HttpRequestData()
        {
            updatedatabutton.Dispatcher.Invoke(new Action(() => { updatedatabutton.IsEnabled = false;}));
            updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "";}));
            updatestatus.Dispatcher.Invoke(new Action(() => { updatesign.Content = "数据下载进行中,请勿退出!"; }));
            progressbar.Dispatcher.Invoke(new Action(() => {
                progressbar.Value = 0;
                progressbar.Visibility = System.Windows.Visibility.Visible;
            }));
            string path = System.IO.Directory.GetCurrentDirectory();
            DirectoryInfo gamedata = new DirectoryInfo(path + @"\Android\masterdata\");
            DirectoryInfo folder = new DirectoryInfo(path + @"\Android\");
            progressbar.Dispatcher.Invoke(new Action(() => {
                progressbar.Value = progressbar.Value + 500;
            }));
            if (!System.IO.Directory.Exists(folder.FullName))
            {
                updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "正在创建Android目录..."; }));
                System.IO.Directory.CreateDirectory(folder.FullName);
                if (File.Exists(gamedata.FullName + "webview") || File.Exists(gamedata.FullName + "raw") || File.Exists(gamedata.FullName + "assetbundle") || File.Exists(gamedata.FullName + "webview") || File.Exists(gamedata.FullName + "master"))
                {
                    FileSystemInfo[] fileinfo = folder.GetFileSystemInfos();  //返回目录中所有文件和子目录
                    foreach (FileSystemInfo i in fileinfo)
                    {
                        if (i is DirectoryInfo)            //判断是否文件夹
                        {
                            DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                            subdir.Delete(true);          //删除子目录和文件
                            updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "删除: " + subdir; }));
                            continue;
                        }
                        i.Delete();
                        updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "删除: " + i; }));

                    }
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "开始下载/更新游戏数据......"; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 500;
                    }));

                    string result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top", "appVer=2.13.2");
                    JObject res = JObject.Parse(result);
                    if (res["response"][0]["fail"]["action"] != null)
                    {
                        if (res["response"][0]["fail"]["action"].ToString() == "app_version_up")
                        {
                            string tmp = res["response"][0]["fail"]["detail"].ToString();
                            tmp = Regex.Replace(tmp, @".*新ver.：(.*)、現.*", "$1", RegexOptions.Singleline);
                            updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "当前游戏版本: " + tmp.ToString(); }));

                            result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top", "appVer=" + tmp.ToString());
                            res = JObject.Parse(result);
                        }
                        else if (res["response"][0]["fail"]["action"].ToString() == "maint")
                        {
                            string tmp = res["response"][0]["fail"]["detail"].ToString();
                            if (MessageBox.Show("游戏服务器正在维护，请在维护后下载数据. \r\n以下为服务器公告内容:\r\n\r\n『" + tmp.Replace("[00FFFF]", "").Replace("[url=", "").Replace("][u]公式サイト お知らせ[/u][/url][-]", "") + "』\r\n\r\n点击\"确定\"可打开公告页面.", "维护中", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                            {
                                Regex re = new Regex(@"(?<url>http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)");
                                MatchCollection mc = re.Matches(tmp);
                                foreach (Match m in mc)
                                {
                                    string url = m.Result("${url}");
                                    System.Diagnostics.Process.Start(url);
                                }
                            }


                            ;
                        }
                        else
                        {
                        }
                    }
                    if (!Directory.Exists(gamedata.FullName))
                        Directory.CreateDirectory(gamedata.FullName);
                    File.WriteAllText(gamedata.FullName + "raw", result);
                    File.WriteAllText(gamedata.FullName + "assetbundle", res["response"][0]["success"]["assetbundle"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "Writing file to: " + gamedata.FullName + "assetbundle"; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 38;
                    }));
                    File.WriteAllText(gamedata.FullName + "master", res["response"][0]["success"]["master"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "Writing file to: " + gamedata.FullName + "master"; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 38;
                    }));
                    File.WriteAllText(gamedata.FullName + "webview", res["response"][0]["success"]["webview"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "Writing file to: " + gamedata.FullName + "webview"; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 38;
                    }));
                    string data = File.ReadAllText(gamedata.FullName + "master");
                    if (!Directory.Exists(gamedata.FullName + "decrypted_masterdata"))
                        Directory.CreateDirectory(gamedata.FullName + "decrypted_masterdata");
                    Dictionary<string, byte[]> masterData = (Dictionary<string, byte[]>)MasterDataUnpacker.MouseGame2Unpacker(Convert.FromBase64String(data));
                    JObject job = new JObject();
                    MiniMessagePacker miniMessagePacker = new MiniMessagePacker();
                    foreach (KeyValuePair<string, byte[]> item in masterData)
                    {
                        List<object> unpackeditem = (List<object>)miniMessagePacker.Unpack(item.Value);
                        string json = JsonConvert.SerializeObject(unpackeditem, Formatting.Indented);
                        File.WriteAllText(gamedata.FullName + "decrypted_masterdata/" + item.Key, json);
                        updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "Writing file to: " + gamedata.FullName + "decrypted_masterdata/" + item.Key; }));
                        progressbar.Dispatcher.Invoke(new Action(() =>
                        {
                            progressbar.Value = progressbar.Value + 38;
                        }));
                    }
                    string data2 = File.ReadAllText(gamedata.FullName + "assetbundle");
                    Dictionary<string, object> dictionary = (Dictionary<string, object>)MasterDataUnpacker.MouseInfoMsgPack(Convert.FromBase64String(data2));
                    string str = null;
                    foreach (var a in dictionary)
                    {
                        str += a.Key + ": " + a.Value.ToString() + "\r\n";
                    }
                    File.WriteAllText(gamedata.FullName + "assetbundle.txt", str);
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "folder name: " + dictionary["folderName"].ToString(); }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 38;
                    }));
                    string data3 = File.ReadAllText(gamedata.FullName + "webview");
                    Dictionary<string, object> dictionary2 = (Dictionary<string, object>)MasterDataUnpacker.MouseGame2MsgPack(Convert.FromBase64String(data3));
                    string str2 = "baseURL: " + dictionary2["baseURL"].ToString() + "\r\n contactURL: " + dictionary2["contactURL"].ToString() + "\r\n";
                    updatestatus.Dispatcher.Invoke(new Action(() => {
                    updatestatus.Content = str2;
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 38;
                    }));
                    Dictionary<string, object> filePassInfo = (Dictionary<string, object>)dictionary2["filePass"];
                    foreach (var a in filePassInfo)
                    {
                        str += a.Key + ": " + a.Value.ToString() + "\r\n";
                    }
                    File.WriteAllText(gamedata.FullName + "webview.txt", str2);
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "Writing file to: " + gamedata.FullName + "webview.txt"; }));

                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "下载完成，可以开始解析."; }));

                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Maximum;
                    }));
                    MessageBox.Show("下载完成，可以开始解析.", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = ""; }));
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatesign.Content = "";
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                    progressbar.Visibility = System.Windows.Visibility.Hidden;
                    updatedatabutton.IsEnabled = true;
                    }));
                }
                else
                {
                    FileSystemInfo[] fileinfo = folder.GetFileSystemInfos();  //返回目录中所有文件和子目录
                    foreach (FileSystemInfo i in fileinfo)
                    {
                        if (i is DirectoryInfo)            //判断是否文件夹
                        {
                            DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                            subdir.Delete(true);          //删除子目录和文件
                            updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "删除: " + subdir; }));

                            continue;
                        }
                        i.Delete();
                        updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "删除: " + i; }));

                    }
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "开始下载/更新游戏数据......"; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 500;
                    }));

                    string result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top", "appVer=2.13.2");
                    JObject res = JObject.Parse(result);
                    if (res["response"][0]["fail"]["action"] != null)
                    {
                        if (res["response"][0]["fail"]["action"].ToString() == "app_version_up")
                        {
                            string tmp = res["response"][0]["fail"]["detail"].ToString();
                            tmp = Regex.Replace(tmp, @".*新ver.：(.*)、現.*", "$1", RegexOptions.Singleline);
                            updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "当前游戏版本: " + tmp.ToString(); }));

                            result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top", "appVer=" + tmp.ToString());
                            res = JObject.Parse(result);
                        }
                        else if (res["response"][0]["fail"]["action"].ToString() == "maint")
                        {
                            string tmp = res["response"][0]["fail"]["detail"].ToString();
                            if (MessageBox.Show("游戏服务器正在维护，请在维护后下载数据. \r\n以下为服务器公告内容:\r\n\r\n『" + tmp.Replace("[00FFFF]", "").Replace("[url=", "").Replace("][u]公式サイト お知らせ[/u][/url][-]", "") + "』\r\n\r\n点击\"确定\"可打开公告页面.", "维护中", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                            {
                                Regex re = new Regex(@"(?<url>http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)");
                                MatchCollection mc = re.Matches(tmp);
                                foreach (Match m in mc)
                                {
                                    string url = m.Result("${url}");
                                    System.Diagnostics.Process.Start(url);
                                }
                            }


                            ;
                        }
                        else
                        {
                        }
                    }
                    if (!Directory.Exists(gamedata.FullName))
                        Directory.CreateDirectory(gamedata.FullName);
                    File.WriteAllText(gamedata.FullName + "raw", result);
                    File.WriteAllText(gamedata.FullName + "assetbundle", res["response"][0]["success"]["assetbundle"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "Writing file to: " + gamedata.FullName + "assetbundle"; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 38;
                    }));
                    File.WriteAllText(gamedata.FullName + "master", res["response"][0]["success"]["master"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "Writing file to: " + gamedata.FullName + "master"; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 38;
                    }));
                    File.WriteAllText(gamedata.FullName + "webview", res["response"][0]["success"]["webview"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "Writing file to: " + gamedata.FullName + "webview"; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 38;
                    }));
                    string data = File.ReadAllText(gamedata.FullName + "master");
                    if (!Directory.Exists(gamedata.FullName + "decrypted_masterdata"))
                        Directory.CreateDirectory(gamedata.FullName + "decrypted_masterdata");
                    Dictionary<string, byte[]> masterData = (Dictionary<string, byte[]>)MasterDataUnpacker.MouseGame2Unpacker(Convert.FromBase64String(data));
                    JObject job = new JObject();
                    MiniMessagePacker miniMessagePacker = new MiniMessagePacker();
                    foreach (KeyValuePair<string, byte[]> item in masterData)
                    {
                        List<object> unpackeditem = (List<object>)miniMessagePacker.Unpack(item.Value);
                        string json = JsonConvert.SerializeObject(unpackeditem, Formatting.Indented);
                        File.WriteAllText(gamedata.FullName + "decrypted_masterdata/" + item.Key, json);
                        updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "Writing file to: " + gamedata.FullName + "decrypted_masterdata/" + item.Key; }));
                        progressbar.Dispatcher.Invoke(new Action(() =>
                        {
                            progressbar.Value = progressbar.Value + 38;
                        }));
                    }
                    string data2 = File.ReadAllText(gamedata.FullName + "assetbundle");
                    Dictionary<string, object> dictionary = (Dictionary<string, object>)MasterDataUnpacker.MouseInfoMsgPack(Convert.FromBase64String(data2));
                    string str = null;
                    foreach (var a in dictionary)
                    {
                        str += a.Key + ": " + a.Value.ToString() + "\r\n";
                    }
                    File.WriteAllText(gamedata.FullName + "assetbundle.txt", str);
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "folder name: " + dictionary["folderName"].ToString(); }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 38;
                    }));
                    string data3 = File.ReadAllText(gamedata.FullName + "webview");
                    Dictionary<string, object> dictionary2 = (Dictionary<string, object>)MasterDataUnpacker.MouseGame2MsgPack(Convert.FromBase64String(data3));
                    string str2 = "baseURL: " + dictionary2["baseURL"].ToString() + "\r\n contactURL: " + dictionary2["contactURL"].ToString() + "\r\n";
                    updatestatus.Dispatcher.Invoke(new Action(() => {
                    updatestatus.Content = str2;
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 38;
                    }));
                    Dictionary<string, object> filePassInfo = (Dictionary<string, object>)dictionary2["filePass"];
                    foreach (var a in filePassInfo)
                    {
                        str += a.Key + ": " + a.Value.ToString() + "\r\n";
                    }
                    File.WriteAllText(gamedata.FullName + "webview.txt", str2);
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "Writing file to: " + gamedata.FullName + "webview.txt"; }));

                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "下载完成，可以开始解析."; }));

                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Maximum;
                    }));
                    MessageBox.Show("下载完成，可以开始解析.", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = ""; }));
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatesign.Content = "";
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Visibility = System.Windows.Visibility.Hidden;
                        updatedatabutton.IsEnabled = true;
                    }));
                }
            }
            else
            {
                if (File.Exists(gamedata.FullName + "webview") || File.Exists(gamedata.FullName + "raw") || File.Exists(gamedata.FullName + "assetbundle") || File.Exists(gamedata.FullName + "webview") || File.Exists(gamedata.FullName + "master"))
                {
                    FileSystemInfo[] fileinfo = folder.GetFileSystemInfos();  //返回目录中所有文件和子目录
                    foreach (FileSystemInfo i in fileinfo)
                    {
                        if (i is DirectoryInfo)            //判断是否文件夹
                        {
                            DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                            subdir.Delete(true);          //删除子目录和文件
                            updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "删除: " + subdir; }));

                            continue;
                        }
                        i.Delete();
                        updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "删除: " + i; }));

                    }
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "开始下载/更新游戏数据......"; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 500;
                    }));

                    string result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top", "appVer=2.13.2");
                    JObject res = JObject.Parse(result);
                    if (res["response"][0]["fail"]["action"] != null)
                    {
                        if (res["response"][0]["fail"]["action"].ToString() == "app_version_up")
                        {
                            string tmp = res["response"][0]["fail"]["detail"].ToString();
                            tmp = Regex.Replace(tmp, @".*新ver.：(.*)、現.*", "$1", RegexOptions.Singleline);
                            updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "当前游戏版本: " + tmp.ToString(); }));

                            result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top", "appVer=" + tmp.ToString());
                            res = JObject.Parse(result);
                        }
                        else if (res["response"][0]["fail"]["action"].ToString() == "maint")
                        {
                            string tmp = res["response"][0]["fail"]["detail"].ToString();
                            if (MessageBox.Show("游戏服务器正在维护，请在维护后下载数据. \r\n以下为服务器公告内容:\r\n\r\n『" + tmp.Replace("[00FFFF]", "").Replace("[url=", "").Replace("][u]公式サイト お知らせ[/u][/url][-]", "") + "』\r\n\r\n点击\"确定\"可打开公告页面.", "维护中", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                            {
                                Regex re = new Regex(@"(?<url>http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)");
                                MatchCollection mc = re.Matches(tmp);
                                foreach (Match m in mc)
                                {
                                    string url = m.Result("${url}");
                                    System.Diagnostics.Process.Start(url);
                                }
                            }


                            ;
                        }
                        else
                        {
                        }
                    }
                    if (!Directory.Exists(gamedata.FullName))
                        Directory.CreateDirectory(gamedata.FullName);
                    File.WriteAllText(gamedata.FullName + "raw", result);
                    File.WriteAllText(gamedata.FullName + "assetbundle", res["response"][0]["success"]["assetbundle"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "Writing file to: " + gamedata.FullName + "assetbundle"; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 38;
                    }));
                    File.WriteAllText(gamedata.FullName + "master", res["response"][0]["success"]["master"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "Writing file to: " + gamedata.FullName + "master"; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 38;
                    }));
                    File.WriteAllText(gamedata.FullName + "webview", res["response"][0]["success"]["webview"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "Writing file to: " + gamedata.FullName + "webview"; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 38;
                    }));
                    string data = File.ReadAllText(gamedata.FullName + "master");
                    if (!Directory.Exists(gamedata.FullName + "decrypted_masterdata"))
                        Directory.CreateDirectory(gamedata.FullName + "decrypted_masterdata");
                    Dictionary<string, byte[]> masterData = (Dictionary<string, byte[]>)MasterDataUnpacker.MouseGame2Unpacker(Convert.FromBase64String(data));
                    JObject job = new JObject();
                    MiniMessagePacker miniMessagePacker = new MiniMessagePacker();
                    foreach (KeyValuePair<string, byte[]> item in masterData)
                    {
                        List<object> unpackeditem = (List<object>)miniMessagePacker.Unpack(item.Value);
                        string json = JsonConvert.SerializeObject(unpackeditem, Formatting.Indented);
                        File.WriteAllText(gamedata.FullName + "decrypted_masterdata/" + item.Key, json);
                        updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "Writing file to: " + gamedata.FullName + "decrypted_masterdata/" + item.Key; }));
                        progressbar.Dispatcher.Invoke(new Action(() =>
                        {
                            progressbar.Value = progressbar.Value + 38;
                        }));
                    }
                    string data2 = File.ReadAllText(gamedata.FullName + "assetbundle");
                    Dictionary<string, object> dictionary = (Dictionary<string, object>)MasterDataUnpacker.MouseInfoMsgPack(Convert.FromBase64String(data2));
                    string str = null;
                    foreach (var a in dictionary)
                    {
                        str += a.Key + ": " + a.Value.ToString() + "\r\n";
                    }
                    File.WriteAllText(gamedata.FullName + "assetbundle.txt", str);
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "folder name: " + dictionary["folderName"].ToString(); }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 38;
                    }));
                    string data3 = File.ReadAllText(gamedata.FullName + "webview");
                    Dictionary<string, object> dictionary2 = (Dictionary<string, object>)MasterDataUnpacker.MouseGame2MsgPack(Convert.FromBase64String(data3));
                    string str2 = "baseURL: " + dictionary2["baseURL"].ToString() + "\r\n contactURL: " + dictionary2["contactURL"].ToString() + "\r\n";
                    updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = str2; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 38;
                    }));
                    Dictionary<string, object> filePassInfo = (Dictionary<string, object>)dictionary2["filePass"];
                    foreach (var a in filePassInfo)
                    {
                        str += a.Key + ": " + a.Value.ToString() + "\r\n";
                    }
                    File.WriteAllText(gamedata.FullName + "webview.txt", str2);
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "Writing file to: " + gamedata.FullName + "webview.txt"; }));

                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "下载完成，可以开始解析."; }));

                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Maximum;
                    }));
                    MessageBox.Show("下载完成，可以开始解析.", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = ""; }));
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatesign.Content = "";
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Visibility = System.Windows.Visibility.Hidden;
                        updatedatabutton.IsEnabled = true;
                    }));
                }
                else
                {
                    FileSystemInfo[] fileinfo = folder.GetFileSystemInfos();  //返回目录中所有文件和子目录
                    foreach (FileSystemInfo i in fileinfo)
                    {
                        if (i is DirectoryInfo)            //判断是否文件夹
                        {
                            DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                            subdir.Delete(true);          //删除子目录和文件
                            updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "删除: " + subdir; }));

                            continue;
                        }
                        i.Delete();
                        updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "删除: " + i; }));

                    }
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "开始下载/更新游戏数据......"; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 500;
                    }));

                    string result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top", "appVer=2.13.2");
                    JObject res = JObject.Parse(result);
                    if (res["response"][0]["fail"]["action"] != null)
                    {
                        if (res["response"][0]["fail"]["action"].ToString() == "app_version_up")
                        {
                            string tmp = res["response"][0]["fail"]["detail"].ToString();
                            tmp = Regex.Replace(tmp, @".*新ver.：(.*)、現.*", "$1", RegexOptions.Singleline);
                            updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "当前游戏版本: " + tmp.ToString(); }));

                            result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top", "appVer=" + tmp.ToString());
                            res = JObject.Parse(result);
                        }
                        else if (res["response"][0]["fail"]["action"].ToString() == "maint")
                        {
                            string tmp = res["response"][0]["fail"]["detail"].ToString();
                            if (MessageBox.Show("游戏服务器正在维护，请在维护后下载数据. \r\n以下为服务器公告内容:\r\n\r\n『" + tmp.Replace("[00FFFF]", "").Replace("[url=", "").Replace("][u]公式サイト お知らせ[/u][/url][-]", "") + "』\r\n\r\n点击\"确定\"可打开公告页面.", "维护中", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                            {
                                Regex re = new Regex(@"(?<url>http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)");
                                MatchCollection mc = re.Matches(tmp);
                                foreach (Match m in mc)
                                {
                                    string url = m.Result("${url}");
                                    System.Diagnostics.Process.Start(url);
                                }
                            }


                            ;
                        }
                        else
                        {
                        }
                    }
                    if (!Directory.Exists(gamedata.FullName))
                        Directory.CreateDirectory(gamedata.FullName);
                    File.WriteAllText(gamedata.FullName + "raw", result);
                    File.WriteAllText(gamedata.FullName + "assetbundle", res["response"][0]["success"]["assetbundle"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "Writing file to: " + gamedata.FullName + "assetbundle"; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 38;
                    }));
                    File.WriteAllText(gamedata.FullName + "master", res["response"][0]["success"]["master"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "Writing file to: " + gamedata.FullName + "master"; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 38;
                    }));
                    File.WriteAllText(gamedata.FullName + "webview", res["response"][0]["success"]["webview"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "Writing file to: " + gamedata.FullName + "webview"; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 38;
                    }));
                    string data = File.ReadAllText(gamedata.FullName + "master");
                    if (!Directory.Exists(gamedata.FullName + "decrypted_masterdata"))
                        Directory.CreateDirectory(gamedata.FullName + "decrypted_masterdata");
                    Dictionary<string, byte[]> masterData = (Dictionary<string, byte[]>)MasterDataUnpacker.MouseGame2Unpacker(Convert.FromBase64String(data));
                    JObject job = new JObject();
                    MiniMessagePacker miniMessagePacker = new MiniMessagePacker();
                    foreach (KeyValuePair<string, byte[]> item in masterData)
                    {
                        List<object> unpackeditem = (List<object>)miniMessagePacker.Unpack(item.Value);
                        string json = JsonConvert.SerializeObject(unpackeditem, Formatting.Indented);
                        File.WriteAllText(gamedata.FullName + "decrypted_masterdata/" + item.Key, json);
                        updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "Writing file to: " + gamedata.FullName + "decrypted_masterdata/" + item.Key; }));
                        progressbar.Dispatcher.Invoke(new Action(() =>
                        {
                            progressbar.Value = progressbar.Value + 38;
                        }));
                    }
                    string data2 = File.ReadAllText(gamedata.FullName + "assetbundle");
                    Dictionary<string, object> dictionary = (Dictionary<string, object>)MasterDataUnpacker.MouseInfoMsgPack(Convert.FromBase64String(data2));
                    string str = null;
                    foreach (var a in dictionary)
                    {
                        str += a.Key + ": " + a.Value.ToString() + "\r\n";
                    }
                    File.WriteAllText(gamedata.FullName + "assetbundle.txt", str);
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "folder name: " + dictionary["folderName"].ToString(); }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 38;
                    }));
                    string data3 = File.ReadAllText(gamedata.FullName + "webview");
                    Dictionary<string, object> dictionary2 = (Dictionary<string, object>)MasterDataUnpacker.MouseGame2MsgPack(Convert.FromBase64String(data3));
                    string str2 = "baseURL: " + dictionary2["baseURL"].ToString() + "\r\n contactURL: " + dictionary2["contactURL"].ToString() + "\r\n";
                    updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = str2; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Value + 38;
                    }));
                    Dictionary<string, object> filePassInfo = (Dictionary<string, object>)dictionary2["filePass"];
                    foreach (var a in filePassInfo)
                    {
                        str += a.Key + ": " + a.Value.ToString() + "\r\n";
                    }
                    File.WriteAllText(gamedata.FullName + "webview.txt", str2);
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "Writing file to: " + gamedata.FullName + "webview.txt"; }));

                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = "下载完成，可以开始解析."; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Value = progressbar.Maximum;
                    }));
                    MessageBox.Show("下载完成，可以开始解析.", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
                    updatestatus.Dispatcher.Invoke(new Action(() => {updatestatus.Content = ""; }));
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatesign.Content = "";
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Visibility = System.Windows.Visibility.Hidden;
                        updatedatabutton.IsEnabled = true;
                    }));
                }
            }
        }
            private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Thread HTTPReq = new Thread(HttpRequestData);
            HTTPReq.Start();
        }
    }
}
