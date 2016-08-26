# Shadowsocks-Plus-Win
An installer for SSPlus local proxy on Windows

## Disclaimer
- The first release was published on the second day of my C# study
- I have limited programming experience and sometimes don't know what I am doing
- This app is not integrated with SSPlus itself, it works as a wrapper or launcher

## Usage
- Fill in your server config details, click `Run`
- A success message will notify you if the local proxy has been started without errors
- You gotta manually configure your browser or other apps to use the local proxy, I recommend using `SwitchyOmega` in Chrome (you can find an offline CRX installer for `SwitchyOmega` in the released archvice)
- Check `Auto start with system` to run this app at system startup, uncheck to disable this feature
- Further information can be found in [my blog](https://jm33.me)

## TO-DO
- [x] Save config details into a config file and load config info from it
- [ ] <s>Hide into system tray</s>
- [ ] Enable system proxy with [Privoxy](https://www.privoxy.org/)
- [ ] Convert SOCKS5 to VPN?

## Acknowledgement & License
- This app uses a compiled [Shadowsocks-Plus](https://github.com/shadowsocks-plus/shadowsocks-plus) client, which is developed by [losfair](https://github.com/losfair)
- Published under GPLv3
