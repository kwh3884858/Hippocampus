//
//Copyright (c) 2019 Star Platinum
//
//Created by Wen Yang Wei, 2019-12-25
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//
#pragma once
#ifndef HeavenGateWindowPauseTable_h
#define HeavenGateWindowPauseTable_h

#include "HeavenGateEditorConstant.h"
#include "HeavenGateEditorBaseWindow.h"

namespace HeavenGateEditor {


    class HeavenGateWindowPauseTable : public HeavenGateEditorBaseWindow {
        WINDOW_DECLARE("HeavenGateWindowPauseTable", Window_Type::MainWindow)

    public:
        HeavenGateWindowPauseTable();
        ~HeavenGateWindowPauseTable();
        virtual void Initialize() override;
        virtual void Shutdown() override;
    protected:

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu()override;

    private:
        char m_fullPath[MAX_FOLDER_PATH];

    };
}

#endif /* HeavenGateWindowPauseTable_h */
