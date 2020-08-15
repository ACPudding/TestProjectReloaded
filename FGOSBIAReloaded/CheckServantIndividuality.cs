namespace FGOSBIAReloaded
{
    internal class CheckServantIndividuality
    {
        public static string IndividualityCommonString = @"
        1+男性|
        2+女性|
        3+不明性别|
        100+Saber|
        101+Lancer|
        102+Archer|
        103+Rider|
        104+Caster|
        105+Assassin|
        106+Berserker|
        107+Shielder|
        108+Ruler|
        109+AlterEgo|
        110+Avenger|
        111+魔神柱|
        112+Grand Caster|
        113+Beast I|
        114+Beast II|
        115+Moon Cancer|
        116+Beast III/R|
        117+Foreigner|
        118+Beast III/L|
        119+Beast?|
        200+天之力|
        201+地之力|
        202+人之力|
        203+星之力|
        204+兽之力|
        300+秩序|
        301+混沌|
        302+中立|
        303+善|
        304+恶|
        305+中庸|
        306+狂|
        308+夏|
        1000+从者|
        1001+人类|
        1002+死灵|
        1003+Artificial Demon|
        1004+Demon Beast|
        1005+Daemon|
        1100+Soldier|
        1101+Amazoness|
        1102+Skeleton|
        1103+Zombie|
        1104+Ghost|
        1105+Automata|
        1106+Golem|
        1107+Spellbook|
        1108+Homunculus|
        1110+Lamia|
        1111+Centaur|
        1112+Werebeast|
        1113+Chimera|
        1117+Wyvern|
        1118+Dragon Type|
        1119+Gazer|
        1120+Hand Or Door|
        1121+Demon God Pillar|
        1122+影从者|
        1132+Oni|
        1133+Hand|
        1134+Door|
        1172+人类威胁|
        2000+神性|
        2001+人形|
        2002+龙|
        2003+屠龙者|
        2004+罗马|
        2005+猛兽|
        2006+atalante|
        2007+阿尔托莉雅脸|
        2008+被EA特攻|
        2009+骑乘|
        2010+亚瑟|
        2011+天地从者|
        2012+所爱之人|
        2018+死灵&恶魔|
        2019+魔性|
        2037+天地从者(非拟拟/亚从者)|
        2038+[阳光] 场地|
        2039+[水边] 场地|
        2040+神性&死灵&恶魔|
        2073+[丛林] 场地|
        2074+Blessed By Kur|
        2075+Saber从者|
        2076+超巨大|
        2113+王|
        2114+希腊神话男性|
        2121+[燃烧] 场地|
        2355+伊莉雅|
        2356+[绅士之爱C]除女性外的对象|
        2387+Kingprotea[增值] 状态|
        2392+[城市] 场地|
        2466+[阿尔戈号] 相关人物|
        2615+性别[凯尼斯]|
        2631+人科从者|
        2632+魔兽型从者|
        2654+现今生存的人类|
        2666+巨人|
        3004+Buff|
        3005+Debuff|
        5000+Can Be In Battle";

        public static string Outputs;

        public static void CheckSvtIndividuality(object Input)
        {
            var IndividualityStringArray = Input.ToString().Split(',');
            var TempSplit1 = IndividualityCommonString.Replace("\r\n        ", "").Split('|');
            var IndividualityCommons = new string[TempSplit1.Length][];
            for (var i = 0; i < TempSplit1.Length; i++)
            {
                var TempSplit2 = TempSplit1[i].Split('+');
                IndividualityCommons[i] = new string[TempSplit2.Length];
                for (var j = 0; j < TempSplit2.Length; j++) IndividualityCommons[i][j] = TempSplit2[j];
            }

            Outputs = "";
            foreach (var Cases in IndividualityStringArray)
                for (var k = 0; k < IndividualityCommons.Length; k++)
                {
                    if (Cases != "5000" && Cases == IndividualityCommons[k][0])
                    {
                        Outputs += IndividualityCommons[k][1] + ",";
                        break;
                    }

                    if (k == IndividualityCommons.Length - 1 && Cases != IndividualityCommons[k][0] && Cases.Length <= 4
                    ) Outputs += "未知特性(" + Cases + "),";
                }

            Outputs = Outputs.Substring(0, Outputs.Length - 1);
        }
    }
}