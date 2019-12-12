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

#define WINDOW_DECLARE(windowName) \
public: \
    const char*         GetWindiwName() const override        { return windowName; } 


    class HeavenGateEditorBaseWindow
    {
    public:
        HeavenGateEditorBaseWindow();
        virtual ~HeavenGateEditorBaseWindow();

        void Update();

        virtual void UpdateMainWindow() = 0;
        virtual void UpdateMenu() = 0;
        virtual const char* GetWindiwName() const = 0;

        bool* GetHandle();
        void OpenWindow();
        void CloseWindow();
        bool IsWindowOpen() const;
    private:
        bool m_open;

    };




}
#endif /* HeavenGateEditorBaseWindow_h */
