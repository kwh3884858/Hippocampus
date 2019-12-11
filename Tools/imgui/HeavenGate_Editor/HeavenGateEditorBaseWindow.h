//
//Copyright (c) 2019 Star Platinum
//
//Created by Kong Wei Hang, 2019-12-10
//example_win32_directx11
//
//Add Description
//
#pragma once

#ifndef HeavenGateEditorBaseWindow_h
#define HeavenGateEditorBaseWindow_h



namespace HeavenGateEditor {


    class HeavenGateEditorBaseWindow
    {
    public:
        HeavenGateEditorBaseWindow() = delete;
        HeavenGateEditorBaseWindow(const char* windowName);
        virtual ~HeavenGateEditorBaseWindow();

        void Update();

        virtual void UpdateMainWindow() = 0;
        virtual void UpdateMenu() = 0;

        bool* GetHandle();
        void OpenWindow();
        void CloseWindow();
        bool IsWindowOpen() const;
    private:
        bool m_open;
        char m_windowName[128];
    };




}
#endif /* HeavenGateEditorBaseWindow_h */
