//
//Copyright (c) 2019 Star Platinum
//
//Created by Wen Yang Wei, 2019-12-25
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//
#pragma once
#ifndef HeavenGateWindowPaintMoveTable_h
#define HeavenGateWindowPaintMoveTable_h

#include "HeavenGateEditorConstant.h"
#include "HeavenGateEditorBaseWindow.h"

namespace HeavenGateEditor {


    class HeavenGateWindowPaintMoveTable : public HeavenGateEditorBaseWindow {
        WINDOW_DECLARE("HeavenGateWindowPaintMoveTable", Window_Type::MainWindow)

    public:
        HeavenGateWindowPaintMoveTable();
        ~HeavenGateWindowPaintMoveTable();

    protected:

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu()override;

    private:
        char m_fullPath[MAX_FOLDER_PATH];

    };
}

#endif /* HeavenGateWindowPaintMoveTable_h */
