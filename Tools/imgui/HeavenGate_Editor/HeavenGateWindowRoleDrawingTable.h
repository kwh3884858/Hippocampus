//
//Copyright (c) 2019 Star Platinum
//
//Created by Wen Yang Wei, 2020-03-14
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//
#pragma once
#ifndef HeavenGateWindowRoleDrawingTable_h
#define HeavenGateWindowRoleDrawingTable_h

//#include "HeavenGateEditorConstant.h"
#include "HeavenGateEditorBaseWindow.h"
#include "HeavenGateEditorConstant.h"

namespace HeavenGateEditor {

    //template<int column>
    //class StoryTable;

    //class StoryFileManager;

    class HeavenGateWindowRoleDrawingTable : public HeavenGateEditorBaseWindow {
        WINDOW_DECLARE("HeavenGateWindowRoleDrawingTable", Window_Type::MainWindow)

    public:
        HeavenGateWindowRoleDrawingTable();
        ~HeavenGateWindowRoleDrawingTable();

    protected:

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu()override;

    private:
        char m_fullPath[MAX_FOLDER_PATH];



        //StoryFileManager* m_fileManager;


    };
}

#endif /* HeavenGateWindowRoleDrawingTable_h */
