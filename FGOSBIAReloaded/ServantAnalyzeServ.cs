using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using FGOSBIAReloaded.Properties;
using Newtonsoft.Json.Linq;

namespace FGOSBIAReloaded
{
    internal class ServantAnalyzeServ
    {
        private static string[] RankString = new string[100];
        private static string[] ClassName = new string[1500];
        private static string[] gender = new string[4];
        private static double[] nprateclassbase = new double[150];
        private static double[] nprateartscount = new double[4];
        private static double[] npratemagicbase = new double[100];

        private static string[] svtTreasureDeviceID(string svtID)
        {
            var svtTDID = "";
            var TDisStrngthened = "0";
            var result = new string[2];
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
                        TDisStrngthened = "1";
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
            catch (Exception)
            {
                //ignore
            }

            result[0] = svtTDID;
            result[1] = TDisStrngthened;
            return result;
        }

        private static string[] ServantCVandIllust(string svtID)
        {
            var svtillust = "unknown"; //illustID 不输出
            var svtcv = "unknown"; //CVID 不输出
            var svtCVName = "unknown";
            var svtILLUSTName = "unknown";
            foreach (var svtIDtmp in GlobalPathsAndDatas.mstSvtArray)
                if (((JObject) svtIDtmp)["id"].ToString() == svtID)
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
                    break;
                }

            foreach (var illustidtmp in GlobalPathsAndDatas.mstIllustratorArray)
                if (((JObject) illustidtmp)["id"].ToString() == svtillust)
                {
                    var mstillustobjtmp = JObject.Parse(illustidtmp.ToString());
                    svtILLUSTName = mstillustobjtmp["name"].ToString();
                    break;
                }

            var result = new string[2];
            result[0] = svtCVName;
            result[1] = svtILLUSTName;
            return result;
        }

        private static JArray ServantCardArrange(string svtID)
        {
            var result = new JArray();
            var svtArtsCardhit = 1;
            var svtArtsCardhitDamage = "unknown";
            var svtArtsCardQuantity = 0;
            var svtBustersCardhit = 1;
            var svtBustersCardhitDamage = "unknown";
            var svtQuicksCardhit = 1;
            var svtQuicksCardhitDamage = "unknown";
            var svtExtraCardhit = 1;
            var svtExtraCardhitDamage = "unknown";
            foreach (var svtCardtmp in GlobalPathsAndDatas.mstSvtCardArray)
            {
                if (((JObject) svtCardtmp)["svtId"].ToString() == svtID &&
                    ((JObject) svtCardtmp)["cardId"].ToString() == "1")
                {
                    var mstSvtCardobjtmp = JObject.Parse(svtCardtmp.ToString());
                    svtArtsCardhitDamage = mstSvtCardobjtmp["normalDamage"].ToString().Replace("\n", "")
                        .Replace("\t", "").Replace("\r", "").Replace(" ", "");
                    svtArtsCardhit += svtArtsCardhitDamage.Count(c => c == ',');
                    result.Add(new JArray("arts", svtArtsCardhit + " hit " + svtArtsCardhitDamage));
                    result.Add(new JArray("artsHit", svtArtsCardhit));
                }

                if (((JObject) svtCardtmp)["svtId"].ToString() == svtID &&
                    ((JObject) svtCardtmp)["cardId"].ToString() == "2")
                {
                    var mstSvtCardobjtmp = JObject.Parse(svtCardtmp.ToString());
                    svtBustersCardhitDamage = mstSvtCardobjtmp["normalDamage"].ToString().Replace("\n", "")
                        .Replace("\t", "").Replace("\r", "").Replace(" ", "");
                    svtBustersCardhit += svtBustersCardhitDamage.Count(c => c == ',');
                    result.Add(new JArray("buster", svtBustersCardhit + " hit " + svtBustersCardhitDamage));
                }

                if (((JObject) svtCardtmp)["svtId"].ToString() == svtID &&
                    ((JObject) svtCardtmp)["cardId"].ToString() == "3")
                {
                    var mstSvtCardobjtmp = JObject.Parse(svtCardtmp.ToString());
                    svtQuicksCardhitDamage = mstSvtCardobjtmp["normalDamage"].ToString().Replace("\n", "")
                        .Replace("\t", "").Replace("\r", "").Replace(" ", "");
                    svtQuicksCardhit += svtQuicksCardhitDamage.Count(c => c == ',');
                    result.Add(new JArray("quick", svtQuicksCardhit + " hit " + svtQuicksCardhitDamage));
                }

                if (((JObject) svtCardtmp)["svtId"].ToString() != svtID ||
                    ((JObject) svtCardtmp)["cardId"].ToString() != "4") continue;
                {
                    var mstSvtCardobjtmp = JObject.Parse(svtCardtmp.ToString());
                    svtExtraCardhitDamage = mstSvtCardobjtmp["normalDamage"].ToString().Replace("\n", "")
                        .Replace("\t", "").Replace("\r", "").Replace(" ", "");
                    svtExtraCardhit += svtExtraCardhitDamage.Count(c => c == ',');
                    result.Add(new JArray("extra", svtExtraCardhit + " hit " + svtExtraCardhitDamage));
                }
            }

            return result;
        }

        private JArray ServantSkillInformation(string svtID)
        {
            var skill1Name = string.Empty;
            var skill2Name = string.Empty;
            var skill3Name = string.Empty;
            var skill1detail = string.Empty;
            var skill2detail = string.Empty;
            var skill3detail = string.Empty;
            var sk1IsStrengthened = 0;
            var sk2IsStrengthened = 0;
            var sk3IsStrengthened = 0;
            var skill1ID = string.Empty;
            var skill2ID = string.Empty;
            var skill3ID = string.Empty;
            foreach (var svtskill in GlobalPathsAndDatas.mstSvtSkillArray)
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
                    sk1IsStrengthened = 1;
                }

                if (((JObject) svtskill)["svtId"].ToString() == svtID &&
                    ((JObject) svtskill)["num"].ToString() == "1" &&
                    ((JObject) svtskill)["priority"].ToString() == "3")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    skill1ID = mstsvtskillobjtmp["skillId"].ToString();
                    sk1IsStrengthened = 1;
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
                    sk2IsStrengthened = 1;
                }

                if (((JObject) svtskill)["svtId"].ToString() == svtID &&
                    ((JObject) svtskill)["num"].ToString() == "2" &&
                    ((JObject) svtskill)["priority"].ToString() == "3")
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    skill2ID = mstsvtskillobjtmp["skillId"].ToString();
                    sk2IsStrengthened = 1;
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
                    sk3IsStrengthened = 1;
                }

                if (((JObject) svtskill)["svtId"].ToString() != svtID ||
                    ((JObject) svtskill)["num"].ToString() != "3" ||
                    ((JObject) svtskill)["priority"].ToString() != "3") continue;
                {
                    var mstsvtskillobjtmp = JObject.Parse(svtskill.ToString());
                    skill3ID = mstsvtskillobjtmp["skillId"].ToString();
                    sk3IsStrengthened = 1;
                }
            }

            if (skill1ID == "" || skill2ID == "" || skill3ID == "")
            {
                skill1ID = FindSkillIDinNPCSvt(svtID, 1);
                skill2ID = FindSkillIDinNPCSvt(svtID, 2);
                skill3ID = FindSkillIDinNPCSvt(svtID, 3);
            }

            foreach (var skilltmp in GlobalPathsAndDatas.mstSkillArray)
            {
                if (((JObject) skilltmp)["id"].ToString() == skill1ID)
                {
                    var skillobjtmp = JObject.Parse(skilltmp.ToString());
                    skill1Name = skillobjtmp["name"].ToString();
                    if (sk1IsStrengthened == 1) skill1Name += " ▲";
                }

                if (((JObject) skilltmp)["id"].ToString() == skill2ID)
                {
                    var skillobjtmp = JObject.Parse(skilltmp.ToString());
                    skill2Name = skillobjtmp["name"].ToString();
                    if (sk2IsStrengthened == 1) skill2Name += " ▲";
                }

                if (((JObject) skilltmp)["id"].ToString() != skill3ID) continue;
                {
                    var skillobjtmp = JObject.Parse(skilltmp.ToString());
                    skill3Name = skillobjtmp["name"].ToString();
                    if (sk3IsStrengthened == 1) skill3Name += " ▲";
                }
            }

            foreach (var skillDetailtmp in GlobalPathsAndDatas.mstSkillDetailArray)
            {
                if (((JObject) skillDetailtmp)["id"].ToString() == skill1ID)
                {
                    var skillDetailobjtmp = JObject.Parse(skillDetailtmp.ToString());
                    skill1detail = skillDetailobjtmp["detail"].ToString().Replace("[{0}]", "[Lv.1 - Lv.10]")
                        .Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "")
                        .Replace(@"＆", " ＋");
                }

                if (((JObject) skillDetailtmp)["id"].ToString() == skill2ID)
                {
                    var skillDetailobjtmp = JObject.Parse(skillDetailtmp.ToString());
                    skill2detail = skillDetailobjtmp["detail"].ToString().Replace("[{0}]", "[Lv.1 - Lv.10]")
                        .Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "")
                        .Replace(@"＆", " ＋");
                }

                if (((JObject) skillDetailtmp)["id"].ToString() != skill3ID) continue;
                {
                    var skillDetailobjtmp = JObject.Parse(skillDetailtmp.ToString());
                    skill3detail = skillDetailobjtmp["detail"].ToString().Replace("[{0}]", "[Lv.1 - Lv.10]")
                        .Replace("[g]", "").Replace("[o]", "").Replace("[/g]", "").Replace("[/o]", "")
                        .Replace(@"＆", " ＋");
                }
            }

            var result = new JArray
            {
                new JArray("skill1ID", skill1ID),
                new JArray("skill2ID", skill2ID),
                new JArray("skill3ID", skill3ID),
                new JArray("skill1Name", skill1Name),
                new JArray("skill2Name", skill2Name),
                new JArray("skill3Name", skill3Name),
                new JArray("skill1Detail", skill1detail),
                new JArray("skill2Detail", skill2detail),
                new JArray("skill3Detail", skill3detail),
                new JArray("sk1IsStrengthened", sk1IsStrengthened),
                new JArray("sk2IsStrengthened", sk2IsStrengthened),
                new JArray("sk3IsStrengthened", sk3IsStrengthened)
            };
            return result;
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

        private JArray ServantBiography(string svtID)
        {
            var result = new JArray();
            foreach (var SCTMP in GlobalPathsAndDatas.mstSvtCommentArray)
            {
                if (((JObject) SCTMP)["svtId"].ToString() == svtID && ((JObject) SCTMP)["id"].ToString() == "1")
                {
                    var SCobjtmp = JObject.Parse(SCTMP.ToString());
                    result.Add(new JArray("Biography1", SCobjtmp["comment"].ToString().Replace("\n", "\r\n")));
                }

                if (((JObject) SCTMP)["svtId"].ToString() == svtID && ((JObject) SCTMP)["id"].ToString() == "2")
                {
                    var SCobjtmp = JObject.Parse(SCTMP.ToString());
                    result.Add(new JArray("Biography2", SCobjtmp["comment"].ToString().Replace("\n", "\r\n")));
                }

                if (((JObject) SCTMP)["svtId"].ToString() == svtID && ((JObject) SCTMP)["id"].ToString() == "3")
                {
                    var SCobjtmp = JObject.Parse(SCTMP.ToString());
                    result.Add(new JArray("Biography3", SCobjtmp["comment"].ToString().Replace("\n", "\r\n")));
                }

                if (((JObject) SCTMP)["svtId"].ToString() == svtID && ((JObject) SCTMP)["id"].ToString() == "4")
                {
                    var SCobjtmp = JObject.Parse(SCTMP.ToString());
                    result.Add(new JArray("Biography4", SCobjtmp["comment"].ToString().Replace("\n", "\r\n")));
                }

                if (((JObject) SCTMP)["svtId"].ToString() == svtID && ((JObject) SCTMP)["id"].ToString() == "5")
                {
                    var SCobjtmp = JObject.Parse(SCTMP.ToString());
                    result.Add(new JArray("Biography5", SCobjtmp["comment"].ToString().Replace("\n", "\r\n")));
                }

                if (((JObject) SCTMP)["svtId"].ToString() == svtID && ((JObject) SCTMP)["id"].ToString() == "6")
                {
                    var SCobjtmp = JObject.Parse(SCTMP.ToString());
                    result.Add(new JArray("Biography6", SCobjtmp["comment"].ToString().Replace("\n", "\r\n")));
                }

                if (((JObject) SCTMP)["svtId"].ToString() != svtID ||
                    ((JObject) SCTMP)["id"].ToString() != "7") continue;
                {
                    var SCobjtmp = JObject.Parse(SCTMP.ToString());
                    result.Add(new JArray("Biography7", SCobjtmp["comment"].ToString().Replace("\n", "\r\n")));
                }
            }

            return result;
        }

        private JArray ServantSkillSvals(string sklid, bool NeedTranslate)
        {
            var skilllv1sval = "";
            var skilllv6sval = "";
            var skilllv10sval = "";
            var skilllv1chargetime = "";
            var skilllv6chargetime = "";
            var skilllv10chargetime = "";
            var SKLFuncstr = "";
            string[] SKLFuncstrArray = null;
            string svtSKFuncID;
            string[] svtSKFuncIDArray;
            List<string> svtSKFuncIDList;
            var svtSKFuncList = new List<string>();
            string[] svtSKFuncArray;
            var svtSKFunc = string.Empty;
            foreach (var SKLTMP in GlobalPathsAndDatas.mstSkillLvArray)
            {
                if (((JObject) SKLTMP)["skillId"].ToString() == sklid && ((JObject) SKLTMP)["lv"].ToString() == "1")
                {
                    var SKLobjtmp = JObject.Parse(SKLTMP.ToString());
                    skilllv1sval = SKLobjtmp["svals"].ToString().Replace("\n", "").Replace("\r", "")
                        .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "").Replace("*,", "|");
                    skilllv1sval = skilllv1sval.Substring(0, skilllv1sval.Length - 2);
                    skilllv1chargetime = SKLobjtmp["chargeTurn"].ToString();
                }

                if (((JObject) SKLTMP)["skillId"].ToString() == sklid && ((JObject) SKLTMP)["lv"].ToString() == "6")
                {
                    var SKLobjtmp = JObject.Parse(SKLTMP.ToString());
                    skilllv6sval = SKLobjtmp["svals"].ToString().Replace("\n", "").Replace("\r", "")
                        .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "").Replace("*,", "|");
                    skilllv6sval = skilllv6sval.Substring(0, skilllv6sval.Length - 2);
                    skilllv6chargetime = SKLobjtmp["chargeTurn"].ToString();
                }

                if (((JObject) SKLTMP)["skillId"].ToString() != sklid ||
                    ((JObject) SKLTMP)["lv"].ToString() != "10") continue;
                {
                    var SKLobjtmp = JObject.Parse(SKLTMP.ToString());
                    skilllv10sval = SKLobjtmp["svals"].ToString().Replace("\n", "").Replace("\r", "")
                        .Replace("[", "").Replace("]", "*").Replace("\"", "").Replace(" ", "").Replace("*,", "|");
                    skilllv10sval = skilllv10sval.Substring(0, skilllv10sval.Length - 2);
                    skilllv10chargetime = SKLobjtmp["chargeTurn"].ToString();
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
            svtSKFunc = string.Join(", ", svtSKFuncArray);

            var result = new JArray
            {
                new JArray("skilllv1sval", skilllv1sval),
                new JArray("skilllv1CDTime", skilllv1chargetime),
                new JArray("skilllv6sval", skilllv6sval),
                new JArray("skilllv6CDTime", skilllv6chargetime),
                new JArray("skilllv10sval", skilllv10sval),
                new JArray("skilllv10CDTime", skilllv10chargetime),
                new JArray("skillFuncStr", svtSKFunc)
            };
            return result;
        }

        private static string TranslateBuff(string buffname)
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
            catch (Exception)
            {
                return buffname;
            }
        }

        private static JArray ServantBasicInformation(string svtID)
        {
            if (RankString[0] != "-")
            {
                LoadConstantData();
            }
            var svtName = "";
            var svtNameDisplay = "unknown";
            var svtClassID = ""; //ClassID
            var svtClass = "unknown";
            var svtgender = "unknown";
            var svtstarrate = "";
            double starrate = 0;
            double deathrate = 0;
            var svtdeathrate = "";
            var svtcollectionid = "";
            var svtrarity = "";
            var CurveType = "";
            var svtcriticalWeight = "";
            var svtHideAttri = "";
            var svtClassPassiveID="";
            var genderData = 0;
            var CardArrange = "[Q,Q,A,B,B]";
            var svtIndividualityInput = "";
            var svtIndividualityOutput = "";
            var askxlsx = 1;
            var svtIsFull = 1;
            var svtGender = "";
            foreach (var svtIDtmp in GlobalPathsAndDatas.mstSvtArray)
                if (((JObject)svtIDtmp)["id"].ToString() == svtID)
                {
                    var mstSvtobjtmp = JObject.Parse(svtIDtmp.ToString());
                    svtName = mstSvtobjtmp["name"].ToString();
                    svtNameDisplay = mstSvtobjtmp["battleName"].ToString();
                    svtClassID = mstSvtobjtmp["classId"].ToString();
                    svtgender = mstSvtobjtmp["genderType"].ToString();//Don't output.
                    svtstarrate = mstSvtobjtmp["starRate"].ToString();
                    svtdeathrate = mstSvtobjtmp["deathRate"].ToString();
                    svtcollectionid = mstSvtobjtmp["collectionNo"].ToString();
                    CurveType = mstSvtobjtmp["expType"].ToString();
                    svtIndividualityInput = mstSvtobjtmp["individuality"].ToString().Replace("\n", "")
                        .Replace("\t", "")
                        .Replace("\r", "").Replace(" ", "").Replace("[", "").Replace("]", "");
                    svtHideAttri = mstSvtobjtmp["attri"].ToString().Replace("1", "人").Replace("2", "天")
                        .Replace("3", "地").Replace("4", "星").Replace("5", "兽");
                    CardArrange = mstSvtobjtmp["cardIds"].ToString().Replace("\n", "").Replace("\t", "")
                        .Replace("\r", "").Replace(" ", "").Replace("2", "B").Replace("1", "A").Replace("3", "Q");
                    if (CardArrange == "[Q,Q,Q,Q,Q]") askxlsx = 0;
                    var SISI = new Task(() => { svtIndividualityOutput = CheckSvtIndividuality(svtIndividualityInput); });
                    SISI.Start();
                    svtClassPassiveID = mstSvtobjtmp["classPassive"].ToString().Replace("\n", "").Replace("\t", "")
                        .Replace("\r", "").Replace(" ", "").Replace("[", "").Replace("]", "");
                    //var SCPSC = new Task(ServantClassPassiveSkillCheck);//待写
                    //SCPSC.Start();
                    try
                    {
                        svtClass = ClassName[int.Parse(svtClassID)];
                    }
                    catch (Exception)
                    {
                        svtClass = ReadClassName.ReadClassOriginName(int.Parse(svtClassID));
                    }
                    var CheckSvt = new Task(() => { svtIsFull= CheckSvtIsFullinGame(svtClassID, svtcollectionid, CardArrange); });
                    CheckSvt.Start();
                    genderData = int.Parse(svtgender);
                    svtGender = gender[genderData];
                    starrate = double.Parse(svtstarrate) / 10;
                    deathrate = double.Parse(svtdeathrate) / 10;
                    Task.WaitAll(SISI,CheckSvt);
                    break;
                }
            var result = new JArray
            {
                new JArray("svtName", svtName),
                new JArray("svtNameDisplay", svtNameDisplay),
                new JArray("starrate", starrate),
                new JArray("deathrate", deathrate),
                new JArray("svtGender", svtGender),
                new JArray("svtIsFull", svtIsFull),
                new JArray("svtClass", svtClass),
                new JArray("svtClassID", svtClassID),
                new JArray("svtClassPassiveID", svtClassPassiveID),
                new JArray("svtIndividualityOutput", svtIndividualityOutput),
                new JArray("CardArrange", CardArrange),
                new JArray("svtHideAttri", svtHideAttri),
                new JArray("CurveType", CurveType),
                new JArray("askxlsx", askxlsx),
                new JArray("svtcollectionid", svtcollectionid)
            };
            return result;
        }

        private static int CheckSvtIsFullinGame(string classid,string collectionid,string cards)
        {
            if (collectionid != "0" || cards == "[Q,Q,Q,Q,Q]") return 1;
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
                        return 0;
                }
                return 0;
        }

        private static JArray ServantBasicInformationLimit(string svtID)
        {
            var result = new JArray();
            foreach (var svtLimittmp in GlobalPathsAndDatas.mstSvtLimitArray)
                if (((JObject)svtLimittmp)["svtId"].ToString() == JB.svtid)
                {
                    var mstsvtLimitobjtmp = JObject.Parse(svtLimittmp.ToString());
                    var svtrarity = mstsvtLimitobjtmp["rarity"] + " ☆";
                    var svthpBase = mstsvtLimitobjtmp["hpBase"].ToString();
                    var svthpMax = mstsvtLimitobjtmp["hpMax"].ToString();
                    var svtatkBase = mstsvtLimitobjtmp["atkBase"].ToString();
                    var svtatkMax = mstsvtLimitobjtmp["atkMax"].ToString();
                    var svtcriticalWeight = mstsvtLimitobjtmp["criticalWeight"].ToString();
                    var svtpower = mstsvtLimitobjtmp["power"].ToString();//Don't output.
                    var svtdefense = mstsvtLimitobjtmp["defense"].ToString();//Don't output.
                    var svtagility = mstsvtLimitobjtmp["agility"].ToString();//Don't output.
                    var svtmagic = mstsvtLimitobjtmp["magic"].ToString();//Don't output.
                    var svtluck = mstsvtLimitobjtmp["luck"].ToString();//Don't output.
                    var svttreasureDevice = mstsvtLimitobjtmp["treasureDevice"].ToString();//Don't output.
                    var powerData = int.Parse(svtpower);//Don't output.
                    var defenseData = int.Parse(svtdefense);//Don't output.
                    var agilityData = int.Parse(svtagility);//Don't output.
                    var magicData = int.Parse(svtmagic);//Don't output.
                    var luckData = int.Parse(svtluck);//Don't output.
                    var TreasureData = int.Parse(svttreasureDevice);//Don't output.
                    var sixwei = "筋力: " + RankString[powerData] + "        耐久: " + RankString[defenseData] +
                                 "\n敏捷: " +
                                 RankString[agilityData] +
                                 "        魔力: " + RankString[magicData] + "\n幸运: " + RankString[luckData] +
                                 "        宝具: " +
                                 RankString[TreasureData];
                    result.Add(new JArray("svtrarity", svtrarity));
                    result.Add(new JArray("svthpBase", svthpBase));
                    result.Add(new JArray("svthpMax", svthpMax));
                    result.Add(new JArray("svtatkBase", svtatkBase));
                    result.Add(new JArray("svtatkMax", svtatkMax));
                    result.Add(new JArray("svtcriticalWeight", svtcriticalWeight));
                    result.Add(new JArray("sixwei", sixwei));
                    break;
                }
            return result;
        }

        public static string CheckSvtIndividuality(string Input)
        {
            var IndividualityStringArray = Input.Split(',');
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
            return Outputs;
        }



        private static void LoadConstantData()
        {
            RankString[0] = "-";
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
            RankString[99] = "?";
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
            gender[1] = "男性";
            gender[2] = "女性";
            gender[3] = "其他";
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
            nprateartscount[1] = 1.5;
            nprateartscount[2] = 1.125;
            nprateartscount[3] = 1;
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
        }
    }
}