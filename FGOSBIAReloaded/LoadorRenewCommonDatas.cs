using System.IO;
using FGOSBIAReloaded.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FGOSBIAReloaded
{
    internal class LoadorRenewCommonDatas
    {
        public static void ReloadData()
        {
            GlobalPathsAndDatas.path = Directory.GetCurrentDirectory();
            GlobalPathsAndDatas.gamedata = new DirectoryInfo(GlobalPathsAndDatas.path + @"\Android\masterdata\");
            GlobalPathsAndDatas.folder = new DirectoryInfo(GlobalPathsAndDatas.path + @"\Android\");
            GlobalPathsAndDatas.outputdir = new DirectoryInfo(GlobalPathsAndDatas.path + @"\Output\");
            GlobalPathsAndDatas.mstSvt =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstSvt");
            GlobalPathsAndDatas.mstSvtLimit =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstSvtLimit");
            GlobalPathsAndDatas.mstCv =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstCv");
            GlobalPathsAndDatas.mstIllustrator =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstIllustrator");
            GlobalPathsAndDatas.mstSvtCard =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstSvtCard");
            GlobalPathsAndDatas.mstSvtTreasureDevice =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" +
                                 "mstSvtTreasureDevice");
            GlobalPathsAndDatas.mstTreasureDevice =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDevice");
            GlobalPathsAndDatas.mstTreasureDeviceDetail =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" +
                                 "mstTreasureDeviceDetail");
            GlobalPathsAndDatas.mstSkill =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstSkill");
            GlobalPathsAndDatas.mstSvtSkill =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstSvtSkill");
            GlobalPathsAndDatas.mstSkillDetail =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstSkillDetail");
            GlobalPathsAndDatas.mstFunc =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstFunc");
            GlobalPathsAndDatas.mstTreasureDeviceLv =
                File.ReadAllText(
                    GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDeviceLv");
            GlobalPathsAndDatas.mstSvtComment =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstSvtComment");
            GlobalPathsAndDatas.mstCombineLimit =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstCombineLimit");
            GlobalPathsAndDatas.mstCombineSkill =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstCombineSkill");
            GlobalPathsAndDatas.mstSkillLv =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstSkillLv");
            GlobalPathsAndDatas.mstQuest =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstQuest");
            GlobalPathsAndDatas.mstItem =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstItem");
            GlobalPathsAndDatas.mstQuestPickup =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstQuestPickup");
            GlobalPathsAndDatas.npcSvtFollower =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "npcSvtFollower");
            GlobalPathsAndDatas.mstEvent =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstEvent");
            GlobalPathsAndDatas.mstClass =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstClass");
            GlobalPathsAndDatas.mstClassRelation =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstClassRelation");
            GlobalPathsAndDatas.mstGift =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstGift");
            GlobalPathsAndDatas.mstSvtExp =
                File.ReadAllText(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstSvtExp");
            GlobalPathsAndDatas.mstSvtExpArray =
                (JArray)JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstSvtExp);
            GlobalPathsAndDatas.mstGiftArray =
                (JArray)JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstGift);
            GlobalPathsAndDatas.mstClassArray =
                (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstClass);
            GlobalPathsAndDatas.mstClassRelationArray =
                (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstClassRelation);
            GlobalPathsAndDatas.mstEventArray =
                (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstEvent);
            GlobalPathsAndDatas.npcSvtFollowerArray =
                (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.npcSvtFollower);
            GlobalPathsAndDatas.mstQuestArray = (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstQuest);
            GlobalPathsAndDatas.mstItemArray = (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstItem);
            GlobalPathsAndDatas.mstQuestPickupArray =
                (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstQuestPickup);
            GlobalPathsAndDatas.mstCombineLimitArray =
                (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstCombineLimit);
            GlobalPathsAndDatas.mstCombineSkillArray =
                (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstCombineSkill);
            GlobalPathsAndDatas.mstSkillLvArray =
                (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstSkillLv);
            GlobalPathsAndDatas.mstSvtCommentArray =
                (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstSvtComment);
            GlobalPathsAndDatas.mstSvtArray = (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstSvt);
            GlobalPathsAndDatas.mstSvtLimitArray =
                (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstSvtLimit);
            GlobalPathsAndDatas.mstCvArray = (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstCv);
            GlobalPathsAndDatas.mstIllustratorArray =
                (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstIllustrator);
            GlobalPathsAndDatas.mstSvtCardArray =
                (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstSvtCard);
            GlobalPathsAndDatas.mstSvtTreasureDevicedArray =
                (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstSvtTreasureDevice);
            GlobalPathsAndDatas.mstTreasureDevicedArray =
                (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstTreasureDevice);
            GlobalPathsAndDatas.mstTreasureDeviceDetailArray =
                (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstTreasureDeviceDetail);
            GlobalPathsAndDatas.mstSkillArray = (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstSkill);
            GlobalPathsAndDatas.mstSvtSkillArray =
                (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstSvtSkill);
            GlobalPathsAndDatas.mstSkillDetailArray =
                (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstSkillDetail);
            GlobalPathsAndDatas.mstFuncArray = (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstFunc);
            GlobalPathsAndDatas.mstTreasureDeviceLvArray =
                (JArray) JsonConvert.DeserializeObject(GlobalPathsAndDatas.mstTreasureDeviceLv);
        }
    }
}