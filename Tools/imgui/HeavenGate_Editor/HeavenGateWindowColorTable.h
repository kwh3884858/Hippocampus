//
//Copyright (c) 2019 Star Platinum
//
//Created by Kong Wei Hang, 2019-12-19
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//
#pragma once
#ifndef HeavenGateWindowColorTable_h
#define HeavenGateWindowColorTable_h

#include "HeavenGateEditorBaseWindow.h"

#include "HeavenGateEditorConstant.h"
namespace HeavenGateEditor {


    class HeavenGateWindowColorTable : public HeavenGateEditorBaseWindow {
        WINDOW_DECLARE("HeavenGateWindowColorTable", Window_Type::MainWindow)

    public:
        HeavenGateWindowColorTable();
        ~HeavenGateWindowColorTable();

    protected:

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu()override;

    private:
        char m_fullPath[MAX_FOLDER_PATH];


    };
}

#endif /* HeavenGateWindowColorTable_h */
