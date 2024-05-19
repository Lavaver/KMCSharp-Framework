KMCSharp Framework
===

一个可以启动 Minecraft 核心的 .NET Framework 类库。范本自已停止维护的开源项目 [KMCCC](https://github.com/MineStudio/KMCCC) 。

> 注：目前版本还是 KMCCC Fork 了一个...很多功能还没完善...虽然已经没什么大问题了部分要改的也开始改了要写的也开始写了就先这样吧...如果你们有什么好想法可以把代码 clone 下来修改/添加一下，并且这个项目主要的 KMCCC 框架代码库从 2019 之后就没人动过了，想修改或者理解比较有难度...

> 最近修改速报：
> - 现已完成微软正版验证器与登录逻辑。写代码写了我大约一周大好时光（

由 [Lavaver](https://github.com/Lavaver) 主要开发的小型范本继承项目。

## 这个项目说来话长...

这个项目本来是为**我和我的好基友搭建的 Minecraft 服务器初稿的项目**，后来直接 Dead Repository 了

（当时设想是制作服务器游戏的配套启动器，但最后因为某些不太现实的原因（例如当时我对 C# 了解的很浅，并不知晓诸如“单个解决方案多个项目”开发逻辑，并且出现了这会用 KMCCC 库那会干脆直接套个 Minecraft 命令行启动器的 GUI 壳糊弄过去啥的，总之很曲折）再加上做出来的启动器只能达到可以用但不能用的状态等诸多原因导致这个项目一度被搁置）

总之，借着今年夏季开始，就想着做一个类库，让没有能力自己手搓启动器的小开发者或者小白也能拥有顺利用代码启动游戏的成就感

于是，这个项目诞生了。

## 项目灵感来源与基础框架范本支持

### 框架支持

- 【主要基础核心框架】[MineStudio/KMCCC - An OpenSource Minecraft Launcher for .Net Developers](https://github.com/MineStudio/KMCCC)
- 【功能灵感来源】[Hex-Dragon/PCL2 - Plain Craft Launcher（我的世界启动器 PCL）的源代码，为支持社区研究而公开。](https://github.com/Hex-Dragon/PCL2)
- 【微软正版验证器源框架】[Yuns-Lab/MCMsLogin-Python - Python 版本的一个微软登录 Minecraft 类库](https://github.com/Yuns-Lab/MCMsLogin-Python)
- 【运行时与 SDK】[.NET Framework 4.8](https://learn.microsoft.com/zh-cn/dotnet/framework/get-started/overview)
- 【扩展语言互操作支持】[Visual Basic（部分使用）](https://learn.microsoft.com/zh-cn/dotnet/visual-basic/)

---

### 人员支持

- [Lavaver（本作者）](https://github.com/Lavaver)
- 以及正在阅读 README 的你

## 使用

> ⚠ 警告
> ---
> 请不要从其他非 GitHub Release 来源获取类库文件。从未知来源获取的本类库可能存在包括但不限于如下风险：
> - 文件可能被植入恶意脚本、病毒或强制格机代码关键字
> - 无法跟进最新版本
> - 原命名空间被更改，使用方法与本作者提供的范例有出入（例如在启动参数时微软调用正版验证需要用方法而非 new 一个新验证器等）
>
> 另外，本作者拒绝任何形式的开源项目付费获取本体（尤其是 CSDN 这种砸开发者碗免费资源强制收费的死妈行为），如果你是以购买方式取得本类库，请退款并做好维权工作！保护开源精神是每个人应尽的责任！
>
> ⚠ 类库校验和与验证方法
> ---
> 除了初版，在每一个发行版发行的时候均会在页面特别强调 SHA256 校验和。如果你认为你获取的类库副本有潜在风险，请立即进行校验！
>
> 请打开**命令提示符**，输入如下内容，并与 Release 页面上**对应版本**的校验和进行比对：
> ```bash
> certutil -hashfile xxx.dll SHA256
> ```
> 你无需单个字符逐个进行比对，一般情况下你只需要扫一眼就可以了

### 首先进行初始化内核

```csharp
LauncherCore core = LauncherCore.Create(
	new LauncherCoreCreationOption(
		javaPath: Config.Instance.JavaPath, // 默认为找到的第一个版本
		gameRootPath: null, // 默认为 ./.minecraft/
		versionLocator: the Version Locator // 默认情况下将会 new JVersionLocator()
	));
```

### 然后指定你的核心版本

```csharp
var versions = core.GetVersions();

var version = core.GetVersion("1.8");

```

> Tips：如果遇到无效的核心，那么此类库就会在扫描时跳过这个无效核心版本。

### 最后享受你的游戏！

```csharp
var result = core.Launch(new LaunchOptions
{
	Version = App.LauncherCore.GetVersion(server.VersionId)
	Auth = new OfflineAuthenticator("Steve"), // 离线模式启动
	// Auth = new YggdrasilLogin("*@*.*", "***", true), // 第三方验证模式（需在游戏启动前安装前置库并做好配置。Mojang 认证已停止运行故如果直接填好张密启动会无法登录）
	// Auth = new MSAuthenticator(URL), // 微软验证模式。需要你提前在 https://login.live.com/oauth20_authorize.srf?prompt=login&client_id=00000000402b5328&response_type=code&scope=service%3A%3Auser.auth.xboxlive.com%3A%3AMBI_SSL&redirect_uri=https:%2F%2Flogin.live.com%2Foauth20_desktop.srf 中登录，并在界面变白的时候复制地址栏地址并粘贴到合适的地方（有可能是一个输入框让你粘贴）并继续登录。某些启动器可能考虑到微软登录问题在正式启动前会让你根据指引提前登录啥的（甚至在启动器内建浏览器，通过获得当前浏览页面是否有 code 键值全自动登录甚至不需要提前登录啥的）...
	MaxMemory = Config.Instance.MaxMemory, // 可选
	MinMemory = Config.Instance.MaxMemory, // 可选
	Mode = LaunchMode.MCLauncher, // 可选
	Server = new ServerInfo {Address = "mc.hypixel.net"}, //可选
	Size = new WindowSize {Height = 768, Width = 1280} //可选
}, (Action<MinecraftLaunchArguments>) (x => { })); // 可选 ( 启动前修改参数
```