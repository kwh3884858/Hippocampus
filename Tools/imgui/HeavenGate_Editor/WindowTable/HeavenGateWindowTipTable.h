//
//Copyright (c) 2019 Star Platinum
//
//Created by Wen Yang Wei, 2019-12-24
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//
#pragma once
#ifndef HeavenGateWindowTipTable_h
#define HeavenGateWindowTipTable_h

//#include "HeavenGateEditorConstant.h"
#include "HeavenGateEditorBaseWindow.h"
#include "HeavenGateEditorConstant.h"

namespace HeavenGateEditor {

    //template<int column>
    //class StoryTable;

    //class StoryFileManager;

    class HeavenGateWindowTipTable : public HeavenGateEditorBaseWindow {
        WINDOW_DECLARE("HeavenGateWindowTipTable", Window_Type::MainWindow)

    public:
        HeavenGateWindowTipTable();
        ~HeavenGateWindowTipTable();
        virtual void Initialize() override;
        virtual void Shutdown() override;

    protected:

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu()override;

    private:
        char m_fullPath[MAX_FOLDER_PATH];



        //StoryFileManager* m_fileManager;


    };
}

#endif /* HeavenGateWindowTipTable_h */
