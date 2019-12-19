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

#include "HeavenGateEditorConstant.h"
#include "HeavenGateEditorBaseWindow.h"


namespace HeavenGateEditor {

    template<int column>
    class StoryTable;

    class StoryFileManager;

    class HeavenGateWindowColorTable : public HeavenGateEditorBaseWindow {

    public:
        HeavenGateWindowColorTable();
        ~HeavenGateWindowColorTable();

    protected:

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu()override;

    private:
        char m_fullPath[MAX_FOLDER_PATH];



        StoryFileManager* m_fileManager;
        StoryTable<COLOR_MAX_COLUMN>* m_table;

    };
}

#endif /* HeavenGateWindowColorTable_h */
