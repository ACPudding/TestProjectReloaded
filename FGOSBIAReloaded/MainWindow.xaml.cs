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
            /*
            ThreadStart childref2 = new ThreadStart(AnalyzeServant);
            Thread childThread2 = new Thread(childref2);
            childThread2.Start();
            */
        }

        public void AnalyzeServant()
        {
            /*
            Button1.Dispatcher.Invoke(new Action(() => 
            {
                Button1.IsEnabled = false;
            }));
            string path = System.IO.Directory.GetCurrentDirectory();
            DirectoryInfo gamedata = new DirectoryInfo(path + @"\Android\masterdata\");
            DirectoryInfo folder = new DirectoryInfo(path + @"\Android\");
            textbox1.Dispatcher.Invoke(new Action(() =>
            {
                textbox1.Text = Regex.Replace(textbox1.Text, @"\s", "");
            }));
            string svtID = Convert.ToString(textbox1.Text);
            if (!System.IO.Directory.Exists(gamedata.FullName))
            {
                MessageBox.Show("没有游戏数据,请先点击下方的按钮下载游戏数据.", "温馨提示:", MessageBoxButton.OK, MessageBoxImage.Information);
                Button1.Dispatcher.Invoke(new Action(() => {Button1.IsEnabled = true;}));
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
                    MessageBox.Show("游戏数据损坏,请先点击下方的按钮下载游戏数据.", "温馨提示:", MessageBoxButton.OK, MessageBoxImage.Error);
                    Button1.Dispatcher.Invoke(new Action(() => {Button1.IsEnabled = true;}));
                    return;
                }
            }
            if (!Regex.IsMatch(svtID, "^\\d+$"))
            {
                MessageBox.Show("从者ID输入错误,请检查.", "温馨提示:", MessageBoxButton.OK, MessageBoxImage.Error);
                Button1.Dispatcher.Invoke(new Action(() => {Button1.IsEnabled = true;}));
                return;
            }
            string mstSvt = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSvt");
            string mstSvtLimit = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSvtLimit");
            string mstCv = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstCv");
            string mstIllustrator = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstIllustrator");
            string mstSvtCard = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSvtCard");
            string mstSvtTreasureDevice = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSvtTreasureDevice");
            string mstTreasureDevice = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDevice");
            string mstTreasureDeviceDetail = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDeviceDetail");
            string mstSkill = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSkill");
            string mstSvtSkill = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSvtSkill");
            string mstSkillDetail = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSkillDetail");
            JArray mstSvtArray = (JArray)JsonConvert.DeserializeObject(mstSvt);
            JArray mstSvtLimitArray = (JArray)JsonConvert.DeserializeObject(mstSvtLimit);
            JArray mstCvArray = (JArray)JsonConvert.DeserializeObject(mstCv);
            JArray mstIllustratorArray = (JArray)JsonConvert.DeserializeObject(mstIllustrator);
            JArray mstSvtCardArray = (JArray)JsonConvert.DeserializeObject(mstSvtCard);
            JArray mstSvtTreasureDevicedArray = (JArray)JsonConvert.DeserializeObject(mstSvtTreasureDevice);
            JArray mstTreasureDevicedArray = (JArray)JsonConvert.DeserializeObject(mstTreasureDevice);
            JArray mstTreasureDeviceDetailArray = (JArray)JsonConvert.DeserializeObject(mstTreasureDeviceDetail);
            JArray mstSkillArray = (JArray)JsonConvert.DeserializeObject(mstSkill);
            JArray mstSvtSkillArray = (JArray)JsonConvert.DeserializeObject(mstSvtSkill);
            JArray mstSkillDetailArray = (JArray)JsonConvert.DeserializeObject(mstSkillDetail);
            if (!File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDeviceLv"))
            {
                MessageBox.Show("游戏数据损坏,请先点击下方的按钮下载游戏数据.", "温馨提示:", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string mstTreasureDeviceLv = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDeviceLv");
            JArray mstTreasureDeviceLvArray = (JArray)JsonConvert.DeserializeObject(mstTreasureDeviceLv);
            string[] PPK = new string[100];
            PPK[11] = "A"; PPK[12] = "A+"; PPK[13] = "A++"; PPK[14] = "A-"; PPK[21] = "B"; PPK[22] = "B+"; PPK[23] = "B++"; PPK[24] = "B-"; PPK[31] = "C"; PPK[32] = "C+"; PPK[33] = "C++"; PPK[34] = "C-"; PPK[41] = "D"; PPK[42] = "D+"; PPK[43] = "D++"; PPK[44] = "D-"; PPK[51] = "E"; PPK[52] = "E+"; PPK[53] = "E++"; PPK[54] = "E-"; PPK[61] = "EX"; PPK[98] = "-"; PPK[0] = "-";
            var svtName = "";
            var svtNameDisplay = "unknown";
            string[] ClassName = new string[1500];
            ClassName[1] = "Saber"; ClassName[2] = "Archer"; ClassName[3] = "Lancer"; ClassName[4] = "Rider"; ClassName[5] = "Caster"; ClassName[6] = "Assassin"; ClassName[7] = "Berserker"; ClassName[8] = "Shielder"; ClassName[9] = "Ruler"; ClassName[10] = "Alterego"; ClassName[11] = "Avenger"; ClassName[23] = "MoonCancer"; ClassName[25] = "Foreigner"; ClassName[20] = "Beast II"; ClassName[22] = "Beast I"; ClassName[24] = "Beast III/R"; ClassName[26] = "Beast III/L"; ClassName[27] = "Beast ?"; ClassName[97] = "不明"; ClassName[1001] = "礼装";
            ClassName[107] = "Berserker"; ClassName[21] = "?"; ClassName[19] = "?"; ClassName[18] = "?"; ClassName[17] = "Grand Caster"; ClassName[16] = "?"; ClassName[15] = "?"; ClassName[14] = "?"; ClassName[13] = "?"; ClassName[12] = "?";
            var svtClass = "unknown";//ClassID
            var svtgender = "unknown";
            string[] gender = new string[4];
            gender[1] = "男性"; gender[2] = "女性"; gender[3] = "其他";
            double[] nprateclassbase = new double[150];
            nprateclassbase[1] = 1.5; nprateclassbase[2] = 1.55; nprateclassbase[3] = 1.45; nprateclassbase[4] = 1.55; nprateclassbase[5] = 1.6; nprateclassbase[6] = 1.45; nprateclassbase[7] = 1.4; nprateclassbase[8] = 1.5; nprateclassbase[9] = 1.5; nprateclassbase[10] = 1.55; nprateclassbase[11] = 1.45; nprateclassbase[23] = 1.6; nprateclassbase[25] = 1.5; nprateclassbase[20] = 0.0; nprateclassbase[22] = 0.0; nprateclassbase[24] = 0.0; nprateclassbase[26] = 0.0; nprateclassbase[27] = 0.0; nprateclassbase[97] = 0.0;
            nprateclassbase[107] = 0.0; nprateclassbase[21] = 0.0; nprateclassbase[19] = 0.0; nprateclassbase[18] = 0.0; nprateclassbase[17] = 0.0; nprateclassbase[16] = 0.0; nprateclassbase[15] = 0.0; nprateclassbase[14] = 0.0; nprateclassbase[13] = 0.0; nprateclassbase[12] = 0.0;
            double[] nprateartscount = new double[4];
            nprateartscount[1] = 1.5; nprateartscount[2] = 1.125; nprateartscount[3] = 1;
            double[] npratemagicbase = new double[100];
            npratemagicbase[11] = 1.02; npratemagicbase[12] = 1.025; npratemagicbase[13] = 1.03; npratemagicbase[14] = 1.015; npratemagicbase[21] = 1; npratemagicbase[22] = 1.005; npratemagicbase[23] = 1.01; npratemagicbase[24] = 0.995; npratemagicbase[31] = 0.99; npratemagicbase[32] = 0.9925; npratemagicbase[33] = 0.995; npratemagicbase[34] = 0.985; npratemagicbase[41] = 0.98; npratemagicbase[42] = 0.9825; npratemagicbase[43] = 0.985; npratemagicbase[44] = 0.975; npratemagicbase[51] = 0.97; npratemagicbase[52] = 0.9725; npratemagicbase[53] = 0.975; npratemagicbase[54] = 0.965; npratemagicbase[61] = 1.04; npratemagicbase[0] = 0.0; npratemagicbase[99] = 0.0;
            var svtstarrate = "unknown";
            double NPrate = 0;
            float starrate = 0;
            float deathrate = 0;
            var svtdeathrate = "unknown";
            var svtillust = "unknown";//illustID 不输出
            var svtcv = "unknown"; ;//CVID 不输出
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
            double NPRateTD = 0.00;
            double NPRateArts = 0.00;
            double NPRateBuster = 0.00;
            double NPRateQuick = 0.00;
            double NPRateEX = 0.00;
            double NPRateDef = 0.00;
            string svtClassPassiveID;
            string[] svtClassPassiveIDArray = new string[] { };
            List<string> svtClassPassiveIDList;
            List<string> svtClassPassiveList = new List<string> { };
            string[] svtClassPassiveArray;
            string svtClassPassive = String.Empty;
            int svtArtsCardhit = 1;
            var svtArtsCardhitDamage = "unknown";
            int svtArtsCardQuantity = 0;
            int svtBustersCardhit = 1;
            var svtBustersCardhitDamage = "unknown";
            int svtQuicksCardhit = 1;
            var svtQuicksCardhitDamage = "unknown";
            int svtExtraCardhit = 1;
            var svtExtraCardhitDamage = "unknown";
            int svtNPCardhit = 1;
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
            int classData = 0;
            int powerData = 0;
            int defenseData = 0;
            int agilityData = 0;
            int magicData = 0;
            int luckData = 0;
            int TreasureData = 0;
            int genderData = 0;
            bool check = true;
            foreach (var svtIDtmp in mstSvtArray) //查找某个字段与值
            {
                if (((JObject)svtIDtmp)["id"].ToString() == svtID)
                {
                    var mstSvtobjtmp = JObject.Parse(svtIDtmp.ToString());
                    svtName = mstSvtobjtmp["name"].ToString();
                    svtNameDisplay = mstSvtobjtmp["battleName"].ToString();
                    svtClass = mstSvtobjtmp["classId"].ToString();
                    svtgender = mstSvtobjtmp["genderType"].ToString();
                    svtstarrate = mstSvtobjtmp["starRate"].ToString();
                    svtdeathrate = mstSvtobjtmp["deathRate"].ToString();
                    svtillust = mstSvtobjtmp["illustratorId"].ToString();//illustID
                    svtcv = mstSvtobjtmp["cvId"].ToString();//CVID
                    svtcollectionid = mstSvtobjtmp["collectionNo"].ToString();
                    svtClassPassiveID = mstSvtobjtmp["classPassive"].ToString().Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace(" ", "").Replace("[", "").Replace("]", "");
                    svtClassPassiveIDList = new List<string>(svtClassPassiveID.Split(','));
                    svtClassPassiveIDArray = svtClassPassiveIDList.ToArray();
                    svtHideAttri = mstSvtobjtmp["attri"].ToString().Replace("1", "人").Replace("2", "天").Replace("3", "地").Replace("4", "星").Replace("5", "兽");
                    CardArrange = mstSvtobjtmp["cardIds"].ToString().Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace(" ", "").Replace("2", "B").Replace("1", "A").Replace("3", "Q");
                    foreach (char c in CardArrange)
                    {
                        if (c == 'A')
                            svtArtsCardQuantity++;
                    }
                    classData = int.Parse(svtClass);
                    genderData = int.Parse(svtgender);
                    starrate = float.Parse(svtstarrate) / 10;
                    deathrate = float.Parse(svtdeathrate) / 10;
                    break;
                }
            }
            foreach (var svtCardtmp in mstSvtCardArray) //查找某个字段与值
            {
                if (((JObject)svtCardtmp)["svtId"].ToString() == svtID && ((JObject)svtCardtmp)["cardId"].ToString() == "1")
                {
                    var mstSvtCardobjtmp = JObject.Parse(svtCardtmp.ToString());
                    svtArtsCardhitDamage = mstSvtCardobjtmp["normalDamage"].ToString().Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace(" ", "");
                    foreach (char c in svtArtsCardhitDamage)
                    {
                        if (c == ',')
                            svtArtsCardhit++;
                    }
                }
                if (((JObject)svtCardtmp)["svtId"].ToString() == svtID && ((JObject)svtCardtmp)["cardId"].ToString() == "2")
                {
                    var mstSvtCardobjtmp = JObject.Parse(svtCardtmp.ToString());
                    svtBustersCardhitDamage = mstSvtCardobjtmp["normalDamage"].ToString().Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace(" ", "");
                    foreach (char c in svtBustersCardhitDamage)
                    {
                        if (c == ',')
                            svtBustersCardhit++;
                    }
                }
                if (((JObject)svtCardtmp)["svtId"].ToString() == svtID && ((JObject)svtCardtmp)["cardId"].ToString() == "3")
                {
                    var mstSvtCardobjtmp = JObject.Parse(svtCardtmp.ToString());
                    svtQuicksCardhitDamage = mstSvtCardobjtmp["normalDamage"].ToString().Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace(" ", "");
                    foreach (char c in svtQuicksCardhitDamage)
                    {
                        if (c == ',')
                            svtQuicksCardhit++;
                    }
                }
                if (((JObject)svtCardtmp)["svtId"].ToString() == svtID && ((JObject)svtCardtmp)["cardId"].ToString() == "4")
                {
                    var mstSvtCardobjtmp = JObject.Parse(svtCardtmp.ToString());
                    svtExtraCardhitDamage = mstSvtCardobjtmp["normalDamage"].ToString().Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace(" ", "");
                    foreach (char c in svtExtraCardhitDamage)
                    {
                        if (c == ',')
                            svtExtraCardhit++;
                    }
                }
            }
            foreach (var cvidtmp in mstCvArray) //查找某个字段与值
            {
                if (((JObject)cvidtmp)["id"].ToString() == svtcv)
                {
                    var mstCVobjtmp = JObject.Parse(cvidtmp.ToString());
                    svtCVName = mstCVobjtmp["name"].ToString();
                    break;
                }
            }
            foreach (var svtskill in mstSvtSkillArray) //查找某个字段与值
            {
                if (((JObject)svtskill)["svtId"].ToString() == svtID && ((JObject)svtskill)["num"].ToString() == "1" && ((JObject)svtskill)["priority"].ToString() == "1")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    skill1ID = mstsvtskillobjtmp["skillId"].ToString();
                }
                if (((JObject)svtskill)["svtId"].ToString() == svtID && ((JObject)svtskill)["num"].ToString() == "1" && ((JObject)svtskill)["priority"].ToString() == "2")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    skill1ID = mstsvtskillobjtmp["skillId"].ToString();
                }
                if (((JObject)svtskill)["svtId"].ToString() == svtID && ((JObject)svtskill)["num"].ToString() == "2" && ((JObject)svtskill)["priority"].ToString() == "1")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    skill2ID = mstsvtskillobjtmp["skillId"].ToString();
                }
                if (((JObject)svtskill)["svtId"].ToString() == svtID && ((JObject)svtskill)["num"].ToString() == "2" && ((JObject)svtskill)["priority"].ToString() == "2")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    skill2ID = mstsvtskillobjtmp["skillId"].ToString();
                }
                if (((JObject)svtskill)["svtId"].ToString() == svtID && ((JObject)svtskill)["num"].ToString() == "3" && ((JObject)svtskill)["priority"].ToString() == "1")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    skill3ID = mstsvtskillobjtmp["skillId"].ToString();
                }
                if (((JObject)svtskill)["svtId"].ToString() == svtID && ((JObject)svtskill)["num"].ToString() == "3" && ((JObject)svtskill)["priority"].ToString() == "2")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    skill3ID = mstsvtskillobjtmp["skillId"].ToString();
                }
            }
            foreach (var skilltmp in mstSkillArray) //查找某个字段与值
            {
                if (((JObject)skilltmp)["id"].ToString() == skill1ID)
                {
                    var skillobjtmp = JObject.Parse(skilltmp.ToString());
                    skill1Name = skillobjtmp["name"].ToString();
                }
                if (((JObject)skilltmp)["id"].ToString() == skill2ID)
                {
                    var skillobjtmp = JObject.Parse(skilltmp.ToString());
                    skill2Name = skillobjtmp["name"].ToString();
                }
                if (((JObject)skilltmp)["id"].ToString() == skill3ID)
                {
                    var skillobjtmp = JObject.Parse(skilltmp.ToString());
                    skill3Name = skillobjtmp["name"].ToString();
                }
                foreach (string classpassiveidtmp in svtClassPassiveIDArray)
                {
                    if (((JObject)skilltmp)["id"].ToString() == classpassiveidtmp)
                    {
                        var mstsvtPskillobjtmp = JObject.Parse(skilltmp.ToString());
                        svtClassPassiveList.Add(mstsvtPskillobjtmp["name"].ToString());
                    }
                }
            }
            foreach (var skillDetailtmp in mstSkillDetailArray) //查找某个字段与值
            {
                if (((JObject)skillDetailtmp)["id"].ToString() == skill1ID)
                {
                    var skillDetailobjtmp = JObject.Parse(skillDetailtmp.ToString());
                    skill1detail = skillDetailobjtmp["detail"].ToString().Replace("[{0}]", " [Lv.1 - Lv.10] ").Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "").Replace(@"＆", "\r\n ＋").Replace(@"＋", "\r\n ＋").Replace("\r\n \r\n", "\r\n");
                }
                if (((JObject)skillDetailtmp)["id"].ToString() == skill2ID)
                {
                    var skillDetailobjtmp = JObject.Parse(skillDetailtmp.ToString());
                    skill2detail = skillDetailobjtmp["detail"].ToString().Replace("[{0}]", " [Lv.1 - Lv.10] ").Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "").Replace(@"＆", "\r\n ＋").Replace(@"＋", "\r\n ＋").Replace("\r\n \r\n", "\r\n");
                }
                if (((JObject)skillDetailtmp)["id"].ToString() == skill3ID)
                {
                    var skillDetailobjtmp = JObject.Parse(skillDetailtmp.ToString());
                    skill3detail = skillDetailobjtmp["detail"].ToString().Replace("[{0}]", " [Lv.1 - Lv.10] ").Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "").Replace(@"＆", "\r\n ＋").Replace(@"＋", "\r\n ＋").Replace("\r\n \r\n", "\r\n");
                }
            }
            foreach (var svtTreasureDevicestmp in mstSvtTreasureDevicedArray) //查找某个字段与值
            {
                if (((JObject)svtTreasureDevicestmp)["svtId"].ToString() == svtID && ((JObject)svtTreasureDevicestmp)["priority"].ToString() == "101")
                {
                    var mstsvtTDobjtmp = JObject.Parse(svtTreasureDevicestmp.ToString());
                    svtNPCardhitDamage = mstsvtTDobjtmp["damage"].ToString().Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace(" ", "");
                    svtNPCardType = mstsvtTDobjtmp["cardId"].ToString().Replace("2", "Buster").Replace("1", "Arts").Replace("3", "Quick");
                    foreach (char c in svtNPCardhitDamage)
                    {
                        if (c == ',')
                            svtNPCardhit++;
                    }
                    svtTDID = mstsvtTDobjtmp["treasureDeviceId"].ToString();
                }
                if (((JObject)svtTreasureDevicestmp)["svtId"].ToString() == svtID && ((JObject)svtTreasureDevicestmp)["priority"].ToString() == "102")
                {
                    var mstsvtTDobjtmp = JObject.Parse(svtTreasureDevicestmp.ToString());
                    svtTDID = mstsvtTDobjtmp["treasureDeviceId"].ToString();
                }
                if (((JObject)svtTreasureDevicestmp)["svtId"].ToString() == svtID && ((JObject)svtTreasureDevicestmp)["priority"].ToString() == "103")
                {
                    var mstsvtTDobjtmp = JObject.Parse(svtTreasureDevicestmp.ToString());
                    svtTDID = mstsvtTDobjtmp["treasureDeviceId"].ToString();
                }
                if (((JObject)svtTreasureDevicestmp)["svtId"].ToString() == svtID && ((JObject)svtTreasureDevicestmp)["priority"].ToString() == "104")
                {
                    var mstsvtTDobjtmp = JObject.Parse(svtTreasureDevicestmp.ToString());
                    svtTDID = mstsvtTDobjtmp["treasureDeviceId"].ToString();
                }
                if (((JObject)svtTreasureDevicestmp)["svtId"].ToString() == svtID && ((JObject)svtTreasureDevicestmp)["priority"].ToString() == "105")
                {
                    var mstsvtTDobjtmp = JObject.Parse(svtTreasureDevicestmp.ToString());
                    svtTDID = mstsvtTDobjtmp["treasureDeviceId"].ToString();
                    break;
                }
            }
            foreach (var TDDtmp in mstTreasureDeviceDetailArray) //查找某个字段与值
            {
                if (((JObject)TDDtmp)["id"].ToString() == svtTDID)
                {
                    var TDDobjtmp = JObject.Parse(TDDtmp.ToString());
                    NPDetail = TDDobjtmp["detail"].ToString().Replace("[{0}]", " [Lv.1 - Lv.5] ").Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "").Replace(@"＆", "\r\n ＋").Replace(@"＋", "\r\n ＋").Replace("\r\n \r\n", "\r\n");
                    break;
                }
            }
            foreach (var TDlvtmp in mstTreasureDeviceLvArray) //查找某个字段与值
            {
                if (((JObject)TDlvtmp)["treaureDeviceId"].ToString() == svtTDID)
                {
                    var TDlvobjtmp = JObject.Parse(TDlvtmp.ToString());
                    NPRateTD = Convert.ToInt32(TDlvobjtmp["tdPoint"].ToString());
                    NPRateArts = Convert.ToInt32(TDlvobjtmp["tdPointA"].ToString());
                    NPRateBuster = Convert.ToInt32(TDlvobjtmp["tdPointB"].ToString());
                    NPRateQuick = Convert.ToInt32(TDlvobjtmp["tdPointQ"].ToString());
                    NPRateEX = Convert.ToInt32(TDlvobjtmp["tdPointEx"].ToString());
                    NPRateDef = Convert.ToInt32(TDlvobjtmp["tdPointDef"].ToString());
                    break;
                }
            }
            foreach (var illustidtmp in mstIllustratorArray) //查找某个字段与值
            {
                if (((JObject)illustidtmp)["id"].ToString() == svtillust)
                {
                    var mstillustobjtmp = JObject.Parse(illustidtmp.ToString());
                    svtILLUSTName = mstillustobjtmp["name"].ToString();
                    break;
                }
            }
            foreach (var svtLimittmp in mstSvtLimitArray) //查找某个字段与值
            {
                if (((JObject)svtLimittmp)["svtId"].ToString() == svtID)
                {
                    var mstsvtLimitobjtmp = JObject.Parse(svtLimittmp.ToString());
                    svtrarity = mstsvtLimitobjtmp["rarity"].ToString();
                    svthpBase = mstsvtLimitobjtmp["hpBase"].ToString();
                    svthpMax = mstsvtLimitobjtmp["hpMax"].ToString();
                    svtatkBase = mstsvtLimitobjtmp["atkBase"].ToString();
                    svtatkMax = mstsvtLimitobjtmp["atkMax"].ToString();
                    svtcriticalWeight = mstsvtLimitobjtmp["criticalWeight"].ToString();
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
                    break;
                }
            }
            foreach (var TreasureDevicestmp in mstTreasureDevicedArray) //查找某个字段与值
            {
                if (((JObject)TreasureDevicestmp)["id"].ToString() == svtTDID)
                {
                    var mstTDobjtmp = JObject.Parse(TreasureDevicestmp.ToString());
                    NPName = mstTDobjtmp["name"].ToString();
                    NPrank = mstTDobjtmp["rank"].ToString();
                    NPruby = mstTDobjtmp["ruby"].ToString();
                    NPtypeText = mstTDobjtmp["typeText"].ToString();
                    svtNPDamageType = mstTDobjtmp["effectFlag"].ToString().Replace("0", "无伤害宝具").Replace("1", "群体宝具").Replace("2", "单体宝具");
                    if (svtNPDamageType == "无伤害宝具")
                    {
                        svtNPCardhit = 0;
                        svtNPCardhitDamage = "[ - ]";
                    }
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button6.Enabled = true;
                    button9.Enabled = true;
                    button10.Enabled = true;
                    break;
                }
                if (((JObject)TreasureDevicestmp)["seqId"].ToString() == svtID && ((JObject)TreasureDevicestmp)["ruby"].ToString() == "-" && ((JObject)TreasureDevicestmp)["id"].ToString().Length == 3)
                {
                    var mstTDobjtmp2 = JObject.Parse(TreasureDevicestmp.ToString());
                    NPName = mstTDobjtmp2["name"].ToString();
                    NPrank = mstTDobjtmp2["rank"].ToString();
                    NPruby = mstTDobjtmp2["ruby"].ToString();
                    NPtypeText = mstTDobjtmp2["typeText"].ToString();
                    svtNPDamageType = mstTDobjtmp2["effectFlag"].ToString().Replace("0", "-").Replace("1", "群体宝具").Replace("2", "单体宝具");
                    if (svtNPDamageType == "-")
                    {
                        svtNPCardhit = 0;
                        svtNPCardhitDamage = "[ - ]";
                    }
                    NPDetail = "该ID的配卡与宝具解析不准确,请留意.";
                    foreach (var svtTreasureDevicestmp in mstSvtTreasureDevicedArray) //查找某个字段与值
                    {
                        if (((JObject)svtTreasureDevicestmp)["treasureDeviceId"].ToString() == ((JObject)TreasureDevicestmp)["id"].ToString())
                        {
                            var mstsvtTDobjtmp2 = JObject.Parse(svtTreasureDevicestmp.ToString());
                            svtNPCardhitDamage = mstsvtTDobjtmp2["damage"].ToString().Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace(" ", "");
                            svtNPCardType = mstsvtTDobjtmp2["cardId"].ToString().Replace("2", "Buster").Replace("1", "Arts").Replace("3", "Quick");
                            break;
                        }
                    }
                    button2.Enabled = false;
                    button3.Enabled = false;
                    button6.Enabled = false;
                    button9.Enabled = false;
                    button10.Enabled = false;
                    break;
                }
            }
            if (NPDetail == "unknown")
            {
                foreach (var TreasureDevicestmp2 in mstTreasureDevicedArray) //查找某个字段与值
                {
                    if (((JObject)TreasureDevicestmp2)["name"].ToString() == NPName)
                    {
                        var TreasureDevicesobjtmp2 = JObject.Parse(TreasureDevicestmp2.ToString());
                        var newtmpid = TreasureDevicesobjtmp2["id"].ToString();
                        if (newtmpid.Length == 6)
                        {
                            var FinTDID_TMP = newtmpid;
                            foreach (var TDDtmp2 in mstTreasureDeviceDetailArray) //查找某个字段与值
                            {
                                if (((JObject)TDDtmp2)["id"].ToString() == FinTDID_TMP)
                                {
                                    var TDDobjtmp2 = JObject.Parse(TDDtmp2.ToString());
                                    NPDetail = TDDobjtmp2["detail"].ToString().Replace("[{0}]", " [Lv.1 - Lv.5] ").Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "").Replace(@"＆", "\r\n ＋").Replace(@"＋", "\r\n ＋").Replace("\r\n \r\n", "\r\n");
                                    var TDDetailDisplay = NPDetail;
                                }
                            }
                        }
                        else if (newtmpid.Length == 7)
                        {
                            if (newtmpid.Substring(0, 2) == "10" || newtmpid.Substring(0, 2) == "11" || newtmpid.Substring(0, 2) == "23" || newtmpid.Substring(0, 2) == "25")
                            {
                                TreasureDevices.FinTDID_TMP = newtmpid;
                                foreach (var TDDtmp2 in mstTreasureDeviceDetailArray) //查找某个字段与值
                                {
                                    if (((JObject)TDDtmp2)["id"].ToString() == TreasureDevices.FinTDID_TMP)
                                    {
                                        var TDDobjtmp2 = JObject.Parse(TDDtmp2.ToString());
                                        NPDetail = TDDobjtmp2["detail"].ToString().Replace("[{0}]", " [Lv.1 - Lv.5] ").Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "").Replace(@"＆", "\r\n ＋").Replace(@"＋", "\r\n ＋").Replace("\r\n \r\n", "\r\n");
                                        TreasureDevices.TDDetailDisplay = NPDetail;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (svtArtsCardQuantity == 0)
            {
                NPrate = 0;
            }
            else
            {
                NPrate = nprateclassbase[classData] * nprateartscount[svtArtsCardQuantity] * npratemagicbase[magicData] / svtArtsCardhit / 100;
                NPrate = Math.Floor(NPrate * 10000) / 10000;
            }
            svtClassPassiveArray = svtClassPassiveList.ToArray();
            svtClassPassive = String.Join(", ", svtClassPassiveList.ToArray());

            if (svtrarity == "unknown")
            {
                MessageBox.Show("从者ID不存在或未实装，请重试.", "温馨提示:", MessageBoxButton.OK, MessageBoxImage.Information);
                Grid g = this.Content as Grid;
                UIElementCollection childrens = g.Children;
                foreach (UIElement ui in childrens)
                {
                    if (ui is TextBox)
                    {
                        (ui as TextBox).Text = "";
                    }
                }
                label41.Text = "";
                label44.Text = "";
                label43.Text = "";
                button2.Enabled = false;
                JibanStringData.str1 = "";
                JibanStringData.str2 = "";
                JibanStringData.str3 = "";
                JibanStringData.str4 = "";
                JibanStringData.str5 = "";
                JibanStringData.str6 = "";
                JibanStringData.str7 = "";
                button3.Enabled = false;
                button6.Enabled = false;
                button9.Enabled = false;
                button10.Enabled = false;
                Button1.Dispatcher.Invoke(new Action(() => {Button1.IsEnabled = true;}));
                return;
            }
            else
            {
                string path = System.IO.Directory.GetCurrentDirectory();
                DirectoryInfo gamedata = new DirectoryInfo(path + @"\Android\masterdata\");
                DirectoryInfo folder = new DirectoryInfo(path + @"\Android\");
                string svtID = Convert.ToString(textBox1.Text);
                if (!File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSvtComment"))
                {
                    MessageBox.Show("游戏数据损坏,请先点击下方的按钮下载游戏数据.", "温馨提示:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string mstSvtComment = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSvtComment");
                JArray mstSvtCommentArray = (JArray)JsonConvert.DeserializeObject(mstSvtComment);
                foreach (var SCTMP in mstSvtCommentArray) //查找某个字段与值
                {
                    if (((JObject)SCTMP)["svtId"].ToString() == svtID && ((JObject)SCTMP)["id"].ToString() == "1")
                    {
                        var SCobjtmp = JObject.Parse(SCTMP.ToString());
                        JibanStringData.str1 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                    }
                    if (((JObject)SCTMP)["svtId"].ToString() == svtID && ((JObject)SCTMP)["id"].ToString() == "2")
                    {
                        var SCobjtmp = JObject.Parse(SCTMP.ToString());
                        JibanStringData.str2 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                    }
                    if (((JObject)SCTMP)["svtId"].ToString() == svtID && ((JObject)SCTMP)["id"].ToString() == "3")
                    {
                        var SCobjtmp = JObject.Parse(SCTMP.ToString());
                        JibanStringData.str3 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                    }
                    if (((JObject)SCTMP)["svtId"].ToString() == svtID && ((JObject)SCTMP)["id"].ToString() == "4")
                    {
                        var SCobjtmp = JObject.Parse(SCTMP.ToString());
                        JibanStringData.str4 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                    }
                    if (((JObject)SCTMP)["svtId"].ToString() == svtID && ((JObject)SCTMP)["id"].ToString() == "5")
                    {
                        var SCobjtmp = JObject.Parse(SCTMP.ToString());
                        JibanStringData.str5 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                    }
                    if (((JObject)SCTMP)["svtId"].ToString() == svtID && ((JObject)SCTMP)["id"].ToString() == "6")
                    {
                        var SCobjtmp = JObject.Parse(SCTMP.ToString());
                        JibanStringData.str6 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                    }
                    if (((JObject)SCTMP)["svtId"].ToString() == svtID && ((JObject)SCTMP)["id"].ToString() == "7")
                    {
                        var SCobjtmp = JObject.Parse(SCTMP.ToString());
                        JibanStringData.str7 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                    }
                }
            }
            textBox2.Text = svtName;
            textBox3.Text = svtNameDisplay;
            textBox4.Text = ClassName[classData];
            if (classData == 3)
            {
                label44.Text = "( x 1.05 △)";
                label43.Text = "( x 1.05 △)";
            }
            else if (classData == 5)
            {
                label44.Text = "( x 0.9 ▽)";
                label43.Text = "( x 0.9 ▽)";
            }
            else if (classData == 6)
            {
                label44.Text = "( x 0.9 ▽)";
                label43.Text = "( x 0.9 ▽)";
            }
            else if (classData == 2)
            {
                label44.Text = "( x 0.95 ▽)";
                label43.Text = "( x 0.95 ▽)";
            }
            else if (classData == 7)
            {
                label44.Text = "( x 1.1 △)";
                label43.Text = "( x 1.1 △)";
            }
            else if (classData == 9)
            {
                label44.Text = "( x 1.1 △)";
                label43.Text = "( x 1.1 △)";
            }
            else if (classData == 11)
            {
                label44.Text = "( x 1.1 △)";
                label43.Text = "( x 1.1 △)";
            }
            else
            {
                label44.Text = "( x 1.0 -)";
                label43.Text = "( x 1.0 -)";
            }
            textBox5.Text = svtrarity + " ☆";
            textBox6.Text = gender[genderData];
            textBox7.Text = svtHideAttri;
            textBox8.Text = svtCVName;
            textBox9.Text = svtILLUSTName;
            textBox10.Text = svtcollectionid;
            textBox11.Text = starrate.ToString() + "%";
            textBox12.Text = deathrate.ToString() + "%";
            textBox13.Text = svtcriticalWeight;
            textBox14.Text = NPrate.ToString("P");
            textBox15.Text = svtClassPassive;
            textBox16.Text = svthpBase;
            textBox17.Text = svtatkBase;
            textBox18.Text = svthpMax;
            textBox19.Text = svtatkMax;
            textBox20.Text = CardArrange;
            textBox21.Text = svtArtsCardhit.ToString() + " hit " + svtArtsCardhitDamage;
            textBox22.Text = svtBustersCardhit.ToString() + " hit " + svtBustersCardhitDamage;
            textBox23.Text = svtQuicksCardhit.ToString() + " hit " + svtQuicksCardhitDamage;
            textBox24.Text = svtExtraCardhit.ToString() + " hit " + svtExtraCardhitDamage;
            textBox25.Text = svtNPCardhit.ToString() + " hit " + svtNPCardhitDamage;
            textBox26.Text = svtNPCardType;
            textBox27.Text = svtNPDamageType;
            textBox28.Text = NPrank + " ( " + NPtypeText + " ) ";
            textBox29.Text = NPruby;
            textBox30.Text = NPName;
            textBox31.Text = NPDetail;
            textBox32.Text = skill1Name;
            textBox33.Text = skill1detail;
            textBox34.Text = skill2detail;
            textBox35.Text = skill2Name;
            textBox36.Text = skill3detail;
            textBox37.Text = skill3Name;
            textBox38.Text = "Quick: " + (NPRateQuick / 10000).ToString("P") + "   Arts: " + (NPRateArts / 10000).ToString("P") + "   Buster: " + (NPRateBuster / 10000).ToString("P") + "\r\nExtra: " + (NPRateEX / 10000).ToString("P") + "   宝具: " + (NPRateTD / 10000).ToString("P") + "   受击: " + (NPRateDef / 10000).ToString("P");
            label41.Text = "筋力: " + PPK[powerData] + "    耐久: " + PPK[defenseData] + "    敏捷: " + PPK[agilityData] +
                    "\n魔力: " + PPK[magicData] + "    幸运: " + PPK[luckData] + "    宝具: " + PPK[TreasureData];
            Button1.Dispatcher.Invoke(new Action(() => {Button1.IsEnabled = true;}));
            SkillLvs.svtid = svtID;
            */
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
