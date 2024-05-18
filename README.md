KMCSharp Framework
===

一个可以启动 Minecraft 核心的 .NET Framework 类库。范本自已停止维护的开源项目 [KMCCC](https://github.com/MineStudio/KMCCC) 。

> 注：目前版本还是 KMCCC Fork 了一个...很多功能还没完善...虽然已经没什么大问题了部分要改的也开始改了要写的也开始写了就先这样吧...如果你们有什么好想法可以把代码 clone 下来修改/添加一下，并且这个项目主要的 KMCCC 框架代码库从 2019 之后就没人动过了，想修改或者理解比较有难度...

由 [Lavaver](https://github.com/Lavaver) 主要开发的小型范本继承项目。

## 这个项目说来话长...

这个项目本来是为**我和我的好基友搭建的 Minecraft 服务器初稿的项目**，后来直接 Dead Repository 了

（当时设想是制作服务器游戏的配套启动器，但最后因为某些不太现实的原因（例如当时我对 C# 了解的很浅，并不知晓诸如“单个解决方案多个项目”开发逻辑，并且出现了这会用 KMCCC 库那会干脆直接套个 Minecraft 命令行启动器的 GUI 壳糊弄过去啥的，总之很曲折）再加上做出来的启动器只能达到可以用但不能用的状态等诸多原因导致这个项目一度被搁置）

总之，借着今年夏季开始，就想着做一个类库，让没有能力自己手搓启动器的小开发者或者小白也能拥有顺利用代码启动游戏的成就感

于是，这个项目诞生了。

## 项目灵感来源与基础框架范本支持

### 框架支持

- [MineStudio/KMCCC - An OpenSource Minecraft Launcher for .Net Developers](https://github.com/MineStudio/KMCCC)
- [Hex-Dragon/PCL2 - Plain Craft Launcher（我的世界启动器 PCL）的源代码，为支持社区研究而公开。](https://github.com/Hex-Dragon/PCL2)
- [Yuns-Lab/MCMsLogin-Python - Python 版本的一个微软登录 Minecraft 类库](https://github.com/Yuns-Lab/MCMsLogin-Python)
- [.NET Framework 4.8](https://learn.microsoft.com/zh-cn/dotnet/framework/get-started/overview)
- [Visual Basic（部分使用）](https://learn.microsoft.com/zh-cn/dotnet/visual-basic/)

---

### 人员支持

- [Lavaver（本作者）](https://github.com/Lavaver)
- 以及正在阅读 README 的你们

> Hex-Dragon/PCL2 项目源语言为 Visual Basic 。部分功能实现（下载游戏、安装游戏）为转换后代码。

> Yuns-Lab/MCMsLogin-Python 项目源语言为 Python 。功能实现为转换后代码。

---

### 参考文献与引用章节

- [Mojang API - 中文 Minecraft Wiki](https://zh.minecraft.wiki/w/Mojang_API?variant=zh-cn)
  - [第一节《服务器的请求与响应》](https://zh.minecraft.wiki/w/Mojang_API?variant=zh-cn#%E6%9C%8D%E5%8A%A1%E5%99%A8%E7%9A%84%E8%AF%B7%E6%B1%82%E4%B8%8E%E5%93%8D%E5%BA%94)
  - [第二节《获取玩家信息》](https://zh.minecraft.wiki/w/Mojang_API?variant=zh-cn#%E8%8E%B7%E5%8F%96%E7%8E%A9%E5%AE%B6%E4%BF%A1%E6%81%AF)
  - [第四节《Microsoft 身份验证》](https://zh.minecraft.wiki/w/Mojang_API?variant=zh-cn#Microsoft%E8%BA%AB%E4%BB%BD%E9%AA%8C%E8%AF%81)