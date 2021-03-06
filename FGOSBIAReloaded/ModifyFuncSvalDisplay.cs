﻿using System;
using FGOSBIAReloaded.Properties;

namespace FGOSBIAReloaded
{
    //对所有从者数值进行规范化显示(可能会有误显示的问题)
    internal class ModifyFuncSvalDisplay
    {
        private static readonly string BuffTranslationListLinkA =
            "https://raw.githubusercontent.com/ACPudding/ACPudding.github.io/master/fileserv/BuffTranslation";

        private static readonly string BuffTranslationListLinkB =
            "https://gitee.com/ACPudding/ACPudding.github.io/raw/master/fileserv/BuffTranslation";

        public static string ModifyFuncStr(string Funcname, string Funcsval)
        {
            var output = Funcsval;
            string[] Tempsval = null;
            var ArrayNum = ReturnArrayNum(Funcname);
            switch (ArrayNum)
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
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 26:
                case 27:
                case 29:
                case 30:
                case 35:
                case 36:
                case 37:
                case 38:
                case 40:
                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                case 46:
                case 47:
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                case 53:
                case 54:
                case 58:
                case 59:
                case 60:
                case 61:
                case 71:
                case 72:
                case 75:
                case 77:
                case 83:
                case 86:
                case 87:
                case 88:
                case 89:
                case 90:
                case 91:
                case 92:
                case 93:
                case 94:
                case 95:
                case 96:
                case 97:
                case 98:
                case 99:
                case 100:
                case 101:
                case 102:
                case 103:
                case 108:
                case 113:
                case 114:
                case 117:
                case 118:
                case 124:
                case 125:
                case 126:
                case 127:
                case 128:
                case 132:
                case 133:
                case 134:
                case 139:
                case 140:
                case 141:
                case 149:
                case 163:
                    Tempsval = Funcsval.Split(',');
                    if (Tempsval.Length == 4)
                    {
                        try
                        {
                            output = Convert.ToDouble(Tempsval[3]) / 10 + "%" +
                                     (Tempsval[0] == "1000" ? "" : "(" + Convert.ToDouble(Tempsval[0]) / 10 + "%成功率)") +
                                     (Tempsval[1] == "-1" ? "" : " - " + Tempsval[1] + "回合") +
                                     (Tempsval[2] == "-1" ? "" : " · " + Tempsval[2] + "次");
                            break;
                        }
                        catch (Exception)
                        {
                            output = Funcsval;
                            break;
                        }
                    }
                    else
                    {
                        if (Tempsval.Length > 4)
                        {
                            var tmpstr = "";
                            for (var Q = 3; Q < Tempsval.Length; Q++) tmpstr += Tempsval[Q] + ",";
                            tmpstr = tmpstr.Substring(0, tmpstr.Length - 1);
                            output = "[" + tmpstr + "]\r\n" +
                                     (Tempsval[0] == "1000" ? "" : "(" + Convert.ToDouble(Tempsval[0]) / 10 + "%成功率)") +
                                     (Tempsval[1] == "-1" ? "" : " - " + Tempsval[1] + "回合") +
                                     (Tempsval[2] == "-1" ? "" : " · " + Tempsval[2] + "次");
                            break;
                        }

                        output = Funcsval;
                        break;
                    }
                case 31:
                case 32:
                    Tempsval = Funcsval.Split(',');
                    if (Tempsval.Length == 4)
                    {
                        try
                        {
                            output = Convert.ToDouble(Tempsval[3]) / 100 + "%" +
                                     (Tempsval[0] == "1000" ? "" : "(" + Convert.ToDouble(Tempsval[0]) / 10 + "%成功率)") +
                                     (Tempsval[1] == "-1" ? "" : " - " + Tempsval[1] + "回合") +
                                     (Tempsval[2] == "-1" ? "" : " · " + Tempsval[2] + "次");
                            break;
                        }
                        catch (Exception)
                        {
                            output = Funcsval;
                            break;
                        }
                    }
                    else
                    {
                        output = Funcsval;
                        break;
                    }
                case 55:
                case 56:
                    Tempsval = Funcsval.Split(',');
                    if (Tempsval.Length == 2)
                    {
                        try
                        {
                            output = Tempsval[1] + "格" + (Tempsval[0] == "1000"
                                ? ""
                                : "(" + Convert.ToDouble(Tempsval[0]) / 10 + "%成功率)");
                            break;
                        }
                        catch (Exception)
                        {
                            output = Funcsval;
                            break;
                        }
                    }
                    else
                    {
                        output = Funcsval;
                        break;
                    }
                case 69:
                    Tempsval = Funcsval.Split(',');
                    if (Tempsval.Length == 3)
                    {
                        try
                        {
                            output =
                                "∅" +
                                (Tempsval[0] == "1000" ? "" : "(" + Convert.ToDouble(Tempsval[0]) / 10 + "%成功率)") +
                                (Tempsval[1] == "-1" ? "" : " - " + Tempsval[1] + "回合") +
                                (Tempsval[2] == "-1" ? "" : " · " + Tempsval[2] + "次");
                            break;
                        }
                        catch (Exception)
                        {
                            output = Funcsval;
                            break;
                        }
                    }
                    else
                    {
                        output = Funcsval;
                        break;
                    }
                case 23:
                case 24:
                case 25:
                case 28:
                case 34:
                case 63:
                case 64:
                case 65:
                case 66:
                case 67:
                    Tempsval = Funcsval.Split(',');
                    if (Tempsval.Length == 4)
                    {
                        try
                        {
                            output = Tempsval[3] +
                                     (Tempsval[0] == "1000" ? "" : "(" + Convert.ToDouble(Tempsval[0]) / 10 + "%成功率)") +
                                     (Tempsval[1] == "-1" ? "" : " - " + Tempsval[1] + "回合") +
                                     (Tempsval[2] == "-1" ? "" : " · " + Tempsval[2] + "次");
                            break;
                        }
                        catch (Exception)
                        {
                            output = Funcsval;
                            break;
                        }
                    }
                    else
                    {
                        output = Funcsval;
                        break;
                    }
                case 74:
                    Tempsval = Funcsval.Split(',');
                    if (Tempsval.Length == 4)
                    {
                        try
                        {
                            output = Tempsval[3] + "段階" +
                                     (Tempsval[0] == "1000" ? "" : "(" + Convert.ToDouble(Tempsval[0]) / 10 + "%成功率)") +
                                     (Tempsval[1] == "-1" ? "" : " - " + Tempsval[1] + "回合") +
                                     (Tempsval[2] == "-1" ? "" : " · " + Tempsval[2] + "次");
                            break;
                        }
                        catch (Exception)
                        {
                            output = Funcsval;
                            break;
                        }
                    }
                    else
                    {
                        output = Funcsval;
                        break;
                    }
                case 33:
                    Tempsval = Funcsval.Split(',');
                    if (Tempsval.Length == 4)
                    {
                        try
                        {
                            output = Tempsval[3] + "個" +
                                     (Tempsval[0] == "1000" ? "" : "(" + Convert.ToDouble(Tempsval[0]) / 10 + "%成功率)") +
                                     (Tempsval[1] == "-1" ? "" : " - " + Tempsval[1] + "回合") +
                                     (Tempsval[2] == "-1" ? "" : " · " + Tempsval[2] + "次");
                            break;
                        }
                        catch (Exception)
                        {
                            output = Funcsval;
                            break;
                        }
                    }
                    else if (Tempsval.Length == 2)
                    {
                        try
                        {
                            output = Tempsval[1] + "個" + (Tempsval[0] == "1000"
                                ? ""
                                : "(" + Convert.ToDouble(Tempsval[0]) / 10 + "%成功率)");
                            break;
                        }
                        catch (Exception)
                        {
                            output = Funcsval;
                            break;
                        }
                    }
                    else
                    {
                        output = Funcsval;
                        break;
                    }
                case 57:
                case 115:
                    Tempsval = Funcsval.Split(',');
                    if (Tempsval.Length == 2)
                    {
                        try
                        {
                            output = Tempsval[1] + "個" + (Tempsval[0] == "1000"
                                ? ""
                                : "(" + Convert.ToDouble(Tempsval[0]) / 10 + "%成功率)");
                            break;
                        }
                        catch (Exception)
                        {
                            output = Funcsval;
                            break;
                        }
                    }
                    else
                    {
                        output = Funcsval;
                        break;
                    }
                default:
                    Tempsval = Funcsval.Split(',');
                    if (Tempsval.Length == 3)
                    {
                        try
                        {
                            output =
                                "∅" +
                                (Tempsval[0] == "1000" ? "" : "(" + Convert.ToDouble(Tempsval[0]) / 10 + "%成功率)") +
                                (Tempsval[1] == "-1" ? "" : " - " + Tempsval[1] + "回合") +
                                (Tempsval[2] == "-1" ? "" : " · " + Tempsval[2] + "次");
                            break;
                        }
                        catch (Exception)
                        {
                            output = Funcsval;
                            break;
                        }
                    }
                    else if (Tempsval.Length == 2)
                    {
                        try
                        {
                            output = Tempsval[1] + (Tempsval[0] == "1000"
                                ? ""
                                : "(" + Convert.ToDouble(Tempsval[0]) / 10 + "%成功率)");
                            break;
                        }
                        catch (Exception)
                        {
                            output = Funcsval;
                            break;
                        }
                    }
                    else if (Tempsval.Length == 4)
                    {
                        try
                        {
                            output = "[" + Tempsval[3] + "]\r\n" +
                                     (Tempsval[0] == "1000" ? "" : "(" + Convert.ToDouble(Tempsval[0]) / 10 + "%成功率)") +
                                     (Tempsval[1] == "-1" ? "" : " - " + Tempsval[1] + "回合") +
                                     (Tempsval[2] == "-1" ? "" : " · " + Tempsval[2] + "次");
                            break;
                        }
                        catch (Exception)
                        {
                            output = Funcsval;
                            break;
                        }
                    }
                    else if (Tempsval.Length == 1)
                    {
                        try
                        {
                            output = Convert.ToDouble(Tempsval[0]) / 10 + "%";
                            break;
                        }
                        catch (Exception)
                        {
                            output = Funcsval;
                            break;
                        }
                    }
                    else
                    {
                        if (Tempsval.Length > 4)
                        {
                            var tmpstr = "";
                            for (var Q = 3; Q < Tempsval.Length; Q++) tmpstr += Tempsval[Q] + ",";
                            tmpstr = tmpstr.Substring(0, tmpstr.Length - 1);
                            output = "[" + tmpstr + "]\r\n" +
                                     (Tempsval[0] == "1000" ? "" : "(" + Convert.ToDouble(Tempsval[0]) / 10 + "%成功率)") +
                                     (Tempsval[1] == "-1" ? "" : " - " + Tempsval[1] + "回合") +
                                     (Tempsval[2] == "-1" ? "" : " · " + Tempsval[2] + "次");
                            break;
                        }

                        output = Funcsval;
                        break;
                    }
            }

            switch (Funcname)
            {
                case "NP増加":
                case "NP減少":
                    Tempsval = Funcsval.Split(',');
                    try
                    {
                        output = Convert.ToDouble(Tempsval[1]) / 100 + "%" + (Tempsval[0] == "1000"
                            ? ""
                            : "(" + Convert.ToDouble(Tempsval[0]) / 10 + "%成功率)");
                    }
                    catch (Exception)
                    {
                        output = Funcsval;
                    }

                    break;
                case "HP減少":
                case "HP回復":
                    Tempsval = Funcsval.Split(',');
                    try
                    {
                        output = Tempsval[1] + (Tempsval[0] == "1000"
                            ? ""
                            : "(" + Convert.ToInt64(Tempsval[0]) / 10 + "%成功率)");
                    }
                    catch (Exception)
                    {
                        output = Funcsval;
                    }

                    break;
                case "毒":
                case "やけど":
                case "灼傷":
                case "呪い":
                case "詛咒":
                    Tempsval = Funcsval.Split(',');
                    if (Tempsval.Length == 4)
                    {
                        try
                        {
                            output = Tempsval[3] + "HP" +
                                     (Tempsval[0] == "1000" ? "" : "(" + Convert.ToDouble(Tempsval[0]) / 10 + "%成功率)") +
                                     (Tempsval[1] == "-1" ? "" : " - " + Tempsval[1] + "回合") +
                                     (Tempsval[2] == "-1" ? "" : " · " + Tempsval[2] + "次");
                            break;
                        }
                        catch (Exception)
                        {
                            output = Funcsval;
                            break;
                        }
                    }
                    else
                    {
                        output = Funcsval;
                        break;
                    }
                case "幻惑":
                case "呪厄":
                case "蝕毒":
                case "延焼":
                    Tempsval = Funcsval.Split(',');
                    if (Tempsval.Length == 4)
                    {
                        try
                        {
                            output = Convert.ToDouble(Tempsval[3]) / 10 + "%" +
                                     (Tempsval[0] == "1000" ? "" : "(" + Convert.ToDouble(Tempsval[0]) / 10 + "%成功率)") +
                                     (Tempsval[1] == "-1" ? "" : " - " + Tempsval[1] + "回合") +
                                     (Tempsval[2] == "-1" ? "" : " · " + Tempsval[2] + "次");
                            break;
                        }
                        catch (Exception)
                        {
                            output = Funcsval;
                            break;
                        }
                    }
                    else
                    {
                        output = Funcsval;
                        break;
                    }
            }

            if (Funcname.Contains("強力攻撃") || Funcname.Contains("防御無視攻撃"))
            {
                Tempsval = Funcsval.Split(',');
                try
                {
                    output = Convert.ToDouble(Tempsval[1]) / 10 + "%";
                }
                catch (Exception)
                {
                    output = Funcsval;
                }
            }

            if (Funcname == "宝具封印" || Funcname == "魅了")
            {
                Tempsval = Funcsval.Split(',');
                if (Tempsval.Length == 3)
                    try
                    {
                        output =
                            "∅" +
                            (Tempsval[0] == "1000" ? "" : "(" + Convert.ToDouble(Tempsval[0]) / 10 + "%成功率)") +
                            (Tempsval[1] == "-1" ? "" : " - " + Tempsval[1] + "回合") +
                            (Tempsval[2] == "-1" ? "" : " · " + Tempsval[2] + "次");
                    }
                    catch (Exception)
                    {
                        output = Funcsval;
                    }
            }

            if (Funcname == "強化解除" || Funcname == "防御強化解除" || Funcname == "攻撃強化解除" || Funcname == "攻撃弱体解除" ||
                Funcname == "防御弱体解除" || Funcname == "弱体解除" || Funcname == "必中解除" || Funcname == "回避状態解除" ||
                Funcname == "ガッツ解除" || Funcname == "毅力解除")

            {
                Tempsval = Funcsval.Split(',');
                if (Tempsval.Length == 1)
                    try
                    {
                        output =
                            "∅" +
                            (Tempsval[0] == "1000" ? "" : "(" + Convert.ToDouble(Tempsval[0]) / 10 + "%成功率)");
                    }
                    catch (Exception)
                    {
                        output = Funcsval;
                    }
            }

            if (Funcname.Contains("對特性") || Funcname.Contains("對Buff"))
            {
                Tempsval = Funcsval.Split(',');
                if (Tempsval.Length == 4)
                    try
                    {
                        output = Convert.ToDouble(Tempsval[1]) / 10 + "%" + "\r\n" + "特攻:" + Tempsval[2] + " - " +
                                 Convert.ToDouble(Tempsval[3]) / 10 + "%";
                    }
                    catch (Exception)
                    {
                        output = Funcsval;
                    }
                else
                    output = Funcsval;
            }

            if (Funcname.Contains("HP越少威力"))
            {
                Tempsval = Funcsval.Split(',');
                if (Tempsval.Length == 3)
                    try
                    {
                        output = Convert.ToDouble(Tempsval[1]) / 10 + "%" + "\r\n" + "倍率:" +
                                 Convert.ToDouble(Tempsval[2]) / 10 + "%";
                    }
                    catch (Exception)
                    {
                        output = Funcsval;
                    }
                else
                    output = Funcsval;
            }

            if (Funcname.Contains("對稀有度") || Funcname.Contains("特殊特攻攻撃"))
            {
                Tempsval = Funcsval.Split(',');
                if (Tempsval.Length > 4)
                    try
                    {
                        var tmpstr = "";
                        for (var Q = 3; Q < Tempsval.Length; Q++) tmpstr += Tempsval[Q] + ",";
                        tmpstr = tmpstr.Substring(0, tmpstr.Length - 1);
                        output = Convert.ToDouble(Tempsval[1]) / 10 + "%" + "\r\n" + "特攻相关:[" + tmpstr + "]";
                    }
                    catch (Exception)
                    {
                        output = Funcsval;
                    }
                else
                    output = Funcsval;
            }

            if (Funcname == "人格交換" || Funcname.Contains("暫無翻譯")) output = Funcsval;
            if (Funcname != "即死") return output;
            output = Convert.ToDouble(Funcsval) / 10 + "%";
            return output;
        }

        private static int ReturnArrayNum(string str)
        {
            if (GlobalPathsAndDatas.TranslationListArray == null)
                GlobalPathsAndDatas.TranslationListArray =
                    HttpRequest.GetList(BuffTranslationListLinkA, BuffTranslationListLinkB).Replace("\r\n", "")
                        .Split('|');
            var TranslationListFullArray = new string[GlobalPathsAndDatas.TranslationListArray.Length][];
            for (var i = 0; i < GlobalPathsAndDatas.TranslationListArray.Length; i++)
            {
                var TempSplit2 = GlobalPathsAndDatas.TranslationListArray[i].Split(',');
                TranslationListFullArray[i] = new string[TempSplit2.Length];
                for (var j = 0; j < TempSplit2.Length; j++) TranslationListFullArray[i][j] = TempSplit2[j];
            }

            for (var i = 0; i < GlobalPathsAndDatas.TranslationListArray.Length - 1; i++)
                if (GlobalPathsAndDatas.TranslationListArray[i].Contains(str))
                    return i + 1;
            for (var j = 0; j < GlobalPathsAndDatas.TranslationListArray.Length; j++)
                if (str.Contains(TranslationListFullArray[j][0]) || str.Contains(TranslationListFullArray[j][1]))
                    return j + 1;
            return 0;
        }
    }
}