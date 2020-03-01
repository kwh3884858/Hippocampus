//
//Copyright (c) 2019 Star Platinum
//
//Created by Wen Yang Wei, 2020-02-21
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//
#pragma once
#ifndef HeavenGateWindowBgmTable_h
#define HeavenGateWindowBgmTable_h

#include "HeavenGateEditorConstant.h"
#include "HeavenGateEditorBaseWindow.h"

namespace HeavenGateEditor {


    class HeavenGateWindowBgmTable : public HeavenGateEditorBaseWindow {
        WINDOW_DECLARE("HeavenGateWindowBgmTable", Window_Type::MainWindow)

    public:
        HeavenGateWindowBgmTable();
        ~HeavenGateWindowBgmTable();

    protected:

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu()override;

    private:
        char m_fullPath[MAX_FOLDER_PATH];

    };
}

#endif /* HeavenGateWindowBgmTable_h */
