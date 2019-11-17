//
//Copyright (c) 2019 Star Platinum
//
//Created by Kong Wei Hang, 2019-11-17
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//
#ifndef HeavenGateEditorWindow_h
#define HeavenGateEditorWindow_h

#include <stdio.h>
#include <string>



using std::string;

namespace HeavenGateEditor {


    void ShowEditorWindow(bool* isOpenPoint);

    static void ShowEditorMenuFile();
    static void OpenSelectStoryWindow(bool* p_open);

    string ExePath();
}


#endif /* HeavenGateEditorWindow_hpp */
