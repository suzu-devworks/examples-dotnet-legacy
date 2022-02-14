# Windows Services

## Service Install commands.

    - sc.cmd
    - ServiceInstaller?,InstallUtil
    - PowerShell
    - Self Install, ManagedInstallerClass
    - Self Install, advapi32.dll

### `sc.exe`

```ps1
sc.exe create NewService binpath= c:\windows\system32\NewServ.exe type= share start= auto depend= +TDI NetBIOS

sc.exe delete NewService
```
* https://docs.microsoft.com/ja-jp/windows-server/administration/windows-commands/sc-create


### PowerShell

```ps1
New-Service -Name "YourServiceName" -BinaryPathName <yourproject>.exe

//not found.
//Remove-Service -Name "YourServiceName"
```


### `InstallUtil.exe`, using ServiceInstaller.

use Visual Studio Developer Command Prompt or Visual Studio Developer PowerShell.

```ps1
installutil <yourproject>.exe

installutil /u <yourproject>.exe
```


### Self installer, using ServiceInstaller.

* https://stackoverflow.com/questions/4144019/self-install-windows-service-in-net


### Self installer, without ServiceInstaller.

use P/Invoke.

* https://stackoverflow.com/questions/358700/how-to-install-a-windows-service-programmatically-in-c

