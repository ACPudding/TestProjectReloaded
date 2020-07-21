using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FGOSBIAReloaded.Properties
{
    static class GlobalPathsAndDatas
    {
        public static string path = Directory.GetCurrentDirectory();
        public static DirectoryInfo gamedata = new DirectoryInfo(path + @"\Android\masterdata\");
        public static DirectoryInfo folder = new DirectoryInfo(path + @"\Android\");
        public static string mstSvt = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSvt");
        public static string mstSvtLimit = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSvtLimit");
        public static string mstCv = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstCv");
        public static string mstIllustrator = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstIllustrator");
        public static string mstSvtCard = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSvtCard");
        public static string mstSvtTreasureDevice =
            File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSvtTreasureDevice");
        public static string mstTreasureDevice =
            File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDevice");
        public static string mstTreasureDeviceDetail =
            File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDeviceDetail");
        public static string mstSkill = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSkill");
        public static string mstSvtSkill = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSvtSkill");
        public static string mstSkillDetail = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSkillDetail");
        public static string mstFunc = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstFunc");
        public static string mstTreasureDeviceLv =
            File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDeviceLv");
        public static string mstSvtComment = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSvtComment");
        public static string mstSkillLv = File.ReadAllText(gamedata.FullName + "decrypted_masterdata/" + "mstSkillLv");
        public static JArray mstSkillLvArray = (JArray)JsonConvert.DeserializeObject(mstSkillLv);
        public static JArray mstSvtCommentArray = (JArray)JsonConvert.DeserializeObject(mstSvtComment);
        public static JArray mstSvtArray = (JArray)JsonConvert.DeserializeObject(mstSvt);
        public static JArray mstSvtLimitArray = (JArray)JsonConvert.DeserializeObject(mstSvtLimit);
        public static JArray mstCvArray = (JArray)JsonConvert.DeserializeObject(mstCv);
        public static JArray mstIllustratorArray = (JArray)JsonConvert.DeserializeObject(mstIllustrator);
        public static JArray mstSvtCardArray = (JArray)JsonConvert.DeserializeObject(mstSvtCard);
        public static JArray mstSvtTreasureDevicedArray = (JArray)JsonConvert.DeserializeObject(mstSvtTreasureDevice);
        public static JArray mstTreasureDevicedArray = (JArray)JsonConvert.DeserializeObject(mstTreasureDevice);
        public static JArray mstTreasureDeviceDetailArray = (JArray)JsonConvert.DeserializeObject(mstTreasureDeviceDetail);
        public static JArray mstSkillArray = (JArray)JsonConvert.DeserializeObject(mstSkill);
        public static JArray mstSvtSkillArray = (JArray)JsonConvert.DeserializeObject(mstSvtSkill);
        public static JArray mstSkillDetailArray = (JArray)JsonConvert.DeserializeObject(mstSkillDetail);
        public static JArray mstFuncArray = (JArray)JsonConvert.DeserializeObject(mstFunc);
        public static JArray mstTreasureDeviceLvArray = (JArray)JsonConvert.DeserializeObject(mstTreasureDeviceLv);
        public static int svtArtsCardhit;
    }
}
