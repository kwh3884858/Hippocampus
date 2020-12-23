
//
//Copyright (c) 2019 Star Platinum
//
//Created by Kong Weihang, 2020-07-01
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//
#pragma once
#ifndef HeavenGateWindowTachiePositionTable_h
#define HeavenGateWindowTachiePositionTable_h

#include "HeavenGateEditorBaseWindow.h"
#include "HeavenGateEditorConstant.h"

namespace HeavenGateEditor {

    class HeavenGateWindowTachiePositionTable : public HeavenGateEditorBaseWindow {
        WINDOW_DECLARE("HeavenGateWindowTachiePositionTable", Window_Type::MainWindow)

    public:
        HeavenGateWindowTachiePositionTable();
        ~HeavenGateWindowTachiePositionTable();
        virtual void Initialize() override;
        virtual void Shutdown() override;
    protected:

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu()override;

    private:
        char m_fullPath[MAX_FOLDER_PATH];
    };
}

#endif /* HeavenGateWindowTachiePositionTable_h */
