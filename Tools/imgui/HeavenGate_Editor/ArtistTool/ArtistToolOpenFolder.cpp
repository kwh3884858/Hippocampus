#include <Windows.h>
#include <shellapi.h>

#include "ArtistTool/ArtistToolOpenFolder.h"

namespace ArtistTool {

    void ArtistToolOpenFolder::OpenFolder(const wchar_t* const folderPath)
    {
        ShellExecute(
            nullptr, //{指定父窗口句柄}
            L"open", //{指定动作, 譬如: open、runas、print、edit、explore、find[2]}
            folderPath, // {指定要打开的文件或程序}
            nullptr, // {给要打开的程序指定参数; 如果打开的是文件这里应该是 nil}
            nullptr, //{缺省目录}
            SW_SHOWNORMAL
        );

        /*
        ShowCmd 参数可选值 : SW_HIDE = 0; {隐藏}
        SW_SHOWNORMAL = 1; {用最近的大小和位置显示, 激活}
        SW_NORMAL = 1; {同 SW_SHOWNORMAL}
        SW_SHOWMINIMIZED = 2; {最小化, 激活}
        SW_SHOWMAXIMIZED = 3; {最大化, 激活}
        SW_MAXIMIZE = 3; {同 SW_SHOWMAXIMIZED}
        SW_SHOWNOACTIVATE = 4; {用最近的大小和位置显示, 不激活}
        SW_SHOW = 5; {同 SW_SHOWNORMAL}
        SW_MINIMIZE = 6; {最小化, 不激活}
        SW_SHOWMINNOACTIVE = 7; {同 SW_MINIMIZE}
        SW_SHOWNA = 8; {同 SW_SHOWNOACTIVATE}
        SW_RESTORE = 9; {同 SW_SHOWNORMAL}
        SW_SHOWDEFAULT = 10; {同 SW_SHOWNORMAL}
        SW_MAX = 10; {同 SW_SHOWNORMAL}
        */

    }

}
