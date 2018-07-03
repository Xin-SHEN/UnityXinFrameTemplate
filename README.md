# UnityXinFrameTemplate
A collection of frequently used scripts and plugins


品奥项目工具
----------------------

Version 1.2.3

更新：
- UDP发送增加了队列机制，发送间隔可调。

增加：
- 增加了弹出注册码时将其复制到Windows剪切板的功能。
- 视频播放器插件增加了切换背景图片的命令。
- 增加了软件内截图功能，截图尺寸为Game视图渲染分辨率。

----------------------

Version 1.1.8

更新：
- 视频插件从【AVPro Windows Media 2.80】更新至 【AVPro Video 1.6.2】。
- 视频音量调节步进从10%减小到5%。


----------------------

Version 1.1.7

删除：
- 因为Excel读取方式的不稳定行，删除了Excel读取功能，建议全部迁移至数据库处理。
- 删除【WindowMod】组件。

增加：
- 增加了MySQL数据的连接与查询功能(需要在【PlayerSettings->OtherSettings->ApiCompatibilityLevel】中开启.Net2.0)。
- 增加了注册功能。
- 增加了【UnityWindowControl】来管理Unity的运行窗口，该功能可以外部配置，检测并重置软件分辨率，检测并使窗体前置。

----------------------

Version 1.1.5

增加：
- 增加了AlphaMask功能。
- 增加了WindowMod功能，包含外部配置窗体位置。
- 增加了Excel读取功能。


----------------------

Version 1.1.4

修复：

增加：
- 增加了【SingleMoviePlayer】和【TextureMoviePlayer】的影片开始处理委托【mvStartHandler】，并传递当前影片序号。


----------------------

Version 1.1.3

修复：
- 修复了影片播放时【返回屏保】时钟继续计时的BUG。

增加：
- 增加了【HiveEffect】UI图片转场特效，用于快速部署 (着色器已开启深度写入)。
- 增加了【SingleMoviePlayer】和【TextureMoviePlayer】的影片完成处理委托【mvCompleteHandler】。
- 增加了【停止影片播放】的API。


----------------------

Version 1.1.2

修复：
- 修复了UDP模块在发布后加载出错的BUG (代码执行顺序问题)。
- 修复了UDP模块在比较字符串永远不相等的BUG (字节流转置字符串长度问题)。

增加：
- 现在可以按快捷键【F10】将UDPAgent快速添加至场景中。
- 现在可以在插件面板中直接查看当前读取到的UDP配置信息，查看前按【重新检查外部配置文件】以确保更新。

----------------------

Version 1.1.1

增加：
- 现在可以按快捷键【F9】将【插件管理器】快速添加至场景中。
- 现在可以勾选【静默】按钮，在检查XML时仅报告错误。
- 现在可以在插件面板中直接查看当前读取到的【鼠标隐藏】配置信息，查看前按【重新检查外部配置文件】以确保更新。
- 现在可以在插件面板中直接查看当前读取到的【返回屏保延迟】配置信息，查看前按【重新检查外部配置文件】以确保更新。
- 现在可以在【插件管理器】中指定执行返回首页/屏保的对象，从而快速部署该功能，该对象必须有一个方法名为【ReturnHome】。
- 现在可以中菜单栏或【插件管理器】中将【全屏视频播放器】、【材质视频播放器】快速添加至场景中。
