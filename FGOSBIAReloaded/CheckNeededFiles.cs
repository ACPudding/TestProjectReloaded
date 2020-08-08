using System.IO;
using System.Windows;
using FGOSBIAReloaded.Properties;

namespace FGOSBIAReloaded
{
    internal class CheckNeededFiles
    {
        public static bool CheckDirectoryExists()
        {
            if (Directory.Exists(GlobalPathsAndDatas.gamedata.FullName)) return false;
            MessageBox.Show("没有游戏数据,请先下载游戏数据(位于\"设置\"选项卡).", "温馨提示:", MessageBoxButton.OK,
                MessageBoxImage.Information);
            return true;

        }

        public static bool CheckFilesExists()
        {
            if (File.Exists(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstSvt") &&
                File.Exists(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstSvtLimit") &&
                File.Exists(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstCv") &&
                File.Exists(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstIllustrator") &&
                File.Exists(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstSvtCard") &&
                File.Exists(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDevice") &&
                File.Exists(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" +
                            "mstSvtTreasureDevice") && File.Exists(
                    GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDeviceDetail") &&
                File.Exists(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstSkill") &&
                File.Exists(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstSkillDetail") &&
                File.Exists(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstSvtSkill") &&
                File.Exists(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstFunc") &&
                File.Exists(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstSvtComment") &&
                File.Exists(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstTreasureDeviceLv") &&
                File.Exists(GlobalPathsAndDatas.gamedata.FullName + "decrypted_masterdata/" + "mstSkillLv"))
                return false;
            MessageBox.Show("游戏数据损坏,请重新下载游戏数据(位于\"设置\"选项卡).", "温馨提示:", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return true;

        }
    }
}