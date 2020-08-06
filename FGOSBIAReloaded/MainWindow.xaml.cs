﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
using OfficeOpenXml;

namespace FGOSBIAReloaded
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button1.IsEnabled = false;
            textbox1.Text = Regex.Replace(textbox1.Text, @"\s", "");
            if (!Regex.IsMatch(textbox1.Text, "^\\d+$"))
            {
                MessageBox.Show("从者ID输入错误,请检查.", "温馨提示:", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearTexts();
                Button1.Dispatcher.Invoke(() => { Button1.IsEnabled = true; });
                return;
            }

            var SA = new Thread(StartAnalyze);
            SA.Start();
        }

        private void StartAnalyze()
        {
            var svtID = "";
            var svtTDID = "";
            var SCAC = new Thread(ServantCardsArrangementCheck);
            var SBIC = new Thread(ServantBasicInformationCheck);
            var SCIC = new Thread(ServantCVandIllustCheck);
            var SJTC = new Thread(ServantJibanTextCheck);
            var STDI = new Thread(ServantTreasureDeviceInformationCheck);
            var SSIC = new Thread(ServantSkillInformationCheck);
            SkillLvs.skillID1 = "";
            SkillLvs.skillID2 = "";
            SkillLvs.skillID3 = "";
            IsNPStrengthened.Dispatcher.Invoke(() => { IsNPStrengthened.Text = "×"; });
            textbox1.Dispatcher.Invoke(() => { svtID = Convert.ToString(textbox1.Text); });
            JB.svtid = svtID;
            JB.JB1 = "";
            JB.JB2 = "";
            JB.JB3 = "";
            JB.JB4 = "";
            JB.JB5 = "";
            JB.JB6 = "";
            JB.JB7 = "";
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
                    IsNPStrengthened.Dispatcher.Invoke(() => { IsNPStrengthened.Text = "√"; });
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

                if (((JObject) svtTreasureDevicestmp)["svtId"].ToString() != svtID ||
                    ((JObject) svtTreasureDevicestmp)["priority"].ToString() != "105") continue;
                {
                    var mstsvtTDobjtmp = JObject.Parse(svtTreasureDevicestmp.ToString());
                    svtTDID = mstsvtTDobjtmp["treasureDeviceId"].ToString();
                    break;
                }
            }

            SCAC.Start();
            SBIC.Start();
            SCIC.Start();
            SJTC.Start();
            STDI.Start(svtTDID);
            ServantTreasureDeviceSvalCheck(svtTDID);
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
                    if (((JObject) TreasureDevicestmp)["seqId"].ToString() != textbox1.Text ||
                        ((JObject) TreasureDevicestmp)["ruby"].ToString() != "-" ||
                        ((JObject) TreasureDevicestmp)["id"].ToString().Length != 3) return;
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
                svtNPCardhit += svtNPCardhitDamage.Count(c => c == ',');
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
                        switch (newtmpid.Length)
                        {
                            case 6:
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
                                    }

                                break;
                            }
                            case 7:
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

                                break;
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
                if (jibantext1.Text == "") return;
                SkillDetailCheck(SkillLvs.skillID1);
                skill1cdlv1.Text = SkillLvs.skilllv1chargetime;
                skill1cdlv6.Text = SkillLvs.skilllv6chargetime;
                skill1cdlv10.Text = SkillLvs.skilllv10chargetime;
                for (var i = 0; i <= SkillLvs.SKLFuncstrArray.Length - 1; i++)
                {
                    if (SkillLvs.SKLFuncstrArray[i] == "") SkillLvs.SKLFuncstrArray[i] = "HP回復";
                    Skill1FuncList.Items.Add(new Skill1ListSval(SkillLvs.SKLFuncstrArray[i],
                        SkillLvs.skilllv1svalArray[i], SkillLvs.skilllv6svalArray[i], SkillLvs.skilllv10svalArray[i]));
                }

                SkillDetailCheck(SkillLvs.skillID2);
                skill2cdlv1.Text = SkillLvs.skilllv1chargetime;
                skill2cdlv6.Text = SkillLvs.skilllv6chargetime;
                skill2cdlv10.Text = SkillLvs.skilllv10chargetime;
                for (var i = 0; i <= SkillLvs.SKLFuncstrArray.Length - 1; i++)
                {
                    if (SkillLvs.SKLFuncstrArray[i] == "") SkillLvs.SKLFuncstrArray[i] = "HP回復";
                    Skill2FuncList.Items.Add(new Skill2ListSval(SkillLvs.SKLFuncstrArray[i],
                        SkillLvs.skilllv1svalArray[i], SkillLvs.skilllv6svalArray[i], SkillLvs.skilllv10svalArray[i]));
                }

                SkillDetailCheck(SkillLvs.skillID3);
                skill3cdlv1.Text = SkillLvs.skilllv1chargetime;
                skill3cdlv6.Text = SkillLvs.skilllv6chargetime;
                skill3cdlv10.Text = SkillLvs.skilllv10chargetime;
                for (var i = 0; i <= SkillLvs.SKLFuncstrArray.Length - 1; i++)
                {
                    if (SkillLvs.SKLFuncstrArray[i] == "") SkillLvs.SKLFuncstrArray[i] = "HP回復";
                    Skill3FuncList.Items.Add(new Skill3ListSval(SkillLvs.SKLFuncstrArray[i],
                        SkillLvs.skilllv1svalArray[i], SkillLvs.skilllv6svalArray[i], SkillLvs.skilllv10svalArray[i]));
                }
            });
        }

        private void ServantBasicInformationCheck()
        {
            Dispatcher.Invoke(() =>
            {
                var RankString = new string[100];
                RankString[11] = "A";
                RankString[12] = "A+";
                RankString[13] = "A++";
                RankString[14] = "A-";
                RankString[15] = "A+++";
                RankString[21] = "B";
                RankString[22] = "B+";
                RankString[23] = "B++";
                RankString[24] = "B-";
                RankString[25] = "B+++";
                RankString[31] = "C";
                RankString[32] = "C+";
                RankString[33] = "C++";
                RankString[34] = "C-";
                RankString[35] = "C+++";
                RankString[41] = "D";
                RankString[42] = "D+";
                RankString[43] = "D++";
                RankString[44] = "D-";
                RankString[45] = "D+++";
                RankString[51] = "E";
                RankString[52] = "E+";
                RankString[53] = "E++";
                RankString[54] = "E-";
                RankString[55] = "E+++";
                RankString[61] = "EX";
                RankString[98] = "-";
                RankString[0] = "-";
                RankString[99] = "?";
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
                GlobalPathsAndDatas.askxlsx = true;
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
                        if (CardArrange == "[Q,Q,Q,Q,Q]")
                            GlobalPathsAndDatas.askxlsx = false;
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
                        sixwei.Content = "筋力: " + RankString[powerData] + "        耐久: " + RankString[defenseData] +
                                         "\n敏捷: " +
                                         RankString[agilityData] +
                                         "        魔力: " + RankString[magicData] + "\n幸运: " + RankString[luckData] +
                                         "        宝具: " +
                                         RankString[TreasureData];
                        break;
                    }

                var svtArtsCardQuantity = CardArrange.Count(c => c == 'A');
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

                switch (classData)
                {
                    case 1:
                    case 4:
                    case 8:
                    case 10:
                    case 20:
                    case 22:
                    case 24:
                    case 26:
                    case 23:
                    case 25:
                    case 17:
                        Dispatcher.Invoke(() =>
                        {
                            atkbalance1.Content = "( x 1.0 -)";
                            atkbalance2.Content = "( x 1.0 -)";
                            if (ToggleMsgboxOutputCheck.IsChecked != true || !GlobalPathsAndDatas.askxlsx) return;
                            if (MessageBox.Show(
                                    "是否需要以xlsx的形式导出该从者的基础数据?",
                                    "导出?", MessageBoxButton.OKCancel, MessageBoxImage.Information) ==
                                MessageBoxResult.OK)
                                ExcelFileOutput();
                        });
                        break;
                    case 3:
                        Dispatcher.Invoke(() =>
                        {
                            atkbalance1.Content = "( x 1.05 △)";
                            atkbalance2.Content = "( x 1.05 △)";
                            if (ToggleMsgboxOutputCheck.IsChecked != true || !GlobalPathsAndDatas.askxlsx) return;
                            if (MessageBox.Show(
                                    "是否需要以xlsx的形式导出该从者的基础数据?",
                                    "导出?", MessageBoxButton.OKCancel, MessageBoxImage.Information) ==
                                MessageBoxResult.OK)
                                ExcelFileOutput();
                        });
                        break;
                    case 5:
                    case 6:
                        Dispatcher.Invoke(() =>
                        {
                            atkbalance1.Content = "( x 0.9 ▽)";
                            atkbalance2.Content = "( x 0.9 ▽)";
                            if (ToggleMsgboxOutputCheck.IsChecked != true || !GlobalPathsAndDatas.askxlsx) return;
                            if (MessageBox.Show(
                                    "是否需要以xlsx的形式导出该从者的基础数据?",
                                    "导出?", MessageBoxButton.OKCancel, MessageBoxImage.Information) ==
                                MessageBoxResult.OK)
                                ExcelFileOutput();
                        });
                        break;
                    case 2:
                        Dispatcher.Invoke(() =>
                        {
                            atkbalance1.Content = "( x 0.95 ▽)";
                            atkbalance2.Content = "( x 0.95 ▽)";
                            if (ToggleMsgboxOutputCheck.IsChecked != true || !GlobalPathsAndDatas.askxlsx) return;
                            if (MessageBox.Show(
                                    "是否需要以xlsx的形式导出该从者的基础数据?",
                                    "导出?", MessageBoxButton.OKCancel, MessageBoxImage.Information) ==
                                MessageBoxResult.OK)
                                ExcelFileOutput();
                        });
                        break;
                    case 7:
                    case 9:
                    case 11:
                        Dispatcher.Invoke(() =>
                        {
                            atkbalance1.Content = "( x 1.1 △)";
                            atkbalance2.Content = "( x 1.1 △)";
                            if (ToggleMsgboxOutputCheck.IsChecked != true || !GlobalPathsAndDatas.askxlsx) return;
                            if (MessageBox.Show(
                                    "是否需要以xlsx的形式导出该从者的基础数据?",
                                    "导出?", MessageBoxButton.OKCancel, MessageBoxImage.Information) ==
                                MessageBoxResult.OK)
                                ExcelFileOutput();
                        });
                        break;
                    case 1001:
                        MessageBox.Show("此ID为礼装ID,图鉴编号为礼装的图鉴编号.礼装描述在羁绊文本的文本1处,礼装效果在技能1栏中.", "温馨提示:",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                        break;
                    default:
                        Dispatcher.Invoke(() =>
                        {
                            atkbalance1.Content = "( x 1.0 -)";
                            atkbalance2.Content = "( x 1.0 -)";
                        });
                        break;
                }
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

                if (((JObject) SCTMP)["svtId"].ToString() != JB.svtid ||
                    ((JObject) SCTMP)["id"].ToString() != "7") continue;
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
                    svtArtsCardhit += svtArtsCardhitDamage.Count(c => c == ',');
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
                    svtBustersCardhit += svtBustersCardhitDamage.Count(c => c == ',');
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
                    svtQuicksCardhit += svtQuicksCardhitDamage.Count(c => c == ',');
                    quickcard.Dispatcher.Invoke(() =>
                    {
                        quickcard.Text = svtQuicksCardhit + " hit " + svtQuicksCardhitDamage;
                    });
                }

                if (((JObject) svtCardtmp)["svtId"].ToString() != JB.svtid ||
                    ((JObject) svtCardtmp)["cardId"].ToString() != "4") continue;
                {
                    var mstSvtCardobjtmp = JObject.Parse(svtCardtmp.ToString());
                    svtExtraCardhitDamage = mstSvtCardobjtmp["normalDamage"].ToString().Replace("\n", "")
                        .Replace("\t", "").Replace("\r", "").Replace(" ", "");
                    svtExtraCardhit += svtExtraCardhitDamage.Count(c => c == ',');
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

                if (((JObject) TDLVtmp)["treaureDeviceId"].ToString() != svtTDID ||
                    ((JObject) TDLVtmp)["lv"].ToString() != "5") continue;
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

            svtTreasureDeviceFuncArray = (from skfuncidtmp in svtTreasureDeviceFuncIDArray
                from functmp in GlobalPathsAndDatas.mstFuncArray
                where ((JObject) functmp)["id"].ToString() == skfuncidtmp
                select JObject.Parse(functmp.ToString())
                into mstFuncobjtmp
                select mstFuncobjtmp["popupText"].ToString()).ToArray();
            svtTreasureDeviceFunc = string.Join(", ", svtTreasureDeviceFuncArray);
            SkillLvs.TDFuncstr = svtTreasureDeviceFunc;
        }

        private void ServantClassPassiveSkillCheck()
        {
            var svtClassPassiveIDArray = new string[] { };
            List<string> svtClassPassiveIDList;
            string[] svtClassPassiveArray;
            var svtClassPassive = string.Empty;
            svtClassPassiveIDList = new List<string>(SkillLvs.ClassPassiveID.Split(','));
            svtClassPassiveIDArray = svtClassPassiveIDList.ToArray();

            svtClassPassiveArray = (from skilltmp in GlobalPathsAndDatas.mstSkillArray
                from classpassiveidtmp in svtClassPassiveIDArray
                where ((JObject) skilltmp)["id"].ToString() == classpassiveidtmp
                select JObject.Parse(skilltmp.ToString())
                into mstsvtPskillobjtmp
                select mstsvtPskillobjtmp["name"].ToString()).ToArray();
            svtClassPassive = string.Join(", ", svtClassPassiveArray);
            classskill.Dispatcher.Invoke(() => { classskill.Text = svtClassPassive; });
        }

        private void ServantSkillInformationCheck()
        {
            var skill1Name = string.Empty;
            var skill2Name = string.Empty;
            var skill3Name = string.Empty;
            var skill1detail = string.Empty;
            var skill2detail = string.Empty;
            var skill3detail = string.Empty;
            var sk1IsStrengthened = false;
            var sk2IsStrengthened = false;
            var sk3IsStrengthened = false;
            IsSk1Strengthened.Dispatcher.Invoke(() => { IsSk1Strengthened.Text = "×"; });
            IsSk2Strengthened.Dispatcher.Invoke(() => { IsSk2Strengthened.Text = "×"; });
            IsSk3Strengthened.Dispatcher.Invoke(() => { IsSk3Strengthened.Text = "×"; });
            foreach (var svtskill in GlobalPathsAndDatas.mstSvtSkillArray) //查找某个字段与值
            {
                if (((JObject) svtskill)["svtId"].ToString() == JB.svtid &&
                    ((JObject) svtskill)["num"].ToString() == "1" &&
                    ((JObject) svtskill)["priority"].ToString() == "1")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    SkillLvs.skillID1 = mstsvtskillobjtmp["skillId"].ToString();
                }

                if (((JObject) svtskill)["svtId"].ToString() == JB.svtid &&
                    ((JObject) svtskill)["num"].ToString() == "1" &&
                    ((JObject) svtskill)["priority"].ToString() == "2")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    SkillLvs.skillID1 = mstsvtskillobjtmp["skillId"].ToString();
                    IsSk1Strengthened.Dispatcher.Invoke(() => { IsSk1Strengthened.Text = "√"; });
                    sk1IsStrengthened = true;
                }

                if (((JObject) svtskill)["svtId"].ToString() == JB.svtid &&
                    ((JObject) svtskill)["num"].ToString() == "2" &&
                    ((JObject) svtskill)["priority"].ToString() == "1")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    SkillLvs.skillID2 = mstsvtskillobjtmp["skillId"].ToString();
                }

                if (((JObject) svtskill)["svtId"].ToString() == JB.svtid &&
                    ((JObject) svtskill)["num"].ToString() == "2" &&
                    ((JObject) svtskill)["priority"].ToString() == "2")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    SkillLvs.skillID2 = mstsvtskillobjtmp["skillId"].ToString();
                    IsSk2Strengthened.Dispatcher.Invoke(() => { IsSk2Strengthened.Text = "√"; });
                    sk2IsStrengthened = true;
                }

                if (((JObject) svtskill)["svtId"].ToString() == JB.svtid &&
                    ((JObject) svtskill)["num"].ToString() == "3" &&
                    ((JObject) svtskill)["priority"].ToString() == "1")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    SkillLvs.skillID3 = mstsvtskillobjtmp["skillId"].ToString();
                }

                if (((JObject) svtskill)["svtId"].ToString() != JB.svtid ||
                    ((JObject) svtskill)["num"].ToString() != "3" ||
                    ((JObject) svtskill)["priority"].ToString() != "2") continue;
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    SkillLvs.skillID3 = mstsvtskillobjtmp["skillId"].ToString();
                    IsSk3Strengthened.Dispatcher.Invoke(() => { IsSk3Strengthened.Text = "√"; });
                    sk3IsStrengthened = true;
                }
            }

            var SSLC = new Thread(ServantSkillLevelCheck);
            SSLC.Start();

            Dispatcher.Invoke(() =>
            {
                foreach (var skilltmp in GlobalPathsAndDatas.mstSkillArray) //查找某个字段与值
                {
                    if (((JObject) skilltmp)["id"].ToString() == SkillLvs.skillID1)
                    {
                        var skillobjtmp = JObject.Parse(skilltmp.ToString());
                        skill1Name = skillobjtmp["name"].ToString();
                        skill1name.Text = skill1Name;
                        if (sk1IsStrengthened) skill1name.Text += " ▲";
                    }

                    if (((JObject) skilltmp)["id"].ToString() == SkillLvs.skillID2)
                    {
                        var skillobjtmp = JObject.Parse(skilltmp.ToString());
                        skill2Name = skillobjtmp["name"].ToString();
                        skill2name.Text = skill2Name;
                        if (sk2IsStrengthened) skill2name.Text += " ▲";
                    }

                    if (((JObject) skilltmp)["id"].ToString() != SkillLvs.skillID3) continue;
                    {
                        var skillobjtmp = JObject.Parse(skilltmp.ToString());
                        skill3Name = skillobjtmp["name"].ToString();
                        skill3name.Text = skill3Name;
                        if (sk3IsStrengthened) skill3name.Text += " ▲";
                    }
                }

                foreach (var skillDetailtmp in GlobalPathsAndDatas.mstSkillDetailArray) //查找某个字段与值
                {
                    if (((JObject) skillDetailtmp)["id"].ToString() == SkillLvs.skillID1)
                    {
                        var skillDetailobjtmp = JObject.Parse(skillDetailtmp.ToString());
                        skill1detail = skillDetailobjtmp["detail"].ToString().Replace("[{0}]", " [Lv.1 - Lv.10] ")
                            .Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "")
                            .Replace(@"＆", "\r\n ＋").Replace(@"＋", "\r\n ＋").Replace("\r\n \r\n", "\r\n");
                        skill1details.Text = skill1detail;
                    }

                    if (((JObject) skillDetailtmp)["id"].ToString() == SkillLvs.skillID2)
                    {
                        var skillDetailobjtmp = JObject.Parse(skillDetailtmp.ToString());
                        skill2detail = skillDetailobjtmp["detail"].ToString().Replace("[{0}]", " [Lv.1 - Lv.10] ")
                            .Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "")
                            .Replace(@"＆", "\r\n ＋").Replace(@"＋", "\r\n ＋").Replace("\r\n \r\n", "\r\n");
                        skill2details.Text = skill2detail;
                    }

                    if (((JObject) skillDetailtmp)["id"].ToString() != SkillLvs.skillID3) continue;
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
                if (CheckNeededFiles.CheckDirectoryExists()) return;
                if (CheckNeededFiles.CheckFilesExists()) return;
                var output = GlobalPathsAndDatas.mstSvtArray.Aggregate("",
                    (current, svtIDtmp) => current + "ID: " + ((JObject) svtIDtmp)["id"] + "    " + "名称: " +
                                           ((JObject) svtIDtmp)["name"] + "\r\n");
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
                    if (ui is TextBox box)
                        box.Text = "";
                var childrens1 = svtdetail.Children;
                foreach (UIElement ui2 in childrens1)
                    if (ui2 is TextBox box)
                        box.Text = "";
                var childrens2 = svttexts.Children;
                foreach (UIElement ui2 in childrens2)
                    if (ui2 is TextBox box)
                        box.Text = "";
                var childrens3 = svtcards.Children;
                foreach (UIElement ui2 in childrens3)
                    if (ui2 is TextBox box)
                        box.Text = "";
                var childrens4 = svtTDs.Children;
                foreach (UIElement ui2 in childrens4)
                    if (ui2 is TextBox box)
                        box.Text = "";
                var childrens5 = svtskill1.Children;
                foreach (UIElement ui2 in childrens5)
                    if (ui2 is TextBox box)
                        box.Text = "";
                var childrens6 = svtskill2.Children;
                foreach (UIElement ui2 in childrens6)
                    if (ui2 is TextBox box)
                        box.Text = "";
                var childrens7 = svtskill3.Children;
                foreach (UIElement ui2 in childrens7)
                    if (ui2 is TextBox box)
                        box.Text = "";
                atkbalance1.Content = "( x 1.0 -)";
                atkbalance2.Content = "( x 1.0 -)";
                JBOutput.IsEnabled = false;
                sixwei.Content = "";
                Skill1FuncList.Items.Clear();
                Skill2FuncList.Items.Clear();
                Skill3FuncList.Items.Clear();
            });
            IsSk1Strengthened.Dispatcher.Invoke(() => { IsSk1Strengthened.Text = "×"; });
            IsSk2Strengthened.Dispatcher.Invoke(() => { IsSk2Strengthened.Text = "×"; });
            IsSk3Strengthened.Dispatcher.Invoke(() => { IsSk3Strengthened.Text = "×"; });
            IsNPStrengthened.Dispatcher.Invoke(() => { IsNPStrengthened.Text = "×"; });
            GC.Collect();
        }

        private void SkillDetailCheck(string sklid)
        {
            Dispatcher.Invoke(() =>
            {
                SkillLvs.skilllv1sval = "";
                SkillLvs.skilllv6sval = "";
                SkillLvs.skilllv10sval = "";
                SkillLvs.skilllv1svalArray = null;
                SkillLvs.skilllv6svalArray = null;
                SkillLvs.skilllv10svalArray = null;
                SkillLvs.skilllv1chargetime = "";
                SkillLvs.skilllv6chargetime = "";
                SkillLvs.skilllv10chargetime = "";
                SkillLvs.SKLFuncstr = "";
                SkillLvs.SKLFuncstrArray = null;
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
                        SkillLvs.skilllv1chargetime = SKLobjtmp["chargeTurn"].ToString();
                        SkillLvs.skilllv1svalArray = SkillLvs.skilllv1sval.Split('/');
                    }

                    if (((JObject) SKLTMP)["skillId"].ToString() == sklid && ((JObject) SKLTMP)["lv"].ToString() == "6")
                    {
                        var SKLobjtmp = JObject.Parse(SKLTMP.ToString());
                        SkillLvs.skilllv6sval = SKLobjtmp["svals"].ToString().Replace("\n", "").Replace("\r", "")
                            .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "").Replace("*,", "/");
                        SkillLvs.skilllv6sval = SkillLvs.skilllv6sval.Substring(0, SkillLvs.skilllv6sval.Length - 2);
                        SkillLvs.skilllv6chargetime = SKLobjtmp["chargeTurn"].ToString();
                        SkillLvs.skilllv6svalArray = SkillLvs.skilllv6sval.Split('/');
                    }

                    if (((JObject) SKLTMP)["skillId"].ToString() != sklid ||
                        ((JObject) SKLTMP)["lv"].ToString() != "10") continue;
                    {
                        var SKLobjtmp = JObject.Parse(SKLTMP.ToString());
                        SkillLvs.skilllv10sval = SKLobjtmp["svals"].ToString().Replace("\n", "").Replace("\r", "")
                            .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "").Replace("*,", "/");
                        SkillLvs.skilllv10sval = SkillLvs.skilllv10sval.Substring(0, SkillLvs.skilllv10sval.Length - 2);
                        SkillLvs.skilllv10chargetime = SKLobjtmp["chargeTurn"].ToString();
                        SkillLvs.skilllv10svalArray = SkillLvs.skilllv10sval.Split('/');
                        svtSKFuncID = SKLobjtmp["funcId"].ToString().Replace("\n", "").Replace("\t", "")
                            .Replace("\r", "").Replace(" ", "").Replace("[", "").Replace("]", "");
                        svtSKFuncIDList = new List<string>(svtSKFuncID.Split(','));
                        svtSKFuncIDArray = svtSKFuncIDList.ToArray();
                        svtSKFuncList.AddRange(from skfuncidtmp in svtSKFuncIDArray
                            from functmp in GlobalPathsAndDatas.mstFuncArray
                            where ((JObject) functmp)["id"].ToString() == skfuncidtmp
                            select JObject.Parse(functmp.ToString())
                            into mstFuncobjtmp
                            select mstFuncobjtmp["popupText"].ToString());
                    }
                }

                svtSKFuncArray = svtSKFuncList.ToArray();
                SkillLvs.SKLFuncstrArray = svtSKFuncArray;
                svtSKFunc = string.Join(", ", svtSKFuncArray);
                SkillLvs.SKLFuncstr = svtSKFunc;
            });
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var clean = new Thread(ClearTexts);
            clean.Start();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)

        {
            if (sender is Hyperlink source) Process.Start(source.NavigateUri.ToString());
        }

        private void HttpRequestData()
        {
            var path = Directory.GetCurrentDirectory();
            var gamedata = new DirectoryInfo(path + @"\Android\masterdata\");
            var folder = new DirectoryInfo(path + @"\Android\");
            string result;
            JObject res;
            var Check = true;
            ToggleDeleteLastData.Dispatcher.Invoke(() =>
            {
                if (ToggleDeleteLastData.IsChecked == true) Check = !Check;
            });
            Button1.Dispatcher.Invoke(() => { Button1.IsEnabled = false; });
            OutputIDs.Dispatcher.Invoke(() => { OutputIDs.IsEnabled = false; });
            updatedatabutton.Dispatcher.Invoke(() => { updatedatabutton.IsEnabled = false; });
            updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = ""; });
            updatestatusring.Dispatcher.Invoke(() => { updatestatusring.IsActive = true; });
            updatestatus.Dispatcher.Invoke(() => { updatesign.Content = "数据下载进行中,请勿退出!"; });
            progressbar.Dispatcher.Invoke(() =>
            {
                progressbar.Value = 0;
                progressbar.Visibility = Visibility.Visible;
            });
            progressbar.Dispatcher.Invoke(() => { progressbar.Value = progressbar.Value + 250; });
            if (!Directory.Exists(folder.FullName))
            {
                updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = "正在创建Android目录..."; });
                Directory.CreateDirectory(folder.FullName);
            }

            if (!Directory.Exists(gamedata.FullName))
                Directory.CreateDirectory(gamedata.FullName);
            updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = "开始下载/更新游戏数据......"; });
            progressbar.Dispatcher.Invoke(() => { progressbar.Value = progressbar.Value + 250; });

            try
            {
                result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top", "appVer=2.14.0");
                res = JObject.Parse(result);
                if (res["response"][0]["fail"]["action"] != null)
                    switch (res["response"][0]["fail"]["action"].ToString())
                    {
                        case "app_version_up":
                        {
                            var tmp = res["response"][0]["fail"]["detail"].ToString();
                            tmp = Regex.Replace(tmp, @".*新ver.：(.*)、現.*", "$1", RegexOptions.Singleline);
                            updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = "当前游戏版本: " + tmp; });

                            result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top",
                                "appVer=" + tmp);
                            res = JObject.Parse(result);
                            break;
                        }
                        case "maint":
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
                            Button1.Dispatcher.Invoke(() => { Button1.IsEnabled = true; });
                            OutputIDs.Dispatcher.Invoke(() => { OutputIDs.IsEnabled = true; });
                            updatestatusring.Dispatcher.Invoke(() => { updatestatusring.IsActive = false; });
                            return;
                        }
                    }
            }
            catch (Exception e)
            {
                MessageBox.Show("网络连接异常,请检查网络连接并重试.\r\n" + e, "网络连接异常", MessageBoxButton.OK, MessageBoxImage.Error);
                updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = ""; });
                updatestatus.Dispatcher.Invoke(() => { updatesign.Content = ""; });
                progressbar.Dispatcher.Invoke(() =>
                {
                    progressbar.Visibility = Visibility.Hidden;
                    updatedatabutton.IsEnabled = true;
                });
                Button1.Dispatcher.Invoke(() => { Button1.IsEnabled = true; });
                OutputIDs.Dispatcher.Invoke(() => { OutputIDs.IsEnabled = true; });
                updatestatusring.Dispatcher.Invoke(() => { updatestatusring.IsActive = false; });
                return;
            }

            if (File.Exists(gamedata.FullName + "webview") || File.Exists(gamedata.FullName + "raw") ||
                File.Exists(gamedata.FullName + "assetbundle") || File.Exists(gamedata.FullName + "webview") ||
                File.Exists(gamedata.FullName + "master"))
            {
                var oldRaw = File.ReadAllText(gamedata.FullName + "raw");
                if (string.CompareOrdinal(oldRaw, result) == 0 && Check)
                {
                    MessageBox.Show("当前的MasterData已是最新版本.", "无需更新", MessageBoxButton.OK, MessageBoxImage.Information);
                    updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = ""; });
                    updatestatus.Dispatcher.Invoke(() => { updatesign.Content = ""; });
                    progressbar.Dispatcher.Invoke(() =>
                    {
                        progressbar.Visibility = Visibility.Hidden;
                        updatedatabutton.IsEnabled = true;
                    });
                    Button1.Dispatcher.Invoke(() => { Button1.IsEnabled = true; });
                    OutputIDs.Dispatcher.Invoke(() => { OutputIDs.IsEnabled = true; });
                    updatestatusring.Dispatcher.Invoke(() => { updatestatusring.IsActive = false; });
                    return;
                }

                var fileinfo = gamedata.GetFileSystemInfos(); //返回目录中所有文件和子目录
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

            File.WriteAllText(gamedata.FullName + "raw", result);
            File.WriteAllText(gamedata.FullName + "assetbundle",
                res["response"][0]["success"]["assetbundle"].ToString());
            updatestatus.Dispatcher.Invoke(() =>
            {
                updatestatus.Content = "Writing file to: " + gamedata.FullName + "assetbundle";
            });
            progressbar.Dispatcher.Invoke(() => { progressbar.Value = progressbar.Value + 40; });
            File.WriteAllText(gamedata.FullName + "master",
                res["response"][0]["success"]["master"].ToString());
            updatestatus.Dispatcher.Invoke(() =>
            {
                updatestatus.Content = "Writing file to: " + gamedata.FullName + "master";
            });
            progressbar.Dispatcher.Invoke(() => { progressbar.Value = progressbar.Value + 40; });
            File.WriteAllText(gamedata.FullName + "webview",
                res["response"][0]["success"]["webview"].ToString());
            updatestatus.Dispatcher.Invoke(() =>
            {
                updatestatus.Content = "Writing file to: " + gamedata.FullName + "webview";
            });
            progressbar.Dispatcher.Invoke(() => { progressbar.Value = progressbar.Value + 40; });
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
                updatestatus.Dispatcher.Invoke(() =>
                {
                    updatestatus.Content = "Writing file to: " + gamedata.FullName +
                                           "decrypted__masterdata\\" + item.Key;
                });
                progressbar.Dispatcher.Invoke(() => { progressbar.Value = progressbar.Value + 40; });
            }

            var data2 = File.ReadAllText(gamedata.FullName + "assetbundle");
            var dictionary =
                (Dictionary<string, object>) MasterDataUnpacker.MouseInfoMsgPack(
                    Convert.FromBase64String(data2));
            var str = dictionary.Aggregate<KeyValuePair<string, object>, string>(null,
                (current, a) => current + a.Key + ": " + a.Value + "\r\n");
            File.WriteAllText(gamedata.FullName + "assetbundle.txt", str);
            updatestatus.Dispatcher.Invoke(() =>
            {
                updatestatus.Content = "folder name: " + dictionary["folderName"];
            });
            progressbar.Dispatcher.Invoke(() => { progressbar.Value = progressbar.Value + 40; });
            var data3 = File.ReadAllText(gamedata.FullName + "webview");
            var dictionary2 =
                (Dictionary<string, object>) MasterDataUnpacker.MouseGame2MsgPack(
                    Convert.FromBase64String(data3));
            var str2 = "baseURL: " + dictionary2["baseURL"] + "\r\n contactURL: " +
                       dictionary2["contactURL"] + "\r\n";
            updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = str2; });
            progressbar.Dispatcher.Invoke(() => { progressbar.Value = progressbar.Value + 40; });
            var filePassInfo = (Dictionary<string, object>) dictionary2["filePass"];
            str = filePassInfo.Aggregate(str, (current, a) => current + a.Key + ": " + a.Value + "\r\n");
            File.WriteAllText(gamedata.FullName + "webview.txt", str2);
            updatestatus.Dispatcher.Invoke(() =>
            {
                updatestatus.Content = "Writing file to: " + gamedata.FullName + "webview.txt";
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
            OutputIDs.Dispatcher.Invoke(() => { OutputIDs.IsEnabled = true; });
            updatestatusring.Dispatcher.Invoke(() => { updatestatusring.IsActive = false; });
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
                "具体数值显示的为文件中的原始数据，水平有限，无法进行有效解析。\r\n\r\n注:由于文本框高度过窄,\"宝具信息\"选项卡内具体数值前的等级Label(lv.x)双击后即可显示详细数据对话框.\r\n\r\n【以下内容均为理论推测，仅适用于大部分结果。】\r\n/ 之间的为一个Buff的幅度\r\n1、如果为[a,b]则a为成功率(除以10就是百分比，如1000就是100%),b需要看技能描述，如果为出星或者生命值则b的大小即为幅度，若为NP，则将该数值除以100即为NP值。若b为1，则该组段可以忽略不看(或为对敌方技能比如减一格充能之类的)。\r\n2、如果为[a,b,c]或者[a,b,c,d]则在一般情况下a表示成功率(同1),b表示持续回合数即Turn,c表示次数(-1即为没有次数限制),d在大多数情况下除以10即为Buff幅度(%)，有时会有例外(可能也是没有意义).\r\n3、如果为[a,b,c,d,e]则a,b,c同2,d和e需要通过源文件进行详细手动分析。",
                "温馨提示:", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            if (sender is Hyperlink source) Process.Start(source.NavigateUri.ToString());
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

        private void Form_Load(object sender, RoutedEventArgs e)
        {
            var path = Directory.GetCurrentDirectory();
            var gamedata = new DirectoryInfo(path + @"\Android\masterdata\");
            VersionLabel.Content = CommonStrings.Version;
            if (!Directory.Exists(gamedata.FullName))
            {
                MessageBox.Show("没有游戏数据,请先下载游戏数据(位于\"设置\"选项卡).", "温馨提示:", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                Button1.IsEnabled = false;
            }
            else
            {
                if (File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSvt") &&
                    File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSvtLimit") &&
                    File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstCv") &&
                    File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstIllustrator") &&
                    File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSvtCard") &&
                    File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDevice") &&
                    File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSvtTreasureDevice") &&
                    File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDeviceDetail") &&
                    File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSkill") &&
                    File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSkillDetail") &&
                    File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSvtSkill") &&
                    File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstFunc") &&
                    File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSvtComment") &&
                    File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDeviceLv") &&
                    File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstSkillLv")) return;
                MessageBox.Show("游戏数据损坏,请重新下载游戏数据(位于\"设置\"选项卡).", "温馨提示:", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Button1.IsEnabled = false;
            }
        }

        private void ExcelFileOutput()
        {
            var path = Directory.GetCurrentDirectory();
            var svtData = new DirectoryInfo(path + @"\ServantData\");
            if (!Directory.Exists(svtData.FullName))
                Directory.CreateDirectory(svtData.FullName);
            var streamget = HttpRequest.GetXlsx();
            var xlsx =
                new ExcelPackage(streamget);
            var worksheet = xlsx.Workbook.Worksheets[0];
            worksheet.Cells["L1"].Value = JB.svtid;
            worksheet.Cells["A1"].Value += "(" + JB.svtnme + ")";
            worksheet.Cells["C2"].Value = Svtname.Text;
            worksheet.Cells["J2"].Value = SvtBattlename.Text;
            worksheet.Cells["B3"].Value = svtclass.Text;
            worksheet.Cells["E3"].Value = rarity.Text;
            worksheet.Cells["G3"].Value = gendle.Text;
            worksheet.Cells["J3"].Value = hiddenattri.Text;
            worksheet.Cells["M3"].Value = collection.Text;
            worksheet.Cells["B4"].Value = cv.Text;
            worksheet.Cells["H4"].Value = illust.Text;
            worksheet.Cells["B5"].Value = ssvtstarrate.Text;
            worksheet.Cells["F5"].Value = ssvtdeathrate.Text;
            worksheet.Cells["J5"].Value = jixing.Text;
            worksheet.Cells["M5"].Value = notrealnprate.Text;
            worksheet.Cells["C6"].Value = nprate.Text;
            worksheet.Cells["C8"].Value = classskill.Text;
            worksheet.Cells["C10"].Value = basichp.Text;
            worksheet.Cells["F10"].Value = basicatk.Text;
            worksheet.Cells["I10"].Value = maxhp.Text;
            worksheet.Cells["L10"].Value = maxatk.Text;
            worksheet.Cells["C11"].Value = cards.Text;
            worksheet.Cells["B14"].Value = bustercard.Text;
            worksheet.Cells["I14"].Value = artscard.Text;
            worksheet.Cells["B15"].Value = quickcard.Text;
            worksheet.Cells["I15"].Value = extracard.Text;
            worksheet.Cells["B16"].Value = treasuredevicescard.Text;
            worksheet.Cells["C18"].Value = npcardtype.Text;
            worksheet.Cells["G18"].Value = nptype.Text;
            worksheet.Cells["J18"].Value = nprank.Text;
            worksheet.Cells["C19"].Value = npruby.Text;
            worksheet.Cells["C20"].Value = npname.Text;
            worksheet.Cells["C21"].Value = npdetail.Text;
            worksheet.Cells["D24"].Value = skill1name.Text;
            worksheet.Cells["H24"].Value = skill1cdlv1.Text;
            worksheet.Cells["J24"].Value = skill1cdlv6.Text;
            worksheet.Cells["L24"].Value = skill1cdlv10.Text;
            worksheet.Cells["D25"].Value = skill1details.Text;
            worksheet.Cells["D26"].Value = skill2name.Text;
            worksheet.Cells["H26"].Value = skill2cdlv1.Text;
            worksheet.Cells["J26"].Value = skill2cdlv6.Text;
            worksheet.Cells["L26"].Value = skill2cdlv10.Text;
            worksheet.Cells["D27"].Value = skill2details.Text;
            worksheet.Cells["D28"].Value = skill3name.Text;
            worksheet.Cells["H28"].Value = skill3cdlv1.Text;
            worksheet.Cells["J28"].Value = skill3cdlv6.Text;
            worksheet.Cells["L28"].Value = skill3cdlv10.Text;
            worksheet.Cells["D29"].Value = skill3details.Text;
            xlsx.SaveAs(new FileInfo(svtData.FullName + JB.svtnme + "_" + JB.svtid + ".xlsx"));
            streamget.Close();
            MessageBox.Show("导出成功,文件名为: " + svtData.FullName + JB.svtnme + "_" + JB.svtid + ".xlsx", "导出完成",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            Process.Start(svtData.FullName + JB.svtnme + "_" + JB.svtid + ".xlsx");
            GC.Collect();
        }

        private void VersionCheckEvent()
        {
            string VerChkRaw;
            JObject VerChk;
            JArray VerAssetsJArray;
            GlobalPathsAndDatas.ExeUpdateUrl = "";
            GlobalPathsAndDatas.NewerVersion = "";
            var Sub = new Thread(DownloadFilesSub);
            try
            {
                VerChkRaw = HttpRequest.GetApplicationUpdateJson();
                VerChk = JObject.Parse(VerChkRaw);
            }
            catch (Exception e)
            {
                MessageBox.Show("网络连接异常,请检查网络连接并重试.\r\n" + e, "网络连接异常", MessageBoxButton.OK, MessageBoxImage.Error);
                CheckUpdate.Dispatcher.Invoke(() => { CheckUpdate.IsEnabled = true; });
                return;
            }

            if (CommonStrings.VersionTag != VerChk["tag_name"].ToString())
            {
                if (MessageBox.Show(
                    "检测到软件更新\r\n\r\n新版本为:  " + VerChk["tag_name"] + "    当前版本为:  " + CommonStrings.VersionTag +
                    "\r\n\r\nChangeLog:\r\n" + VerChk["body"] + "\r\n\r\n点击\"确认\"按钮可选择更新.", "检查更新",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Information) == MessageBoxResult.OK)
                {
                    VerAssetsJArray = (JArray) JsonConvert.DeserializeObject(VerChk["assets"].ToString());
                    for (var i = 0; i <= VerAssetsJArray.Count - 1; i++)
                        if (VerAssetsJArray[i]["name"].ToString() == "FGOSBIAReloaded.exe")
                            GlobalPathsAndDatas.ExeUpdateUrl = VerAssetsJArray[i]["browser_download_url"].ToString();
                    if (GlobalPathsAndDatas.ExeUpdateUrl == "")
                    {
                        MessageBox.Show("确认到新版本更新,但是获取下载Url失败.\r\n", "获取Url失败", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        CheckUpdate.Dispatcher.Invoke(() => { CheckUpdate.IsEnabled = true; });
                        return;
                    }

                    Sub.Start(VerChk["tag_name"].ToString());
                }
                else
                {
                    CheckUpdate.Dispatcher.Invoke(() => { CheckUpdate.IsEnabled = true; });
                }
            }
            else
            {
                MessageBox.Show("当前版本为:  " + CommonStrings.VersionTag + "\r\n\r\n无需更新.", "检查更新", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                CheckUpdate.Dispatcher.Invoke(() => { CheckUpdate.IsEnabled = true; });
            }
        }

        private void DownloadFilesSub(object VerChk)
        {
            var path = Directory.GetCurrentDirectory();
            try
            {
                DownloadFile(GlobalPathsAndDatas.ExeUpdateUrl, path + "/FGOSBIAReloaded(Update " + VerChk + ").exe");
                GlobalPathsAndDatas.NewerVersion = VerChk.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show("写入文件异常.\r\n" + e, "异常", MessageBoxButton.OK, MessageBoxImage.Error);
                CheckUpdate.Dispatcher.Invoke(() => { CheckUpdate.IsEnabled = true; });
                throw;
            }
        }

        public void DownloadFile(string url, string filePath)
        {
            var Downloads = new WebClient();
            GlobalPathsAndDatas.StartTime = DateTime.Now;
            progressbar1.Dispatcher.Invoke(() =>
            {
                progressbar1.Visibility = Visibility.Visible;
                progressbar1.Value = 0;
            });
            updatestatusring2.Dispatcher.Invoke(() => { updatestatusring2.IsActive = true; });
            updatestatus2.Dispatcher.Invoke(() => { updatestatus2.Content = ""; });
            Downloads.DownloadProgressChanged += OnDownloadProgressChanged;
            Downloads.DownloadFileCompleted += OnDownloadFileCompleted;
            Downloads.DownloadFileAsync(new Uri(url), filePath);
        }

        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var path = Directory.GetCurrentDirectory();
            MessageBox.Show(
                "下载完成.下载目录为: \r\n" + path + "\\FGOSBIAReloaded(Update " + GlobalPathsAndDatas.NewerVersion +
                ").exe\r\n\r\n请自行替换文件.", "检查更新", MessageBoxButton.OK, MessageBoxImage.Information);
            CheckUpdate.Dispatcher.Invoke(() => { CheckUpdate.IsEnabled = true; });
            progressbar1.Dispatcher.Invoke(() =>
            {
                progressbar1.Visibility = Visibility.Hidden;
                progressbar1.Value = 0;
            });
            updatestatusring2.Dispatcher.Invoke(() => { updatestatusring2.IsActive = false; });
            updatestatus2.Dispatcher.Invoke(() => { updatestatus2.Content = ""; });
        }

        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressbar1.Dispatcher.Invoke(() => { progressbar1.Value = e.ProgressPercentage; });
            var s = (DateTime.Now - GlobalPathsAndDatas.StartTime).TotalSeconds;
            var sd = HttpRequest.ReadableFilesize(e.BytesReceived / s);
            updatestatus2.Dispatcher.Invoke(() =>
            {
                updatestatus2.Content = "下载速度: " + sd + "/s" + ", 已下载: " +
                                        HttpRequest.ReadableFilesize(e.BytesReceived) + " / 总计: " +
                                        HttpRequest.ReadableFilesize(e.TotalBytesToReceive);
            });
        }


        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            CheckUpdate.IsEnabled = false;
            var VCE = new Thread(VersionCheckEvent);
            VCE.Start();
        }

        private struct Skill1ListSval
        {
            public string Skill1Name { get; }
            public string Skill1Svallv1 { get; }
            public string Skill1Svallv6 { get; }
            public string Skill1Svallv10 { get; }

            public Skill1ListSval(string v1, string v2, string v3, string v4) : this()
            {
                Skill1Name = v1;
                Skill1Svallv1 = v2;
                Skill1Svallv6 = v3;
                Skill1Svallv10 = v4;
            }
        }

        private struct Skill2ListSval
        {
            public string Skill2Name { get; }
            public string Skill2Svallv1 { get; }
            public string Skill2Svallv6 { get; }
            public string Skill2Svallv10 { get; }

            public Skill2ListSval(string v1, string v2, string v3, string v4) : this()
            {
                Skill2Name = v1;
                Skill2Svallv1 = v2;
                Skill2Svallv6 = v3;
                Skill2Svallv10 = v4;
            }
        }

        private struct Skill3ListSval
        {
            public string Skill3Name { get; }
            public string Skill3Svallv1 { get; }
            public string Skill3Svallv6 { get; }
            public string Skill3Svallv10 { get; }

            public Skill3ListSval(string v1, string v2, string v3, string v4) : this()
            {
                Skill3Name = v1;
                Skill3Svallv1 = v2;
                Skill3Svallv6 = v3;
                Skill3Svallv10 = v4;
            }
        }
    }
}