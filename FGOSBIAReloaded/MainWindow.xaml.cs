using System;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button1.IsEnabled = false;
            textbox1.Text = Regex.Replace(textbox1.Text, @"\s", "");
            var ES = new Task(() => { EasternEggSvt(); });
            /*if (textbox1.Text == "ACPD" || textbox1.Text == "acpd")
            {
                ClearTexts();
                ES.Start();
                Button1.Dispatcher.Invoke(() => { Button1.IsEnabled = true; });
                return;
            }
            */
            if (!Regex.IsMatch(textbox1.Text, "^\\d+$"))
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        Application.Current.MainWindow, "从者ID输入错误,请检查.", "温馨提示:", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                });
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
            var TempTDArray = new string[2];
            IsNPStrengthened.Dispatcher.Invoke(() => { IsNPStrengthened.Text = "×"; });
            textbox1.Dispatcher.Invoke(() => { svtID = Convert.ToString(textbox1.Text); });
            ClearTexts();
            TempTDArray = ServantAnalyzeServ.svtTreasureDeviceID(svtID);
            svtTDID = TempTDArray[0];

            var CVTask = new Task(() => { ServantCVandIllustTask(svtID); }); 
            CVTask.Start();
            var BiographyTask = new Task(() => { ServantBiographyTask(svtID); });
            BiographyTask.Start();
            var CardArrangementTask = new Task(() => { ServantCardArrangementTask(svtID); });
            CardArrangementTask.Start();

            Button1.Dispatcher.Invoke(() => { Button1.IsEnabled = true; });
            Dispatcher.Invoke(() =>
            { /*
                if (rarity.Text == "")
                {
                    MessageBox.Show(
                        Application.Current.MainWindow, "从者ID不存在或未实装,请重试.", "温馨提示:", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    ClearTexts();
                    Button1.IsEnabled = true;
                    return;
                }*/

                if (cards.Text == "[Q,Q,Q,Q,Q]" && svtclass.Text != "礼装")
                    RemindText.Text = "此ID为小怪(或部分boss以及种火芙芙),配卡、技能、宝具信息解析并不准确,请知悉.";
            });
            GC.Collect();
        }

        private void ServantSkillTask(string svtID)
        {
            var TempSkillBasicJArray = ServantAnalyzeServ.ServantSkillInformation(svtID);
            var TempSelectedJArray = TempSkillBasicJArray[0];
            var skill1ID = TempSelectedJArray["skill1ID"].ToString();
            var skill2ID = TempSelectedJArray["skill2ID"].ToString();
            var skill3ID = TempSelectedJArray["skill3ID"].ToString();
        }


        private void ServantCVandIllustTask(string svtID)
        {
            var TempValue = ServantAnalyzeServ.ServantCVandIllust(svtID);
            Dispatcher.Invoke(() =>
            {
                cv.Text = TempValue[0];
                illust.Text = TempValue[1];
            });
        }

        private void ServantBiographyTask(string svtID)
        {
            var TempBiographyJArray = ServantAnalyzeServ.ServantBiography(svtID);
            var TempSelectedJArray = TempBiographyJArray[0];
            Dispatcher.Invoke(() =>
            {
                Biography1.Text = TempSelectedJArray["Biography1"].ToString();
                Biography2.Text = TempSelectedJArray["Biography2"].ToString();
                Biography3.Text = TempSelectedJArray["Biography3"].ToString();
                Biography4.Text = TempSelectedJArray["Biography4"].ToString();
                Biography5.Text = TempSelectedJArray["Biography5"].ToString();
                Biography6.Text = TempSelectedJArray["Biography6"].ToString();
                Biography7.Text = TempSelectedJArray["Biography7"].ToString();
            });
        }

        private void ServantSkillSvalsTask(string sklid,int skillNumber,bool buffTransfer) //参数未填
        {

        }

        private void ServantBasicInformationTask(string svtID)
        {

        }

        private void ServantBasicInformationLimitTask(string svtID)
        {

        }

        private void ServantCardArrangementTask(string svtID)
        {
            var TempCardJArray = ServantAnalyzeServ.ServantCardArrange(svtID);
            var TempSelectedJArray = TempCardJArray[0];
            Dispatcher.Invoke(() =>
            {
                bustercard.Text = TempSelectedJArray["buster"].ToString();
                artscard.Text = TempSelectedJArray["arts"].ToString();
                quickcard.Text = TempSelectedJArray["quick"].ToString();
                extracard.Text = TempSelectedJArray["extra"].ToString();
            });
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

        public string TranslateBuff(string buffname)
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


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var OSI = new Task(() => { OutputSVTIDs(); });
            OSI.Start();
        }

        private void OutputSVTIDs()
        {
            Dispatcher.Invoke(() =>
            {
                try
                {
                    var output = GlobalPathsAndDatas.mstSvtArray.Aggregate("",
                        (current, svtIDtmp) => current + "ID: " + ((JObject) svtIDtmp)["id"] + "    " + "名称: " +
                                               ((JObject) svtIDtmp)["name"] + "\r\n");
                    File.WriteAllText(GlobalPathsAndDatas.path + "/SearchIDList.txt", output);
                    MessageBox.Show(Application.Current.MainWindow, "导出成功,文件名为 SearchIDList.txt", "完成",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
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

        private void HttpRequestData()
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
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show(Application.Current.MainWindow, "下载完成，可以开始解析.", "完成", MessageBoxButton.OK,
                    MessageBoxImage.Information);
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
            LoadData.Start();
            GC.Collect();
        }


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var UpgradeMasterData = new Task(() => { HttpRequestData(); });
            UpgradeMasterData.Start();
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
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show(
                    Application.Current.MainWindow, "导出完成.\n\r文件名为: " + GlobalPathsAndDatas.outputdir.FullName +
                                                    "羁绊文本_" + JB.svtid + "_" + JB.svtnme +
                                                    ".txt",
                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void Form_Load(object sender, RoutedEventArgs e)
        {
            var path = Directory.GetCurrentDirectory();
            var gamedata = new DirectoryInfo(path + @"\Android\masterdata\");
            var LoadData = new Task(() => { LoadorRenewCommonDatas.ReloadData(); });
            VersionLabel.Content = CommonStrings.Version;
            DrawScale();
            if (!Directory.Exists(gamedata.FullName))
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        Application.Current.MainWindow, "没有游戏数据,请先下载游戏数据(位于\"设置\"选项卡).", "温馨提示:", MessageBoxButton.OK,
                        MessageBoxImage.Information);
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
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(
                            Application.Current.MainWindow, "游戏数据损坏,请重新下载游戏数据(位于\"设置\"选项卡).", "温馨提示:",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
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

        private void VersionCheckEvent()
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
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        Application.Current.MainWindow, "网络连接异常,请检查网络连接并重试.\r\n" + e, "网络连接异常", MessageBoxButton.OK,
                        MessageBoxImage.Error);
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
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        Application.Current.MainWindow, "当前版本为:  " + CommonStrings.VersionTag + "\r\n\r\n无需更新.", "检查更新",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
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

        private void LoadPickUPQuestList()
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
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        Application.Current.MainWindow, "游戏数据损坏,请重新下载游戏数据(位于\"设置\"选项卡).", "温馨提示:", MessageBoxButton.OK,
                        MessageBoxImage.Error);
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

        private void LoadClassAndRelations()
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
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        Application.Current.MainWindow, "游戏数据损坏,请重新下载游戏数据(位于\"设置\"选项卡).", "温馨提示:", MessageBoxButton.OK,
                        MessageBoxImage.Error);
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

        private void LoadEventList()
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
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        Application.Current.MainWindow, "游戏数据损坏,请重新下载游戏数据(位于\"设置\"选项卡).", "温馨提示:", MessageBoxButton.OK,
                        MessageBoxImage.Error);
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
                Biography1.Text = "一名初学C#的辣鸡,这个是我随便瞎诌的自设orz.\r\n有很多地方根本就不合理233.";
                Biography2.Text = "〇 迷乱代码 B：\r\n因为自己写的代码不仅自己看着麻烦给我朋友看也觉得极其繁琐和复杂.";
                Biography3.Text = "〇 危险代码注入 EX：\r\n为了规避掉一些奇怪的问题就写了很多奇怪的代码(我自己也看不太懂,直接百度233).";
                Biography4.Text = "〇 共享安乐 B+：\r\n这个就没啥好解释了((((\r\n胡诌的技能.";
                Biography5.Text = "兜率天·極楽曼荼羅 (トゥシタ·ヘブンズフィールド):\r\n自己比较喜欢杀生院的宝具名,于是查了Wiki随便按样式编了一个名字.(好中二啊orz)";
                Biography6.Text = "为什么选择Foreigner是因为感觉逼格比较高(WinForm版程序的一设是Caster).\r\n或许可以给自己设置一个3破改变宝具名称x.";
                Biography7.Text = "希望有缘人能够重写本烂程序.\r\n                           ---作者记";
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