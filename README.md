# FGO Servant Basic Information Analyzer (WPF.Reloaded)
该项目使用WPF进行了重置  
和上个工程一样垃圾  
如遇任何兼容性问题无法保证有效修复  
本身是作为初学C#的练手之作，写的非常烂  
如果真的有人发现这里的话希望可以重构一个orz  
所有对数据进行的解密部分代码使用了nishuoshenme的FGOAssetsModifyTool的代码   
  
实现内容:  
可以对FGO(JP)的所有从者的基本信息进行解析.并且可以导出为xlsx文件.  
  
使用方法:  
1、下载Release中的文件.  
2、首次打开会提示"没有游戏数据",先切换至"设置"选项卡下载文件(如果连接过慢请事先挂一个全局梯子)  
3、在左上角的文本框输入ID点击"解析"即可调出从者信息.  
  
注意事项:  
1、如果不知道具体ID可以先点击"导出ID列表"按钮导出ID名单.  
2、宝具和技能的详细数据由于没有完整的规则所以暂时只能写成从文件中读取的最原始的数据.  
3、如果需要导出从者的基础信息,勾选"设置"选项卡下的"询问是否导出xlsx文件"复选框即可.  
4、检查更新使用的是api.github.com,如果出现连接失败请连接全局梯子.  
5、不保证长期更新.  
6、可能之后还会添加其他功能?  
