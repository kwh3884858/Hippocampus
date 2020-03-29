//
//Copyright (c) 2019 Star Platinum
//
//Created by Wen Yang Wei, 2020-1-14
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//
#pragma once
#ifndef HeavenGateWindowExhibitTable_h
#define HeavenGateWindowExhibitTable_h

#include "HeavenGateEditorConstant.h"
#include "HeavenGateEditorBaseWindow.h"

namespace HeavenGateEditor {


    class HeavenGateWindowExhibitTable : public HeavenGateEditorBaseWindow {
        WINDOW_DECLARE("HeavenGateWindowExhibitTable", Window_Type::MainWindow)

    public:
        HeavenGateWindowExhibitTable();
        ~HeavenGateWindowExhibitTable();
        virtual void Initialize() override;
        virtual void Shutdown() override;
    protected:

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu()override;

    private:
        char m_fullPath[MAX_FOLDER_PATH];

    };
}

#endif /* HeavenGateWindowExhibitTable_h */
