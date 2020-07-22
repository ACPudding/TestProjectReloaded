using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using FGOSBIAReloaded.Properties;
using MahApps.Metro.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FGOSBIAReloaded
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button1.IsEnabled = false;
            var SA = new Thread(StartAnalyze);
            SA.Start();
        }

        public void StartAnalyze()
        {
            textbox1.Dispatcher.Invoke(() => { textbox1.Text = Regex.Replace(textbox1.Text, @"\s", ""); });
            var svtID = "";
            var svtTDID = "";
            textbox1.Dispatcher.Invoke(() => { svtID = Convert.ToString(textbox1.Text); });
            JB.svtid = svtID;
            JB.JB1 = "";
            JB.JB2 = "";
            JB.JB3 = "";
            JB.JB4 = "";
            JB.JB5 = "";
            JB.JB6 = "";
            JB.JB7 = "";

            if (!Regex.IsMatch(svtID, "^\\d+$"))
            {
                MessageBox.Show("从者ID输入错误,请检查.", "温馨提示:", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearTexts();
                Button1.Dispatcher.Invoke(() => { Button1.IsEnabled = true; });
                return;
            }

            ClearTexts();
            textbox1.Dispatcher.Invoke(() => { textbox1.Text = svtID; });
            foreach (var svtTreasureDevicestmp in GlobalPathsAndDatas.mstSvtTreasureDevicedArray) //查找某个字段与值
            {
                if (((JObject) svtTreasureDevicestmp)["svtId"].ToString() == svtID &&
                    ((JObject) svtTreasureDevicestmp)["priority"].ToString() == "101")
                {
                    var mstsvtTDobjtmp = JObject.Parse(svtTreasureDevicestmp.ToString());
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

            var SCAC = new Thread(ServantCardsArrangementCheck);
            SCAC.Start();
            var SBIC = new Thread(ServantBasicInformationCheck);
            SBIC.Start();
            var SCIC = new Thread(ServantCVandIllustCheck);
            SCIC.Start();
            var SJTC = new Thread(ServantJibanTextCheck);
            SJTC.Start();
            var STDI = new Thread(ServantTreasureDeviceInformationCheck);
            STDI.Start(svtTDID);
            ServantTreasureDeviceSvalCheck(svtTDID);
            var SSIC = new Thread(ServantSkillInformationCheck);
            SSIC.Start();
            Button1.Dispatcher.Invoke(() => { Button1.IsEnabled = true; });
            Dispatcher.Invoke(() =>
            {
                if (rarity.Text == "")
                {
                    MessageBox.Show("从者ID不存在或未实装，请重试.", "温馨提示:", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearTexts();
                    Button1.IsEnabled = true;
                    return;
                }

                if (cards.Text == "[Q,Q,Q,Q,Q]" && svtclass.Text != "礼装")
                    MessageBox.Show("此ID为小怪(或部分boss以及种火芙芙),配卡、技能、宝具信息解析并不准确，请知悉.", "温馨提示:", MessageBoxButton.OK,
                        MessageBoxImage.Information);
            });
            GC.Collect();
        }

        private void ServantTreasureDeviceInformationCheck(object svtTDID)
        {
            var NPDetail = "unknown";
            var NPName = "";
            var NPrank = "";
            var NPruby = "";
            var NPtypeText = "";
            var svtNPDamageType = "";
            var svtNPCardhit = 1;
            var svtNPCardhitDamage = "";
            var svtNPCardType = "";
            var NPRateTD = 0.0;
            var NPRateArts = 0.0;
            var NPRateBuster = 0.0;
            var NPRateQuick = 0.0;
            var NPRateEX = 0.0;
            var NPRateDef = 0.0;
            foreach (var TDDtmp in GlobalPathsAndDatas.mstTreasureDeviceDetailArray) //查找某个字段与值
                if (((JObject) TDDtmp)["id"].ToString() == svtTDID.ToString())
                {
                    var TDDobjtmp = JObject.Parse(TDDtmp.ToString());
                    NPDetail = TDDobjtmp["detail"].ToString().Replace("[{0}]", " [Lv.1 - Lv.5] ").Replace("[g]", "")
                        .Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "").Replace(@"＆", "\r\n ＋")
                        .Replace(@"＋", "\r\n ＋").Replace("\r\n \r\n", "\r\n");
                    break;
                }

            foreach (var TDlvtmp in GlobalPathsAndDatas.mstTreasureDeviceLvArray) //查找某个字段与值
                if (((JObject) TDlvtmp)["treaureDeviceId"].ToString() == svtTDID.ToString())
                {
                    var TDlvobjtmp = JObject.Parse(TDlvtmp.ToString());
                    NPRateTD = Convert.ToDouble(TDlvobjtmp["tdPoint"].ToString());
                    NPRateArts = Convert.ToDouble(TDlvobjtmp["tdPointA"].ToString());
                    NPRateBuster = Convert.ToDouble(TDlvobjtmp["tdPointB"].ToString());
                    NPRateQuick = Convert.ToDouble(TDlvobjtmp["tdPointQ"].ToString());
                    NPRateEX = Convert.ToDouble(TDlvobjtmp["tdPointEx"].ToString());
                    NPRateDef = Convert.ToDouble(TDlvobjtmp["tdPointDef"].ToString());
                    break;
                }

            foreach (var TreasureDevicestmp in GlobalPathsAndDatas.mstTreasureDevicedArray) //查找某个字段与值
            {
                if (((JObject) TreasureDevicestmp)["id"].ToString() == svtTDID.ToString())
                {
                    var mstTDobjtmp = JObject.Parse(TreasureDevicestmp.ToString());
                    NPName = mstTDobjtmp["name"].ToString();
                    npname.Dispatcher.Invoke(() => { npname.Text = NPName; });
                    NPrank = mstTDobjtmp["rank"].ToString();
                    NPruby = mstTDobjtmp["ruby"].ToString();
                    npruby.Dispatcher.Invoke(() => { npruby.Text = NPruby; });
                    NPtypeText = mstTDobjtmp["typeText"].ToString();
                    nprank.Dispatcher.Invoke(() => { nprank.Text = NPrank + " ( " + NPtypeText + " ) "; });
                    svtNPDamageType = mstTDobjtmp["effectFlag"].ToString().Replace("0", "无伤害宝具")
                        .Replace("1", "群体宝具").Replace("2", "单体宝具");
                    nptype.Dispatcher.Invoke(() => { nptype.Text = svtNPDamageType; });

                    if (svtNPDamageType == "无伤害宝具")
                    {
                        svtNPCardhit = 0;
                        svtNPCardhitDamage = "[ - ]";
                    }

                    foreach (var svtTreasureDevicestmp in GlobalPathsAndDatas.mstSvtTreasureDevicedArray) //查找某个字段与值
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

                textbox1.Dispatcher.Invoke(() =>
                {
                    if (((JObject) TreasureDevicestmp)["seqId"].ToString() == textbox1.Text &&
                        ((JObject) TreasureDevicestmp)["ruby"].ToString() == "-" &&
                        ((JObject) TreasureDevicestmp)["id"].ToString().Length == 3)
                    {
                        var mstTDobjtmp2 = JObject.Parse(TreasureDevicestmp.ToString());
                        NPName = mstTDobjtmp2["name"].ToString();
                        npname.Dispatcher.Invoke(() => { npname.Text = NPName; });
                        NPrank = mstTDobjtmp2["rank"].ToString();
                        NPruby = mstTDobjtmp2["ruby"].ToString();
                        npruby.Dispatcher.Invoke(() => { npruby.Text = NPruby; });
                        NPtypeText = mstTDobjtmp2["typeText"].ToString();
                        nprank.Dispatcher.Invoke(() => { nprank.Text = NPrank + " ( " + NPtypeText + " ) "; });
                        svtNPDamageType = mstTDobjtmp2["effectFlag"].ToString().Replace("0", "无伤害宝具")
                            .Replace("1", "群体宝具").Replace("2", "单体宝具");
                        nptype.Dispatcher.Invoke(() => { nptype.Text = svtNPDamageType; });
                        if (svtNPDamageType == "-")
                        {
                            svtNPCardhit = 0;
                            svtNPCardhitDamage = "[ - ]";
                        }

                        NPDetail = "该ID的配卡与宝具解析不准确,请留意.";
                        foreach (var svtTreasureDevicestmp in GlobalPathsAndDatas.mstSvtTreasureDevicedArray) //查找某个字段与值
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
                    }
                });
            }

            nprate.Dispatcher.Invoke(() =>
            {
                nprate.Text = "Quick: " + (NPRateQuick / 10000).ToString("P") + "   Arts: " +
                              (NPRateArts / 10000).ToString("P") + "   Buster: " +
                              (NPRateBuster / 10000).ToString("P") + "\r\nExtra: " +
                              (NPRateEX / 10000).ToString("P") + "   宝具: " + (NPRateTD / 10000).ToString("P") +
                              "   受击: " + (NPRateDef / 10000).ToString("P");
            });
            treasuredevicescard.Dispatcher.Invoke(() =>
            {
                foreach (var c in svtNPCardhitDamage)
                    if (c == ',')
                        svtNPCardhit++;
                treasuredevicescard.Text = svtNPCardhit + " hit " + svtNPCardhitDamage;
            });
            npcardtype.Dispatcher.Invoke(() => { npcardtype.Text = svtNPCardType; });

            var newtmpid = "";
            if (NPDetail == "unknown")
            {
                foreach (var TreasureDevicestmp2 in GlobalPathsAndDatas.mstTreasureDevicedArray) //查找某个字段与值
                    if (((JObject) TreasureDevicestmp2)["name"].ToString() == NPName)
                    {
                        var TreasureDevicesobjtmp2 = JObject.Parse(TreasureDevicestmp2.ToString());
                        newtmpid = TreasureDevicesobjtmp2["id"].ToString();
                        if (newtmpid.Length == 6)
                        {
                            var FinTDID_TMP = newtmpid;
                            foreach (var TDDtmp2 in GlobalPathsAndDatas.mstTreasureDeviceDetailArray) //查找某个字段与值
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
                                foreach (var TDDtmp2 in GlobalPathsAndDatas.mstTreasureDeviceDetailArray) //查找某个字段与值
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
                newtmpid = svtTDID.ToString();
            }

            npdetail.Dispatcher.Invoke(() => { npdetail.Text = NPDetail; });
        }

        private void ServantSkillLevelCheck()
        {
            Dispatcher.Invoke(() =>
            {
                if (jibantext1.Text != "")
                {
                    SkillDetailCheck(SkillLvs.skillID1);
                    skill1cdlv1.Text = SkillLvs.skilllv1chargetime;
                    skill1cdlv6.Text = SkillLvs.skilllv6chargetime;
                    skill1cdlv10.Text = SkillLvs.skilllv10chargetime;
                    skill1valuelv1.Text = SkillLvs.skilllv1sval;
                    skill1valuelv6.Text = SkillLvs.skilllv6sval;
                    skill1valuelv10.Text = SkillLvs.skilllv10sval;
                    skill1Funcs.Text = SkillLvs.SKLFuncstr;
                    SkillDetailCheck(SkillLvs.skillID2);
                    skill2cdlv1.Text = SkillLvs.skilllv1chargetime;
                    skill2cdlv6.Text = SkillLvs.skilllv6chargetime;
                    skill2cdlv10.Text = SkillLvs.skilllv10chargetime;
                    skill2valuelv1.Text = SkillLvs.skilllv1sval;
                    skill2valuelv6.Text = SkillLvs.skilllv6sval;
                    skill2valuelv10.Text = SkillLvs.skilllv10sval;
                    skill2Funcs.Text = SkillLvs.SKLFuncstr;
                    SkillDetailCheck(SkillLvs.skillID3);
                    skill3cdlv1.Text = SkillLvs.skilllv1chargetime;
                    skill3cdlv6.Text = SkillLvs.skilllv6chargetime;
                    skill3cdlv10.Text = SkillLvs.skilllv10chargetime;
                    skill3valuelv1.Text = SkillLvs.skilllv1sval;
                    skill3valuelv6.Text = SkillLvs.skilllv6sval;
                    skill3valuelv10.Text = SkillLvs.skilllv10sval;
                    skill3Funcs.Text = SkillLvs.SKLFuncstr;
                }
            });
        }

        private void ServantBasicInformationCheck()
        {
            Dispatcher.Invoke(() =>
            {
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
                npratemagicbase[15] = 1.035;
                npratemagicbase[21] = 1;
                npratemagicbase[22] = 1.005;
                npratemagicbase[23] = 1.01;
                npratemagicbase[24] = 0.995;
                npratemagicbase[25] = 1.015;
                npratemagicbase[31] = 0.99;
                npratemagicbase[32] = 0.9925;
                npratemagicbase[33] = 0.995;
                npratemagicbase[34] = 0.985;
                npratemagicbase[35] = 0.9975;
                npratemagicbase[41] = 0.98;
                npratemagicbase[42] = 0.9825;
                npratemagicbase[43] = 0.985;
                npratemagicbase[44] = 0.975;
                npratemagicbase[45] = 0.9875;
                npratemagicbase[51] = 0.97;
                npratemagicbase[52] = 0.9725;
                npratemagicbase[53] = 0.975;
                npratemagicbase[54] = 0.965;
                npratemagicbase[55] = 0.9775;
                npratemagicbase[61] = 1.04;
                npratemagicbase[0] = 0.0;
                npratemagicbase[99] = 0.0;
                npratemagicbase[98] = 0.0;
                npratemagicbase[97] = 0.0;
                var svtstarrate = "unknown";
                double NPrate = 0;
                float starrate = 0;
                float deathrate = 0;
                var svtdeathrate = "unknown";
                var svtillust = "unknown"; //illustID 不输出
                var svtcv = "unknown"; //CVID 不输出
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
                var svtHideAttri = "unknown";
                string svtClassPassiveID;
                var classData = 0;
                var powerData = 0;
                var defenseData = 0;
                var agilityData = 0;
                var magicData = 0;
                var luckData = 0;
                var TreasureData = 0;
                var genderData = 0;
                var CardArrange = "[A,B,C,D,E]";
                var svtArtsCardQuantity = 0;
                foreach (var svtIDtmp in GlobalPathsAndDatas.mstSvtArray) //查找某个字段与值
                    if (((JObject) svtIDtmp)["id"].ToString() == JB.svtid)
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
                        svtcollectionid = mstSvtobjtmp["collectionNo"].ToString();
                        collection.Text = svtcollectionid;
                        svtHideAttri = mstSvtobjtmp["attri"].ToString().Replace("1", "人").Replace("2", "天")
                            .Replace("3", "地").Replace("4", "星").Replace("5", "兽");
                        CardArrange = mstSvtobjtmp["cardIds"].ToString().Replace("\n", "").Replace("\t", "")
                            .Replace("\r", "").Replace(" ", "").Replace("2", "B").Replace("1", "A").Replace("3", "Q");
                        cards.Text = CardArrange;
                        svtClassPassiveID = mstSvtobjtmp["classPassive"].ToString().Replace("\n", "").Replace("\t", "")
                            .Replace("\r", "").Replace(" ", "").Replace("[", "").Replace("]", "");
                        SkillLvs.ClassPassiveID = svtClassPassiveID;
                        var SCPSC = new Thread(ServantClassPassiveSkillCheck);
                        SCPSC.Start();
                        hiddenattri.Text = svtHideAttri;
                        classData = int.Parse(svtClass);
                        svtclass.Text = ClassName[classData];
                        genderData = int.Parse(svtgender);
                        gendle.Text = gender[genderData];
                        starrate = float.Parse(svtstarrate) / 10;
                        ssvtstarrate.Text = starrate + "%";
                        deathrate = float.Parse(svtdeathrate) / 10;
                        ssvtdeathrate.Text = deathrate + "%";
                        break;
                    }

                foreach (var svtLimittmp in GlobalPathsAndDatas.mstSvtLimitArray) //查找某个字段与值
                    if (((JObject) svtLimittmp)["svtId"].ToString() == JB.svtid)
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

                foreach (var c in CardArrange)
                    if (c == 'A')
                        svtArtsCardQuantity++;
                if (svtArtsCardQuantity == 0)
                {
                    NPrate = 0;
                    notrealnprate.Text = NPrate.ToString("P");
                }
                else
                {
                    NPrate = nprateclassbase[classData] * nprateartscount[svtArtsCardQuantity] *
                        npratemagicbase[magicData] / GlobalPathsAndDatas.svtArtsCardhit / 100;
                    NPrate = Math.Floor(NPrate * 10000) / 10000;
                    notrealnprate.Text = NPrate.ToString("P");
                }

                if (classData == 3)
                    Dispatcher.Invoke(() =>
                    {
                        atkbalance1.Content = "( x 1.05 △)";
                        atkbalance2.Content = "( x 1.05 △)";
                    });
                else if (classData == 5)
                    Dispatcher.Invoke(() =>
                    {
                        atkbalance1.Content = "( x 0.9 ▽)";
                        atkbalance2.Content = "( x 0.9 ▽)";
                    });
                else if (classData == 6)
                    Dispatcher.Invoke(() =>
                    {
                        atkbalance1.Content = "( x 0.9 ▽)";
                        atkbalance2.Content = "( x 0.9 ▽)";
                    });
                else if (classData == 2)
                    Dispatcher.Invoke(() =>
                    {
                        atkbalance1.Content = "( x 0.95 ▽)";
                        atkbalance2.Content = "( x 0.95 ▽)";
                    });
                else if (classData == 7)
                    Dispatcher.Invoke(() =>
                    {
                        atkbalance1.Content = "( x 1.1 △)";
                        atkbalance2.Content = "( x 1.1 △)";
                    });
                else if (classData == 9)
                    Dispatcher.Invoke(() =>
                    {
                        atkbalance1.Content = "( x 1.1 △)";
                        atkbalance2.Content = "( x 1.1 △)";
                    });
                else if (classData == 11)
                    Dispatcher.Invoke(() =>
                    {
                        atkbalance1.Content = "( x 1.1 △)";
                        atkbalance2.Content = "( x 1.1 △)";
                    });
                else if (classData == 1001)
                    MessageBox.Show("此ID为礼装ID,图鉴编号为礼装的图鉴编号.礼装描述在羁绊文本的文本1处,礼装效果在技能1栏中.", "温馨提示:", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                else
                    Dispatcher.Invoke(() =>
                    {
                        atkbalance1.Content = "( x 1.0 -)";
                        atkbalance2.Content = "( x 1.0 -)";
                    });
            });
        }

        private void ServantJibanTextCheck()
        {
            foreach (var SCTMP in GlobalPathsAndDatas.mstSvtCommentArray) //查找某个字段与值
            {
                if (((JObject) SCTMP)["svtId"].ToString() == JB.svtid && ((JObject) SCTMP)["id"].ToString() == "1")
                {
                    var SCobjtmp = JObject.Parse(SCTMP.ToString());
                    jibantext1.Dispatcher.Invoke(() =>
                    {
                        jibantext1.Text = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                        if (jibantext1.Text != "")
                            JBOutput.Dispatcher.Invoke(() => { JBOutput.IsEnabled = true; });
                    });
                    JB.JB1 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                }

                if (((JObject) SCTMP)["svtId"].ToString() == JB.svtid && ((JObject) SCTMP)["id"].ToString() == "2")
                {
                    var SCobjtmp = JObject.Parse(SCTMP.ToString());
                    jibantext2.Dispatcher.Invoke(() =>
                    {
                        jibantext2.Text = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                    });
                    JB.JB2 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                }

                if (((JObject) SCTMP)["svtId"].ToString() == JB.svtid && ((JObject) SCTMP)["id"].ToString() == "3")
                {
                    var SCobjtmp = JObject.Parse(SCTMP.ToString());
                    jibantext3.Dispatcher.Invoke(() =>
                    {
                        jibantext3.Text = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                    });
                    JB.JB3 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                }

                if (((JObject) SCTMP)["svtId"].ToString() == JB.svtid && ((JObject) SCTMP)["id"].ToString() == "4")
                {
                    var SCobjtmp = JObject.Parse(SCTMP.ToString());
                    jibantext4.Dispatcher.Invoke(() =>
                    {
                        jibantext4.Text = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                    });
                    JB.JB4 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                }

                if (((JObject) SCTMP)["svtId"].ToString() == JB.svtid && ((JObject) SCTMP)["id"].ToString() == "5")
                {
                    var SCobjtmp = JObject.Parse(SCTMP.ToString());
                    jibantext5.Dispatcher.Invoke(() =>
                    {
                        jibantext5.Text = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                    });
                    JB.JB5 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                }

                if (((JObject) SCTMP)["svtId"].ToString() == JB.svtid && ((JObject) SCTMP)["id"].ToString() == "6")
                {
                    var SCobjtmp = JObject.Parse(SCTMP.ToString());
                    jibantext6.Dispatcher.Invoke(() =>
                    {
                        jibantext6.Text = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                    });
                    JB.JB6 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                }

                if (((JObject) SCTMP)["svtId"].ToString() == JB.svtid && ((JObject) SCTMP)["id"].ToString() == "7")
                {
                    var SCobjtmp = JObject.Parse(SCTMP.ToString());
                    jibantext7.Dispatcher.Invoke(() =>
                    {
                        jibantext7.Text = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                    });
                    JB.JB7 = SCobjtmp["comment"].ToString().Replace("\n", "\r\n");
                }
            }
        }

        private void ServantCVandIllustCheck()
        {
            var svtillust = "unknown"; //illustID 不输出
            var svtcv = "unknown"; //CVID 不输出
            var svtCVName = "unknown";
            var svtILLUSTName = "unknown";
            foreach (var svtIDtmp in GlobalPathsAndDatas.mstSvtArray) //查找某个字段与值
                if (((JObject) svtIDtmp)["id"].ToString() == JB.svtid)
                {
                    var mstSvtobjtmp = JObject.Parse(svtIDtmp.ToString());
                    svtillust = mstSvtobjtmp["illustratorId"].ToString(); //illustID
                    svtcv = mstSvtobjtmp["cvId"].ToString(); //CVID
                    break;
                }

            foreach (var cvidtmp in GlobalPathsAndDatas.mstCvArray) //查找某个字段与值
                if (((JObject) cvidtmp)["id"].ToString() == svtcv)
                {
                    var mstCVobjtmp = JObject.Parse(cvidtmp.ToString());
                    svtCVName = mstCVobjtmp["name"].ToString();
                    cv.Dispatcher.Invoke(() => { cv.Text = svtCVName; });
                    break;
                }

            foreach (var illustidtmp in GlobalPathsAndDatas.mstIllustratorArray) //查找某个字段与值
                if (((JObject) illustidtmp)["id"].ToString() == svtillust)
                {
                    var mstillustobjtmp = JObject.Parse(illustidtmp.ToString());
                    svtILLUSTName = mstillustobjtmp["name"].ToString();
                    illust.Dispatcher.Invoke(() => { illust.Text = svtILLUSTName; });
                    break;
                }
        }

        private void ServantCardsArrangementCheck() //必须比basic先解析
        {
            var svtArtsCardhit = 1;
            var svtArtsCardhitDamage = "unknown";
            var svtArtsCardQuantity = 0;
            var svtBustersCardhit = 1;
            var svtBustersCardhitDamage = "unknown";
            var svtQuicksCardhit = 1;
            var svtQuicksCardhitDamage = "unknown";
            var svtExtraCardhit = 1;
            var svtExtraCardhitDamage = "unknown";
            GlobalPathsAndDatas.svtArtsCardhit = 1;
            foreach (var svtCardtmp in GlobalPathsAndDatas.mstSvtCardArray) //查找某个字段与值
            {
                if (((JObject) svtCardtmp)["svtId"].ToString() == JB.svtid &&
                    ((JObject) svtCardtmp)["cardId"].ToString() == "1")
                {
                    var mstSvtCardobjtmp = JObject.Parse(svtCardtmp.ToString());
                    svtArtsCardhitDamage = mstSvtCardobjtmp["normalDamage"].ToString().Replace("\n", "")
                        .Replace("\t", "").Replace("\r", "").Replace(" ", "");
                    foreach (var c in svtArtsCardhitDamage)
                        if (c == ',')
                            svtArtsCardhit++;
                    GlobalPathsAndDatas.svtArtsCardhit = svtArtsCardhit;
                    artscard.Dispatcher.Invoke(() =>
                    {
                        artscard.Text = svtArtsCardhit + " hit " + svtArtsCardhitDamage;
                    });
                }

                if (((JObject) svtCardtmp)["svtId"].ToString() == JB.svtid &&
                    ((JObject) svtCardtmp)["cardId"].ToString() == "2")
                {
                    var mstSvtCardobjtmp = JObject.Parse(svtCardtmp.ToString());
                    svtBustersCardhitDamage = mstSvtCardobjtmp["normalDamage"].ToString().Replace("\n", "")
                        .Replace("\t", "").Replace("\r", "").Replace(" ", "");
                    foreach (var c in svtBustersCardhitDamage)
                        if (c == ',')
                            svtBustersCardhit++;
                    bustercard.Dispatcher.Invoke(() =>
                    {
                        bustercard.Text = svtBustersCardhit + " hit " + svtBustersCardhitDamage;
                    });
                }

                if (((JObject) svtCardtmp)["svtId"].ToString() == JB.svtid &&
                    ((JObject) svtCardtmp)["cardId"].ToString() == "3")
                {
                    var mstSvtCardobjtmp = JObject.Parse(svtCardtmp.ToString());
                    svtQuicksCardhitDamage = mstSvtCardobjtmp["normalDamage"].ToString().Replace("\n", "")
                        .Replace("\t", "").Replace("\r", "").Replace(" ", "");
                    foreach (var c in svtQuicksCardhitDamage)
                        if (c == ',')
                            svtQuicksCardhit++;
                    quickcard.Dispatcher.Invoke(() =>
                    {
                        quickcard.Text = svtQuicksCardhit + " hit " + svtQuicksCardhitDamage;
                    });
                }

                if (((JObject) svtCardtmp)["svtId"].ToString() == JB.svtid &&
                    ((JObject) svtCardtmp)["cardId"].ToString() == "4")
                {
                    var mstSvtCardobjtmp = JObject.Parse(svtCardtmp.ToString());
                    svtExtraCardhitDamage = mstSvtCardobjtmp["normalDamage"].ToString().Replace("\n", "")
                        .Replace("\t", "").Replace("\r", "").Replace(" ", "");
                    foreach (var c in svtExtraCardhitDamage)
                        if (c == ',')
                            svtExtraCardhit++;
                    extracard.Dispatcher.Invoke(() =>
                    {
                        extracard.Text = svtExtraCardhit + " hit " + svtExtraCardhitDamage;
                    });
                }
            }
        }

        private void ServantTreasureDeviceSvalCheck(string svtTDID)
        {
            string svtTreasureDeviceFuncID;
            var svtTreasureDeviceFuncIDArray = new string[] { };
            List<string> svtTreasureDeviceFuncIDList;
            var svtTreasureDeviceFuncList = new List<string>();
            string[] svtTreasureDeviceFuncArray;
            var svtTreasureDeviceFunc = string.Empty;
            foreach (var TDLVtmp in GlobalPathsAndDatas.mstTreasureDeviceLvArray) //查找某个字段与值
            {
                if (((JObject) TDLVtmp)["treaureDeviceId"].ToString() == svtTDID &&
                    ((JObject) TDLVtmp)["lv"].ToString() == "1")
                {
                    var TDLVobjtmp = JObject.Parse(TDLVtmp.ToString());
                    npvaluelv1.Dispatcher.Invoke(() =>
                    {
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
                    });
                }

                if (((JObject) TDLVtmp)["treaureDeviceId"].ToString() == svtTDID &&
                    ((JObject) TDLVtmp)["lv"].ToString() == "2")
                {
                    var TDLVobjtmp = JObject.Parse(TDLVtmp.ToString());
                    npvaluelv2.Dispatcher.Invoke(() =>
                    {
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
                    });
                }

                if (((JObject) TDLVtmp)["treaureDeviceId"].ToString() == svtTDID &&
                    ((JObject) TDLVtmp)["lv"].ToString() == "3")
                {
                    var TDLVobjtmp = JObject.Parse(TDLVtmp.ToString());
                    npvaluelv3.Dispatcher.Invoke(() =>
                    {
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
                    });
                }

                if (((JObject) TDLVtmp)["treaureDeviceId"].ToString() == svtTDID &&
                    ((JObject) TDLVtmp)["lv"].ToString() == "4")
                {
                    var TDLVobjtmp = JObject.Parse(TDLVtmp.ToString());
                    npvaluelv4.Dispatcher.Invoke(() =>
                    {
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
                    });
                }

                if (((JObject) TDLVtmp)["treaureDeviceId"].ToString() == svtTDID &&
                    ((JObject) TDLVtmp)["lv"].ToString() == "5")
                {
                    var TDLVobjtmp = JObject.Parse(TDLVtmp.ToString());
                    npvaluelv5.Dispatcher.Invoke(() =>
                    {
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
                    });
                    svtTreasureDeviceFuncID = TDLVobjtmp["funcId"].ToString().Replace("\n", "").Replace("\t", "")
                        .Replace("\r", "").Replace(" ", "").Replace("[", "").Replace("]", "");
                    svtTreasureDeviceFuncIDList = new List<string>(svtTreasureDeviceFuncID.Split(','));
                    svtTreasureDeviceFuncIDArray = svtTreasureDeviceFuncIDList.ToArray();
                }
            }

            foreach (var skfuncidtmp in svtTreasureDeviceFuncIDArray)
            foreach (var functmp in GlobalPathsAndDatas.mstFuncArray)
                if (((JObject) functmp)["id"].ToString() == skfuncidtmp)
                {
                    var mstFuncobjtmp = JObject.Parse(functmp.ToString());
                    svtTreasureDeviceFuncList.Add(mstFuncobjtmp["popupText"].ToString());
                }

            svtTreasureDeviceFuncArray = svtTreasureDeviceFuncList.ToArray();
            svtTreasureDeviceFunc = string.Join(", ", svtTreasureDeviceFuncArray);
            SkillLvs.TDFuncstr = svtTreasureDeviceFunc;
        }

        private void ServantClassPassiveSkillCheck()
        {
            var svtClassPassiveIDArray = new string[] { };
            List<string> svtClassPassiveIDList;
            var svtClassPassiveList = new List<string>();
            string[] svtClassPassiveArray;
            var svtClassPassive = string.Empty;
            svtClassPassiveIDList = new List<string>(SkillLvs.ClassPassiveID.Split(','));
            svtClassPassiveIDArray = svtClassPassiveIDList.ToArray();
            foreach (var skilltmp in GlobalPathsAndDatas.mstSkillArray) //查找某个字段与值
            foreach (var classpassiveidtmp in svtClassPassiveIDArray)
                if (((JObject) skilltmp)["id"].ToString() == classpassiveidtmp)
                {
                    var mstsvtPskillobjtmp = JObject.Parse(skilltmp.ToString());
                    svtClassPassiveList.Add(mstsvtPskillobjtmp["name"].ToString());
                }

            svtClassPassiveArray = svtClassPassiveList.ToArray();
            svtClassPassive = string.Join(", ", svtClassPassiveArray);
            classskill.Dispatcher.Invoke(() => { classskill.Text = svtClassPassive; });
        }

        private void ServantSkillInformationCheck()
        {
            var skill1ID = "";
            var skill2ID = "";
            var skill3ID = "";
            var skill1Name = "";
            var skill2Name = "";
            var skill3Name = "";
            var skill1detail = "";
            var skill2detail = "";
            var skill3detail = "";
            foreach (var svtskill in GlobalPathsAndDatas.mstSvtSkillArray) //查找某个字段与值
            {
                if (((JObject) svtskill)["svtId"].ToString() == JB.svtid &&
                    ((JObject) svtskill)["num"].ToString() == "1" &&
                    ((JObject) svtskill)["priority"].ToString() == "1")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    skill1ID = mstsvtskillobjtmp["skillId"].ToString();
                }

                if (((JObject) svtskill)["svtId"].ToString() == JB.svtid &&
                    ((JObject) svtskill)["num"].ToString() == "1" &&
                    ((JObject) svtskill)["priority"].ToString() == "2")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    skill1ID = mstsvtskillobjtmp["skillId"].ToString();
                }

                if (((JObject) svtskill)["svtId"].ToString() == JB.svtid &&
                    ((JObject) svtskill)["num"].ToString() == "2" &&
                    ((JObject) svtskill)["priority"].ToString() == "1")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    skill2ID = mstsvtskillobjtmp["skillId"].ToString();
                }

                if (((JObject) svtskill)["svtId"].ToString() == JB.svtid &&
                    ((JObject) svtskill)["num"].ToString() == "2" &&
                    ((JObject) svtskill)["priority"].ToString() == "2")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    skill2ID = mstsvtskillobjtmp["skillId"].ToString();
                }

                if (((JObject) svtskill)["svtId"].ToString() == JB.svtid &&
                    ((JObject) svtskill)["num"].ToString() == "3" &&
                    ((JObject) svtskill)["priority"].ToString() == "1")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    skill3ID = mstsvtskillobjtmp["skillId"].ToString();
                }

                if (((JObject) svtskill)["svtId"].ToString() == JB.svtid &&
                    ((JObject) svtskill)["num"].ToString() == "3" &&
                    ((JObject) svtskill)["priority"].ToString() == "2")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    skill3ID = mstsvtskillobjtmp["skillId"].ToString();
                }
            }

            SkillLvs.skillID1 = skill1ID;
            SkillLvs.skillID2 = skill2ID;
            SkillLvs.skillID3 = skill3ID;
            var SSLC = new Thread(ServantSkillLevelCheck);
            SSLC.Start();

            Dispatcher.Invoke(() =>
            {
                foreach (var skilltmp in GlobalPathsAndDatas.mstSkillArray) //查找某个字段与值
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
                }

                foreach (var skillDetailtmp in GlobalPathsAndDatas.mstSkillDetailArray) //查找某个字段与值
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
            });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var OSI = new Thread(OutputSVTIDs);
            OSI.Start();
        }

        private void OutputSVTIDs()
        {
            Dispatcher.Invoke(() =>
            {
                var output = "";
                if (CheckNeededFiles.CheckDirectoryExists()) return;
                if (CheckNeededFiles.CheckFilesExists()) return;
                foreach (var svtIDtmp in GlobalPathsAndDatas.mstSvtArray) //查找某个字段与值
                    output = output + "ID: " + ((JObject) svtIDtmp)["id"] + "    " + "名称: " +
                             ((JObject) svtIDtmp)["name"] + "\r\n";
                File.WriteAllText(GlobalPathsAndDatas.path + "/SearchIDList.txt", output);
                MessageBox.Show("导出成功,文件名为 SearchIDList.txt", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
                Process.Start(GlobalPathsAndDatas.path + "/SearchIDList.txt");
            });
        }

        private void ClearTexts()
        {
            Dispatcher.Invoke(() =>
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
            });
            GC.Collect();
        }

        private void SkillDetailCheck(string sklid)
        {
            Dispatcher.Invoke(() =>
            {
                SkillLvs.skilllv1sval = "";
                SkillLvs.skilllv6sval = "";
                SkillLvs.skilllv10sval = "";
                SkillLvs.skilllv1chargetime = "";
                SkillLvs.skilllv6chargetime = "";
                SkillLvs.skilllv10chargetime = "";
                SkillLvs.SKLFuncstr = "";
                string svtSKFuncID;
                string[] svtSKFuncIDArray;
                List<string> svtSKFuncIDList;
                var svtSKFuncList = new List<string>();
                string[] svtSKFuncArray;
                var svtSKFunc = string.Empty;
                foreach (var SKLTMP in GlobalPathsAndDatas.mstSkillLvArray) //查找某个字段与值
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
                        foreach (var functmp in GlobalPathsAndDatas.mstFuncArray)
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
            });
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ClearTexts();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)

        {
            // open URL

            var source = sender as Hyperlink;

            if (source != null) Process.Start(source.NavigateUri.ToString());
        }

        private void HttpRequestData()
        {
            Button1.Dispatcher.Invoke(() => { Button1.IsEnabled = false; });
            updatedatabutton.Dispatcher.Invoke(() => { updatedatabutton.IsEnabled = false; });
            updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = ""; });
            updatestatus.Dispatcher.Invoke(() => { updatesign.Content = "数据下载进行中,请勿退出!"; });
            progressbar.Dispatcher.Invoke(() =>
            {
                progressbar.Value = 0;
                progressbar.Visibility = Visibility.Visible;
            });
            progressbar.Dispatcher.Invoke(() => { progressbar.Value = progressbar.Value + 250; });
            if (CheckNeededFiles.BeforeDownloadCheckFolderExists())
            {
                updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = "正在创建Android目录..."; });
                Directory.CreateDirectory(GlobalPathsAndDatas.folder.FullName);
            }

            if (!Directory.Exists(GlobalPathsAndDatas.gamedata.FullName))
                Directory.CreateDirectory(GlobalPathsAndDatas.gamedata.FullName);
            updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = "开始下载/更新游戏数据......"; });
            progressbar.Dispatcher.Invoke(() => { progressbar.Value = progressbar.Value + 250; });

            var result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top", "appVer=2.14.0");
            var res = JObject.Parse(result);
            if (res["response"][0]["fail"]["action"] != null)
            {
                if (res["response"][0]["fail"]["action"].ToString() == "app_version_up")
                {
                    var tmp = res["response"][0]["fail"]["detail"].ToString();
                    tmp = Regex.Replace(tmp, @".*新ver.：(.*)、現.*", "$1", RegexOptions.Singleline);
                    updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = "当前游戏版本: " + tmp; });

                    result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top",
                        "appVer=" + tmp);
                    res = JObject.Parse(result);
                }
                else if (res["response"][0]["fail"]["action"].ToString() == "maint")
                {
                    var tmp = res["response"][0]["fail"]["detail"].ToString();
                    if (MessageBox.Show(
                            "游戏服务器正在维护，请在维护后下载/更新数据. \r\n以下为服务器公告内容:\r\n\r\n『" +
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
                            Process.Start(url);
                        }
                    }

                    updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = ""; });
                    updatestatus.Dispatcher.Invoke(() => { updatesign.Content = ""; });
                    progressbar.Dispatcher.Invoke(() =>
                    {
                        progressbar.Visibility = Visibility.Hidden;
                        updatedatabutton.IsEnabled = true;
                    });
                    return;
                }
            }

            if (CheckNeededFiles.BeforeDownloadCheckAssetExists())
            {
                var fileinfo = GlobalPathsAndDatas.folder.GetFileSystemInfos(); //返回目录中所有文件和子目录
                foreach (var i in fileinfo)
                {
                    if (i is DirectoryInfo) //判断是否文件夹
                    {
                        var subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true); //删除子目录和文件
                        updatestatus.Dispatcher.Invoke(
                            () => { updatestatus.Content = "删除: " + subdir; });
                        continue;
                    }

                    i.Delete();
                    updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = "删除: " + i; });
                }
            }

            File.WriteAllText(GlobalPathsAndDatas.gamedata.FullName + "raw", result);
            File.WriteAllText(GlobalPathsAndDatas.gamedata.FullName + "assetbundle",
                res["response"][0]["success"]["assetbundle"].ToString());
            updatestatus.Dispatcher.Invoke(() =>
            {
                updatestatus.Content = "Writing file to: " + GlobalPathsAndDatas.gamedata.FullName + "assetbundle";
            });
            progressbar.Dispatcher.Invoke(() => { progressbar.Value = progressbar.Value + 40; });
            File.WriteAllText(GlobalPathsAndDatas.gamedata.FullName + "master",
                res["response"][0]["success"]["master"].ToString());
            updatestatus.Dispatcher.Invoke(() =>
            {
                updatestatus.Content = "Writing file to: " + GlobalPathsAndDatas.gamedata.FullName + "master";
            });
            progressbar.Dispatcher.Invoke(() => { progressbar.Value = progressbar.Value + 40; });
            File.WriteAllText(GlobalPathsAndDatas.gamedata.FullName + "webview",
                res["response"][0]["success"]["webview"].ToString());
            updatestatus.Dispatcher.Invoke(() =>
            {
                updatestatus.Content = "Writing file to: " + GlobalPathsAndDatas.gamedata.FullName + "webview";
            });
            progressbar.Dispatcher.Invoke(() => { progressbar.Value = progressbar.Value + 40; });
            var data = File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "master");
            if (!Directory.Exists(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata"))
                Directory.CreateDirectory(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata");
            var masterData =
                (Dictionary<string, byte[]>) MasterDataUnpacker.MouseGame2Unpacker(
                    Convert.FromBase64String(data));
            var job = new JObject();
            var miniMessagePacker = new MiniMessagePacker();
            foreach (var item in masterData)
            {
                var unpackeditem = (List<object>) miniMessagePacker.Unpack(item.Value);
                var json = JsonConvert.SerializeObject(unpackeditem, Formatting.Indented);
                File.WriteAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + item.Key, json);
                updatestatus.Dispatcher.Invoke(() =>
                {
                    updatestatus.Content = "Writing file to: " + GlobalPathsAndDatas.gamedata.FullName +
                                           "decrypted_masterdata\\" + item.Key;
                });
                progressbar.Dispatcher.Invoke(() => { progressbar.Value = progressbar.Value + 40; });
            }

            var data2 = File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "assetbundle");
            var dictionary =
                (Dictionary<string, object>) MasterDataUnpacker.MouseInfoMsgPack(
                    Convert.FromBase64String(data2));
            string str = null;
            foreach (var a in dictionary) str += a.Key + ": " + a.Value + "\r\n";
            File.WriteAllText(GlobalPathsAndDatas.gamedata.FullName + "assetbundle.txt", str);
            updatestatus.Dispatcher.Invoke(() =>
            {
                updatestatus.Content = "folder name: " + dictionary["folderName"];
            });
            progressbar.Dispatcher.Invoke(() => { progressbar.Value = progressbar.Value + 40; });
            var data3 = File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "webview");
            var dictionary2 =
                (Dictionary<string, object>) MasterDataUnpacker.MouseGame2MsgPack(
                    Convert.FromBase64String(data3));
            var str2 = "baseURL: " + dictionary2["baseURL"] + "\r\n contactURL: " +
                       dictionary2["contactURL"] + "\r\n";
            updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = str2; });
            progressbar.Dispatcher.Invoke(() => { progressbar.Value = progressbar.Value + 40; });
            var filePassInfo = (Dictionary<string, object>) dictionary2["filePass"];
            foreach (var a in filePassInfo) str += a.Key + ": " + a.Value + "\r\n";
            File.WriteAllText(GlobalPathsAndDatas.gamedata.FullName + "webview.txt", str2);
            updatestatus.Dispatcher.Invoke(() =>
            {
                updatestatus.Content = "Writing file to: " + GlobalPathsAndDatas.gamedata.FullName + "webview.txt";
            });

            updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = "下载完成，可以开始解析."; });

            progressbar.Dispatcher.Invoke(() => { progressbar.Value = progressbar.Maximum; });
            MessageBox.Show("下载完成，可以开始解析.", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
            updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = ""; });
            updatestatus.Dispatcher.Invoke(() => { updatesign.Content = ""; });
            progressbar.Dispatcher.Invoke(() =>
            {
                progressbar.Visibility = Visibility.Hidden;
                updatedatabutton.IsEnabled = true;
            });
            Button1.Dispatcher.Invoke(() => { Button1.IsEnabled = true; });
            GC.Collect();
        }


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var HTTPReq = new Thread(HttpRequestData);
            HTTPReq.Start();
        }

        private void JBOut()
        {
            var output = "";
            output = "文本1:\n\r" + JB.JB1 + "\n\r" +
                     "文本2:\n\r" + JB.JB2 + "\n\r" +
                     "文本3:\n\r" + JB.JB3 + "\n\r" +
                     "文本4:\n\r" + JB.JB4 + "\n\r" +
                     "文本5:\n\r" + JB.JB5 + "\n\r" +
                     "文本6:\n\r" + JB.JB6 + "\n\r" +
                     "文本7:\n\r" + JB.JB7;
            if (!Directory.Exists(GlobalPathsAndDatas.outputdir.FullName))
                Directory.CreateDirectory(GlobalPathsAndDatas.outputdir.FullName);
            File.WriteAllText(GlobalPathsAndDatas.outputdir.FullName + "羁绊文本_" + JB.svtid + "_" + JB.svtnme + ".txt",
                output);
            MessageBox.Show(
                "导出完成.\n\r文件名为: " + GlobalPathsAndDatas.outputdir.FullName + "羁绊文本_" + JB.svtid + "_" + JB.svtnme +
                ".txt",
                "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Process.Start(GlobalPathsAndDatas.outputdir.FullName + "/" + "羁绊文本_" + JB.svtid + "_" + JB.svtnme + ".txt");
        }

        private void JBOutput_Click(object sender, RoutedEventArgs e)
        {
            var JO = new Thread(JBOut);
            JO.Start();
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

            if (source != null) Process.Start(source.NavigateUri.ToString());
        }

        private void Npvaluelv1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (npvaluelv1.Text == "") return;
            MessageBox.Show(
                "宝具所有函数有(对应/之间的内容):\r\n" + SkillLvs.TDFuncstr + "\r\n\r\n以下列出一宝时所有Over Charge情况下的数据:\r\n" +
                npvaluelv1.Text, "宝具等级lv1详细数据", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void Npvaluelv2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (npvaluelv2.Text == "") return;
            MessageBox.Show(
                "宝具所有函数有(对应/之间的内容):\r\n" + SkillLvs.TDFuncstr + "\r\n\r\n以下列出二宝时所有Over Charge情况下的数据:\r\n" +
                npvaluelv2.Text, "宝具等级lv2详细数据", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void Npvaluelv3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (npvaluelv3.Text == "") return;
            MessageBox.Show(
                "宝具所有函数有(对应/之间的内容):\r\n" + SkillLvs.TDFuncstr + "\r\n\r\n以下列出三宝时所有Over Charge情况下的数据:\r\n" +
                npvaluelv3.Text, "宝具等级lv3详细数据", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void Npvaluelv4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (npvaluelv4.Text == "") return;
            MessageBox.Show(
                "宝具所有函数有(对应/之间的内容):\r\n" + SkillLvs.TDFuncstr + "\r\n\r\n以下列出四宝时所有Over Charge情况下的数据:\r\n" +
                npvaluelv4.Text, "宝具等级lv4详细数据", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void Npvaluelv5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (npvaluelv5.Text == "") return;
            MessageBox.Show(
                "宝具所有函数有(对应/之间的内容):\r\n" + SkillLvs.TDFuncstr + "\r\n\r\n以下列出五宝时所有Over Charge情况下的数据:\r\n" +
                npvaluelv5.Text, "宝具等级lv5详细数据", MessageBoxButton.OK,
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

        private void Form_Load(object sender, RoutedEventArgs e)
        {
            var path = Directory.GetCurrentDirectory();
            var gamedata = new DirectoryInfo(path + @"\Android\masterdata\");
            var folder = new DirectoryInfo(path + @"\Android\");
            VersionLabel.Content = CommonStrings.Version;
            if (!Directory.Exists(gamedata.FullName))
            {
                MessageBox.Show("没有游戏数据,请先下载游戏数据(位于\"关于\"选项卡).", "温馨提示:", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                Button1.IsEnabled = false;
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
                    !File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstFunc") ||
                    !File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSvtComment") ||
                    !File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDeviceLv") ||
                    !File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSkillLv"))
                {
                    MessageBox.Show("游戏数据损坏,请重新下载游戏数据(位于\"关于\"选项卡).", "温馨提示:", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    Button1.IsEnabled = false;
                }
            }
        }
    }
}