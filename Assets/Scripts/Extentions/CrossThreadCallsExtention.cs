
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Threading;

/// <summary>
/// 线程中安全访问控件，避免重复的delegate,Invoke
/// </summary>
public static class ControlCrossThreadCalls
{
    public delegate void InvokeHandler();

    //public static void SafeInvoke(this Control control, InvokeHandler handler)
    //{
    //    if (control.InvokeRequired)
    //    {
    //        control.Invoke(handler);
    //    }
    //    else
    //    {
    //        handler();
    //    }
    //}
}