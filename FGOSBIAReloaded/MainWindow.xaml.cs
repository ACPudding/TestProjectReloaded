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
using FGOSBIAReloaded.Properties;

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
            var childref2 = new ThreadStart(AnalyzeServant);
            var childThread2 = new Thread(childref2);
            childThread2.Start();
        }

        public void AnalyzeServant()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                Button1.IsEnabled = false;
                var path = Directory.GetCurrentDirectory();
                var gamedata = new DirectoryInfo(path + @"\Android\masterdata\");
                var folder = new DirectoryInfo(path + @"\Android\");
                textbox1.Text = Regex.Replace(textbox1.Text, @"\s", "");
                var svtID = "";
                svtID = Convert.ToString(textbox1.Text);
                JB.svtid = svtID;
                JB.JB1 = "";
                JB.JB2 = "";
                JB.JB3 = "";
                JB.JB4 = "";
                JB.JB5 = "";
                JB.JB6 = "";
                JB.JB7 = "";
                if (!Directory.Exists(gamedata.FullName))
                {
                    MessageBox.Show("没有游戏数据,请先下载游戏数据(位于\"关于\"选项卡).", "温馨提示:", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    Button1.IsEnabled = true;
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
                        !File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSvtSkill") ||
                        !File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstFunc"))
                    {
                        MessageBox.Show("游戏数据损坏,请重新下载游戏数据(位于\"关于\"选项卡).", "温馨提示:", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        Button1.IsEnabled = true;
                        return;
                    }
                }

                if (!Regex.IsMatch(svtID, "^\\d+$"))
                {
                    MessageBox.Show("从者ID输入错误,请检查.", "温馨提示:", MessageBoxButton.OK, MessageBoxImage.Error);
                    ClearTexts();
                    Button1.IsEnabled = true;
                    return;
                }

                if (!File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSvtComment"))
                {
                    MessageBox.Show("游戏数据损坏,请重新下载游戏数据(位于\"关于\"选项卡).", "温馨提示:", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                ClearTexts();
                textbox1.Text = svtID;
                var mstSvt = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSvt");
                var mstSvtLimit = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSvtLimit");
                var mstCv = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstCv");
                var mstIllustrator = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstIllustrator");
                var mstSvtCard = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSvtCard");
                var mstSvtTreasureDevice =
                    File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSvtTreasureDevice");
                var mstTreasureDevice =
                    File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDevice");
                var mstTreasureDeviceDetail =
                    File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDeviceDetail");
                var mstSkill = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSkill");
                var mstSvtSkill = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSvtSkill");
                var mstSkillDetail = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSkillDetail");
                var mstSvtArray = (JArray) JsonConvert.DeserializeObject(mstSvt);
                var mstSvtLimitArray = (JArray) JsonConvert.DeserializeObject(mstSvtLimit);
                var mstCvArray = (JArray) JsonConvert.DeserializeObject(mstCv);
                var mstIllustratorArray = (JArray) JsonConvert.DeserializeObject(mstIllustrator);
                var mstSvtCardArray = (JArray) JsonConvert.DeserializeObject(mstSvtCard);
                var mstSvtTreasureDevicedArray = (JArray) JsonConvert.DeserializeObject(mstSvtTreasureDevice);
                var mstTreasureDevicedArray = (JArray) JsonConvert.DeserializeObject(mstTreasureDevice);
                var mstTreasureDeviceDetailArray = (JArray) JsonConvert.DeserializeObject(mstTreasureDeviceDetail);
                var mstSkillArray = (JArray) JsonConvert.DeserializeObject(mstSkill);
                var mstSvtSkillArray = (JArray) JsonConvert.DeserializeObject(mstSvtSkill);
                var mstSkillDetailArray = (JArray) JsonConvert.DeserializeObject(mstSkillDetail);
                var mstFunc = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstFunc");
                var mstFuncArray = (JArray)JsonConvert.DeserializeObject(mstFunc);
                if (!File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDeviceLv"))
                {
                    MessageBox.Show("游戏数据损坏,请重新下载游戏数据(位于\"关于\"选项卡).", "温馨提示:", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                if (!File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSkillLv"))
                {
                    MessageBox.Show("游戏数据损坏,请重新下载游戏数据(位于\"关于\"选项卡).", "温馨提示:", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                var mstTreasureDeviceLv =
                    File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDeviceLv");
                var mstTreasureDeviceLvArray = (JArray) JsonConvert.DeserializeObject(mstTreasureDeviceLv);
                var PPK = new string[100];
                PPK[11] = "A";
                PPK[12] = "A+";
                PPK[13] = "A++";
                PPK[14] = "A-";
                PPK[15] = "A+++";
                PPK[21] = "B";
                PPK[22] = "B+";
                PPK[23] = "B++";
                PPK[24] = "B-";
                PPK[25] = "B+++";
                PPK[31] = "C";
                PPK[32] = "C+";
                PPK[33] = "C++";
                PPK[34] = "C-";
                PPK[35] = "C+++";
                PPK[41] = "D";
                PPK[42] = "D+";
                PPK[43] = "D++";
                PPK[44] = "D-";
                PPK[45] = "D+++";
                PPK[51] = "E";
                PPK[52] = "E+";
                PPK[53] = "E++";
                PPK[54] = "E-";
                PPK[55] = "E+++";
                PPK[61] = "EX";
                PPK[98] = "-";
                PPK[0] = "-";
                PPK[99] = "?";
                var svtName = "";
                var svtNameDisplay = "unknown";
                var ClassName = new string[1500];
                ClassName[1] = "Saber";
                ClassName[2] = "Archer";
                ClassName[3] = "Lancer";
                ClassName[4] = "Rider";
                ClassName[5] = "Caster";
                ClassName[6] = "Assassin";
                ClassName[7] = "Berserker";
                ClassName[8] = "Shielder";
                ClassName[9] = "Ruler";
                ClassName[10] = "Alterego";
                ClassName[11] = "Avenger";
                ClassName[23] = "MoonCancer";
                ClassName[25] = "Foreigner";
                ClassName[20] = "Beast II";
                ClassName[22] = "Beast I";
                ClassName[24] = "Beast III/R";
                ClassName[26] = "Beast III/L";
                ClassName[27] = "Beast ?";
                ClassName[97] = "不明";
                ClassName[1001] = "礼装";
                ClassName[107] = "Berserker";
                ClassName[21] = "?";
                ClassName[19] = "?";
                ClassName[18] = "?";
                ClassName[17] = "Grand Caster";
                ClassName[16] = "?";
                ClassName[15] = "?";
                ClassName[14] = "?";
                ClassName[13] = "?";
                ClassName[12] = "?";
                SkillLvs.TDFuncstr = "";
                var svtClass = "unknown"; //ClassID
                var svtgender = "unknown";
                var gender = new string[4];
                gender[1] = "男性";
                gender[2] = "女性";
                gender[3] = "其他";
                var nprateclassbase = new double[150];
                nprateclassbase[1] = 1.5;
                nprateclassbase[2] = 1.55;
                nprateclassbase[3] = 1.45;
                nprateclassbase[4] = 1.55;
                nprateclassbase[5] = 1.6;
                nprateclassbase[6] = 1.45;
                nprateclassbase[7] = 1.4;
                nprateclassbase[8] = 1.5;
                nprateclassbase[9] = 1.5;
                nprateclassbase[10] = 1.55;
                nprateclassbase[11] = 1.45;
                nprateclassbase[23] = 1.6;
                nprateclassbase[25] = 1.5;
                nprateclassbase[20] = 0.0;
                nprateclassbase[22] = 0.0;
                nprateclassbase[24] = 0.0;
                nprateclassbase[26] = 0.0;
                nprateclassbase[27] = 0.0;
                nprateclassbase[97] = 0.0;
                nprateclassbase[107] = 0.0;
                nprateclassbase[21] = 0.0;
                nprateclassbase[19] = 0.0;
                nprateclassbase[18] = 0.0;
                nprateclassbase[17] = 0.0;
                nprateclassbase[16] = 0.0;
                nprateclassbase[15] = 0.0;
                nprateclassbase[14] = 0.0;
                nprateclassbase[13] = 0.0;
                nprateclassbase[12] = 0.0;
                var nprateartscount = new double[4];
                nprateartscount[1] = 1.5;
                nprateartscount[2] = 1.125;
                nprateartscount[3] = 1;
                var npratemagicbase = new double[100];
                npratemagicbase[11] = 1.02;
                npratemagicbase[12] = 1.025;
                npratemagicbase[13] = 1.03;
                npratemagicbase[14] = 1.015;
                npratemagicbase[21] = 1;
                npratemagicbase[22] = 1.005;
                npratemagicbase[23] = 1.01;
                npratemagicbase[24] = 0.995;
                npratemagicbase[31] = 0.99;
                npratemagicbase[32] = 0.9925;
                npratemagicbase[33] = 0.995;
                npratemagicbase[34] = 0.985;
                npratemagicbase[41] = 0.98;
                npratemagicbase[42] = 0.9825;
                npratemagicbase[43] = 0.985;
                npratemagicbase[44] = 0.975;
                npratemagicbase[51] = 0.97;
                npratemagicbase[52] = 0.9725;
                npratemagicbase[53] = 0.975;
                npratemagicbase[54] = 0.965;
                npratemagicbase[61] = 1.04;
                npratemagicbase[0] = 0.0;
                npratemagicbase[99] = 0.0;
                var svtstarrate = "unknown";
                double NPrate = 0;
                float starrate = 0;
                float deathrate = 0;
                var svtdeathrate = "unknown";
                var svtillust = "unknown"; //illustID 不输出
                var svtcv = "unknown";
                ; //CVID 不输出
                var svtcollectionid = "unknown";
                var svtCVName = "unknown";
                var svtILLUSTName = "unknown";
                var svtrarity = "unknown";
                var svthpBase = "unknown";
                var svthpMax = "unknown";
                var svtatkBase = "unknown";
                var svtatkMax = "unknown";
                var svtcriticalWeight = "unknown";
                var svtpower = "unknown";
                var svtdefense = "unknown";
                var svtagility = "unknown";
                var svtmagic = "unknown";
                var svtluck = "unknown";
                var svttreasureDevice = "unknown";
                var svtTDID = "unknown";
                var svtHideAttri = "unknown";
                var CardArrange = "unknown";
                var newtmpid = "";
                var NPRateTD = 0.00;
                var NPRateArts = 0.00;
                var NPRateBuster = 0.00;
                var NPRateQuick = 0.00;
                var NPRateEX = 0.00;
                var NPRateDef = 0.00;
                string svtClassPassiveID;
                var svtClassPassiveIDArray = new string[] { };
                List<string> svtClassPassiveIDList;
                var svtClassPassiveList = new List<string> { };
                string[] svtClassPassiveArray;
                var svtClassPassive = string.Empty;
                string svtTreasureDeviceFuncID;
                var svtTreasureDeviceFuncIDArray = new string[] { };
                List<string> svtTreasureDeviceFuncIDList;
                var svtTreasureDeviceFuncList = new List<string> { };
                string[] svtTreasureDeviceFuncArray;
                var svtTreasureDeviceFunc = string.Empty;
                var svtArtsCardhit = 1;
                var svtArtsCardhitDamage = "unknown";
                var svtArtsCardQuantity = 0;
                var svtBustersCardhit = 1;
                var svtBustersCardhitDamage = "unknown";
                var svtQuicksCardhit = 1;
                var svtQuicksCardhitDamage = "unknown";
                var svtExtraCardhit = 1;
                var svtExtraCardhitDamage = "unknown";
                var svtNPCardhit = 1;
                var svtNPCardhitDamage = "-";
                var svtNPCardType = "unknown";
                var svtNPDamageType = "unknown";
                var NPName = "unknown";
                var NPruby = "unknown";
                var NPtypeText = "unknown";
                var NPrank = "unknown";
                var NPDetail = "unknown";
                var skill1Name = "unknown";
                var skill1detail = "unknown";
                var skill1ID = "unknown";
                var skill2Name = "unknown";
                var skill2detail = "unknown";
                var skill2ID = "unknown";
                var skill3Name = "unknown";
                var skill3detail = "unknown";
                var skill3ID = "unknown";
                var classData = 0;
                var powerData = 0;
                var defenseData = 0;
                var agilityData = 0;
                var magicData = 0;
                var luckData = 0;
                var TreasureData = 0;
                var genderData = 0;
                foreach (var svtIDtmp in mstSvtArray) //查找某个字段与值
                    if (((JObject) svtIDtmp)["id"].ToString() == svtID)
                    {
                        var mstSvtobjtmp = JObject.Parse(svtIDtmp.ToString());
                        svtName = mstSvtobjtmp["name"].ToString();
                        Svtname.Text = svtName;
                        JB.svtnme = svtName;
                        svtNameDisplay = mstSvtobjtmp["battleName"].ToString();
                        SvtBattlename.Text = svtNameDisplay;
                        svtClass = mstSvtobjtmp["classId"].ToString();
                        svtgender = mstSvtobjtmp["genderType"].ToString();
                        svtstarrate = mstSvtobjtmp["starRate"].ToString();
                        svtdeathrate = mstSvtobjtmp["deathRate"].ToString();
                        svtillust = mstSvtobjtmp["illustratorId"].ToString(); //illustID
                        svtcv = mstSvtobjtmp["cvId"].ToString(); //CVID
                        svtcollectionid = mstSvtobjtmp["collectionNo"].ToString();
                        collection.Text = svtcollectionid;
                        svtClassPassiveID = mstSvtobjtmp["classPassive"].ToString().Replace("\n", "").Replace("\t", "")
                            .Replace("\r", "").Replace(" ", "").Replace("[", "").Replace("]", "");
                        svtClassPassiveIDList = new List<string>(svtClassPassiveID.Split(','));
                        svtClassPassiveIDArray = svtClassPassiveIDList.ToArray();
                        svtHideAttri = mstSvtobjtmp["attri"].ToString().Replace("1", "人").Replace("2", "天")
                            .Replace("3", "地").Replace("4", "星").Replace("5", "兽");
                        hiddenattri.Text = svtHideAttri;
                        CardArrange = mstSvtobjtmp["cardIds"].ToString().Replace("\n", "").Replace("\t", "")
                            .Replace("\r", "").Replace(" ", "").Replace("2", "B").Replace("1", "A").Replace("3", "Q");
                        cards.Text = CardArrange;
                        foreach (var c in CardArrange)
                            if (c == 'A')
                                svtArtsCardQuantity++;
                        classData = int.Parse(svtClass);
                        svtclass.Text = ClassName[classData];
                        genderData = int.Parse(svtgender);
                        gendle.Text = gender[genderData];
                        starrate = float.Parse(svtstarrate) / 10;
                        ssvtstarrate.Text = starrate.ToString() + "%";
                        deathrate = float.Parse(svtdeathrate) / 10;
                        ssvtdeathrate.Text = deathrate.ToString() + "%";
                        break;
                    }

                foreach (var svtCardtmp in mstSvtCardArray) //查找某个字段与值
                {
                    if (((JObject) svtCardtmp)["svtId"].ToString() == svtID &&
                        ((JObject) svtCardtmp)["cardId"].ToString() == "1")
                    {
                        var mstSvtCardobjtmp = JObject.Parse(svtCardtmp.ToString());
                        svtArtsCardhitDamage = mstSvtCardobjtmp["normalDamage"].ToString().Replace("\n", "")
                            .Replace("\t", "").Replace("\r", "").Replace(" ", "");
                        foreach (var c in svtArtsCardhitDamage)
                            if (c == ',')
                                svtArtsCardhit++;
                        artscard.Text = svtArtsCardhit.ToString() + " hit " + svtArtsCardhitDamage;
                    }

                    if (((JObject) svtCardtmp)["svtId"].ToString() == svtID &&
                        ((JObject) svtCardtmp)["cardId"].ToString() == "2")
                    {
                        var mstSvtCardobjtmp = JObject.Parse(svtCardtmp.ToString());
                        svtBustersCardhitDamage = mstSvtCardobjtmp["normalDamage"].ToString().Replace("\n", "")
                            .Replace("\t", "").Replace("\r", "").Replace(" ", "");
                        foreach (var c in svtBustersCardhitDamage)
                            if (c == ',')
                                svtBustersCardhit++;
                        bustercard.Text = svtBustersCardhit.ToString() + " hit " + svtBustersCardhitDamage;
                    }

                    if (((JObject) svtCardtmp)["svtId"].ToString() == svtID &&
                        ((JObject) svtCardtmp)["cardId"].ToString() == "3")
                    {
                        var mstSvtCardobjtmp = JObject.Parse(svtCardtmp.ToString());
                        svtQuicksCardhitDamage = mstSvtCardobjtmp["normalDamage"].ToString().Replace("\n", "")
                            .Replace("\t", "").Replace("\r", "").Replace(" ", "");
                        foreach (var c in svtQuicksCardhitDamage)
                            if (c == ',')
                                svtQuicksCardhit++;
                        quickcard.Text = svtQuicksCardhit.ToString() + " hit " + svtQuicksCardhitDamage;
                    }

                    if (((JObject) svtCardtmp)["svtId"].ToString() == svtID &&
                        ((JObject) svtCardtmp)["cardId"].ToString() == "4")
                    {
                        var mstSvtCardobjtmp = JObject.Parse(svtCardtmp.ToString());
                        svtExtraCardhitDamage = mstSvtCardobjtmp["normalDamage"].ToString().Replace("\n", "")
                            .Replace("\t", "").Replace("\r", "").Replace(" ", "");
                        foreach (var c in svtExtraCardhitDamage)
                            if (c == ',')
                                svtExtraCardhit++;
                        extracard.Text = svtExtraCardhit.ToString() + " hit " + svtExtraCardhitDamage;
                    }
                }

                foreach (var cvidtmp in mstCvArray) //查找某个字段与值
                    if (((JObject) cvidtmp)["id"].ToString() == svtcv)
                    {
                        var mstCVobjtmp = JObject.Parse(cvidtmp.ToString());
                        svtCVName = mstCVobjtmp["name"].ToString();
                        cv.Text = svtCVName;
                        break;
                    }

                foreach (var svtskill in mstSvtSkillArray) //查找某个字段与值
                {
                    if (((JObject) svtskill)["svtId"].ToString() == svtID &&
                        ((JObject) svtskill)["num"].ToString() == "1" &&
                        ((JObject) svtskill)["priority"].ToString() == "1")
                    {
                        var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                        skill1ID = mstsvtskillobjtmp["skillId"].ToString();
                    }

                    if (((JObject) svtskill)["svtId"].ToString() == svtID &&
                        ((JObject) svtskill)["num"].ToString() == "1" &&
                        ((JObject) svtskill)["priority"].ToString() == "2")
                    {
                        var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                        skill1ID = mstsvtskillobjtmp["skillId"].ToString();
                    }

                    if (((JObject) svtskill)["svtId"].ToString() == svtID &&
                        ((JObject) svtskill)["num"].ToString() == "2" &&
                        ((JObject) svtskill)["priority"].ToString() == "1")
                    {
                        var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                        skill2ID = mstsvtskillobjtmp["skillId"].ToString();
                    }

                    if (((JObject) svtskill)["svtId"].ToString() == svtID &&
                        ((JObject) svtskill)["num"].ToString() == "2" &&
                        ((JObject) svtskill)["priority"].ToString() == "2")
                    {
                        var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                        skill2ID = mstsvtskillobjtmp["skillId"].ToString();
                    }

                    if (((JObject) svtskill)["svtId"].ToString() == svtID &&
                        ((JObject) svtskill)["num"].ToString() == "3" &&
                        ((JObject) svtskill)["priority"].ToString() == "1")
                    {
                        var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                        skill3ID = mstsvtskillobjtmp["skillId"].ToString();
                    }

                    if (((JObject) svtskill)["svtId"].ToString() == svtID &&
                        ((JObject) svtskill)["num"].ToString() == "3" &&
                        ((JObject) svtskill)["priority"].ToString() == "2")
                    {
                        var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                        skill3ID = mstsvtskillobjtmp["skillId"].ToString();
                    }
                }

                foreach (var skilltmp in mstSkillArray) //查找某个字段与值
                {
                    if (((JObject) skilltmp)["id"].ToString() == skill1ID)
                    {
                        var skillobjtmp = JObject.Parse(skilltmp.ToString());
                        skill1Name = skillobjtmp["name"].ToString();
                        skill1name.Text = skill1Name;
                    }

                    if (((JObject) skilltmp)["id"].ToString() == skill2ID)
                    {
                        var skillobjtmp = JObject.Parse(skilltmp.ToString());
                        skill2Name = skillobjtmp["name"].ToString();
                        skill2name.Text = skill2Name;
                    }

                    if (((JObject) skilltmp)["id"].ToString() == skill3ID)
                    {
                        var skillobjtmp = JObject.Parse(skilltmp.ToString());
                        skill3Name = skillobjtmp["name"].ToString();
                        skill3name.Text = skill3Name;
                    }

                    foreach (var classpassiveidtmp in svtClassPassiveIDArray)
                        if (((JObject) skilltmp)["id"].ToString() == classpassiveidtmp)
                        {
                            var mstsvtPskillobjtmp = JObject.Parse(skilltmp.ToString());
                            svtClassPassiveList.Add(mstsvtPskillobjtmp["name"].ToString());
                        }
                }

                foreach (var skillDetailtmp in mstSkillDetailArray) //查找某个字段与值
                {
                    if (((JObject) skillDetailtmp)["id"].ToString() == skill1ID)
                    {
                        var skillDetailobjtmp = JObject.Parse(skillDetailtmp.ToString());
                        skill1detail = skillDetailobjtmp["detail"].ToString().Replace("[{0}]", " [Lv.1 - Lv.10] ")
                            .Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "")
                            .Replace(@"＆", "\r\n ＋").Replace(@"＋", "\r\n ＋").Replace("\r\n \r\n", "\r\n");
                        skill1details.Text = skill1detail;
                    }

                    if (((JObject) skillDetailtmp)["id"].ToString() == skill2ID)
                    {
                        var skillDetailobjtmp = JObject.Parse(skillDetailtmp.ToString());
                        skill2detail = skillDetailobjtmp["detail"].ToString().Replace("[{0}]", " [Lv.1 - Lv.10] ")
                            .Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "")
                            .Replace(@"＆", "\r\n ＋").Replace(@"＋", "\r\n ＋").Replace("\r\n \r\n", "\r\n");
                        skill2details.Text = skill2detail;
                    }

                    if (((JObject) skillDetailtmp)["id"].ToString() == skill3ID)
                    {
                        var skillDetailobjtmp = JObject.Parse(skillDetailtmp.ToString());
                        skill3detail = skillDetailobjtmp["detail"].ToString().Replace("[{0}]", " [Lv.1 - Lv.10] ")
                            .Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "")
                            .Replace(@"＆", "\r\n ＋").Replace(@"＋", "\r\n ＋").Replace("\r\n \r\n", "\r\n");
                        skill3details.Text = skill3detail;
                    }
                }

                foreach (var svtTreasureDevicestmp in mstSvtTreasureDevicedArray) //查找某个字段与值
                {
                    if (((JObject) svtTreasureDevicestmp)["svtId"].ToString() == svtID &&
                        ((JObject) svtTreasureDevicestmp)["priority"].ToString() == "101")
                    {
                        var mstsvtTDobjtmp = JObject.Parse(svtTreasureDevicestmp.ToString());
                        svtNPCardhitDamage = mstsvtTDobjtmp["damage"].ToString().Replace("\n", "").Replace("\t", "")
                            .Replace("\r", "").Replace(" ", "");
                        svtNPCardType = mstsvtTDobjtmp["cardId"].ToString().Replace("2", "Buster").Replace("1", "Arts")
                            .Replace("3", "Quick");
                        foreach (var c in svtNPCardhitDamage)
                            if (c == ',')
                                svtNPCardhit++;
                        svtTDID = mstsvtTDobjtmp["treasureDeviceId"].ToString();
                    }

                    if (((JObject) svtTreasureDevicestmp)["svtId"].ToString() == svtID &&
                        ((JObject) svtTreasureDevicestmp)["priority"].ToString() == "102")
                    {
                        var mstsvtTDobjtmp = JObject.Parse(svtTreasureDevicestmp.ToString());
                        svtTDID = mstsvtTDobjtmp["treasureDeviceId"].ToString();
                    }

                    if (((JObject) svtTreasureDevicestmp)["svtId"].ToString() == svtID &&
                        ((JObject) svtTreasureDevicestmp)["priority"].ToString() == "103")
                    {
                        var mstsvtTDobjtmp = JObject.Parse(svtTreasureDevicestmp.ToString());
                        svtTDID = mstsvtTDobjtmp["treasureDeviceId"].ToString();
                    }

                    if (((JObject) svtTreasureDevicestmp)["svtId"].ToString() == svtID &&
                        ((JObject) svtTreasureDevicestmp)["priority"].ToString() == "104")
                    {
                        var mstsvtTDobjtmp = JObject.Parse(svtTreasureDevicestmp.ToString());
                        svtTDID = mstsvtTDobjtmp["treasureDeviceId"].ToString();
                    }

                    if (((JObject) svtTreasureDevicestmp)["svtId"].ToString() == svtID &&
                        ((JObject) svtTreasureDevicestmp)["priority"].ToString() == "105")
                    {
                        var mstsvtTDobjtmp = JObject.Parse(svtTreasureDevicestmp.ToString());
                        svtTDID = mstsvtTDobjtmp["treasureDeviceId"].ToString();
                        break;
                    }
                }

                foreach (var TDDtmp in mstTreasureDeviceDetailArray) //查找某个字段与值
                    if (((JObject) TDDtmp)["id"].ToString() == svtTDID)
                    {
                        var TDDobjtmp = JObject.Parse(TDDtmp.ToString());
                        NPDetail = TDDobjtmp["detail"].ToString().Replace("[{0}]", " [Lv.1 - Lv.5] ").Replace("[g]", "")
                            .Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "").Replace(@"＆", "\r\n ＋")
                            .Replace(@"＋", "\r\n ＋").Replace("\r\n \r\n", "\r\n");
                        break;
                    }

                foreach (var TDlvtmp in mstTreasureDeviceLvArray) //查找某个字段与值
                    if (((JObject) TDlvtmp)["treaureDeviceId"].ToString() == svtTDID)
                    {
                        var TDlvobjtmp = JObject.Parse(TDlvtmp.ToString());
                        NPRateTD = Convert.ToInt32(TDlvobjtmp["tdPoint"].ToString());
                        NPRateArts = Convert.ToInt32(TDlvobjtmp["tdPointA"].ToString());
                        NPRateBuster = Convert.ToInt32(TDlvobjtmp["tdPointB"].ToString());
                        NPRateQuick = Convert.ToInt32(TDlvobjtmp["tdPointQ"].ToString());
                        NPRateEX = Convert.ToInt32(TDlvobjtmp["tdPointEx"].ToString());
                        NPRateDef = Convert.ToInt32(TDlvobjtmp["tdPointDef"].ToString());
                        nprate.Text = "Quick: " + (NPRateQuick / 10000).ToString("P") + "   Arts: " +
                                      (NPRateArts / 10000).ToString("P") + "   Buster: " +
                                      (NPRateBuster / 10000).ToString("P") + "\r\nExtra: " +
                                      (NPRateEX / 10000).ToString("P") + "   宝具: " + (NPRateTD / 10000).ToString("P") +
                                      "   受击: " + (NPRateDef / 10000).ToString("P");
                        break;
                    }

                foreach (var illustidtmp in mstIllustratorArray) //查找某个字段与值
                    if (((JObject) illustidtmp)["id"].ToString() == svtillust)
                    {
                        var mstillustobjtmp = JObject.Parse(illustidtmp.ToString());
                        svtILLUSTName = mstillustobjtmp["name"].ToString();
                        illust.Text = svtILLUSTName;
                        break;
                    }

                foreach (var svtLimittmp in mstSvtLimitArray) //查找某个字段与值
                    if (((JObject) svtLimittmp)["svtId"].ToString() == svtID)
                    {
                        var mstsvtLimitobjtmp = JObject.Parse(svtLimittmp.ToString());
                        svtrarity = mstsvtLimitobjtmp["rarity"].ToString();
                        rarity.Text = svtrarity + " ☆";
                        svthpBase = mstsvtLimitobjtmp["hpBase"].ToString();
                        basichp.Text = svthpBase;
                        svthpMax = mstsvtLimitobjtmp["hpMax"].ToString();
                        maxhp.Text = svthpMax;
                        svtatkBase = mstsvtLimitobjtmp["atkBase"].ToString();
                        basicatk.Text = svtatkBase;
                        svtatkMax = mstsvtLimitobjtmp["atkMax"].ToString();
                        maxatk.Text = svtatkMax;
                        svtcriticalWeight = mstsvtLimitobjtmp["criticalWeight"].ToString();
                        jixing.Text = svtcriticalWeight;
                        svtpower = mstsvtLimitobjtmp["power"].ToString();
                        svtdefense = mstsvtLimitobjtmp["defense"].ToString();
                        svtagility = mstsvtLimitobjtmp["agility"].ToString();
                        svtmagic = mstsvtLimitobjtmp["magic"].ToString();
                        svtluck = mstsvtLimitobjtmp["luck"].ToString();
                        svttreasureDevice = mstsvtLimitobjtmp["treasureDevice"].ToString();
                        powerData = int.Parse(svtpower);
                        defenseData = int.Parse(svtdefense);
                        agilityData = int.Parse(svtagility);
                        magicData = int.Parse(svtmagic);
                        luckData = int.Parse(svtluck);
                        TreasureData = int.Parse(svttreasureDevice);
                        sixwei.Content = "筋力: " + PPK[powerData] + "        耐久: " + PPK[defenseData] + "\n敏捷: " +
                                         PPK[agilityData] +
                                         "        魔力: " + PPK[magicData] + "\n幸运: " + PPK[luckData] + "        宝具: " +
                                         PPK[TreasureData];
                        break;
                    }

                foreach (var TreasureDevicestmp in mstTreasureDevicedArray) //查找某个字段与值
                {
                    if (((JObject) TreasureDevicestmp)["id"].ToString() == svtTDID)
                    {
                        var mstTDobjtmp = JObject.Parse(TreasureDevicestmp.ToString());
                        NPName = mstTDobjtmp["name"].ToString();
                        npname.Text = NPName;
                        NPrank = mstTDobjtmp["rank"].ToString();
                        NPruby = mstTDobjtmp["ruby"].ToString();
                        npruby.Text = NPruby;
                        NPtypeText = mstTDobjtmp["typeText"].ToString();
                        nprank.Text = NPrank + " ( " + NPtypeText + " ) ";
                        svtNPDamageType = mstTDobjtmp["effectFlag"].ToString().Replace("0", "无伤害宝具")
                            .Replace("1", "群体宝具").Replace("2", "单体宝具");
                        nptype.Text = svtNPDamageType;
                        if (svtNPDamageType == "无伤害宝具")
                        {
                            svtNPCardhit = 0;
                            svtNPCardhitDamage = "[ - ]";
                        }

                        foreach (var svtTreasureDevicestmp in mstSvtTreasureDevicedArray) //查找某个字段与值
                            if (((JObject) svtTreasureDevicestmp)["treasureDeviceId"].ToString() ==
                                ((JObject) TreasureDevicestmp)["id"].ToString())
                            {
                                var mstsvtTDobjtmp2 = JObject.Parse(svtTreasureDevicestmp.ToString());
                                svtNPCardhitDamage = mstsvtTDobjtmp2["damage"].ToString().Replace("\n", "")
                                    .Replace("\t", "").Replace("\r", "").Replace(" ", "");
                                svtNPCardType = mstsvtTDobjtmp2["cardId"].ToString().Replace("2", "Buster")
                                    .Replace("1", "Arts").Replace("3", "Quick");
                                break;
                            }

                        break;
                    }

                    if (((JObject) TreasureDevicestmp)["seqId"].ToString() == svtID &&
                        ((JObject) TreasureDevicestmp)["ruby"].ToString() == "-" &&
                        ((JObject) TreasureDevicestmp)["id"].ToString().Length == 3)
                    {
                        var mstTDobjtmp2 = JObject.Parse(TreasureDevicestmp.ToString());
                        NPName = mstTDobjtmp2["name"].ToString();
                        npname.Text = NPName;
                        NPrank = mstTDobjtmp2["rank"].ToString();
                        NPruby = mstTDobjtmp2["ruby"].ToString();
                        npruby.Text = NPruby;
                        NPtypeText = mstTDobjtmp2["typeText"].ToString();
                        nprank.Text = NPrank + " ( " + NPtypeText + " ) ";
                        svtNPDamageType = mstTDobjtmp2["effectFlag"].ToString().Replace("0", "无伤害宝具")
                            .Replace("1", "群体宝具").Replace("2", "单体宝具");
                        nptype.Text = svtNPDamageType;
                        if (svtNPDamageType == "-")
                        {
                            svtNPCardhit = 0;
                            svtNPCardhitDamage = "[ - ]";
                        }

                        NPDetail = "该ID的配卡与宝具解析不准确,请留意.";
                        foreach (var svtTreasureDevicestmp in mstSvtTreasureDevicedArray) //查找某个字段与值
                            if (((JObject) svtTreasureDevicestmp)["treasureDeviceId"].ToString() ==
                                ((JObject) TreasureDevicestmp)["id"].ToString())
                            {
                                var mstsvtTDobjtmp2 = JObject.Parse(svtTreasureDevicestmp.ToString());
                                svtNPCardhitDamage = mstsvtTDobjtmp2["damage"].ToString().Replace("\n", "")
                                    .Replace("\t", "").Replace("\r", "").Replace(" ", "");
                                svtNPCardType = mstsvtTDobjtmp2["cardId"].ToString().Replace("2", "Buster")
                                    .Replace("1", "Arts").Replace("3", "Quick");
                                break;
                            }

                        break;
                    }

                    treasuredevicescard.Text = svtNPCardhit + " hit " + svtNPCardhitDamage;
                    npcardtype.Text = svtNPCardType;
                }

                if (NPDetail == "unknown")
                {
                    foreach (var TreasureDevicestmp2 in mstTreasureDevicedArray) //查找某个字段与值
                        if (((JObject) TreasureDevicestmp2)["name"].ToString() == NPName)
                        {
                            var TreasureDevicesobjtmp2 = JObject.Parse(TreasureDevicestmp2.ToString());
                            newtmpid = TreasureDevicesobjtmp2["id"].ToString();
                            if (newtmpid.Length == 6)
                            {
                                var FinTDID_TMP = newtmpid;
                                foreach (var TDDtmp2 in mstTreasureDeviceDetailArray) //查找某个字段与值
                                    if (((JObject) TDDtmp2)["id"].ToString() == FinTDID_TMP)
                                    {
                                        var TDDobjtmp2 = JObject.Parse(TDDtmp2.ToString());
                                        NPDetail = TDDobjtmp2["detail"].ToString().Replace("[{0}]", " [Lv.1 - Lv.5] ")
                                            .Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "")
                                            .Replace("[/o]", "").Replace(@"＆", "\r\n ＋").Replace(@"＋", "\r\n ＋")
                                            .Replace("\r\n \r\n", "\r\n");
                                        var TDDetailDisplay = NPDetail;
                                    }
                            }
                            else if (newtmpid.Length == 7)
                            {
                                if (newtmpid.Substring(0, 2) == "10" || newtmpid.Substring(0, 2) == "11" ||
                                    newtmpid.Substring(0, 2) == "23" || newtmpid.Substring(0, 2) == "25")
                                {
                                    var FinTDID_TMP = newtmpid;
                                    foreach (var TDDtmp2 in mstTreasureDeviceDetailArray) //查找某个字段与值
                                        if (((JObject) TDDtmp2)["id"].ToString() == FinTDID_TMP)
                                        {
                                            var TDDobjtmp2 = JObject.Parse(TDDtmp2.ToString());
                                            NPDetail = TDDobjtmp2["detail"].ToString()
                                                .Replace("[{0}]", " [Lv.1 - Lv.5] ").Replace("[g]", "")
                                                .Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "")
                                                .Replace(@"＆", "\r\n ＋").Replace(@"＋", "\r\n ＋")
                                                .Replace("\r\n \r\n", "\r\n");
                                        }
                                }
                            }
                        }
                }
                else
                {
                    newtmpid = svtTDID;
                }

                npdetail.Text = NPDetail;
                foreach (var TDLVtmp in mstTreasureDeviceLvArray) //查找某个字段与值
                {
                    if (((JObject) TDLVtmp)["treaureDeviceId"].ToString() == svtTDID &&
                        ((JObject) TDLVtmp)["lv"].ToString() == "1")
                    {
                        var TDLVobjtmp = JObject.Parse(TDLVtmp.ToString());
                        npvaluelv1.Text =
                            "OC1: " + TDLVobjtmp["svals"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/") + "\r\n" +
                            "OC2: " + TDLVobjtmp["svals2"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/") + "\r\n" +
                            "OC3: " + TDLVobjtmp["svals3"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/") + "\r\n" +
                            "OC4: " + TDLVobjtmp["svals4"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/") + "\r\n" +
                            "OC5: " + TDLVobjtmp["svals5"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/");
                    }

                    if (((JObject) TDLVtmp)["treaureDeviceId"].ToString() == svtTDID &&
                        ((JObject) TDLVtmp)["lv"].ToString() == "2")
                    {
                        var TDLVobjtmp = JObject.Parse(TDLVtmp.ToString());
                        npvaluelv2.Text =
                            "OC1: " + TDLVobjtmp["svals"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/") + "\r\n" +
                            "OC2: " + TDLVobjtmp["svals2"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/") + "\r\n" +
                            "OC3: " + TDLVobjtmp["svals3"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/") + "\r\n" +
                            "OC4: " + TDLVobjtmp["svals4"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/") + "\r\n" +
                            "OC5: " + TDLVobjtmp["svals5"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/");
                    }

                    if (((JObject) TDLVtmp)["treaureDeviceId"].ToString() == svtTDID &&
                        ((JObject) TDLVtmp)["lv"].ToString() == "3")
                    {
                        var TDLVobjtmp = JObject.Parse(TDLVtmp.ToString());
                        npvaluelv3.Text =
                            "OC1: " + TDLVobjtmp["svals"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/") + "\r\n" +
                            "OC2: " + TDLVobjtmp["svals2"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/") + "\r\n" +
                            "OC3: " + TDLVobjtmp["svals3"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/") + "\r\n" +
                            "OC4: " + TDLVobjtmp["svals4"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/") + "\r\n" +
                            "OC5: " + TDLVobjtmp["svals5"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/");
                    }

                    if (((JObject) TDLVtmp)["treaureDeviceId"].ToString() == svtTDID &&
                        ((JObject) TDLVtmp)["lv"].ToString() == "4")
                    {
                        var TDLVobjtmp = JObject.Parse(TDLVtmp.ToString());
                        npvaluelv4.Text =
                            "OC1: " + TDLVobjtmp["svals"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/") + "\r\n" +
                            "OC2: " + TDLVobjtmp["svals2"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/") + "\r\n" +
                            "OC3: " + TDLVobjtmp["svals3"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/") + "\r\n" +
                            "OC4: " + TDLVobjtmp["svals4"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/") + "\r\n" +
                            "OC5: " + TDLVobjtmp["svals5"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/");
                    }

                    if (((JObject) TDLVtmp)["treaureDeviceId"].ToString() == svtTDID &&
                        ((JObject) TDLVtmp)["lv"].ToString() == "5")
                    {
                        var TDLVobjtmp = JObject.Parse(TDLVtmp.ToString());
                        npvaluelv5.Text =
                            "OC1: " + TDLVobjtmp["svals"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/") + "\r\n" +
                            "OC2: " + TDLVobjtmp["svals2"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/") + "\r\n" +
                            "OC3: " + TDLVobjtmp["svals3"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/") + "\r\n" +
                            "OC4: " + TDLVobjtmp["svals4"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/") + "\r\n" +
                            "OC5: " + TDLVobjtmp["svals5"].ToString().Replace("\n", "").Replace("\r", "")
                                .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "")
                                .Replace("*,", "/");
                        svtTreasureDeviceFuncID = TDLVobjtmp["funcId"].ToString().Replace("\n", "").Replace("\t", "")
                            .Replace("\r", "").Replace(" ", "").Replace("[", "").Replace("]", "");
                        svtTreasureDeviceFuncIDList = new List<string>(svtTreasureDeviceFuncID.Split(','));
                        svtTreasureDeviceFuncIDArray = svtTreasureDeviceFuncIDList.ToArray();
                    }
                }
                foreach (var skfuncidtmp in svtTreasureDeviceFuncIDArray)
                foreach (var functmp in mstFuncArray)
                    if (((JObject)functmp)["id"].ToString() == skfuncidtmp)
                    {
                        var mstFuncobjtmp = JObject.Parse(functmp.ToString());
                        svtTreasureDeviceFuncList.Add(mstFuncobjtmp["popupText"].ToString());
                    }
                svtTreasureDeviceFuncArray = svtTreasureDeviceFuncList.ToArray();
                svtTreasureDeviceFunc = string.Join(", ", svtTreasureDeviceFuncArray);
                SkillLvs.TDFuncstr = svtTreasureDeviceFunc;
                if (svtArtsCardQuantity == 0)
                {
                    NPrate = 0;
                    notrealnprate.Text = NPrate.ToString("P");
                }
                else
                {
                    NPrate = nprateclassbase[classData] * nprateartscount[svtArtsCardQuantity] *
                        npratemagicbase[magicData] / svtArtsCardhit / 100;
                    NPrate = Math.Floor(NPrate * 10000) / 10000;
                    notrealnprate.Text = NPrate.ToString("P");
                }

                svtClassPassiveArray = svtClassPassiveList.ToArray();
                svtClassPassive = string.Join(", ", svtClassPassiveArray);
                classskill.Text = svtClassPassive;
                if (svtrarity == "unknown")
                {
                    MessageBox.Show("从者ID不存在或未实装，请重试.", "温馨提示:", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearTexts();
                    Button1.IsEnabled = true;
                    return;
                }

                var mstSvtComment = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSvtComment");
                var mstSvtCommentArray = (JArray) JsonConvert.DeserializeObject(mstSvtComment);
                foreach (var SCTMP in mstSvtCommentArray) //查找某个字段与值
                {
                    if (((JObject) SCTMP)["svtId"].ToString() == svtID && ((JObject) SCTMP)["id"].ToString() == "1")
                    {
                        var SCobjtmp = JObject.Parse(SCTMP.ToString());
                        jibantext1.Text = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                        JB.JB1 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                        var temp_JB = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                        if (temp_JB != "")
                        {
                            JBOutput.IsEnabled = true;
                            SkillDetailCheck(skill1ID);
                            skill1cdlv1.Text = SkillLvs.skilllv1chargetime;
                            skill1cdlv6.Text = SkillLvs.skilllv6chargetime;
                            skill1cdlv10.Text = SkillLvs.skilllv10chargetime;
                            skill1valuelv1.Text = SkillLvs.skilllv1sval;
                            skill1valuelv6.Text = SkillLvs.skilllv6sval;
                            skill1valuelv10.Text = SkillLvs.skilllv10sval;
                            skill1Funcs.Text = SkillLvs.SKLFuncstr;
                            SkillDetailCheck(skill2ID);
                            skill2cdlv1.Text = SkillLvs.skilllv1chargetime;
                            skill2cdlv6.Text = SkillLvs.skilllv6chargetime;
                            skill2cdlv10.Text = SkillLvs.skilllv10chargetime;
                            skill2valuelv1.Text = SkillLvs.skilllv1sval;
                            skill2valuelv6.Text = SkillLvs.skilllv6sval;
                            skill2valuelv10.Text = SkillLvs.skilllv10sval;
                            skill2Funcs.Text = SkillLvs.SKLFuncstr;
                            SkillDetailCheck(skill3ID);
                            skill3cdlv1.Text = SkillLvs.skilllv1chargetime;
                            skill3cdlv6.Text = SkillLvs.skilllv6chargetime;
                            skill3cdlv10.Text = SkillLvs.skilllv10chargetime;
                            skill3valuelv1.Text = SkillLvs.skilllv1sval;
                            skill3valuelv6.Text = SkillLvs.skilllv6sval;
                            skill3valuelv10.Text = SkillLvs.skilllv10sval;
                            skill3Funcs.Text = SkillLvs.SKLFuncstr;
                        }
                    }

                    if (((JObject) SCTMP)["svtId"].ToString() == svtID && ((JObject) SCTMP)["id"].ToString() == "2")
                    {
                        var SCobjtmp = JObject.Parse(SCTMP.ToString());
                        jibantext2.Text = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                        JB.JB2 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                    }

                    if (((JObject) SCTMP)["svtId"].ToString() == svtID && ((JObject) SCTMP)["id"].ToString() == "3")
                    {
                        var SCobjtmp = JObject.Parse(SCTMP.ToString());
                        jibantext3.Text = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                        JB.JB3 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                    }

                    if (((JObject) SCTMP)["svtId"].ToString() == svtID && ((JObject) SCTMP)["id"].ToString() == "4")
                    {
                        var SCobjtmp = JObject.Parse(SCTMP.ToString());
                        jibantext4.Text = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                        JB.JB4 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                    }

                    if (((JObject) SCTMP)["svtId"].ToString() == svtID && ((JObject) SCTMP)["id"].ToString() == "5")
                    {
                        var SCobjtmp = JObject.Parse(SCTMP.ToString());
                        jibantext5.Text = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                        JB.JB5 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                    }

                    if (((JObject) SCTMP)["svtId"].ToString() == svtID && ((JObject) SCTMP)["id"].ToString() == "6")
                    {
                        var SCobjtmp = JObject.Parse(SCTMP.ToString());
                        jibantext6.Text = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                        JB.JB6 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                    }

                    if (((JObject) SCTMP)["svtId"].ToString() == svtID && ((JObject) SCTMP)["id"].ToString() == "7")
                    {
                        var SCobjtmp = JObject.Parse(SCTMP.ToString());
                        jibantext7.Text = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                        JB.JB7 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                    }
                }

                if (classData == 3)
                    Dispatcher.Invoke(new Action(() =>
                    {
                        atkbalance1.Content = "( x 1.05 △)";
                        atkbalance2.Content = "( x 1.05 △)";
                    }));
                else if (classData == 5)
                    Dispatcher.Invoke(new Action(() =>
                    {
                        atkbalance1.Content = "( x 0.9 ▽)";
                        atkbalance2.Content = "( x 0.9 ▽)";
                    }));
                else if (classData == 6)
                    Dispatcher.Invoke(new Action(() =>
                    {
                        atkbalance1.Content = "( x 0.9 ▽)";
                        atkbalance2.Content = "( x 0.9 ▽)";
                    }));
                else if (classData == 2)
                    Dispatcher.Invoke(new Action(() =>
                    {
                        atkbalance1.Content = "( x 0.95 ▽)";
                        atkbalance2.Content = "( x 0.95 ▽)";
                    }));
                else if (classData == 7)
                    Dispatcher.Invoke(new Action(() =>
                    {
                        atkbalance1.Content = "( x 1.1 △)";
                        atkbalance2.Content = "( x 1.1 △)";
                    }));
                else if (classData == 9)
                    Dispatcher.Invoke(new Action(() =>
                    {
                        atkbalance1.Content = "( x 1.1 △)";
                        atkbalance2.Content = "( x 1.1 △)";
                    }));
                else if (classData == 11)
                    Dispatcher.Invoke(new Action(() =>
                    {
                        atkbalance1.Content = "( x 1.1 △)";
                        atkbalance2.Content = "( x 1.1 △)";
                    }));
                else
                    Dispatcher.Invoke(new Action(() =>
                    {
                        atkbalance1.Content = "( x 1.0 -)";
                        atkbalance2.Content = "( x 1.0 -)";
                    }));
                Button1.IsEnabled = true;
                if (classData == 1001)
                    MessageBox.Show("此ID为礼装ID,图鉴编号为礼装的图鉴编号.礼装描述在羁绊文本的文本1处,礼装效果在技能1栏中.", "温馨提示:", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                if (cards.Text == "[Q,Q,Q,Q,Q]" && classData != 1001)
                    MessageBox.Show("此ID为小怪(或部分boss以及种火芙芙),配卡、技能、宝具信息解析并不准确，请知悉.", "温馨提示:", MessageBoxButton.OK,
                        MessageBoxImage.Information);
            }));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var path = Directory.GetCurrentDirectory();
            var gamedata = new DirectoryInfo(path + @"\Android\masterdata\");
            var folder = new DirectoryInfo(path + @"\Android\");
            var output = "";
            if (!Directory.Exists(gamedata.FullName))
            {
                MessageBox.Show("没有游戏数据,请先下载游戏数据(位于\"关于\"菜单栏中).", "温馨提示:", MessageBoxButton.OK,
                    MessageBoxImage.Information);
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
                    MessageBox.Show("游戏数据损坏,请先下载游戏数据(位于\"关于\"菜单栏中).", "温馨提示:", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
            }

            var mstSvt = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSvt");
            var mstSvtArray = (JArray) JsonConvert.DeserializeObject(mstSvt);
            foreach (var svtIDtmp in mstSvtArray) //查找某个字段与值
                output = output + "ID: " + ((JObject) svtIDtmp)["id"].ToString() + "    " + "名称: " +
                         ((JObject) svtIDtmp)["name"].ToString() + "\r\n";
            File.WriteAllText(path + "/SearchIDList.txt", output);
            MessageBox.Show("导出成功,文件名为 SearchIDList.txt", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
            System.Diagnostics.Process.Start(path + "/SearchIDList.txt");
        }

        private void ClearTexts()
        {
            var g = Content as Grid;
            var childrens = g.Children;
            foreach (UIElement ui in childrens)
                if (ui is TextBox)
                    (ui as TextBox).Text = "";
            var childrens1 = svtdetail.Children;
            foreach (UIElement ui2 in childrens1)
                if (ui2 is TextBox)
                    (ui2 as TextBox).Text = "";
            var childrens2 = svttexts.Children;
            foreach (UIElement ui2 in childrens2)
                if (ui2 is TextBox)
                    (ui2 as TextBox).Text = "";
            var childrens3 = svtcards.Children;
            foreach (UIElement ui2 in childrens3)
                if (ui2 is TextBox)
                    (ui2 as TextBox).Text = "";
            var childrens4 = svtTDs.Children;
            foreach (UIElement ui2 in childrens4)
                if (ui2 is TextBox)
                    (ui2 as TextBox).Text = "";
            var childrens5 = svtskill1.Children;
            foreach (UIElement ui2 in childrens5)
                if (ui2 is TextBox)
                    (ui2 as TextBox).Text = "";
            var childrens6 = svtskill2.Children;
            foreach (UIElement ui2 in childrens6)
                if (ui2 is TextBox)
                    (ui2 as TextBox).Text = "";
            var childrens7 = svtskill3.Children;
            foreach (UIElement ui2 in childrens7)
                if (ui2 is TextBox)
                    (ui2 as TextBox).Text = "";
            atkbalance1.Content = "( x 1.0 -)";
            atkbalance2.Content = "( x 1.0 -)";
            JBOutput.IsEnabled = false;
            sixwei.Content = "";
        }

        private void SkillDetailCheck(string sklid)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var path = Directory.GetCurrentDirectory();
                var gamedata = new DirectoryInfo(path + @"\Android\masterdata\");
                var folder = new DirectoryInfo(path + @"\Android\");
                var svtID = Convert.ToString(textbox1.Text);
                SkillLvs.skilllv1sval = "";
                SkillLvs.skilllv6sval = "";
                SkillLvs.skilllv10sval = "";
                SkillLvs.skilllv1chargetime = "";
                SkillLvs.skilllv6chargetime = "";
                SkillLvs.skilllv10chargetime = "";
                SkillLvs.SKLFuncstr = "";
                var mstSkillLv = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSkillLv");
                var mstSkillLvArray = (JArray) JsonConvert.DeserializeObject(mstSkillLv);
                var mstFunc = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstFunc");
                var mstFuncArray = (JArray) JsonConvert.DeserializeObject(mstFunc);
                string svtSKFuncID;
                string[] svtSKFuncIDArray;
                List<string> svtSKFuncIDList;
                var svtSKFuncList = new List<string> { };
                string[] svtSKFuncArray;
                var svtSKFunc = string.Empty;
                foreach (var SKLTMP in mstSkillLvArray) //查找某个字段与值
                {
                    if (((JObject) SKLTMP)["skillId"].ToString() == sklid && ((JObject) SKLTMP)["lv"].ToString() == "1")
                    {
                        var SKLobjtmp = JObject.Parse(SKLTMP.ToString());
                        SkillLvs.skilllv1sval = SKLobjtmp["svals"].ToString().Replace("\n", "").Replace("\r", "")
                            .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "").Replace("*,", "/");
                        SkillLvs.skilllv1sval = SkillLvs.skilllv1sval.Substring(0, SkillLvs.skilllv1sval.Length - 2);
                        SkillLvs.skilllv1EffectArray1 = SkillLvs.skilllv1sval.Split('/');
                        var skilllv1EffectArray2 = new string[SkillLvs.skilllv1sval.Length][];
                        SkillLvs.skilllv1chargetime = SKLobjtmp["chargeTurn"].ToString();
                    }

                    if (((JObject) SKLTMP)["skillId"].ToString() == sklid && ((JObject) SKLTMP)["lv"].ToString() == "6")
                    {
                        var SKLobjtmp = JObject.Parse(SKLTMP.ToString());
                        SkillLvs.skilllv6sval = SKLobjtmp["svals"].ToString().Replace("\n", "").Replace("\r", "")
                            .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "").Replace("*,", "/");
                        SkillLvs.skilllv6sval = SkillLvs.skilllv6sval.Substring(0, SkillLvs.skilllv6sval.Length - 2);
                        SkillLvs.skilllv6EffectArray1 = SkillLvs.skilllv6sval.Split('/');
                        var skilllv6EffectArray2 = new string[SkillLvs.skilllv6sval.Length][];
                        SkillLvs.skilllv6chargetime = SKLobjtmp["chargeTurn"].ToString();
                    }

                    if (((JObject) SKLTMP)["skillId"].ToString() == sklid &&
                        ((JObject) SKLTMP)["lv"].ToString() == "10")
                    {
                        var SKLobjtmp = JObject.Parse(SKLTMP.ToString());
                        SkillLvs.skilllv10sval = SKLobjtmp["svals"].ToString().Replace("\n", "").Replace("\r", "")
                            .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "").Replace("*,", "/");
                        SkillLvs.skilllv10sval = SkillLvs.skilllv10sval.Substring(0, SkillLvs.skilllv10sval.Length - 2);
                        SkillLvs.skilllv10EffectArray1 = SkillLvs.skilllv10sval.Split('/');
                        var skilllv10EffectArray2 = new string[SkillLvs.skilllv10sval.Length][];
                        SkillLvs.skilllv10chargetime = SKLobjtmp["chargeTurn"].ToString();
                        svtSKFuncID = SKLobjtmp["funcId"].ToString().Replace("\n", "").Replace("\t", "")
                            .Replace("\r", "").Replace(" ", "").Replace("[", "").Replace("]", "");
                        svtSKFuncIDList = new List<string>(svtSKFuncID.Split(','));
                        svtSKFuncIDArray = svtSKFuncIDList.ToArray();
                        foreach (var skfuncidtmp in svtSKFuncIDArray)
                        foreach (var functmp in mstFuncArray)
                            if (((JObject) functmp)["id"].ToString() == skfuncidtmp)
                            {
                                var mstFuncobjtmp = JObject.Parse(functmp.ToString());
                                svtSKFuncList.Add(mstFuncobjtmp["popupText"].ToString());
                            }
                    }
                }

                svtSKFuncArray = svtSKFuncList.ToArray();
                svtSKFunc = string.Join(", ", svtSKFuncArray);
                SkillLvs.SKLFuncstr = svtSKFunc;
            }));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ClearTexts();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)

        {
            // open URL

            var source = sender as Hyperlink;

            if (source != null) System.Diagnostics.Process.Start(source.NavigateUri.ToString());
        }

        private void HttpRequestData()
        {
            updatedatabutton.Dispatcher.Invoke(new Action(() => { updatedatabutton.IsEnabled = false; }));
            updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = ""; }));
            updatestatus.Dispatcher.Invoke(new Action(() => { updatesign.Content = "数据下载进行中,请勿退出!"; }));
            progressbar.Dispatcher.Invoke(new Action(() =>
            {
                progressbar.Value = 0;
                progressbar.Visibility = Visibility.Visible;
            }));
            var path = Directory.GetCurrentDirectory();
            var gamedata = new DirectoryInfo(path + @"\Android\masterdata\");
            var folder = new DirectoryInfo(path + @"\Android\");
            progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 500; }));
            if (!Directory.Exists(folder.FullName))
            {
                updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = "正在创建Android目录..."; }));
                Directory.CreateDirectory(folder.FullName);
                if (File.Exists(gamedata.FullName + "webview") || File.Exists(gamedata.FullName + "raw") ||
                    File.Exists(gamedata.FullName + "assetbundle") || File.Exists(gamedata.FullName + "webview") ||
                    File.Exists(gamedata.FullName + "master"))
                {
                    var fileinfo = folder.GetFileSystemInfos(); //返回目录中所有文件和子目录
                    foreach (var i in fileinfo)
                    {
                        if (i is DirectoryInfo) //判断是否文件夹
                        {
                            var subdir = new DirectoryInfo(i.FullName);
                            subdir.Delete(true); //删除子目录和文件
                            updatestatus.Dispatcher.Invoke(
                                new Action(() => { updatestatus.Content = "删除: " + subdir; }));
                            continue;
                        }

                        i.Delete();
                        updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = "删除: " + i; }));
                    }

                    updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = "开始下载/更新游戏数据......"; }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 500; }));

                    var result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top", "appVer=2.13.2");
                    var res = JObject.Parse(result);
                    if (res["response"][0]["fail"]["action"] != null)
                    {
                        if (res["response"][0]["fail"]["action"].ToString() == "app_version_up")
                        {
                            var tmp = res["response"][0]["fail"]["detail"].ToString();
                            tmp = Regex.Replace(tmp, @".*新ver.：(.*)、現.*", "$1", RegexOptions.Singleline);
                            updatestatus.Dispatcher.Invoke(new Action(() =>
                            {
                                updatestatus.Content = "当前游戏版本: " + tmp.ToString();
                            }));

                            result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top",
                                "appVer=" + tmp.ToString());
                            res = JObject.Parse(result);
                        }
                        else if (res["response"][0]["fail"]["action"].ToString() == "maint")
                        {
                            var tmp = res["response"][0]["fail"]["detail"].ToString();
                            if (MessageBox.Show(
                                    "游戏服务器正在维护，请在维护后下载数据. \r\n以下为服务器公告内容:\r\n\r\n『" +
                                    tmp.Replace("[00FFFF]", "").Replace("[url=", "")
                                        .Replace("][u]公式サイト お知らせ[/u][/url][-]", "") + "』\r\n\r\n点击\"确定\"可打开公告页面.",
                                    "维护中", MessageBoxButton.OKCancel, MessageBoxImage.Information) ==
                                MessageBoxResult.OK)
                            {
                                var re = new Regex(@"(?<url>http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)");
                                var mc = re.Matches(tmp);
                                foreach (Match m in mc)
                                {
                                    var url = m.Result("${url}");
                                    System.Diagnostics.Process.Start(url);
                                }
                            }

                            updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = ""; }));
                            updatestatus.Dispatcher.Invoke(new Action(() => { updatesign.Content = ""; }));
                            progressbar.Dispatcher.Invoke(new Action(() =>
                            {
                                progressbar.Visibility = Visibility.Hidden;
                                updatedatabutton.IsEnabled = true;
                            }));
                            return;
                        }
                        else
                        {
                        }
                    }

                    if (!Directory.Exists(gamedata.FullName))
                        Directory.CreateDirectory(gamedata.FullName);
                    File.WriteAllText(gamedata.FullName + "raw", result);
                    File.WriteAllText(gamedata.FullName + "assetbundle",
                        res["response"][0]["success"]["assetbundle"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatestatus.Content = "Writing file to: " + gamedata.FullName + "assetbundle";
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 38; }));
                    File.WriteAllText(gamedata.FullName + "master", res["response"][0]["success"]["master"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatestatus.Content = "Writing file to: " + gamedata.FullName + "master";
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 38; }));
                    File.WriteAllText(gamedata.FullName + "webview",
                        res["response"][0]["success"]["webview"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatestatus.Content = "Writing file to: " + gamedata.FullName + "webview";
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 38; }));
                    var data = File.ReadAllText(gamedata.FullName + "master");
                    if (!Directory.Exists(gamedata.FullName + "decrypted_masterdata"))
                        Directory.CreateDirectory(gamedata.FullName + "decrypted_masterdata");
                    var masterData =
                        (Dictionary<string, byte[]>) MasterDataUnpacker.MouseGame2Unpacker(
                            Convert.FromBase64String(data));
                    var job = new JObject();
                    var miniMessagePacker = new MiniMessagePacker();
                    foreach (var item in masterData)
                    {
                        var unpackeditem = (List<object>) miniMessagePacker.Unpack(item.Value);
                        var json = JsonConvert.SerializeObject(unpackeditem, Formatting.Indented);
                        File.WriteAllText(gamedata.FullName + "decrypted_masterdata/" + item.Key, json);
                        updatestatus.Dispatcher.Invoke(new Action(() =>
                        {
                            updatestatus.Content = "Writing file to: " + gamedata.FullName +
                                                   "decrypted_masterdata\\" + item.Key;
                        }));
                        progressbar.Dispatcher.Invoke(new Action(() =>
                        {
                            progressbar.Value = progressbar.Value + 38;
                        }));
                    }

                    var data2 = File.ReadAllText(gamedata.FullName + "assetbundle");
                    var dictionary =
                        (Dictionary<string, object>) MasterDataUnpacker.MouseInfoMsgPack(
                            Convert.FromBase64String(data2));
                    string str = null;
                    foreach (var a in dictionary) str += a.Key + ": " + a.Value.ToString() + "\r\n";
                    File.WriteAllText(gamedata.FullName + "assetbundle.txt", str);
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatestatus.Content = "folder name: " + dictionary["folderName"].ToString();
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 38; }));
                    var data3 = File.ReadAllText(gamedata.FullName + "webview");
                    var dictionary2 =
                        (Dictionary<string, object>) MasterDataUnpacker.MouseGame2MsgPack(
                            Convert.FromBase64String(data3));
                    var str2 = "baseURL: " + dictionary2["baseURL"].ToString() + "\r\n contactURL: " +
                               dictionary2["contactURL"].ToString() + "\r\n";
                    updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = str2; }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 38; }));
                    var filePassInfo = (Dictionary<string, object>) dictionary2["filePass"];
                    foreach (var a in filePassInfo) str += a.Key + ": " + a.Value.ToString() + "\r\n";
                    File.WriteAllText(gamedata.FullName + "webview.txt", str2);
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatestatus.Content = "Writing file to: " + gamedata.FullName + "webview.txt";
                    }));

                    updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = "下载完成，可以开始解析."; }));

                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Maximum; }));
                    MessageBox.Show("下载完成，可以开始解析.", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
                    updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = ""; }));
                    updatestatus.Dispatcher.Invoke(new Action(() => { updatesign.Content = ""; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Visibility = Visibility.Hidden;
                        updatedatabutton.IsEnabled = true;
                    }));
                }
                else
                {
                    var fileinfo = folder.GetFileSystemInfos(); //返回目录中所有文件和子目录
                    foreach (var i in fileinfo)
                    {
                        if (i is DirectoryInfo) //判断是否文件夹
                        {
                            var subdir = new DirectoryInfo(i.FullName);
                            subdir.Delete(true); //删除子目录和文件
                            updatestatus.Dispatcher.Invoke(
                                new Action(() => { updatestatus.Content = "删除: " + subdir; }));

                            continue;
                        }

                        i.Delete();
                        updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = "删除: " + i; }));
                    }

                    updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = "开始下载/更新游戏数据......"; }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 500; }));

                    var result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top", "appVer=2.13.2");
                    var res = JObject.Parse(result);
                    if (res["response"][0]["fail"]["action"] != null)
                    {
                        if (res["response"][0]["fail"]["action"].ToString() == "app_version_up")
                        {
                            var tmp = res["response"][0]["fail"]["detail"].ToString();
                            tmp = Regex.Replace(tmp, @".*新ver.：(.*)、現.*", "$1", RegexOptions.Singleline);
                            updatestatus.Dispatcher.Invoke(new Action(() =>
                            {
                                updatestatus.Content = "当前游戏版本: " + tmp.ToString();
                            }));

                            result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top",
                                "appVer=" + tmp.ToString());
                            res = JObject.Parse(result);
                        }
                        else if (res["response"][0]["fail"]["action"].ToString() == "maint")
                        {
                            var tmp = res["response"][0]["fail"]["detail"].ToString();
                            if (MessageBox.Show(
                                    "游戏服务器正在维护，请在维护后下载数据. \r\n以下为服务器公告内容:\r\n\r\n『" +
                                    tmp.Replace("[00FFFF]", "").Replace("[url=", "")
                                        .Replace("][u]公式サイト お知らせ[/u][/url][-]", "") + "』\r\n\r\n点击\"确定\"可打开公告页面.",
                                    "维护中", MessageBoxButton.OKCancel, MessageBoxImage.Information) ==
                                MessageBoxResult.OK)
                            {
                                var re = new Regex(@"(?<url>http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)");
                                var mc = re.Matches(tmp);
                                foreach (Match m in mc)
                                {
                                    var url = m.Result("${url}");
                                    System.Diagnostics.Process.Start(url);
                                }
                            }

                            updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = ""; }));
                            updatestatus.Dispatcher.Invoke(new Action(() => { updatesign.Content = ""; }));
                            progressbar.Dispatcher.Invoke(new Action(() =>
                            {
                                progressbar.Visibility = Visibility.Hidden;
                                updatedatabutton.IsEnabled = true;
                            }));
                            return;
                        }
                        else
                        {
                        }
                    }

                    if (!Directory.Exists(gamedata.FullName))
                        Directory.CreateDirectory(gamedata.FullName);
                    File.WriteAllText(gamedata.FullName + "raw", result);
                    File.WriteAllText(gamedata.FullName + "assetbundle",
                        res["response"][0]["success"]["assetbundle"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatestatus.Content = "Writing file to: " + gamedata.FullName + "assetbundle";
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 38; }));
                    File.WriteAllText(gamedata.FullName + "master", res["response"][0]["success"]["master"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatestatus.Content = "Writing file to: " + gamedata.FullName + "master";
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 38; }));
                    File.WriteAllText(gamedata.FullName + "webview",
                        res["response"][0]["success"]["webview"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatestatus.Content = "Writing file to: " + gamedata.FullName + "webview";
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 38; }));
                    var data = File.ReadAllText(gamedata.FullName + "master");
                    if (!Directory.Exists(gamedata.FullName + "decrypted_masterdata"))
                        Directory.CreateDirectory(gamedata.FullName + "decrypted_masterdata");
                    var masterData =
                        (Dictionary<string, byte[]>) MasterDataUnpacker.MouseGame2Unpacker(
                            Convert.FromBase64String(data));
                    var job = new JObject();
                    var miniMessagePacker = new MiniMessagePacker();
                    foreach (var item in masterData)
                    {
                        var unpackeditem = (List<object>) miniMessagePacker.Unpack(item.Value);
                        var json = JsonConvert.SerializeObject(unpackeditem, Formatting.Indented);
                        File.WriteAllText(gamedata.FullName + "decrypted_masterdata/" + item.Key, json);
                        updatestatus.Dispatcher.Invoke(new Action(() =>
                        {
                            updatestatus.Content = "Writing file to: " + gamedata.FullName +
                                                   "decrypted_masterdata\\" + item.Key;
                        }));
                        progressbar.Dispatcher.Invoke(new Action(() =>
                        {
                            progressbar.Value = progressbar.Value + 38;
                        }));
                    }

                    var data2 = File.ReadAllText(gamedata.FullName + "assetbundle");
                    var dictionary =
                        (Dictionary<string, object>) MasterDataUnpacker.MouseInfoMsgPack(
                            Convert.FromBase64String(data2));
                    string str = null;
                    foreach (var a in dictionary) str += a.Key + ": " + a.Value.ToString() + "\r\n";
                    File.WriteAllText(gamedata.FullName + "assetbundle.txt", str);
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatestatus.Content = "folder name: " + dictionary["folderName"].ToString();
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 38; }));
                    var data3 = File.ReadAllText(gamedata.FullName + "webview");
                    var dictionary2 =
                        (Dictionary<string, object>) MasterDataUnpacker.MouseGame2MsgPack(
                            Convert.FromBase64String(data3));
                    var str2 = "baseURL: " + dictionary2["baseURL"].ToString() + "\r\n contactURL: " +
                               dictionary2["contactURL"].ToString() + "\r\n";
                    updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = str2; }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 38; }));
                    var filePassInfo = (Dictionary<string, object>) dictionary2["filePass"];
                    foreach (var a in filePassInfo) str += a.Key + ": " + a.Value.ToString() + "\r\n";
                    File.WriteAllText(gamedata.FullName + "webview.txt", str2);
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatestatus.Content = "Writing file to: " + gamedata.FullName + "webview.txt";
                    }));

                    updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = "下载完成，可以开始解析."; }));

                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Maximum; }));
                    MessageBox.Show("下载完成，可以开始解析.", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
                    updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = ""; }));
                    updatestatus.Dispatcher.Invoke(new Action(() => { updatesign.Content = ""; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Visibility = Visibility.Hidden;
                        updatedatabutton.IsEnabled = true;
                    }));
                }
            }
            else
            {
                if (File.Exists(gamedata.FullName + "webview") || File.Exists(gamedata.FullName + "raw") ||
                    File.Exists(gamedata.FullName + "assetbundle") || File.Exists(gamedata.FullName + "webview") ||
                    File.Exists(gamedata.FullName + "master"))
                {
                    var fileinfo = folder.GetFileSystemInfos(); //返回目录中所有文件和子目录
                    foreach (var i in fileinfo)
                    {
                        if (i is DirectoryInfo) //判断是否文件夹
                        {
                            var subdir = new DirectoryInfo(i.FullName);
                            subdir.Delete(true); //删除子目录和文件
                            updatestatus.Dispatcher.Invoke(
                                new Action(() => { updatestatus.Content = "删除: " + subdir; }));

                            continue;
                        }

                        i.Delete();
                        updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = "删除: " + i; }));
                    }

                    updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = "开始下载/更新游戏数据......"; }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 500; }));

                    var result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top", "appVer=2.13.2");
                    var res = JObject.Parse(result);
                    if (res["response"][0]["fail"]["action"] != null)
                    {
                        if (res["response"][0]["fail"]["action"].ToString() == "app_version_up")
                        {
                            var tmp = res["response"][0]["fail"]["detail"].ToString();
                            tmp = Regex.Replace(tmp, @".*新ver.：(.*)、現.*", "$1", RegexOptions.Singleline);
                            updatestatus.Dispatcher.Invoke(new Action(() =>
                            {
                                updatestatus.Content = "当前游戏版本: " + tmp.ToString();
                            }));

                            result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top",
                                "appVer=" + tmp.ToString());
                            res = JObject.Parse(result);
                        }
                        else if (res["response"][0]["fail"]["action"].ToString() == "maint")
                        {
                            var tmp = res["response"][0]["fail"]["detail"].ToString();
                            if (MessageBox.Show(
                                    "游戏服务器正在维护，请在维护后下载数据. \r\n以下为服务器公告内容:\r\n\r\n『" +
                                    tmp.Replace("[00FFFF]", "").Replace("[url=", "")
                                        .Replace("][u]公式サイト お知らせ[/u][/url][-]", "") + "』\r\n\r\n点击\"确定\"可打开公告页面.",
                                    "维护中", MessageBoxButton.OKCancel, MessageBoxImage.Information) ==
                                MessageBoxResult.OK)
                            {
                                var re = new Regex(@"(?<url>http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)");
                                var mc = re.Matches(tmp);
                                foreach (Match m in mc)
                                {
                                    var url = m.Result("${url}");
                                    System.Diagnostics.Process.Start(url);
                                }
                            }

                            updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = ""; }));
                            updatestatus.Dispatcher.Invoke(new Action(() => { updatesign.Content = ""; }));
                            progressbar.Dispatcher.Invoke(new Action(() =>
                            {
                                progressbar.Visibility = Visibility.Hidden;
                                updatedatabutton.IsEnabled = true;
                            }));
                            return;
                        }
                        else
                        {
                        }
                    }

                    if (!Directory.Exists(gamedata.FullName))
                        Directory.CreateDirectory(gamedata.FullName);
                    File.WriteAllText(gamedata.FullName + "raw", result);
                    File.WriteAllText(gamedata.FullName + "assetbundle",
                        res["response"][0]["success"]["assetbundle"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatestatus.Content = "Writing file to: " + gamedata.FullName + "assetbundle";
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 38; }));
                    File.WriteAllText(gamedata.FullName + "master", res["response"][0]["success"]["master"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatestatus.Content = "Writing file to: " + gamedata.FullName + "master";
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 38; }));
                    File.WriteAllText(gamedata.FullName + "webview",
                        res["response"][0]["success"]["webview"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatestatus.Content = "Writing file to: " + gamedata.FullName + "webview";
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 38; }));
                    var data = File.ReadAllText(gamedata.FullName + "master");
                    if (!Directory.Exists(gamedata.FullName + "decrypted_masterdata"))
                        Directory.CreateDirectory(gamedata.FullName + "decrypted_masterdata");
                    var masterData =
                        (Dictionary<string, byte[]>) MasterDataUnpacker.MouseGame2Unpacker(
                            Convert.FromBase64String(data));
                    var job = new JObject();
                    var miniMessagePacker = new MiniMessagePacker();
                    foreach (var item in masterData)
                    {
                        var unpackeditem = (List<object>) miniMessagePacker.Unpack(item.Value);
                        var json = JsonConvert.SerializeObject(unpackeditem, Formatting.Indented);
                        File.WriteAllText(gamedata.FullName + "decrypted_masterdata/" + item.Key, json);
                        updatestatus.Dispatcher.Invoke(new Action(() =>
                        {
                            updatestatus.Content = "Writing file to: " + gamedata.FullName +
                                                   "decrypted_masterdata\\" + item.Key;
                        }));
                        progressbar.Dispatcher.Invoke(new Action(() =>
                        {
                            progressbar.Value = progressbar.Value + 38;
                        }));
                    }

                    var data2 = File.ReadAllText(gamedata.FullName + "assetbundle");
                    var dictionary =
                        (Dictionary<string, object>) MasterDataUnpacker.MouseInfoMsgPack(
                            Convert.FromBase64String(data2));
                    string str = null;
                    foreach (var a in dictionary) str += a.Key + ": " + a.Value.ToString() + "\r\n";
                    File.WriteAllText(gamedata.FullName + "assetbundle.txt", str);
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatestatus.Content = "folder name: " + dictionary["folderName"].ToString();
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 38; }));
                    var data3 = File.ReadAllText(gamedata.FullName + "webview");
                    var dictionary2 =
                        (Dictionary<string, object>) MasterDataUnpacker.MouseGame2MsgPack(
                            Convert.FromBase64String(data3));
                    var str2 = "baseURL: " + dictionary2["baseURL"].ToString() + "\r\n contactURL: " +
                               dictionary2["contactURL"].ToString() + "\r\n";
                    updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = str2; }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 38; }));
                    var filePassInfo = (Dictionary<string, object>) dictionary2["filePass"];
                    foreach (var a in filePassInfo) str += a.Key + ": " + a.Value.ToString() + "\r\n";
                    File.WriteAllText(gamedata.FullName + "webview.txt", str2);
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatestatus.Content = "Writing file to: " + gamedata.FullName + "webview.txt";
                    }));

                    updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = "下载完成，可以开始解析."; }));

                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Maximum; }));
                    MessageBox.Show("下载完成，可以开始解析.", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
                    updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = ""; }));
                    updatestatus.Dispatcher.Invoke(new Action(() => { updatesign.Content = ""; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Visibility = Visibility.Hidden;
                        updatedatabutton.IsEnabled = true;
                    }));
                }
                else
                {
                    var fileinfo = folder.GetFileSystemInfos(); //返回目录中所有文件和子目录
                    foreach (var i in fileinfo)
                    {
                        if (i is DirectoryInfo) //判断是否文件夹
                        {
                            var subdir = new DirectoryInfo(i.FullName);
                            subdir.Delete(true); //删除子目录和文件
                            updatestatus.Dispatcher.Invoke(
                                new Action(() => { updatestatus.Content = "删除: " + subdir; }));

                            continue;
                        }

                        i.Delete();
                        updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = "删除: " + i; }));
                    }

                    updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = "开始下载/更新游戏数据......"; }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 500; }));

                    var result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top", "appVer=2.13.2");
                    var res = JObject.Parse(result);
                    if (res["response"][0]["fail"]["action"] != null)
                    {
                        if (res["response"][0]["fail"]["action"].ToString() == "app_version_up")
                        {
                            var tmp = res["response"][0]["fail"]["detail"].ToString();
                            tmp = Regex.Replace(tmp, @".*新ver.：(.*)、現.*", "$1", RegexOptions.Singleline);
                            updatestatus.Dispatcher.Invoke(new Action(() =>
                            {
                                updatestatus.Content = "当前游戏版本: " + tmp.ToString();
                            }));

                            result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top",
                                "appVer=" + tmp.ToString());
                            res = JObject.Parse(result);
                        }
                        else if (res["response"][0]["fail"]["action"].ToString() == "maint")
                        {
                            var tmp = res["response"][0]["fail"]["detail"].ToString();
                            if (MessageBox.Show(
                                    "游戏服务器正在维护，请在维护后下载数据. \r\n以下为服务器公告内容:\r\n\r\n『" +
                                    tmp.Replace("[00FFFF]", "").Replace("[url=", "")
                                        .Replace("][u]公式サイト お知らせ[/u][/url][-]", "") + "』\r\n\r\n点击\"确定\"可打开公告页面.",
                                    "维护中", MessageBoxButton.OKCancel, MessageBoxImage.Information) ==
                                MessageBoxResult.OK)
                            {
                                var re = new Regex(@"(?<url>http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)");
                                var mc = re.Matches(tmp);
                                foreach (Match m in mc)
                                {
                                    var url = m.Result("${url}");
                                    System.Diagnostics.Process.Start(url);
                                }
                            }

                            updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = ""; }));
                            updatestatus.Dispatcher.Invoke(new Action(() => { updatesign.Content = ""; }));
                            progressbar.Dispatcher.Invoke(new Action(() =>
                            {
                                progressbar.Visibility = Visibility.Hidden;
                                updatedatabutton.IsEnabled = true;
                            }));
                            return;
                        }
                        else
                        {
                        }
                    }

                    if (!Directory.Exists(gamedata.FullName))
                        Directory.CreateDirectory(gamedata.FullName);
                    File.WriteAllText(gamedata.FullName + "raw", result);
                    File.WriteAllText(gamedata.FullName + "assetbundle",
                        res["response"][0]["success"]["assetbundle"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatestatus.Content = "Writing file to: " + gamedata.FullName + "assetbundle";
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 38; }));
                    File.WriteAllText(gamedata.FullName + "master", res["response"][0]["success"]["master"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatestatus.Content = "Writing file to: " + gamedata.FullName + "master";
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 38; }));
                    File.WriteAllText(gamedata.FullName + "webview",
                        res["response"][0]["success"]["webview"].ToString());
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatestatus.Content = "Writing file to: " + gamedata.FullName + "webview";
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 38; }));
                    var data = File.ReadAllText(gamedata.FullName + "master");
                    if (!Directory.Exists(gamedata.FullName + "decrypted_masterdata"))
                        Directory.CreateDirectory(gamedata.FullName + "decrypted_masterdata");
                    var masterData =
                        (Dictionary<string, byte[]>) MasterDataUnpacker.MouseGame2Unpacker(
                            Convert.FromBase64String(data));
                    var job = new JObject();
                    var miniMessagePacker = new MiniMessagePacker();
                    foreach (var item in masterData)
                    {
                        var unpackeditem = (List<object>) miniMessagePacker.Unpack(item.Value);
                        var json = JsonConvert.SerializeObject(unpackeditem, Formatting.Indented);
                        File.WriteAllText(gamedata.FullName + "decrypted_masterdata/" + item.Key, json);
                        updatestatus.Dispatcher.Invoke(new Action(() =>
                        {
                            updatestatus.Content = "Writing file to: " + gamedata.FullName +
                                                   "decrypted_masterdata\\" + item.Key;
                        }));
                        progressbar.Dispatcher.Invoke(new Action(() =>
                        {
                            progressbar.Value = progressbar.Value + 38;
                        }));
                    }

                    var data2 = File.ReadAllText(gamedata.FullName + "assetbundle");
                    var dictionary =
                        (Dictionary<string, object>) MasterDataUnpacker.MouseInfoMsgPack(
                            Convert.FromBase64String(data2));
                    string str = null;
                    foreach (var a in dictionary) str += a.Key + ": " + a.Value.ToString() + "\r\n";
                    File.WriteAllText(gamedata.FullName + "assetbundle.txt", str);
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatestatus.Content = "folder name: " + dictionary["folderName"].ToString();
                    }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 38; }));
                    var data3 = File.ReadAllText(gamedata.FullName + "webview");
                    var dictionary2 =
                        (Dictionary<string, object>) MasterDataUnpacker.MouseGame2MsgPack(
                            Convert.FromBase64String(data3));
                    var str2 = "baseURL: " + dictionary2["baseURL"].ToString() + "\r\n contactURL: " +
                               dictionary2["contactURL"].ToString() + "\r\n";
                    updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = str2; }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Value + 38; }));
                    var filePassInfo = (Dictionary<string, object>) dictionary2["filePass"];
                    foreach (var a in filePassInfo) str += a.Key + ": " + a.Value.ToString() + "\r\n";
                    File.WriteAllText(gamedata.FullName + "webview.txt", str2);
                    updatestatus.Dispatcher.Invoke(new Action(() =>
                    {
                        updatestatus.Content = "Writing file to: " + gamedata.FullName + "webview.txt";
                    }));

                    updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = "下载完成，可以开始解析."; }));
                    progressbar.Dispatcher.Invoke(new Action(() => { progressbar.Value = progressbar.Maximum; }));
                    MessageBox.Show("下载完成，可以开始解析.", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
                    updatestatus.Dispatcher.Invoke(new Action(() => { updatestatus.Content = ""; }));
                    updatestatus.Dispatcher.Invoke(new Action(() => { updatesign.Content = ""; }));
                    progressbar.Dispatcher.Invoke(new Action(() =>
                    {
                        progressbar.Visibility = Visibility.Hidden;
                        updatedatabutton.IsEnabled = true;
                    }));
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var HTTPReq = new Thread(HttpRequestData);
            HTTPReq.Start();
        }

        private void JBOutput_Click(object sender, RoutedEventArgs e)
        {
            var path = Directory.GetCurrentDirectory();
            var folder = new DirectoryInfo(path + @"\Android\");
            var outputdir = new DirectoryInfo(path + @"\Output\");
            var output = "";
            output = "文本1:\n\r" + JB.JB1 + "\n\r" +
                     "文本2:\n\r" + JB.JB2 + "\n\r" +
                     "文本3:\n\r" + JB.JB3 + "\n\r" +
                     "文本4:\n\r" + JB.JB4 + "\n\r" +
                     "文本5:\n\r" + JB.JB5 + "\n\r" +
                     "文本6:\n\r" + JB.JB6 + "\n\r" +
                     "文本7:\n\r" + JB.JB7;
            if (!Directory.Exists(outputdir.FullName))
                Directory.CreateDirectory(outputdir.FullName);
            File.WriteAllText(outputdir.FullName + "羁绊文本_" + JB.svtid + "_" + JB.svtnme + ".txt", output);
            MessageBox.Show("导出完成.\n\r文件名为: " + outputdir.FullName + "羁绊文本_" + JB.svtid + "_" + JB.svtnme + ".txt",
                "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "具体数值显示的为文件中的原始数据，水平有限，无法进行有效解析。\r\n\r\n注:由于文本框高度过窄,\"宝具信息\"选项卡内具体数值前的等级Label(lv.x)双击后即可显示详细数据对话框.\r\n\"技能\"选项卡的函数列表为具体数值中每个/间对应的函数名称(双击Label\"函数列表\"也可查看详情.)\r\n\r\n【以下内容均为理论推测，仅适用于大部分结果。】\r\n/ 之间的为一个Buff的幅度\r\n1、如果为[a,b]则a为成功率(除以10就是百分比，如1000就是100%),b需要看技能描述，如果为出星或者生命值则b的大小即为幅度，若为NP，则将该数值除以100即为NP值。若b为1，则该组段可以忽略不看。\r\n2、如果为[a,b,c]或者[a,b,c,d]则在一般情况下a表示成功率(同1),b表示持续回合数即Turn,c表示次数(-1即为没有次数限制),d在大多数情况下除以10即为Buff幅度(%)，有时会有例外(可能也是没有意义).\r\n3、如果为[a,b,c,d,e]则a,b,c同2,d和e需要通过源文件进行详细手动分析。",
                "温馨提示:", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            // open URL

            var source = sender as Hyperlink;

            if (source != null) System.Diagnostics.Process.Start(source.NavigateUri.ToString());
        }

        private void Npvaluelv1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (npvaluelv1.Text == "") return;
            MessageBox.Show("宝具所有函数有(对应/之间的内容):\r\n" + SkillLvs.TDFuncstr + "\r\n\r\n以下列出一宝时所有Over Charge情况下的数据:\r\n" + npvaluelv1.Text, "宝具等级lv1详细数据", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void Npvaluelv2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (npvaluelv2.Text == "") return;
            MessageBox.Show("宝具所有函数有(对应/之间的内容):\r\n" + SkillLvs.TDFuncstr + "\r\n\r\n以下列出二宝时所有Over Charge情况下的数据:\r\n" + npvaluelv2.Text, "宝具等级lv2详细数据", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void Npvaluelv3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (npvaluelv3.Text == "") return;
            MessageBox.Show("宝具所有函数有(对应/之间的内容):\r\n" + SkillLvs.TDFuncstr + "\r\n\r\n以下列出三宝时所有Over Charge情况下的数据:\r\n" + npvaluelv3.Text, "宝具等级lv3详细数据", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void Npvaluelv4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (npvaluelv4.Text == "") return;
            MessageBox.Show("宝具所有函数有(对应/之间的内容):\r\n" + SkillLvs.TDFuncstr + "\r\n\r\n以下列出四宝时所有Over Charge情况下的数据:\r\n" + npvaluelv4.Text, "宝具等级lv4详细数据", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void Npvaluelv5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (npvaluelv5.Text == "") return;
            MessageBox.Show("宝具所有函数有(对应/之间的内容):\r\n" + SkillLvs.TDFuncstr + "\r\n\r\n以下列出五宝时所有Over Charge情况下的数据:\r\n" + npvaluelv5.Text, "宝具等级lv5详细数据", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void skl1Func_Clicked_Mission(object sender, MouseButtonEventArgs e)
        {
            if (skill1Funcs.Text == "") return;
            MessageBox.Show("以下列出1技能所有函数名称:\r\n" + skill1Funcs.Text, "1技能所有函数名称", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void skl2Func_Clicked_Mission(object sender, MouseButtonEventArgs e)
        {
            if (skill2Funcs.Text == "") return;
            MessageBox.Show("以下列出2技能所有函数名称:\r\n" + skill2Funcs.Text, "2技能所有函数名称", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void skl3Func_Clicked_Mission(object sender, MouseButtonEventArgs e)
        {
            if (skill3Funcs.Text == "") return;
            MessageBox.Show("以下列出3技能所有函数名称:\r\n" + skill3Funcs.Text, "3技能所有函数名称", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}