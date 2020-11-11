﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using FGOSBIAReloaded.Properties;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
        private Polyline platk;
        private Polyline plhp;

        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Button1.IsEnabled = false;
            textbox1.Text = Regex.Replace(textbox1.Text, @"\s", "");
            var ES = new Task(() => { EasternEggSvt(); });
            if (textbox1.Text == "ACPD" || textbox1.Text == "acpd")
            {
                ClearTexts();
                ES.Start();
                Button1.Dispatcher.Invoke(() => { Button1.IsEnabled = true; });
                return;
            }

            if (!Regex.IsMatch(textbox1.Text, "^\\d+$"))
            {
                await this.ShowMessageAsync("温馨提示:", "从者ID输入错误,请检查.");
                ClearTexts();
                Button1.Dispatcher.Invoke(() => { Button1.IsEnabled = true; });
                return;
            }

            var SA = new Task(StartAnalyze);
            SA.Start();
        }

        private void StartAnalyze()
        {
            var svtID = "";
            var svtTDID = "";
            var SCAC = new Task(ServantCardsArrangementCheck);
            var SBIC = new Task(ServantBasicInformationCheck);
            var SCIC = new Task(ServantCVandIllustCheck);
            var SJTC = new Task(ServantJibanTextCheck);
            var STDI = new Task(() => { ServantTreasureDeviceInformationCheck(svtTDID); });
            var SSIC = new Task(ServantSkillInformationCheck);
            var SCLIC = new Task(ServantCombineLimitItemsCheck);
            var SCSIC = new Task(ServantCombineSkillItemsCheck);
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
            try
            {
                foreach (var svtTreasureDevicestmp in GlobalPathsAndDatas.mstSvtTreasureDevicedArray)
                {
                    if (((JObject) svtTreasureDevicestmp)["svtId"].ToString() == svtID &&
                        ((JObject) svtTreasureDevicestmp)["num"].ToString() == "1" &&
                        ((JObject) svtTreasureDevicestmp)["treasureDeviceId"].ToString().Length <= 5)
                    {
                        var mstsvtTDobjtmp = JObject.Parse(svtTreasureDevicestmp.ToString());
                        svtTDID = mstsvtTDobjtmp["treasureDeviceId"].ToString();
                    }

                    if (((JObject) svtTreasureDevicestmp)["svtId"].ToString() == svtID &&
                        ((JObject) svtTreasureDevicestmp)["num"].ToString() == "98" &&
                        ((JObject) svtTreasureDevicestmp)["priority"].ToString() == "0")
                    {
                        var mstsvtTDobjtmp = JObject.Parse(svtTreasureDevicestmp.ToString());
                        svtTDID = mstsvtTDobjtmp["treasureDeviceId"].ToString();
                    }

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
            }
            catch (Exception e)
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        Application.Current.MainWindow, "您太心急了,稍等一下再解析吧!\r\n" + e, "温馨提示:", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    Button1.IsEnabled = true;
                });
                return;
            }

            textbox1.Dispatcher.Invoke(() => { textbox1.Text = svtID; });

            ToggleBuffFuncTranslate.Dispatcher.Invoke(() =>
            {
                if (ToggleBuffFuncTranslate.IsChecked == true)
                    GlobalPathsAndDatas.TranslationList = HttpRequest.GetBuffTranslationList();
            });
            SCAC.Start();
            SBIC.Start();
            SCIC.Start();
            SJTC.Start();
            SCLIC.Start();
            SCSIC.Start();
            STDI.Start();
            Task.WaitAny(SCLIC);
            var STDSC = new Task(() => { ServantTreasureDeviceSvalCheck(svtTDID); });
            STDSC.Start();
            SSIC.Start();
            Task.WaitAll(STDSC, SSIC);
            Button1.Dispatcher.Invoke(() => { Button1.IsEnabled = true; });
            Dispatcher.Invoke(async () =>
            {
                if (rarity.Text == "")
                {
                    await this.ShowMessageAsync("温馨提示:", "从者ID不存在或未实装,请重试.");
                    ClearTexts();
                    Button1.IsEnabled = true;
                    return;
                }

                if (cards.Text == "[Q,Q,Q,Q,Q]" && svtclass.Text != "礼装")
                    RemindText.Text = "此ID为小怪(或部分boss以及种火芙芙),配卡、技能、宝具信息解析并不准确,请知悉.";
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
            foreach (var TDDtmp in GlobalPathsAndDatas.mstTreasureDeviceDetailArray)
                if (((JObject) TDDtmp)["id"].ToString() == svtTDID.ToString())
                {
                    var TDDobjtmp = JObject.Parse(TDDtmp.ToString());
                    ToggleDetailbr.Dispatcher.Invoke(() =>
                    {
                        if (ToggleDetailbr.IsChecked == true)
                            NPDetail = TDDobjtmp["detail"].ToString().Replace("[{0}]", "[Lv.1 - Lv.5]")
                                .Replace("[g]", "")
                                .Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "").Replace(@"＆", "\r\n ＋")
                                .Replace(@"＋", "\r\n ＋").Replace("\r\n \r\n", "\r\n");
                        else
                            NPDetail = TDDobjtmp["detail"].ToString().Replace("[{0}]", "[Lv.1 - Lv.5]")
                                .Replace("[g]", "")
                                .Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "").Replace(@"＆", " ＋");
                    });
                    break;
                }

            foreach (var TDlvtmp in GlobalPathsAndDatas.mstTreasureDeviceLvArray)
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

            foreach (var TreasureDevicestmp in GlobalPathsAndDatas.mstTreasureDevicedArray)
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
                    svtNPDamageType = mstTDobjtmp["effectFlag"].ToString().Replace("0", "辅助宝具")
                        .Replace("1", "全体宝具").Replace("2", "单体宝具");
                    nptype.Dispatcher.Invoke(() => { nptype.Text = svtNPDamageType; });
                    var DSSCL = new Task(() => { DrawServantStrengthenCurveLine(GlobalPathsAndDatas.CurveType); });
                    DSSCL.Start();
                    if (svtNPDamageType == "辅助宝具")
                    {
                        svtNPCardhit = 0;
                        svtNPCardhitDamage = "[ - ]";
                    }

                    foreach (var svtTreasureDevicestmp in GlobalPathsAndDatas.mstSvtTreasureDevicedArray)
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
                    svtNPDamageType = mstTDobjtmp2["effectFlag"].ToString().Replace("0", "宝具攻击")
                        .Replace("1", "宝具攻击").Replace("2", "宝具攻击");
                    nptype.Dispatcher.Invoke(() => { nptype.Text = svtNPDamageType; });
                    /*if (svtNPDamageType == "辅助宝具")
                    {
                        svtNPCardhit = 0;
                        svtNPCardhitDamage = "[ - ]";
                    }
                    */
                    NPDetail = "该ID的配卡与宝具解析可能不准确,请留意.";
                    foreach (var svtTreasureDevicestmp in GlobalPathsAndDatas.mstSvtTreasureDevicedArray)
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
                foreach (var TreasureDevicestmp2 in GlobalPathsAndDatas.mstTreasureDevicedArray)
                    if (((JObject) TreasureDevicestmp2)["name"].ToString() == NPName)
                    {
                        var TreasureDevicesobjtmp2 = JObject.Parse(TreasureDevicestmp2.ToString());
                        newtmpid = TreasureDevicesobjtmp2["id"].ToString();
                        switch (newtmpid.Length)
                        {
                            case 6:
                            {
                                var FinTDID_TMP = newtmpid;
                                foreach (var TDDtmp2 in GlobalPathsAndDatas.mstTreasureDeviceDetailArray)
                                    if (((JObject) TDDtmp2)["id"].ToString() == FinTDID_TMP)
                                    {
                                        var TDDobjtmp2 = JObject.Parse(TDDtmp2.ToString());
                                        if (ToggleDetailbr.IsChecked == true)
                                            NPDetail = TDDobjtmp2["detail"].ToString().Replace("[{0}]", "[Lv.1 - Lv.5]")
                                                .Replace("[g]", "")
                                                .Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "")
                                                .Replace(@"＆", "\r\n ＋")
                                                .Replace(@"＋", "\r\n ＋").Replace("\r\n \r\n", "\r\n");
                                        else
                                            NPDetail = TDDobjtmp2["detail"].ToString().Replace("[{0}]", "[Lv.1 - Lv.5]")
                                                .Replace("[g]", "")
                                                .Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "")
                                                .Replace(@"＆", " ＋");
                                    }

                                break;
                            }
                            case 7:
                            {
                                if (newtmpid.Substring(0, 2) == "10" || newtmpid.Substring(0, 2) == "11" ||
                                    newtmpid.Substring(0, 2) == "23" || newtmpid.Substring(0, 2) == "25")
                                {
                                    var FinTDID_TMP = newtmpid;
                                    foreach (var TDDtmp2 in GlobalPathsAndDatas.mstTreasureDeviceDetailArray
                                    )
                                        if (((JObject) TDDtmp2)["id"].ToString() == FinTDID_TMP)
                                        {
                                            var TDDobjtmp2 = JObject.Parse(TDDtmp2.ToString());
                                            if (ToggleDetailbr.IsChecked == true)
                                                NPDetail = TDDobjtmp2["detail"].ToString()
                                                    .Replace("[{0}]", "[Lv.1 - Lv.5]").Replace("[g]", "")
                                                    .Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "")
                                                    .Replace(@"＆", "\r\n ＋")
                                                    .Replace(@"＋", "\r\n ＋").Replace("\r\n \r\n", "\r\n");
                                            else
                                                NPDetail = TDDobjtmp2["detail"].ToString()
                                                    .Replace("[{0}]", "[Lv.1 - Lv.5]").Replace("[g]", "")
                                                    .Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "")
                                                    .Replace(@"＆", " ＋");
                                        }
                                }

                                break;
                            }
                        }
                    }

            npdetail.Dispatcher.Invoke(() => { npdetail.Text = NPDetail; });
            if (NPName == "" && NPDetail == "") npdetail.Dispatcher.Invoke(() => { npdetail.Text = "该宝具暂时没有描述."; });
        }

        private void ServantSkillLevelCheck()
        {
            SkillLvs.skill1forExcel = "";
            SkillLvs.skill2forExcel = "";
            SkillLvs.skill3forExcel = "";
            Dispatcher.Invoke(() =>
            {
                if (skill1ID.Text == "") return;
                SkillDetailCheck(SkillLvs.skillID1);
                skill1cdlv1.Text = SkillLvs.skilllv1chargetime;
                skill1cdlv6.Text = SkillLvs.skilllv6chargetime;
                skill1cdlv10.Text = SkillLvs.skilllv10chargetime;
                for (var i = 0; i <= SkillLvs.SKLFuncstrArray.Length - 1; i++)
                {
                    if (SkillLvs.SKLFuncstrArray[i] == "" && SkillLvs.skilllv1svalArray[i].Count(c => c == ',') == 1 &&
                        !SkillLvs.skilllv1svalArray[i].Contains("Hide")) SkillLvs.SKLFuncstrArray[i] = "HP回復";
                    if (ToggleFuncDiffer.IsChecked == true)
                    {
                        SkillLvs.skilllv1svalArray[i] = ModifyFuncSvalDisplay.ModifyFuncStr(SkillLvs.SKLFuncstrArray[i],
                            SkillLvs.skilllv1svalArray[i]);
                        SkillLvs.skilllv6svalArray[i] = ModifyFuncSvalDisplay.ModifyFuncStr(SkillLvs.SKLFuncstrArray[i],
                            SkillLvs.skilllv6svalArray[i]);
                        SkillLvs.skilllv10svalArray[i] =
                            ModifyFuncSvalDisplay.ModifyFuncStr(SkillLvs.SKLFuncstrArray[i],
                                SkillLvs.skilllv10svalArray[i]);
                    }

                    Skill1FuncList.Items.Add(new SkillListSval(SkillLvs.SKLFuncstrArray[i],
                        SkillLvs.skilllv1svalArray[i], SkillLvs.skilllv6svalArray[i], SkillLvs.skilllv10svalArray[i]));
                    SkillLvs.skill1forExcel += SkillLvs.SKLFuncstrArray[i] + " 【{" +
                                               (SkillLvs.skilllv1svalArray[i].Replace("\r\n", " ") ==
                                                SkillLvs.skilllv10svalArray[i].Replace("\r\n", " ")
                                                   ? SkillLvs.skilllv10svalArray[i].Replace("\r\n", " ")
                                                   : SkillLvs.skilllv1svalArray[i].Replace("\r\n", " ") + "} - {" +
                                                     SkillLvs.skilllv10svalArray[i].Replace("\r\n", " ")) + "}】\r\n";
                }

                try
                {
                    SkillLvs.skill1forExcel = SkillLvs.skill1forExcel.Substring(0, SkillLvs.skill1forExcel.Length - 2);
                }
                catch (Exception)
                {
                    // ignored
                }

                SkillDetailCheck(SkillLvs.skillID2);
                skill2cdlv1.Text = SkillLvs.skilllv1chargetime;
                skill2cdlv6.Text = SkillLvs.skilllv6chargetime;
                skill2cdlv10.Text = SkillLvs.skilllv10chargetime;
                for (var i = 0; i <= SkillLvs.SKLFuncstrArray.Length - 1; i++)
                {
                    if (SkillLvs.SKLFuncstrArray[i] == "" && SkillLvs.skilllv1svalArray[i].Count(c => c == ',') == 1 &&
                        !SkillLvs.skilllv1svalArray[i].Contains("Hide")) SkillLvs.SKLFuncstrArray[i] = "HP回復";
                    if (ToggleFuncDiffer.IsChecked == true)
                    {
                        SkillLvs.skilllv1svalArray[i] = ModifyFuncSvalDisplay.ModifyFuncStr(SkillLvs.SKLFuncstrArray[i],
                            SkillLvs.skilllv1svalArray[i]);
                        SkillLvs.skilllv6svalArray[i] = ModifyFuncSvalDisplay.ModifyFuncStr(SkillLvs.SKLFuncstrArray[i],
                            SkillLvs.skilllv6svalArray[i]);
                        SkillLvs.skilllv10svalArray[i] =
                            ModifyFuncSvalDisplay.ModifyFuncStr(SkillLvs.SKLFuncstrArray[i],
                                SkillLvs.skilllv10svalArray[i]);
                    }

                    Skill2FuncList.Items.Add(new SkillListSval(SkillLvs.SKLFuncstrArray[i],
                        SkillLvs.skilllv1svalArray[i], SkillLvs.skilllv6svalArray[i], SkillLvs.skilllv10svalArray[i]));
                    SkillLvs.skill2forExcel += SkillLvs.SKLFuncstrArray[i] + " 【{" +
                                               (SkillLvs.skilllv1svalArray[i].Replace("\r\n", " ") ==
                                                SkillLvs.skilllv10svalArray[i].Replace("\r\n", " ")
                                                   ? SkillLvs.skilllv10svalArray[i].Replace("\r\n", " ")
                                                   : SkillLvs.skilllv1svalArray[i].Replace("\r\n", " ") + "} - {" +
                                                     SkillLvs.skilllv10svalArray[i].Replace("\r\n", " ")) + "}】\r\n";
                }

                try
                {
                    SkillLvs.skill2forExcel = SkillLvs.skill2forExcel.Substring(0, SkillLvs.skill2forExcel.Length - 2);
                }
                catch (Exception)
                {
                    // ignored
                }

                SkillDetailCheck(SkillLvs.skillID3);
                skill3cdlv1.Text = SkillLvs.skilllv1chargetime;
                skill3cdlv6.Text = SkillLvs.skilllv6chargetime;
                skill3cdlv10.Text = SkillLvs.skilllv10chargetime;
                for (var i = 0; i <= SkillLvs.SKLFuncstrArray.Length - 1; i++)
                {
                    if (SkillLvs.SKLFuncstrArray[i] == "" && SkillLvs.skilllv1svalArray[i].Count(c => c == ',') == 1 &&
                        !SkillLvs.skilllv1svalArray[i].Contains("Hide")) SkillLvs.SKLFuncstrArray[i] = "HP回復";
                    if (ToggleFuncDiffer.IsChecked == true)
                    {
                        SkillLvs.skilllv1svalArray[i] = ModifyFuncSvalDisplay.ModifyFuncStr(SkillLvs.SKLFuncstrArray[i],
                            SkillLvs.skilllv1svalArray[i]);
                        SkillLvs.skilllv6svalArray[i] = ModifyFuncSvalDisplay.ModifyFuncStr(SkillLvs.SKLFuncstrArray[i],
                            SkillLvs.skilllv6svalArray[i]);
                        SkillLvs.skilllv10svalArray[i] =
                            ModifyFuncSvalDisplay.ModifyFuncStr(SkillLvs.SKLFuncstrArray[i],
                                SkillLvs.skilllv10svalArray[i]);
                    }

                    Skill3FuncList.Items.Add(new SkillListSval(SkillLvs.SKLFuncstrArray[i],
                        SkillLvs.skilllv1svalArray[i], SkillLvs.skilllv6svalArray[i], SkillLvs.skilllv10svalArray[i]));
                    SkillLvs.skill3forExcel += SkillLvs.SKLFuncstrArray[i] + " 【{" +
                                               (SkillLvs.skilllv1svalArray[i].Replace("\r\n", " ") ==
                                                SkillLvs.skilllv10svalArray[i].Replace("\r\n", " ")
                                                   ? SkillLvs.skilllv10svalArray[i].Replace("\r\n", " ")
                                                   : SkillLvs.skilllv1svalArray[i].Replace("\r\n", " ") + "} - {" +
                                                     SkillLvs.skilllv10svalArray[i].Replace("\r\n", " ")) + "}】\r\n";
                }

                try
                {
                    SkillLvs.skill3forExcel = SkillLvs.skill3forExcel.Substring(0, SkillLvs.skill3forExcel.Length - 2);
                }
                catch (Exception)
                {
                    // ignored
                }
            });
            var SCPSSLC = new Task(ServantClassPassiveSkillSvalListCheck);
            SCPSSLC.Start();
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
                RankString[98] = "?";
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
                ClassName[17] = "GrandCaster";
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
                nprateclassbase[17] = 1.6;
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
                var svtstarrate = "";
                double NPrate = 0;
                float starrate = 0;
                float deathrate = 0;
                var svtdeathrate = "";
                var svtillust = "unknown"; //illustID 不输出
                var svtcv = "unknown"; //CVID 不输出
                var svtcollectionid = "";
                var svtCVName = "unknown";
                var svtILLUSTName = "unknown";
                var svtrarity = "";
                var svthpBase = "";
                var svthpMax = "";
                var svtatkBase = "";
                var svtatkMax = "";
                GlobalPathsAndDatas.basicatk = 0;
                GlobalPathsAndDatas.basichp = 0;
                GlobalPathsAndDatas.CurveType = "";
                GlobalPathsAndDatas.maxatk = 0;
                GlobalPathsAndDatas.maxhp = 0;
                var svtcriticalWeight = "";
                var svtpower = "";
                var svtdefense = "";
                var svtagility = "";
                var svtmagic = "";
                var svtluck = "";
                var svttreasureDevice = "";
                var svtHideAttri = "";
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
                var svtIndividualityInput = "";
                GlobalPathsAndDatas.askxlsx = true;
                foreach (var svtIDtmp in GlobalPathsAndDatas.mstSvtArray)
                    if (((JObject) svtIDtmp)["id"].ToString() == JB.svtid)
                    {
                        var mstSvtobjtmp = JObject.Parse(svtIDtmp.ToString());
                        svtName = mstSvtobjtmp["name"].ToString();
                        Svtname.Text = svtName;
                        JB.svtnme = svtName;
                        svtNameDisplay = mstSvtobjtmp["battleName"].ToString();
                        SvtBattlename.Text = svtNameDisplay;
                        Title += " - " + svtNameDisplay;
                        svtClass = mstSvtobjtmp["classId"].ToString();
                        svtgender = mstSvtobjtmp["genderType"].ToString();
                        svtstarrate = mstSvtobjtmp["starRate"].ToString();
                        svtdeathrate = mstSvtobjtmp["deathRate"].ToString();
                        svtcollectionid = mstSvtobjtmp["collectionNo"].ToString();
                        GlobalPathsAndDatas.CurveType = mstSvtobjtmp["expType"].ToString();
                        svtIndividualityInput = mstSvtobjtmp["individuality"].ToString().Replace("\n", "")
                            .Replace("\t", "")
                            .Replace("\r", "").Replace(" ", "").Replace("[", "").Replace("]", "");
                        collection.Text = svtcollectionid;
                        svtHideAttri = mstSvtobjtmp["attri"].ToString().Replace("1", "人").Replace("2", "天")
                            .Replace("3", "地").Replace("4", "星").Replace("5", "兽");
                        CardArrange = mstSvtobjtmp["cardIds"].ToString().Replace("\n", "").Replace("\t", "")
                            .Replace("\r", "").Replace(" ", "").Replace("2", "B").Replace("1", "A").Replace("3", "Q");
                        if (CardArrange == "[Q,Q,Q,Q,Q]") GlobalPathsAndDatas.askxlsx = false;
                        var SISI = new Task(() => { CheckSvtIndividuality(svtIndividualityInput); });
                        if (ToggleDispIndi.IsChecked == true)
                            SISI.Start();
                        else
                            svtIndividuality.Text = svtIndividualityInput;
                        cards.Text = CardArrange;
                        svtClassPassiveID = mstSvtobjtmp["classPassive"].ToString().Replace("\n", "").Replace("\t", "")
                            .Replace("\r", "").Replace(" ", "").Replace("[", "").Replace("]", "");
                        SkillLvs.ClassPassiveID = svtClassPassiveID;
                        var SCPSC = new Task(ServantClassPassiveSkillCheck);
                        SCPSC.Start();
                        hiddenattri.Text = svtHideAttri;
                        classData = int.Parse(svtClass);
                        try
                        {
                            svtclass.Text = ClassName[classData];
                        }
                        catch (Exception)
                        {
                            svtclass.Text = ReadClassName.ReadClassOriginName(classData);
                        }

                        var CheckShiZhuang = new Task(() => { CheckSvtIsFullinGame(classData); });
                        CheckShiZhuang.Start();
                        genderData = int.Parse(svtgender);
                        gendle.Text = gender[genderData];
                        starrate = float.Parse(svtstarrate) / 10;
                        ssvtstarrate.Text = starrate + "%";
                        deathrate = float.Parse(svtdeathrate) / 10;
                        ssvtdeathrate.Text = deathrate + "%";
                        break;
                    }

                foreach (var svtLimittmp in GlobalPathsAndDatas.mstSvtLimitArray)
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
                        GlobalPathsAndDatas.basicatk = Convert.ToInt32(svtatkBase);
                        GlobalPathsAndDatas.basichp = Convert.ToInt32(svthpBase);
                        GlobalPathsAndDatas.maxatk = Convert.ToInt32(svtatkMax);
                        GlobalPathsAndDatas.maxhp = Convert.ToInt32(svthpMax);
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
                        atkbalance1.Content = "( x 1.0 -)";
                        atkbalance2.Content = "( x 1.0 -)";
                        if (ToggleMsgboxOutputCheck.IsChecked != true || !GlobalPathsAndDatas.askxlsx) return;
                        Thread.Sleep(500);
                        Dispatcher.Invoke(() =>
                        {
                            GlobalPathsAndDatas.SuperMsgBoxRes = MessageBox.Show(
                                Application.Current.MainWindow,
                                "是否需要以xlsx的形式导出该从者的基础数据?",
                                "导出?", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                        });
                        if (GlobalPathsAndDatas.SuperMsgBoxRes == MessageBoxResult.OK)
                            ExcelFileOutput();
                        break;
                    case 3:
                        atkbalance1.Content = "( x 1.05 △)";
                        atkbalance2.Content = "( x 1.05 △)";
                        if (ToggleMsgboxOutputCheck.IsChecked != true || !GlobalPathsAndDatas.askxlsx) return;
                        Thread.Sleep(500);
                        Dispatcher.Invoke(() =>
                        {
                            GlobalPathsAndDatas.SuperMsgBoxRes = MessageBox.Show(
                                Application.Current.MainWindow,
                                "是否需要以xlsx的形式导出该从者的基础数据?",
                                "导出?", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                        });
                        if (GlobalPathsAndDatas.SuperMsgBoxRes == MessageBoxResult.OK)
                            ExcelFileOutput();
                        break;
                    case 5:
                    case 6:
                        atkbalance1.Content = "( x 0.9 ▽)";
                        atkbalance2.Content = "( x 0.9 ▽)";
                        if (ToggleMsgboxOutputCheck.IsChecked != true || !GlobalPathsAndDatas.askxlsx) return;
                        Thread.Sleep(500);
                        Dispatcher.Invoke(() =>
                        {
                            GlobalPathsAndDatas.SuperMsgBoxRes = MessageBox.Show(
                                Application.Current.MainWindow,
                                "是否需要以xlsx的形式导出该从者的基础数据?",
                                "导出?", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                        });
                        if (GlobalPathsAndDatas.SuperMsgBoxRes == MessageBoxResult.OK)
                            ExcelFileOutput();
                        break;
                    case 2:
                        atkbalance1.Content = "( x 0.95 ▽)";
                        atkbalance2.Content = "( x 0.95 ▽)";
                        if (ToggleMsgboxOutputCheck.IsChecked != true || !GlobalPathsAndDatas.askxlsx) return;
                        Thread.Sleep(500);
                        Dispatcher.Invoke(() =>
                        {
                            GlobalPathsAndDatas.SuperMsgBoxRes = MessageBox.Show(
                                Application.Current.MainWindow,
                                "是否需要以xlsx的形式导出该从者的基础数据?",
                                "导出?", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                        });
                        if (GlobalPathsAndDatas.SuperMsgBoxRes == MessageBoxResult.OK)
                            ExcelFileOutput();
                        break;
                    case 7:
                    case 9:
                    case 11:
                        atkbalance1.Content = "( x 1.1 △)";
                        atkbalance2.Content = "( x 1.1 △)";
                        if (ToggleMsgboxOutputCheck.IsChecked != true || !GlobalPathsAndDatas.askxlsx) return;
                        Thread.Sleep(500);
                        Dispatcher.Invoke(() =>
                        {
                            GlobalPathsAndDatas.SuperMsgBoxRes = MessageBox.Show(
                                Application.Current.MainWindow,
                                "是否需要以xlsx的形式导出该从者的基础数据?",
                                "导出?", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                        });
                        if (GlobalPathsAndDatas.SuperMsgBoxRes == MessageBoxResult.OK)
                            ExcelFileOutput();
                        break;
                    case 1001:
                        RemindText.Text = "此ID为礼装ID,图鉴编号为礼装的图鉴编号.礼装描述在羁绊文本的文本1处.";
                        break;
                    default:
                        atkbalance1.Content = "( x 1.0 -)";
                        atkbalance2.Content = "( x 1.0 -)";
                        break;
                }
            });
        }

        private void ServantJibanTextCheck()
        {
            foreach (var SCTMP in GlobalPathsAndDatas.mstSvtCommentArray)
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

        private void ServantCombineLimitItemsCheck()
        {
            foreach (var mstCombineLimittmp in GlobalPathsAndDatas.mstCombineLimitArray)
                if (((JObject) mstCombineLimittmp)["id"].ToString() == JB.svtid)
                {
                    var LimitID = ((JObject) mstCombineLimittmp)["svtLimit"].ToString();
                    switch (Convert.ToInt64(LimitID))
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            var itemIds = ((JObject) mstCombineLimittmp)["itemIds"].ToString().Replace("\n", "")
                                .Replace("\t", "")
                                .Replace("\r", "").Replace(" ", "").Replace("[", "").Replace("]", "");
                            var itemNums = ((JObject) mstCombineLimittmp)["itemNums"].ToString().Replace("\n", "")
                                .Replace("\t", "")
                                .Replace("\r", "").Replace(" ", "").Replace("[", "").Replace("]", "");
                            var qp = ((JObject) mstCombineLimittmp)["qp"].ToString();
                            var itemIdArray = itemIds.Split(',');
                            var itemNumsArray = itemNums.Split(',');
                            var itemDisplay = "";
                            for (var i = 0; i < itemIdArray.Length; i++)
                                itemDisplay += CheckItemName(itemIdArray[i]) + "(" + itemNumsArray[i] + "),";
                            itemDisplay = itemDisplay.Substring(0, itemDisplay.Length - 1);
                            LimitCombineItems.Dispatcher.Invoke(() =>
                            {
                                LimitCombineItems.Items.Add(
                                    new ItemList(LimitID + " → " + (Convert.ToInt64(LimitID) + 1), itemDisplay,
                                        qp));
                            });
                            break;
                    }
                }
        }

        private void ServantCombineSkillItemsCheck()
        {
            foreach (var mstCombineSkilltmp in GlobalPathsAndDatas.mstCombineSkillArray)
                if (((JObject) mstCombineSkilltmp)["id"].ToString() == JB.svtid)
                {
                    var LimitID = ((JObject) mstCombineSkilltmp)["skillLv"].ToString();
                    switch (Convert.ToInt64(LimitID))
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            var itemIds = ((JObject) mstCombineSkilltmp)["itemIds"].ToString().Replace("\n", "")
                                .Replace("\t", "")
                                .Replace("\r", "").Replace(" ", "").Replace("[", "").Replace("]", "");
                            var itemNums = ((JObject) mstCombineSkilltmp)["itemNums"].ToString().Replace("\n", "")
                                .Replace("\t", "")
                                .Replace("\r", "").Replace(" ", "").Replace("[", "").Replace("]", "");
                            var qp = ((JObject) mstCombineSkilltmp)["qp"].ToString();
                            var itemIdArray = itemIds.Split(',');
                            var itemNumsArray = itemNums.Split(',');
                            var itemDisplay = "";
                            for (var i = 0; i < itemIdArray.Length; i++)
                                itemDisplay += CheckItemName(itemIdArray[i]) + "(" + itemNumsArray[i] + "),";
                            itemDisplay = itemDisplay.Substring(0, itemDisplay.Length - 1);
                            SkillCombineItems.Dispatcher.Invoke(() =>
                            {
                                SkillCombineItems.Items.Add(
                                    new ItemList(LimitID + " → " + (Convert.ToInt64(LimitID) + 1), itemDisplay,
                                        qp));
                            });
                            break;
                    }
                }
        }

        private string CheckItemName(object ID)
        {
            switch (Convert.ToInt64(ID))
            {
                case 8:
                    return "宝具强化";
                case 9:
                    return "技能强化";
            }

            foreach (var mstItemtmp in GlobalPathsAndDatas.mstItemArray)
            {
                if (((JObject) mstItemtmp)["id"].ToString() != ID.ToString()) continue;
                var mstItemtmpobjtmp = JObject.Parse(mstItemtmp.ToString());
                return mstItemtmpobjtmp["name"].ToString();
            }

            return "未知材料" + ID;
        }

        private void ServantCVandIllustCheck()
        {
            var svtillust = "unknown"; //illustID 不输出
            var svtcv = "unknown"; //CVID 不输出
            var svtCVName = "unknown";
            var svtILLUSTName = "unknown";
            foreach (var svtIDtmp in GlobalPathsAndDatas.mstSvtArray)
                if (((JObject) svtIDtmp)["id"].ToString() == JB.svtid)
                {
                    var mstSvtobjtmp = JObject.Parse(svtIDtmp.ToString());
                    svtillust = mstSvtobjtmp["illustratorId"].ToString(); //illustID
                    svtcv = mstSvtobjtmp["cvId"].ToString(); //CVID
                    break;
                }

            foreach (var cvidtmp in GlobalPathsAndDatas.mstCvArray)
                if (((JObject) cvidtmp)["id"].ToString() == svtcv)
                {
                    var mstCVobjtmp = JObject.Parse(cvidtmp.ToString());
                    svtCVName = mstCVobjtmp["name"].ToString();
                    cv.Dispatcher.Invoke(() => { cv.Text = svtCVName; });
                    break;
                }

            foreach (var illustidtmp in GlobalPathsAndDatas.mstIllustratorArray)
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
            foreach (var svtCardtmp in GlobalPathsAndDatas.mstSvtCardArray)
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
            string[] svtTreasureDeviceFuncIDArray = null;
            string[] svtTreasureDeviceFuncArray;
            var svtTreasureDeviceFunc = string.Empty;
            SkillLvs.TDFuncstrArray = null;
            SkillLvs.TDlv1OC1strArray = null;
            SkillLvs.TDlv2OC2strArray = null;
            SkillLvs.TDlv3OC3strArray = null;
            SkillLvs.TDlv4OC4strArray = null;
            SkillLvs.TDlv5OC5strArray = null;
            SkillLvs.TDforExcel = "";
            foreach (var TDLVtmp in GlobalPathsAndDatas.mstTreasureDeviceLvArray)
            {
                if (((JObject) TDLVtmp)["treaureDeviceId"].ToString() == svtTDID &&
                    ((JObject) TDLVtmp)["lv"].ToString() == "1")
                {
                    var TDLVobjtmp = JObject.Parse(TDLVtmp.ToString());
                    var NPval1 = TDLVobjtmp["svals"].ToString().Replace("\n", "").Replace("\r", "")
                        .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "").Replace("*,", "|");
                    if (NPval1.Length >= 2) NPval1 = NPval1.Substring(0, NPval1.Length - 2);
                    SkillLvs.TDlv1OC1strArray = NPval1.Split('|');
                }

                if (((JObject) TDLVtmp)["treaureDeviceId"].ToString() == svtTDID &&
                    ((JObject) TDLVtmp)["lv"].ToString() == "2")
                {
                    var TDLVobjtmp = JObject.Parse(TDLVtmp.ToString());
                    var NPval2 = TDLVobjtmp["svals2"].ToString().Replace("\n", "").Replace("\r", "")
                        .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "").Replace("*,", "|");
                    if (NPval2.Length >= 2) NPval2 = NPval2.Substring(0, NPval2.Length - 2);
                    SkillLvs.TDlv2OC2strArray = NPval2.Split('|');
                }

                if (((JObject) TDLVtmp)["treaureDeviceId"].ToString() == svtTDID &&
                    ((JObject) TDLVtmp)["lv"].ToString() == "3")
                {
                    var TDLVobjtmp = JObject.Parse(TDLVtmp.ToString());
                    var NPval3 = TDLVobjtmp["svals3"].ToString().Replace("\n", "").Replace("\r", "")
                        .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "").Replace("*,", "|");
                    if (NPval3.Length >= 2) NPval3 = NPval3.Substring(0, NPval3.Length - 2);
                    SkillLvs.TDlv3OC3strArray = NPval3.Split('|');
                }

                if (((JObject) TDLVtmp)["treaureDeviceId"].ToString() == svtTDID &&
                    ((JObject) TDLVtmp)["lv"].ToString() == "4")
                {
                    var TDLVobjtmp = JObject.Parse(TDLVtmp.ToString());
                    var NPval4 = TDLVobjtmp["svals4"].ToString().Replace("\n", "").Replace("\r", "")
                        .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "").Replace("*,", "|");
                    if (NPval4.Length >= 2) NPval4 = NPval4.Substring(0, NPval4.Length - 2);
                    SkillLvs.TDlv4OC4strArray = NPval4.Split('|');
                }

                if (((JObject) TDLVtmp)["treaureDeviceId"].ToString() != svtTDID ||
                    ((JObject) TDLVtmp)["lv"].ToString() != "5") continue;
                {
                    var TDLVobjtmp = JObject.Parse(TDLVtmp.ToString());
                    var NPval5 = TDLVobjtmp["svals5"].ToString().Replace("\n", "").Replace("\r", "")
                        .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "").Replace("*,", "|");
                    if (NPval5.Length >= 2) NPval5 = NPval5.Substring(0, NPval5.Length - 2);
                    SkillLvs.TDlv5OC5strArray = NPval5.Split('|');
                    svtTreasureDeviceFuncID = TDLVobjtmp["funcId"].ToString().Replace("\n", "").Replace("\t", "")
                        .Replace("\r", "").Replace(" ", "").Replace("[", "").Replace("]", "");
                    svtTreasureDeviceFuncIDArray = svtTreasureDeviceFuncID.Split(',');
                }
            }

            try
            {
                var NeedTranslate = false;
                ToggleBuffFuncTranslate.Dispatcher.Invoke(() =>
                {
                    if (ToggleBuffFuncTranslate.IsChecked == true) NeedTranslate = true;
                });
                if (NeedTranslate)
                    svtTreasureDeviceFuncArray = (from skfuncidtmp in svtTreasureDeviceFuncIDArray
                        from functmp in GlobalPathsAndDatas.mstFuncArray
                        where ((JObject) functmp)["id"].ToString() == skfuncidtmp
                        select JObject.Parse(functmp.ToString())
                        into mstFuncobjtmp
                        select TranslateBuff(mstFuncobjtmp["popupText"].ToString())).ToArray();
                else
                    svtTreasureDeviceFuncArray = (from skfuncidtmp in svtTreasureDeviceFuncIDArray
                        from functmp in GlobalPathsAndDatas.mstFuncArray
                        where ((JObject) functmp)["id"].ToString() == skfuncidtmp
                        select JObject.Parse(functmp.ToString())
                        into mstFuncobjtmp
                        select mstFuncobjtmp["popupText"].ToString()).ToArray();
                SkillLvs.TDFuncstrArray = svtTreasureDeviceFuncArray;
                svtTreasureDeviceFunc = string.Join(", ", svtTreasureDeviceFuncArray);
                SkillLvs.TDFuncstr = svtTreasureDeviceFunc;
                for (var i = 0; i <= SkillLvs.TDFuncstrArray.Length - 1; i++)
                {
                    if ((SkillLvs.TDFuncstrArray[i] == "なし" || SkillLvs.TDFuncstrArray[i] == "" &&
                            SkillLvs.TDlv1OC1strArray[i].Contains("Hide")) &&
                        SkillLvs.TDlv1OC1strArray[i].Count(c => c == ',') > 0)
                        SkillLvs.TDFuncstrArray[i] = TranslateTDAttackName(svtTreasureDeviceFuncIDArray[i]);

                    if (SkillLvs.TDFuncstrArray[i] == "" && SkillLvs.TDlv1OC1strArray[i].Count(c => c == ',') == 1 &&
                        !SkillLvs.TDlv1OC1strArray[i].Contains("Hide")) SkillLvs.TDFuncstrArray[i] = "HP回復";
                    ToggleFuncDiffer.Dispatcher.Invoke(() =>
                    {
                        if (ToggleFuncDiffer.IsChecked != true) return;
                        SkillLvs.TDlv1OC1strArray[i] = ModifyFuncSvalDisplay.ModifyFuncStr(SkillLvs.TDFuncstrArray[i],
                            SkillLvs.TDlv1OC1strArray[i]);
                        SkillLvs.TDlv2OC2strArray[i] = ModifyFuncSvalDisplay.ModifyFuncStr(SkillLvs.TDFuncstrArray[i],
                            SkillLvs.TDlv2OC2strArray[i]);
                        SkillLvs.TDlv3OC3strArray[i] = ModifyFuncSvalDisplay.ModifyFuncStr(SkillLvs.TDFuncstrArray[i],
                            SkillLvs.TDlv3OC3strArray[i]);
                        SkillLvs.TDlv4OC4strArray[i] = ModifyFuncSvalDisplay.ModifyFuncStr(SkillLvs.TDFuncstrArray[i],
                            SkillLvs.TDlv4OC4strArray[i]);
                        SkillLvs.TDlv5OC5strArray[i] = ModifyFuncSvalDisplay.ModifyFuncStr(SkillLvs.TDFuncstrArray[i],
                            SkillLvs.TDlv5OC5strArray[i]);
                    });
                    TDFuncList.Dispatcher.Invoke(() =>
                    {
                        TDFuncList.Items.Add(new TDlistSval(
                            SkillLvs.TDFuncstrArray[i] != "" ? SkillLvs.TDFuncstrArray[i] : "未知效果",
                            SkillLvs.TDlv1OC1strArray[i], SkillLvs.TDlv2OC2strArray[i], SkillLvs.TDlv3OC3strArray[i],
                            SkillLvs.TDlv4OC4strArray[i], SkillLvs.TDlv5OC5strArray[i]));
                    });
                    SkillLvs.TDforExcel += (SkillLvs.TDFuncstrArray[i] != ""
                                               ? SkillLvs.TDFuncstrArray[i].Replace("\r\n", "")
                                               : "未知效果") +
                                           " 【{" + (SkillLvs.TDlv1OC1strArray[i].Replace("\r\n", " ") ==
                                                    SkillLvs.TDlv5OC5strArray[i].Replace("\r\n", " ")
                                               ? SkillLvs.TDlv5OC5strArray[i].Replace("\r\n", " ")
                                               : SkillLvs.TDlv1OC1strArray[i].Replace("\r\n", " ") + "} - {" +
                                                 SkillLvs.TDlv5OC5strArray[i].Replace("\r\n", " ")) + "}】\r\n";
                }

                try
                {
                    SkillLvs.TDforExcel = SkillLvs.TDforExcel.Substring(0, SkillLvs.TDforExcel.Length - 2);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private string TranslateTDAttackName(string TDFuncID)
        {
            try
            {
                var GetTDFuncTranslationListArray = HttpRequest.GetTDAttackNameTranslationList().Replace("\r\n", "")
                    .Replace("+", Environment.NewLine).Split('|');
                var TDTranslistFullArray = new string[GetTDFuncTranslationListArray.Length][];
                for (var i = 0; i < GetTDFuncTranslationListArray.Length; i++)
                {
                    var TempSplit2 = GetTDFuncTranslationListArray[i].Split(',');
                    TDTranslistFullArray[i] = new string[TempSplit2.Length];
                    for (var j = 0; j < TempSplit2.Length; j++) TDTranslistFullArray[i][j] = TempSplit2[j];
                }

                for (var k = 0; k < GetTDFuncTranslationListArray.Length; k++)
                    if (TDTranslistFullArray[k][0] == TDFuncID)
                        return TDTranslistFullArray[k][1];
                return "暫無翻譯\r\nID: " + TDFuncID;
            }
            catch (Exception e)
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        Application.Current.MainWindow, "翻译列表损坏.\r\n" + e, "错误", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                });
                return "FuncID: " + TDFuncID;
            }
        }

        private string TranslateBuff(string buffname)
        {
            try
            {
                var TranslationListArray = GlobalPathsAndDatas.TranslationList.Replace("\r\n", "").Split('|');
                var TranslationListFullArray = new string[TranslationListArray.Length][];
                for (var i = 0; i < TranslationListArray.Length; i++)
                {
                    var TempSplit2 = TranslationListArray[i].Split(',');
                    TranslationListFullArray[i] = new string[TempSplit2.Length];
                    for (var j = 0; j < TempSplit2.Length; j++) TranslationListFullArray[i][j] = TempSplit2[j];
                }

                for (var k = 0; k < TranslationListArray.Length; k++)
                    if (buffname.Contains(TranslationListFullArray[k][0]))
                        buffname = buffname.Replace(TranslationListFullArray[k][0], TranslationListFullArray[k][1]);
                return buffname;
            }
            catch (Exception e)
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        Application.Current.MainWindow, "翻译列表损坏.\r\n" + e, "错误", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                });
                return buffname;
            }
        }

        private void CheckSvtIsFullinGame(object classid)
        {
            Dispatcher.Invoke(() =>
            {
                if (collection.Text != "0" || cards.Text == "[Q,Q,Q,Q,Q]") return;
                switch (Convert.ToInt64(classid))
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 23:
                    case 25:
                        RemindText.Text = "该从者尚未实装或为敌方数据,故最终实装的数据可能会与目前的解析结果不同,请以实装之后的数据为准!望知悉.";
                        break;
                }
            });
        }

        private void ServantClassPassiveSkillCheck()
        {
            try
            {
                string[] svtClassPassiveIDArray = null;
                string[] svtClassPassiveArray;
                var svtClassPassive = string.Empty;
                svtClassPassiveIDArray = SkillLvs.ClassPassiveID.Split(',');
                svtClassPassiveArray = (from skilltmp in GlobalPathsAndDatas.mstSkillArray
                    from classpassiveidtmp in svtClassPassiveIDArray
                    where ((JObject) skilltmp)["id"].ToString() == classpassiveidtmp
                    select JObject.Parse(skilltmp.ToString())
                    into mstsvtPskillobjtmp
                    select mstsvtPskillobjtmp["name"].ToString() != ""
                        ? mstsvtPskillobjtmp["name"] + " (" + mstsvtPskillobjtmp["id"] + ")"
                        : "未知技能???" + " (" + mstsvtPskillobjtmp["id"] + ")").ToArray();
                svtClassPassive = string.Join(", ", svtClassPassiveArray);
                classskill.Dispatcher.Invoke(() => { classskill.Text = svtClassPassive; });
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void ServantClassPassiveSkillSvalListCheck()
        {
            try
            {
                var svtClassPassiveIDListArray = SkillLvs.ClassPassiveID.Split(',');
                var ClassPassiveSkillFuncName = "";
                var SvalStr = "";
                var NeedTranslate = false;
                ToggleBuffFuncTranslate.Dispatcher.Invoke(() =>
                {
                    if (ToggleBuffFuncTranslate.IsChecked == true) NeedTranslate = true;
                });
                for (var i = 0; i <= svtClassPassiveIDListArray.Length - 1; i++)
                {
                    foreach (var skilltmp in GlobalPathsAndDatas.mstSkillArray)
                    {
                        if (((JObject) skilltmp)["id"].ToString() != svtClassPassiveIDListArray[i]) continue;
                        var skillobjtmp = JObject.Parse(skilltmp.ToString());
                        ClassPassiveSkillFuncName = NeedTranslate
                            ? TranslateBuff(skillobjtmp["name"].ToString())
                            : skillobjtmp["name"].ToString();
                    }

                    SkillDetailCheck(svtClassPassiveIDListArray[i]);
                    for (var j = 0; j <= SkillLvs.SKLFuncstrArray.Length - 1; j++)
                    {
                        if (SkillLvs.SKLFuncstrArray[j] == "" &&
                            SkillLvs.skilllv1svalArray[j].Count(c => c == ',') == 1 &&
                            !SkillLvs.skilllv10svalArray[j].Contains("Hide"))
                            SkillLvs.SKLFuncstrArray[j] = "HP回復";
                        ToggleFuncDiffer.Dispatcher.Invoke(() =>
                        {
                            if (ToggleFuncDiffer.IsChecked == true)
                                SkillLvs.skilllv10svalArray[j] =
                                    ModifyFuncSvalDisplay.ModifyFuncStr(SkillLvs.SKLFuncstrArray[j],
                                        SkillLvs.skilllv10svalArray[j]);
                        });
                    }

                    var FuncStr = "\r\n" + string.Join("\r\n", SkillLvs.SKLFuncstrArray) + "\r\n";
                    if (SkillLvs.skilllv10svalArray == null)
                    {
                        SvalStr = "\r\n";
                    }
                    else
                    {
                        SvalStr = "\r\n" + string.Join("\r\n", SkillLvs.skilllv10svalArray) + "\r\n";
                        ClassPassiveFuncList.Dispatcher.Invoke(() =>
                        {
                            ClassPassiveFuncList.Items.Add(new ClassPassiveSvalList(ClassPassiveSkillFuncName,
                                svtClassPassiveIDListArray[i], FuncStr, SvalStr));
                        });
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
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
            foreach (var svtskill in GlobalPathsAndDatas.mstSvtSkillArray)
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
                    ((JObject) svtskill)["num"].ToString() == "1" &&
                    ((JObject) svtskill)["priority"].ToString() == "3")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    SkillLvs.skillID1 = mstsvtskillobjtmp["skillId"].ToString();
                    IsSk1Strengthened.Dispatcher.Invoke(() => { IsSk1Strengthened.Text = "√(2)"; });
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
                    ((JObject) svtskill)["num"].ToString() == "2" &&
                    ((JObject) svtskill)["priority"].ToString() == "3")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    SkillLvs.skillID2 = mstsvtskillobjtmp["skillId"].ToString();
                    IsSk2Strengthened.Dispatcher.Invoke(() => { IsSk2Strengthened.Text = "√(2)"; });
                    sk2IsStrengthened = true;
                }

                if (((JObject) svtskill)["svtId"].ToString() == JB.svtid &&
                    ((JObject) svtskill)["num"].ToString() == "3" &&
                    ((JObject) svtskill)["priority"].ToString() == "1")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    SkillLvs.skillID3 = mstsvtskillobjtmp["skillId"].ToString();
                }

                if (((JObject) svtskill)["svtId"].ToString() == JB.svtid &&
                    ((JObject) svtskill)["num"].ToString() == "3" &&
                    ((JObject) svtskill)["priority"].ToString() == "2")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    SkillLvs.skillID3 = mstsvtskillobjtmp["skillId"].ToString();
                    IsSk3Strengthened.Dispatcher.Invoke(() => { IsSk3Strengthened.Text = "√"; });
                    sk3IsStrengthened = true;
                }

                if (((JObject) svtskill)["svtId"].ToString() != JB.svtid ||
                    ((JObject) svtskill)["num"].ToString() != "3" ||
                    ((JObject) svtskill)["priority"].ToString() != "3") continue;
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    SkillLvs.skillID3 = mstsvtskillobjtmp["skillId"].ToString();
                    IsSk3Strengthened.Dispatcher.Invoke(() => { IsSk3Strengthened.Text = "√(2)"; });
                    sk3IsStrengthened = true;
                }
            }

            if (SkillLvs.skillID1 == "" || SkillLvs.skillID2 == "" || SkillLvs.skillID3 == "")
            {
                SkillLvs.skillID1 = FindSkillIDinNPCSvt(JB.svtid, 1);
                SkillLvs.skillID2 = FindSkillIDinNPCSvt(JB.svtid, 2);
                SkillLvs.skillID3 = FindSkillIDinNPCSvt(JB.svtid, 3);
            }

            var SSLC = new Task(() => { ServantSkillLevelCheck(); });
            SSLC.Start();

            Dispatcher.Invoke(() =>
            {
                skill1ID.Text = SkillLvs.skillID1;
                skill2ID.Text = SkillLvs.skillID2;
                skill3ID.Text = SkillLvs.skillID3;
                foreach (var skilltmp in GlobalPathsAndDatas.mstSkillArray)
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

                foreach (var skillDetailtmp in GlobalPathsAndDatas.mstSkillDetailArray)
                {
                    if (((JObject) skillDetailtmp)["id"].ToString() == SkillLvs.skillID1)
                    {
                        var skillDetailobjtmp = JObject.Parse(skillDetailtmp.ToString());
                        if (ToggleDetailbr.IsChecked == true)
                            skill1detail = skillDetailobjtmp["detail"].ToString().Replace("[{0}]", "[Lv.1 - Lv.10]")
                                .Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "")
                                .Replace(@"＆", "\r\n ＋").Replace(@"＋", "\r\n ＋").Replace("\r\n \r\n", "\r\n");
                        else
                            skill1detail = skillDetailobjtmp["detail"].ToString().Replace("[{0}]", "[Lv.1 - Lv.10]")
                                .Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "")
                                .Replace(@"＆", " ＋");
                        skill1details.Text = skill1detail;
                    }

                    if (((JObject) skillDetailtmp)["id"].ToString() == SkillLvs.skillID2)
                    {
                        var skillDetailobjtmp = JObject.Parse(skillDetailtmp.ToString());
                        if (ToggleDetailbr.IsChecked == true)
                            skill2detail = skillDetailobjtmp["detail"].ToString().Replace("[{0}]", "[Lv.1 - Lv.10]")
                                .Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "")
                                .Replace(@"＆", "\r\n ＋").Replace(@"＋", "\r\n ＋").Replace("\r\n \r\n", "\r\n");
                        else
                            skill2detail = skillDetailobjtmp["detail"].ToString().Replace("[{0}]", "[Lv.1 - Lv.10]")
                                .Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "")
                                .Replace(@"＆", " ＋");
                        skill2details.Text = skill2detail;
                    }

                    if (((JObject) skillDetailtmp)["id"].ToString() != SkillLvs.skillID3) continue;
                    {
                        var skillDetailobjtmp = JObject.Parse(skillDetailtmp.ToString());
                        if (ToggleDetailbr.IsChecked == true)
                            skill3detail = skillDetailobjtmp["detail"].ToString().Replace("[{0}]", "[Lv.1 - Lv.10]")
                                .Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "")
                                .Replace(@"＆", "\r\n ＋").Replace(@"＋", "\r\n ＋").Replace("\r\n \r\n", "\r\n");
                        else
                            skill3detail = skillDetailobjtmp["detail"].ToString().Replace("[{0}]", "[Lv.1 - Lv.10]")
                                .Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "")
                                .Replace(@"＆", " ＋");
                        skill3details.Text = skill3detail;
                    }
                }
            });
        }

        private string FindSkillIDinNPCSvt(string svtid, int skillnum)
        {
            foreach (var npcSvtFollowertmp in GlobalPathsAndDatas.npcSvtFollowerArray)
            {
                if (((JObject) npcSvtFollowertmp)["svtId"].ToString() != svtid) continue;
                var npcSvtFollowerobjtmp = JObject.Parse(npcSvtFollowertmp.ToString());
                switch (skillnum)
                {
                    case 1:
                        return npcSvtFollowerobjtmp["skillId1"].ToString();
                    case 2:
                        return npcSvtFollowerobjtmp["skillId2"].ToString();
                    case 3:
                        return npcSvtFollowerobjtmp["skillId3"].ToString();
                }
            }

            return "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var OSI = new Task(() => { OutputSVTIDs(); });
            OSI.Start();
        }

        private void OutputSVTIDs()
        {
            Dispatcher.Invoke(async () =>
            {
                try
                {
                    var output = GlobalPathsAndDatas.mstSvtArray.Aggregate("",
                        (current, svtIDtmp) => current + "ID: " + ((JObject) svtIDtmp)["id"] + "    " + "名称: " +
                                               ((JObject) svtIDtmp)["name"] + "\r\n");
                    File.WriteAllText(GlobalPathsAndDatas.path + "/SearchIDList.txt", output);
                    Dispatcher.Invoke(async () =>
                    {
                        await this.ShowMessageAsync("完成", "导出成功, 文件名为 SearchIDList.txt");
                    });
                    Process.Start(GlobalPathsAndDatas.path + "/SearchIDList.txt");
                }
                catch (Exception)
                {
                    //ignore
                }
            });
        }

        private void ClearTexts()
        {
            Dispatcher.Invoke(() =>
            {
                var g = Content as Grid;
                GlobalPathsAndDatas.TranslationListArray = null;
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
                LimitCombineItems.Items.Clear();
                SkillCombineItems.Items.Clear();
                TDFuncList.Items.Clear();
                ClassPassiveFuncList.Items.Clear();
                RemindText.Text = "";
                Title = "FGO从者基础信息解析";
                chartCanvas.Children.Remove(plhp);
                chartCanvas.Children.Remove(platk);
            });
            IsSk1Strengthened.Dispatcher.Invoke(() => { IsSk1Strengthened.Text = "×"; });
            IsSk2Strengthened.Dispatcher.Invoke(() => { IsSk2Strengthened.Text = "×"; });
            IsSk3Strengthened.Dispatcher.Invoke(() => { IsSk3Strengthened.Text = "×"; });
            IsNPStrengthened.Dispatcher.Invoke(() => { IsNPStrengthened.Text = "×"; });
            GC.Collect();
        }

        private void ClearLists()
        {
            Dispatcher.Invoke(() =>
            {
                PickupEventList.Items.Clear();
                PickupEndedEventList.Items.Clear();
                ClassList.Items.Clear();
                PickupQuestList.Items.Clear();
            });
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
                var NeedTranslate = false || ToggleBuffFuncTranslate.IsChecked == true;
                foreach (var SKLTMP in GlobalPathsAndDatas.mstSkillLvArray)
                {
                    if (((JObject) SKLTMP)["skillId"].ToString() == sklid && ((JObject) SKLTMP)["lv"].ToString() == "1")
                    {
                        var SKLobjtmp = JObject.Parse(SKLTMP.ToString());
                        SkillLvs.skilllv1sval = SKLobjtmp["svals"].ToString().Replace("\n", "").Replace("\r", "")
                            .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "").Replace("*,", "|");
                        SkillLvs.skilllv1sval = SkillLvs.skilllv1sval.Substring(0, SkillLvs.skilllv1sval.Length - 2);
                        SkillLvs.skilllv1chargetime = SKLobjtmp["chargeTurn"].ToString();
                        SkillLvs.skilllv1svalArray = SkillLvs.skilllv1sval.Split('|');
                    }

                    if (((JObject) SKLTMP)["skillId"].ToString() == sklid && ((JObject) SKLTMP)["lv"].ToString() == "6")
                    {
                        var SKLobjtmp = JObject.Parse(SKLTMP.ToString());
                        SkillLvs.skilllv6sval = SKLobjtmp["svals"].ToString().Replace("\n", "").Replace("\r", "")
                            .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "").Replace("*,", "|");
                        SkillLvs.skilllv6sval = SkillLvs.skilllv6sval.Substring(0, SkillLvs.skilllv6sval.Length - 2);
                        SkillLvs.skilllv6chargetime = SKLobjtmp["chargeTurn"].ToString();
                        SkillLvs.skilllv6svalArray = SkillLvs.skilllv6sval.Split('|');
                    }

                    if (((JObject) SKLTMP)["skillId"].ToString() != sklid ||
                        ((JObject) SKLTMP)["lv"].ToString() != "10") continue;
                    {
                        var SKLobjtmp = JObject.Parse(SKLTMP.ToString());
                        SkillLvs.skilllv10sval = SKLobjtmp["svals"].ToString().Replace("\n", "").Replace("\r", "")
                            .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "").Replace("*,", "|");
                        SkillLvs.skilllv10sval = SkillLvs.skilllv10sval.Substring(0, SkillLvs.skilllv10sval.Length - 2);
                        SkillLvs.skilllv10chargetime = SKLobjtmp["chargeTurn"].ToString();
                        SkillLvs.skilllv10svalArray = SkillLvs.skilllv10sval.Split('|');
                        svtSKFuncID = SKLobjtmp["funcId"].ToString().Replace("\n", "").Replace("\t", "")
                            .Replace("\r", "").Replace(" ", "").Replace("[", "").Replace("]", "");
                        svtSKFuncIDList = new List<string>(svtSKFuncID.Split(','));
                        svtSKFuncIDArray = svtSKFuncIDList.ToArray();
                        if (NeedTranslate)
                            svtSKFuncList.AddRange(from skfuncidtmp in svtSKFuncIDArray
                                from functmp in GlobalPathsAndDatas.mstFuncArray
                                where ((JObject) functmp)["id"].ToString() == skfuncidtmp
                                select JObject.Parse(functmp.ToString())
                                into mstFuncobjtmp
                                select TranslateBuff(mstFuncobjtmp["popupText"].ToString()));
                        else
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
            var clean = new Task(() => { ClearTexts(); });
            var clean2 = new Task(() => { ClearLists(); });
            clean2.Start();
            clean.Start();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)

        {
            if (sender is Hyperlink source) Process.Start(source.NavigateUri.ToString());
        }

        private async void HttpRequestData()
        {
            var path = Directory.GetCurrentDirectory();
            var gamedata = new DirectoryInfo(path + @"\Android\masterdata\");
            var folder = new DirectoryInfo(path + @"\Android\");
            var LoadData = new Task(LoadorRenewCommonDatas.ReloadData);
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
            progressbar.Dispatcher.Invoke(() => { progressbar.Value += 250; });
            if (!Directory.Exists(folder.FullName))
            {
                updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = "正在创建Android目录..."; });
                Directory.CreateDirectory(folder.FullName);
            }

            if (!Directory.Exists(gamedata.FullName))
                Directory.CreateDirectory(gamedata.FullName);
            updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = "开始下载/更新游戏数据......"; });
            progressbar.Dispatcher.Invoke(() => { progressbar.Value += 250; });

            try
            {
                result = HttpRequest.PhttpReq("https://game.fate-go.jp/gamedata/top", "appVer=2.17.0");
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
                            Dispatcher.Invoke(() =>
                            {
                                GlobalPathsAndDatas.SuperMsgBoxRes = MessageBox.Show(
                                    Application.Current.MainWindow,
                                    "游戏服务器正在维护，请在维护后下载/更新数据. \r\n以下为服务器公告内容:\r\n\r\n『" +
                                    tmp.Replace("[00FFFF]", "").Replace("[url=", "")
                                        .Replace("][u]公式サイト お知らせ[/u][/url][-]", "") + "』\r\n\r\n点击\"确定\"可打开公告页面.",
                                    "维护中", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                            });
                            if (GlobalPathsAndDatas.SuperMsgBoxRes == MessageBoxResult.OK)
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
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(Application.Current.MainWindow, "网络连接异常,请检查网络连接并重试.\r\n" + e, "网络连接异常",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                });
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
                    Dispatcher.Invoke(() =>
                    {
                        GlobalPathsAndDatas.SuperMsgBoxRes = MessageBox.Show(
                            Application.Current.MainWindow,
                            "当前的MasterData已是最新版本,无需重复下载.\r\n\r\n您确定要重新覆盖当前的MasterData数据吗?\r\n\r\n点击\"确认\"进行覆盖.",
                            "无需更新",
                            MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    });
                    if (GlobalPathsAndDatas.SuperMsgBoxRes == MessageBoxResult.Cancel)
                    {
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
                updatestatus.Content = "写入: " + gamedata.FullName + "assetbundle";
            });
            progressbar.Dispatcher.Invoke(() => { progressbar.Value += 40; });
            File.WriteAllText(gamedata.FullName + "master",
                res["response"][0]["success"]["master"].ToString());
            updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = "写入: " + gamedata.FullName + "master"; });
            progressbar.Dispatcher.Invoke(() => { progressbar.Value += 40; });
            File.WriteAllText(gamedata.FullName + "webview",
                res["response"][0]["success"]["webview"].ToString());
            updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = "写入: " + gamedata.FullName + "webview"; });
            progressbar.Dispatcher.Invoke(() => { progressbar.Value += 40; });
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
                    updatestatus.Content = "写入: " + gamedata.FullName +
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
            progressbar.Dispatcher.Invoke(() => { progressbar.Value += 40; });
            var data3 = File.ReadAllText(gamedata.FullName + "webview");
            var dictionary2 =
                (Dictionary<string, object>) MasterDataUnpacker.MouseGame2MsgPack(
                    Convert.FromBase64String(data3));
            var str2 = "baseURL: " + dictionary2["baseURL"] + "\r\n contactURL: " +
                       dictionary2["contactURL"] + "\r\n";
            updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = str2; });
            progressbar.Dispatcher.Invoke(() => { progressbar.Value += 40; });
            var filePassInfo = (Dictionary<string, object>) dictionary2["filePass"];
            str = filePassInfo.Aggregate(str, (current, a) => current + a.Key + ": " + a.Value + "\r\n");
            File.WriteAllText(gamedata.FullName + "webview.txt", str2);
            updatestatus.Dispatcher.Invoke(() =>
            {
                updatestatus.Content = "写入: " + gamedata.FullName + "webview.txt";
            });

            updatestatus.Dispatcher.Invoke(() => { updatestatus.Content = "下载完成，可以开始解析."; });

            progressbar.Dispatcher.Invoke(() => { progressbar.Value = progressbar.Maximum; });
            Dispatcher.Invoke(async () => { await this.ShowMessageAsync("完成", "下载完成，可以开始解析."); });
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
            LoadData.Start();
            GC.Collect();
        }


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var UpgradeMasterData = new Task(() => { HttpRequestData(); });
            UpgradeMasterData.Start();
        }

        private async void JBOut()
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
            Dispatcher.Invoke(async () =>
            {
                await this.ShowMessageAsync("完成", "导出完成.\n\r文件名为: " + GlobalPathsAndDatas.outputdir.FullName +
                                                  "羁绊文本_" + JB.svtid + "_" + JB.svtnme +
                                                  ".txt");
            });

            Process.Start(GlobalPathsAndDatas.outputdir.FullName + "/" + "羁绊文本_" + JB.svtid + "_" + JB.svtnme + ".txt");
        }

        private void JBOutput_Click(object sender, RoutedEventArgs e)
        {
            var JO = new Task(() => { JBOut(); });
            JO.Start();
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            if (sender is Hyperlink source) Process.Start(source.NavigateUri.ToString());
        }

        private async void Form_Load(object sender, RoutedEventArgs e)
        {
            var path = Directory.GetCurrentDirectory();
            var gamedata = new DirectoryInfo(path + @"\Android\masterdata\");
            var LoadData = new Task(() => { LoadorRenewCommonDatas.ReloadData(); });
            VersionLabel.Content = CommonStrings.Version;
            DrawScale();
            if (!Directory.Exists(gamedata.FullName))
            {
                Dispatcher.Invoke(async () =>
                {
                    await this.ShowMessageAsync("温馨提示:", "没有游戏数据,请先下载游戏数据(位于\"设置\"选项卡).");
                });
                Button1.IsEnabled = false;
            }
            else
            {
                try
                {
                    LoadData.Start();
                }
                catch (Exception)
                {
                    Dispatcher.Invoke(async () =>
                    {
                        await this.ShowMessageAsync("温馨提示:", "游戏数据损坏,请重新下载游戏数据(位于\"设置\"选项卡).");
                    });

                    Button1.IsEnabled = false;
                }
            }
        }

        private void ExcelFileOutput()
        {
            try
            {
                var path = Directory.GetCurrentDirectory();
                var svtData = new DirectoryInfo(path + @"\ServantData\");
                if (!Directory.Exists(svtData.FullName))
                    Directory.CreateDirectory(svtData.FullName);
                var streamget = HttpRequest.GetXlsx();
                var xlsx =
                    new ExcelPackage(streamget);
                var worksheet = xlsx.Workbook.Worksheets[0];
                worksheet.Cells["H3"].Value = JB.svtid;
                worksheet.Cells["A1"].Value += "(" + JB.svtnme + ")";
                worksheet.Cells["B3"].Value = Svtname.Text;
                worksheet.Cells["B4"].Value = svtclass.Text;
                worksheet.Cells["E4"].Value = rarity.Text;
                worksheet.Cells["H4"].Value = gendle.Text;
                worksheet.Cells["L4"].Value = hiddenattri.Text;
                worksheet.Cells["K3"].Value = collection.Text;
                worksheet.Cells["B5"].Value = cv.Text;
                worksheet.Cells["H5"].Value = illust.Text;
                worksheet.Cells["B6"].Value = ssvtstarrate.Text;
                worksheet.Cells["E6"].Value = ssvtdeathrate.Text;
                worksheet.Cells["I6"].Value = jixing.Text;
                worksheet.Cells["L6"].Value = notrealnprate.Text;
                worksheet.Cells["C7"].Value = nprate.Text;
                worksheet.Cells["C10"].Value = classskill.Text;
                worksheet.Cells["C9"].Value = basichp.Text;
                worksheet.Cells["F9"].Value = basicatk.Text;
                worksheet.Cells["I9"].Value = maxhp.Text;
                worksheet.Cells["L9"].Value = maxatk.Text;
                worksheet.Cells["C13"].Value = cards.Text;
                worksheet.Cells["D15"].Value = bustercard.Text;
                worksheet.Cells["I15"].Value = artscard.Text;
                worksheet.Cells["D16"].Value = quickcard.Text;
                worksheet.Cells["I16"].Value = extracard.Text;
                worksheet.Cells["E17"].Value = treasuredevicescard.Text;
                worksheet.Cells["C18"].Value = npcardtype.Text;
                worksheet.Cells["G18"].Value = nptype.Text;
                worksheet.Cells["J18"].Value = nprank.Text;
                worksheet.Cells["C19"].Value = npruby.Text;
                worksheet.Cells["C20"].Value = npname.Text;
                worksheet.Cells["C21"].Value = npdetail.Text;
                worksheet.Cells["P1"].Value = skill1name.Text;
                worksheet.Cells["T1"].Value = skill1cdlv1.Text;
                worksheet.Cells["V1"].Value = skill1cdlv6.Text;
                worksheet.Cells["X1"].Value = skill1cdlv10.Text;
                worksheet.Cells["P2"].Value = skill1details.Text;
                worksheet.Cells["P10"].Value = skill2name.Text;
                worksheet.Cells["T10"].Value = skill2cdlv1.Text;
                worksheet.Cells["V10"].Value = skill2cdlv6.Text;
                worksheet.Cells["X10"].Value = skill2cdlv10.Text;
                worksheet.Cells["P11"].Value = skill2details.Text;
                worksheet.Cells["P19"].Value = skill3name.Text;
                worksheet.Cells["T19"].Value = skill3cdlv1.Text;
                worksheet.Cells["V19"].Value = skill3cdlv6.Text;
                worksheet.Cells["X19"].Value = skill3cdlv10.Text;
                worksheet.Cells["P20"].Value = skill3details.Text;
                worksheet.Cells["C28"].Value = svtIndividuality.Text;
                worksheet.Cells["C12"].Value = Convert.ToString(sixwei.Content).Replace("\n", "        ");
                worksheet.Cells["P6"].Value = SkillLvs.skill1forExcel;
                worksheet.Cells["P15"].Value = SkillLvs.skill2forExcel;
                worksheet.Cells["P24"].Value = SkillLvs.skill3forExcel;
                worksheet.Cells["C24"].Value = SkillLvs.TDforExcel;
                xlsx.SaveAs(new FileInfo(svtData.FullName + JB.svtnme + "_" + JB.svtid + ".xlsx"));
                streamget.Close();
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        Application.Current.MainWindow,
                        "导出成功,文件名为: " + svtData.FullName + JB.svtnme + "_" + JB.svtid + ".xlsx", "导出完成",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                });
                Process.Start(svtData.FullName + JB.svtnme + "_" + JB.svtid + ".xlsx");
                GC.Collect();
            }
            catch (Exception e)
            {
                Dispatcher.Invoke(() =>
                {
                    GlobalPathsAndDatas.SuperMsgBoxRes = MessageBox.Show(
                        Application.Current.MainWindow,
                        "导出时遇到错误,请查看该从者的xlsx是否被占用?\r\n\r\n点击\"确认\"进行重试.\r\n\r\n" + e,
                        "导出错误",
                        MessageBoxButton.OKCancel, MessageBoxImage.Error);
                });
                if (GlobalPathsAndDatas.SuperMsgBoxRes == MessageBoxResult.OK) ExcelFileOutput();
            }
        }

        private async void VersionCheckEvent()
        {
            string VerChkRaw;
            JObject VerChk;
            JArray VerAssetsJArray;
            GlobalPathsAndDatas.ExeUpdateUrl = "";
            GlobalPathsAndDatas.NewerVersion = "";
            try
            {
                VerChkRaw = HttpRequest.GetApplicationUpdateJson();
                VerChk = JObject.Parse(VerChkRaw);
            }
            catch (Exception e)
            {
                Dispatcher.Invoke(async () =>
                {
                    await this.ShowMessageAsync("网络连接异常", "网络连接异常,请检查网络连接并重试.\r\n" + e);
                    ;
                });
                CheckUpdate.Dispatcher.Invoke(() => { CheckUpdate.IsEnabled = true; });
                return;
            }

            if (CommonStrings.VersionTag != VerChk["tag_name"].ToString())
            {
                Dispatcher.Invoke(() =>
                {
                    GlobalPathsAndDatas.SuperMsgBoxRes = MessageBox.Show(
                        Application.Current.MainWindow,
                        "检测到软件更新\r\n\r\n新版本为:  " + VerChk["tag_name"] + "    当前版本为:  " + CommonStrings.VersionTag +
                        "\r\n\r\nChangeLog:\r\n" + VerChk["body"] + "\r\n\r\n点击\"确认\"按钮可选择更新.", "检查更新",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Information);
                });
                if (GlobalPathsAndDatas.SuperMsgBoxRes == MessageBoxResult.OK)
                {
                    VerAssetsJArray = (JArray) JsonConvert.DeserializeObject(VerChk["assets"].ToString());
                    for (var i = 0; i <= VerAssetsJArray.Count - 1; i++)
                        if (VerAssetsJArray[i]["name"].ToString() == "FGOSBIAReloaded.exe")
                            GlobalPathsAndDatas.ExeUpdateUrl = VerAssetsJArray[i]["browser_download_url"].ToString();
                    if (GlobalPathsAndDatas.ExeUpdateUrl == "")
                    {
                        Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show(
                                Application.Current.MainWindow, "确认到新版本更新,但是获取下载Url失败.\r\n", "获取Url失败",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        });
                        await this.ShowMessageAsync("获取Url失败", "确认到新版本更新,但是获取下载Url失败.\r\n");
                        CheckUpdate.Dispatcher.Invoke(() => { CheckUpdate.IsEnabled = true; });
                        return;
                    }

                    var Sub = new Task(() => { DownloadFilesSub(VerChk["tag_name"].ToString()); });
                    Sub.Start();
                }
                else
                {
                    CheckUpdate.Dispatcher.Invoke(() => { CheckUpdate.IsEnabled = true; });
                }
            }
            else
            {
                Dispatcher.Invoke(async () =>
                {
                    await this.ShowMessageAsync("检查更新", "当前版本为:  " + CommonStrings.VersionTag + "\r\n\r\n无需更新.");
                });
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
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        Application.Current.MainWindow, "写入文件异常.\r\n" + e, "异常", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                });
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
            Dispatcher.Invoke(() =>
            {
                GlobalPathsAndDatas.SuperMsgBoxRes = MessageBox.Show(
                    Application.Current.MainWindow,
                    "下载完成.下载目录为: \r\n" + path + "\\FGOSBIAReloaded(Update " +
                    GlobalPathsAndDatas.NewerVersion +
                    ").exe\r\n\r\n请自行替换文件.\r\n\r\n您是否要关闭当前版本的程序?", "检查更新", MessageBoxButton.YesNo,
                    MessageBoxImage.Information);
            });
            if (GlobalPathsAndDatas.SuperMsgBoxRes == MessageBoxResult.Yes)
                Dispatcher.Invoke(Close);
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
            var VCE = new Task(VersionCheckEvent);
            VCE.Start();
        }

        private void Button_Click_Quest(object sender, RoutedEventArgs e)
        {
            var LPQL = new Task(LoadPickUPQuestList);
            ButtonQuest.IsEnabled = false;
            LPQL.Start();
        }

        private async void LoadPickUPQuestList()
        {
            var path = Directory.GetCurrentDirectory();
            var gamedata = new DirectoryInfo(path + @"\Android\masterdata\");
            var dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var QuestName = "";
            var questid = "";
            PickupQuestList.Dispatcher.Invoke(() => { PickupQuestList.Items.Clear(); });
            if (File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstQuest") &&
                File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstQuestPickup"))
            {
                foreach (var mstQuestPickup in GlobalPathsAndDatas.mstQuestPickupArray)
                {
                    var QuestPUEndTimeStamp = Convert.ToInt32(((JObject) mstQuestPickup)["endedAt"]);
                    var QuestPUStartTimeStamp = Convert.ToInt32(((JObject) mstQuestPickup)["startedAt"]);
                    var TimeMinus = (DateTime.Now.Ticks - dateTimeStart.Ticks) / 10000000;
                    if (TimeMinus > QuestPUEndTimeStamp) continue;
                    questid = ((JObject) mstQuestPickup)["questId"].ToString();
                    QuestName = GetQuestNameAndType(questid);
                    var QuestPUStartTime = new TimeSpan(long.Parse(QuestPUStartTimeStamp + "0000000"));
                    var QuestPUEndTime = new TimeSpan(long.Parse(QuestPUEndTimeStamp + "0000000"));
                    var StartStr = Convert.ToString(dateTimeStart + QuestPUStartTime);
                    var EndStr = Convert.ToString(dateTimeStart + QuestPUEndTime);
                    PickupQuestList.Dispatcher.Invoke(() =>
                    {
                        PickupQuestList.Items.Add(new QuestList(questid, QuestName, StartStr, EndStr));
                    });
                }

                ButtonQuest.Dispatcher.Invoke(() => { ButtonQuest.IsEnabled = true; });
            }
            else
            {
                Dispatcher.Invoke(async () =>
                {
                    await this.ShowMessageAsync("温馨提示:", "游戏数据损坏,请重新下载游戏数据(位于\"设置\"选项卡).");
                });
                ButtonQuest.Dispatcher.Invoke(() => { ButtonQuest.IsEnabled = true; });
            }

            GC.Collect();
        }

        private string GetQuestNameAndType(string questid)
        {
            var QuestName = "";
            foreach (var mstQuest in GlobalPathsAndDatas.mstQuestArray)
            {
                if (((JObject) mstQuest)["id"].ToString() != questid) continue;
                var CharaID = ((JObject) mstQuest)["charaIconId"].ToString();
                try
                {
                    CharaID = CharaID.Substring(0, CharaID.Length - 3) + "00";
                }
                catch (Exception)
                {
                    //ignore
                }

                var TempName = ((JObject) mstQuest)["name"].ToString();
                if (TempName.Length > 14) TempName = TempName.Insert(14, "\r\n");
                QuestName = "\r\n" + TempName + "\r\n\r\nAP消耗: " + ((JObject) mstQuest)["actConsume"] + "   等级推荐: lv." +
                            ((JObject) mstQuest)["recommendLv"] + "\r\n从者ID: " + CharaID + "\r\n";
                if (((JObject) mstQuest)["giftId"].ToString() != "0")
                {
                    var giftid = ((JObject) mstQuest)["giftId"].ToString();
                    QuestName += "任务奖励: " + CheckGiftNames(giftid) + "\r\n";
                }
                else
                {
                    var itemid = ((JObject) mstQuest)["giftIconId"].ToString();
                    QuestName += "任务奖励: " + CheckItemName(itemid) + "\r\n";
                }

                break;
            }

            return QuestName;
        }

        private string CheckGiftNames(string giftid)
        {
            var giftids = "";
            var giftquantities = "";
            var giftNames = "";
            foreach (var mstGifttmp in GlobalPathsAndDatas.mstGiftArray)
            {
                if (((JObject) mstGifttmp)["id"].ToString() != giftid) continue;
                giftids += ((JObject) mstGifttmp)["objectId"] + ",";
                giftquantities += ((JObject) mstGifttmp)["num"] + ",";
            }

            try
            {
                giftids = giftids.Substring(0, giftids.Length - 1);
                giftquantities = giftquantities.Substring(0, giftquantities.Length - 1);
                var giftidArray = giftids.Split(',');
                var giftQuantityArray = giftquantities.Split(',');
                for (var ii = 0; ii < giftidArray.Length; ii++)
                    giftNames += CheckItemName(giftidArray[ii]) +
                                 (giftQuantityArray[ii] == "0" ? "" : " x " + giftQuantityArray[ii]) + ",";
                giftNames = giftNames.Substring(0, giftNames.Length - 1);
                return giftNames;
            }
            catch (Exception)
            {
                return "Error Loading GiftNames.";
            }
        }

        private void Button_Click_Class(object sender, RoutedEventArgs e)
        {
            var LCAR = new Task(LoadClassAndRelations);
            ButtonClass.IsEnabled = false;
            LCAR.Start();
        }

        private async void LoadClassAndRelations()
        {
            var path = Directory.GetCurrentDirectory();
            var gamedata = new DirectoryInfo(path + @"\Android\masterdata\");
            ClassList.Dispatcher.Invoke(() => { ClassList.Items.Clear(); });
            if (File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstClass") &&
                File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstClassRelation"))
            {
                foreach (var mstClasstmp in GlobalPathsAndDatas.mstClassArray)
                {
                    var ClassName = "";
                    var WeakClass = "\r\n";
                    var ResistClass = "\r\n";
                    ClassName = ((JObject) mstClasstmp)["name"] + "(" + ((JObject) mstClasstmp)["id"] + ")";
                    var tmpid = ((JObject) mstClasstmp)["id"].ToString();
                    foreach (var mstClassRelationtmp in GlobalPathsAndDatas.mstClassRelationArray)
                    {
                        if (((JObject) mstClassRelationtmp)["atkClass"].ToString() != tmpid) continue;
                        var ATKRATE = Convert.ToInt64(((JObject) mstClassRelationtmp)["attackRate"].ToString());
                        if (ATKRATE > 1000)
                        {
                            var tmpweakid = ((JObject) mstClassRelationtmp)["defClass"].ToString();
                            WeakClass +=
                                (GetClassName(tmpweakid) == "？"
                                    ? GetClassName(tmpweakid) + "(ID:" + tmpweakid + ")"
                                    : GetClassName(tmpweakid)) + " (" + (float) ATKRATE / 1000 + "x)\r\n";
                        }
                        else if (ATKRATE < 1000)
                        {
                            var tmpresistid = ((JObject) mstClassRelationtmp)["defClass"].ToString();
                            ResistClass +=
                                (GetClassName(tmpresistid) == "？"
                                    ? GetClassName(tmpresistid) + "(ID:" + tmpresistid + ")"
                                    : GetClassName(tmpresistid)) + "(" + (float) ATKRATE / 1000 + "x)\r\n";
                        }
                    }

                    WeakClass = WeakClass.Substring(0, WeakClass.Length - 2) + "\r\n";
                    ResistClass = ResistClass.Substring(0, ResistClass.Length - 2) + "\r\n";
                    ClassList.Dispatcher.Invoke(() =>
                    {
                        ClassList.Items.Add(new ClassRelationList(ClassName, WeakClass, ResistClass));
                    });
                }

                ButtonClass.Dispatcher.Invoke(() => { ButtonClass.IsEnabled = true; });
                RemindText.Dispatcher.Invoke(() => { RemindText.Text = "显示完毕."; });
                Thread.Sleep(1500);
                RemindText.Dispatcher.Invoke(() =>
                {
                    if (RemindText.Text != "") RemindText.Text = "";
                });
            }
            else
            {
                Dispatcher.Invoke(async () =>
                {
                    await this.ShowMessageAsync("温馨提示:", "游戏数据损坏,请重新下载游戏数据(位于\"设置\"选项卡).");
                });
                ButtonClass.Dispatcher.Invoke(() => { ButtonClass.IsEnabled = true; });
            }
        }

        private static string GetClassName(string id)
        {
            var ClassName = "";
            foreach (var mstClasstmp in GlobalPathsAndDatas.mstClassArray)
            {
                if (((JObject) mstClasstmp)["id"].ToString() != id) continue;
                ClassName = ((JObject) mstClasstmp)["name"].ToString();
                break;
            }

            return ClassName;
        }

        private async void LoadEventList()
        {
            var path = Directory.GetCurrentDirectory();
            var gamedata = new DirectoryInfo(path + @"\Android\masterdata\");
            var dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var EventName = "";
            var Eventid = "";
            PickupEventList.Dispatcher.Invoke(() => { PickupEventList.Items.Clear(); });
            PickupEndedEventList.Dispatcher.Invoke(() => { PickupEndedEventList.Items.Clear(); });
            if (File.Exists(gamedata.FullName + "decrypted_masterdata/" + "mstEvent"))
            {
                foreach (var mstEventtmp in GlobalPathsAndDatas.mstEventArray)
                {
                    var EventEndTimeStamp = Convert.ToInt32(((JObject) mstEventtmp)["endedAt"]);
                    if (EventEndTimeStamp == 1893423600) continue;
                    var TimeMinus = (DateTime.Now.Ticks - dateTimeStart.Ticks) / 10000000;
                    EventName = ((JObject) mstEventtmp)["name"].ToString();
                    if (EventName.Length > 40) EventName = EventName.Insert(40, "\r\n");
                    Eventid = ((JObject) mstEventtmp)["id"].ToString();
                    var EventEndTime = new TimeSpan(long.Parse(EventEndTimeStamp + "0000000"));
                    var EndStr = Convert.ToString(dateTimeStart + EventEndTime);
                    RemindText.Dispatcher.Invoke(() =>
                    {
                        RemindText.Text = "查看: " + EventName + " (" + Eventid + ").";
                    });
                    if (TimeMinus > EventEndTimeStamp)
                        PickupEndedEventList.Dispatcher.Invoke(() =>
                        {
                            PickupEndedEventList.Items.Add(new EventList(Eventid, EventName, EndStr));
                        });
                    else
                        PickupEventList.Dispatcher.Invoke(() =>
                        {
                            PickupEventList.Items.Add(new EventList(Eventid, EventName, EndStr));
                        });
                }

                ButtonEvent.Dispatcher.Invoke(() => { ButtonEvent.IsEnabled = true; });
                RemindText.Dispatcher.Invoke(() => { RemindText.Text = "显示完毕."; });
                Thread.Sleep(1500);
                RemindText.Dispatcher.Invoke(() =>
                {
                    if (RemindText.Text != "") RemindText.Text = "";
                });
            }
            else
            {
                Dispatcher.Invoke(async () =>
                {
                    await this.ShowMessageAsync("温馨提示:", "游戏数据损坏,请重新下载游戏数据(位于\"设置\"选项卡).");
                });
                ButtonEvent.Dispatcher.Invoke(() => { ButtonEvent.IsEnabled = true; });
            }

            GC.Collect();
        }

        private void AddChart(int[] Array)
        {
            Dispatcher.Invoke(() =>
            {
                if (Array == null) throw new ArgumentNullException(nameof(Array));
                var xmin = 0.0;
                var xmax = 100.0;
                var ymin = 0.0;
                var ymax = 0.0;
                var AdjustHPCurve = new int[101];
                var AdjustATKCurve = new int[101];
                for (var lv = 0; lv < 101; lv++)
                {
                    AdjustHPCurve[lv] = GlobalPathsAndDatas.basichp +
                                        Array[lv] * (GlobalPathsAndDatas.maxhp - GlobalPathsAndDatas.basichp) / 1000;
                    AdjustATKCurve[lv] = GlobalPathsAndDatas.basicatk +
                                         Array[lv] * (GlobalPathsAndDatas.maxatk - GlobalPathsAndDatas.basicatk) / 1000;
                }

                chartCanvas.Children.Remove(plhp);
                chartCanvas.Children.Remove(platk);
                ymin = 0.0;
                ymax = Math.Max(AdjustATKCurve[100], AdjustHPCurve[100]);
                // Draw ATK curve:
                platk = new Polyline {Stroke = Brushes.Red, StrokeThickness = 2};
                for (var i = 1; i < 101; i++)
                {
                    double x = i;
                    double y = AdjustATKCurve[i];
                    var atklinep = CurvePoint(new Point(x, y), xmin, xmax, ymin, ymax);
                    platk.Points.Add(atklinep);
                }

                chartCanvas.Children.Add(platk);
                // Draw HP curve:
                plhp = new Polyline {Stroke = Brushes.Blue, StrokeThickness = 2};
                for (var i = 1; i < 101; i++)
                {
                    double x = i;
                    double y = AdjustHPCurve[i];
                    var hplinep = CurvePoint(new Point(x, y), xmin, xmax, ymin, ymax);
                    plhp.Points.Add(hplinep);
                }

                chartCanvas.Children.Add(plhp);
            });
        }

        private Point CurvePoint(Point pt, double xmin, double xmax, double ymin, double ymax)
        {
            var result = new Point
            {
                X = (pt.X - xmin) * chartCanvas.Width * 0.95 / (xmax - xmin),
                Y = chartCanvas.Height - (pt.Y - ymin) * chartCanvas.Height * 0.80
                    / (ymax - ymin)
            };
            return result;
        }

        private void DrawScale()
        {
            for (var i = 0; i < 101; i++)
            {
                var x_scale = new Line();
                x_scale.StrokeEndLineCap = PenLineCap.Triangle;
                x_scale.StrokeThickness = 1;
                x_scale.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                x_scale.X1 = 0 + i * 4.5;
                x_scale.X2 = x_scale.X1;

                x_scale.Y1 = 352;
                if (i % 5 == 0 && i != 0)
                {
                    x_scale.StrokeThickness = 2;
                    x_scale.Y2 = x_scale.Y1 - 8;
                }
                else
                {
                    x_scale.Y2 = x_scale.Y1 - 4;
                }

                Dispatcher.Invoke(() => { chartCanvas.Children.Add(x_scale); });
            }
        }

        private static int[] GetSvtCurveData(object TypeID)
        {
            var TempData = new int[101];
            foreach (var mstSvtExptmp in GlobalPathsAndDatas.mstSvtExpArray)
            {
                if (((JObject) mstSvtExptmp)["type"].ToString() != TypeID.ToString()) continue;
                TempData[Convert.ToInt32(((JObject) mstSvtExptmp)["lv"])] =
                    Convert.ToInt32(((JObject) mstSvtExptmp)["curve"].ToString());
            }

            return TempData;
        }

        private void DrawServantStrengthenCurveLine(object TypeID)
        {
            try
            {
                var BaseCurveData = GetSvtCurveData(TypeID);
                AddChart(BaseCurveData);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        public void CheckSvtIndividuality(object Input)
        {
            var IndividualityStringArray = Input.ToString().Split(',');
            var TempSplit1 = HttpRequest.GetIndividualityList().Replace("\r\n", "").Split('|');
            var IndividualityCommons = new string[TempSplit1.Length][];
            for (var i = 0; i < TempSplit1.Length; i++)
            {
                var TempSplit2 = TempSplit1[i].Split('+');
                IndividualityCommons[i] = new string[TempSplit2.Length];
                for (var j = 0; j < TempSplit2.Length; j++) IndividualityCommons[i][j] = TempSplit2[j];
            }

            var Outputs = "";
            foreach (var Cases in IndividualityStringArray)
            {
                if (Cases.Length >= 6) continue;
                if (Cases == "5010" || Cases == "5000") continue;
                for (var k = 0; k < IndividualityCommons.Length; k++)
                {
                    if (Cases == IndividualityCommons[k][0])
                    {
                        Outputs += IndividualityCommons[k][1] + ",";
                        break;
                    }

                    if (k == IndividualityCommons.Length - 1 && Cases != IndividualityCommons[k][0])
                        Outputs += "未知特性(" + Cases + "),";
                }
            }

            Outputs = Outputs.Substring(0, Outputs.Length - 1);
            svtIndividuality.Dispatcher.Invoke(() => { svtIndividuality.Text = Outputs; });
        }

        private void EasternEggSvt()
        {
            Dispatcher.Invoke(() =>
            {
                Svtname.Text = "ACPudding";
                SvtBattlename.Text = "ACPudding";
                svtIndividuality.Text = "男性,Foreigner,人之力,秩序,善,从者,人形,被EA特攻,现今生存的人类,人科从者";
                svtclass.Text = "Foreigner";
                rarity.Text = "5 ☆";
                gendle.Text = "男性";
                hiddenattri.Text = "人";
                cv.Text = "-";
                illust.Text = "-";
                ssvtstarrate.Text = "14.9%";
                ssvtdeathrate.Text = "6.8%";
                jixing.Text = "153";
                notrealnprate.Text = "0.57%";
                nprate.Text = "Quick: " + "0.77%" + "   Arts: " +
                              "0.77%" + "   Buster: " +
                              "0.77%" + "\r\nExtra: " +
                              "0.77%" + "   宝具: " + "0.77%" +
                              "   受击: " + "4.00%";
                classskill.Text = "单独行动(自宅) EX, 阵地建造(异) B+, 领域外生命 C, 道具作成(Code) B";
                basichp.Text = "2132";
                basicatk.Text = "1789";
                maxhp.Text = "14535";
                maxatk.Text = "11593";
                sixwei.Content = "筋力: " + "B+" + "        耐久: " + "A" +
                                 "\n敏捷: " +
                                 "C+" +
                                 "        魔力: " + "B+" + "\n幸运: " + "A" +
                                 "        宝具: " +
                                 "C";
                cards.Text = "[Q,Q,A,A,B]";
                bustercard.Text = "4 hit [10,20,30,40]";
                artscard.Text = "3 hit [16,33,51]";
                quickcard.Text = "5 hit [6,13,20,26,35]";
                extracard.Text = "4 hit [10,20,30,40]";
                treasuredevicescard.Text = "4 hit [10,20,30,40]";
                npcardtype.Text = "Arts";
                nptype.Text = "全体宝具";
                nprank.Text = "C (对人宝具)";
                npruby.Text = "トゥシタ·ヘブンズフィールド (Tushita Heaven's Field)";
                npname.Text = "兜率天·極楽曼荼羅";
                npdetail.Text =
                    "解除敌方全体的攻击强化状态 + 自身攻击力与防御力提升(3回合)<Over Charge时效果提升> + 对敌方全体发动强大的无视防御力攻击[lv.1-lv.5] + 自身NP获得量下降(1回合)【负面效果】";
                skill1name.Text = "迷乱代码 B";
                skill1details.Text =
                    "解除敌方全体的防御强化状态 + 当位于〔屋内〕场景时赋予防御力大幅下降的状态(3回合)[lv.1-lv.10] + 自身的Arts指令卡性能提升(5次・3回合)[lv.1-lv.10]";
                skill1cdlv1.Text = "7";
                skill1cdlv6.Text = "6";
                skill1cdlv10.Text = "5";
                Skill1FuncList.Items.Add(new SkillListSval("防御强化解除", "1000", "1000", "1000"));
                Skill1FuncList.Items.Add(new SkillListSval("防御力下降", "1000,3,-1,400", "1000,3,-1,500", "1000,3,-1,600"));
                Skill1FuncList.Items.Add(new SkillListSval("Arts性能提升", "1000,3,5,200", "1000,3,5,300", "1000,3,5,400"));
                skill2name.Text = "危险代码注入 EX";
                skill2details.Text = "己方单体的NP增加[lv.1-lv.10] + 宝具威力超绝提升(1次·1回合)[lv.1-lv.10] + 付与〔1回合后必定即死〕效果";
                skill2cdlv1.Text = "12";
                skill2cdlv6.Text = "11";
                skill2cdlv10.Text = "10";
                Skill2FuncList.Items.Add(new SkillListSval("NP增加", "1000,8000", "1000,9000", "1000,10000"));
                Skill2FuncList.Items.Add(new SkillListSval("宝具威力提升", "1000,1,1,600", "1000,1,1,700", "1000,1,1,800"));
                Skill2FuncList.Items.Add(new SkillListSval("〔1回合后必定即死〕效果", "1000,3,-1", "1000,3,-1", "1000,3,-1"));
                skill3name.Text = "共享安乐 B+";
                skill3details.Text =
                    "敌方全体攻击力下降(3回合)[lv.1-lv.10] + 除自身以外的己方攻击力下降(3回合)【负面效果】+ 己方全体强化解除耐性提升(1次·3回合)[lv.1-lv.10] + 己方全体每回合最大HP提升状态(3回合)[lv.1-lv.10] + 自身暴击威力提升(3次·3回合)[lv.1-lv.10] + 己方随机单体获得弱体无效状态(1次·3回合)";
                skill3cdlv1.Text = "8";
                skill3cdlv6.Text = "7";
                skill3cdlv10.Text = "6";
                Skill3FuncList.Items.Add(new SkillListSval("攻击力下降", "1000,3,-1,100", "1000,3,-1,200", "1000,3,-1,300"));
                Skill3FuncList.Items.Add(new SkillListSval("攻击力下降", "1000,3,-1,300", "1000,3,-1,300", "1000,3,-1,300"));
                Skill3FuncList.Items.Add(new SkillListSval("强化解除耐性提升", "1000,3,1,600", "1000,3,1,800",
                    "1000,3,1,1000"));
                Skill3FuncList.Items.Add(new SkillListSval("最大HP提升", "1000,3,-1,1000", "1000,3,-1,1500",
                    "1000,3,-1,2000"));
                Skill3FuncList.Items.Add(new SkillListSval("暴击威力提升", "1000,3,3,200", "1000,3,3,250", "1000,3,3,300"));
                Skill3FuncList.Items.Add(new SkillListSval("弱化无效状态", "1000,3,1", "1000,3,1", "1000,3,1"));
                TDFuncList.Items.Add(new TDlistSval("攻击强化解除", "1000", "1000", "1000", "1000", "1000"));
                TDFuncList.Items.Add(new TDlistSval("攻击力提升", "1000,3,-1,100", "1000,3,-1,150", "1000,3,-1,200",
                    "1000,3,-1,250", "1000,3,-1,300"));
                TDFuncList.Items.Add(new TDlistSval("防御力提升", "1000,3,-1,100", "1000,3,-1,150", "1000,3,-1,200",
                    "1000,3,-1,250", "1000,3,-1,300"));
                TDFuncList.Items.Add(new TDlistSval("防御无视攻击", "1000,6000", "1000,7500", "1000,8250", "1000,8625",
                    "1000,9000"));
                TDFuncList.Items.Add(new TDlistSval("NP获得量下降", "1000,1,-1,400", "1000,1,-1,400", "1000,1,-1,400",
                    "1000,1,-1,400", "1000,1,-1,400"));
                RemindText.Text = "注意,您触发了彩蛋,此处显示的为软件作者自己捏造的自制从者.并非游戏内实际内容!";
                jibantext1.Text = "一名初学C#的辣鸡,这个是我随便瞎诌的自设orz.\r\n有很多地方根本就不合理233.";
                jibantext2.Text = "〇 迷乱代码 B：\r\n因为自己写的代码不仅自己看着麻烦给我朋友看也觉得极其繁琐和复杂.";
                jibantext3.Text = "〇 危险代码注入 EX：\r\n为了规避掉一些奇怪的问题就写了很多奇怪的代码(我自己也看不太懂,直接百度233).";
                jibantext4.Text = "〇 共享安乐 B+：\r\n这个就没啥好解释了((((\r\n胡诌的技能.";
                jibantext5.Text = "兜率天·極楽曼荼羅 (トゥシタ·ヘブンズフィールド):\r\n自己比较喜欢杀生院的宝具名,于是查了Wiki随便按样式编了一个名字.(好中二啊orz)";
                jibantext6.Text = "为什么选择Foreigner是因为感觉逼格比较高(WinForm版程序的一设是Caster).\r\n或许可以给自己设置一个3破改变宝具名称x.";
                jibantext7.Text = "希望有缘人能够重写本烂程序.\r\n                           ---作者记";
            });
        }

        private void Button_Click_Event(object sender, RoutedEventArgs e)
        {
            ButtonEvent.IsEnabled = false;
            var LEL = new Task(LoadEventList);
            LEL.Start();
        }

        private struct SkillListSval
        {
            public string SkillName { get; }
            public string SkillSvallv1 { get; }
            public string SkillSvallv6 { get; }
            public string SkillSvallv10 { get; }

            public SkillListSval(string v1, string v2, string v3, string v4) : this()
            {
                SkillName = v1;
                SkillSvallv1 = v2;
                SkillSvallv6 = v3;
                SkillSvallv10 = v4;
            }
        }

        private struct ClassPassiveSvalList
        {
            public string ClassPassiveName { get; }
            public string ClassPassiveID { get; }
            public string ClassPassiveFuncName { get; }
            public string ClassPassiveFuncSval { get; }

            public ClassPassiveSvalList(string v1, string v2, string v3, string v4) : this()
            {
                ClassPassiveName = v1;
                ClassPassiveID = v2;
                ClassPassiveFuncName = v3;
                ClassPassiveFuncSval = v4;
            }
        }

        private struct TDlistSval
        {
            public string TDFuncName { get; }
            public string TDSvalOC1lv1 { get; }
            public string TDSvalOC2lv2 { get; }
            public string TDSvalOC3lv3 { get; }
            public string TDSvalOC4lv4 { get; }
            public string TDSvalOC5lv5 { get; }

            public TDlistSval(string v1, string v2, string v3, string v4, string v5, string v6) : this()
            {
                TDFuncName = v1;
                TDSvalOC1lv1 = v2;
                TDSvalOC2lv2 = v3;
                TDSvalOC3lv3 = v4;
                TDSvalOC4lv4 = v5;
                TDSvalOC5lv5 = v6;
            }
        }

        private struct QuestList
        {
            public string QuestNumber { get; }
            public string QuestName { get; }
            public string QuestStartTime { get; }
            public string QuestEndTime { get; }

            public QuestList(string v1, string v2, string v3, string v4) : this()
            {
                QuestNumber = v1;
                QuestName = v2;
                QuestStartTime = v3;
                QuestEndTime = v4;
            }
        }

        private struct EventList
        {
            public string EventNumber { get; }
            public string EventName { get; }
            public string EventEndTime { get; }

            public EventList(string v1, string v2, string v3) : this()
            {
                EventNumber = v1;
                EventName = v2;
                EventEndTime = v3;
            }
        }

        private struct ItemList
        {
            public string Status { get; }
            public string Items { get; }
            public string QP { get; }

            public ItemList(string v1, string v2, string v3) : this()
            {
                Status = v1;
                Items = v2;
                QP = v3;
            }
        }

        private struct ClassRelationList
        {
            public string ClassName { get; }
            public string WeakClass { get; }
            public string ResistClass { get; }

            public ClassRelationList(string v1, string v2, string v3) : this()
            {
                ClassName = v1;
                WeakClass = v2;
                ResistClass = v3;
            }
        }
    }
}