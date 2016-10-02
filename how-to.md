# 使用说明

## 主程序

- 按照你的具体情况输入服务器配置信息，确认无误后点击`Run`

- 如果最后看到`Success`对话框，请点击`Exit`退出程序，代理服务器将在后台继续运行

- `Stop`用于停止代理服务器的运行

- `Launch proxy at start up`会在Windows启动时自动运行代理服务器，你不会看到任何窗口或提示，服务器后台进程名称为`ssp.exe`

- 接下来请配置你的浏览器或其它网络应用

## Chrome浏览器
> 如果你没有，请自行下载安装，[天极网Chrome下载页面](http://mydown.yesky.com/desktop/gamedesk/manhuakatong/318/416318.shtml)

> 请拒绝任何基于Chrome的第三方浏览器，包括但不限于360，猎豹，QQ。如果你有安装那些浏览器，那么你的插件将由它们接管，Chrome将无法使用插件

> 注意：如果你使用下面的方法，**请确保你在主程序界面把`Local port`设置成了`1088`**

### 安装SwitchyOmega插件

- 在地址栏输入`chrome://extensions/`打开插件管理页面

- 如图，请勾选开发者模式

![install](https://raw.githubusercontent.com/jm33-m0/jm33-m0.github.io/master/img/chrome1.png)

- 然后把`SwitchyOmega.crx`拖到页面上释放，并同意安装插件

- 进入配置页面，选择从配置文件恢复，并选择`OmegaOptions.bak`文件，如图

![restore](https://raw.githubusercontent.com/jm33-m0/jm33-m0.github.io/master/img/chrome2.png)

### 使用SwitchyOmega管理Chrome浏览器代理设置
- 如果没有意外，你应该可以看到这样的控制菜单

![menu](https://raw.githubusercontent.com/jm33-m0/jm33-m0.github.io/master/img/chrome3.png)

- 使用`proxy`模式，则浏览器所有流量都会走代理通道，`auto switch`会根据GFWList的规则选择是否使用代理，`direct connect`是直连，一般无需使用`system proxy`系统代理

- Now, 你的Chrome浏览器已经科学上网了

## Firefox浏览器

- 请选择`设置`  >  `高级`  > `网络`  > `代理`

- 如图，请选择SOCKS5代理，远程DNS解析，服务器地址`127.0.0.1` ，端口`1088` (如果你在主程序界面设置的`Local port`是其它值，请把这里的端口和它**保持一致**)

![firefox](https://jm33.me/img/ff-proxy.png)

## 其它网络应用

> 由于Shadowsocks属于SOCKS5代理，你的系统应该无法直接使用它，请使用*privoxy*
