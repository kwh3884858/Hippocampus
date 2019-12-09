//
//  HeavenGateEditorFontSizeTable.hpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 7/12/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//

#ifndef HeavenGateEditorFontSizeTable_h
#define HeavenGateEditorFontSizeTable_h

#include <stdio.h>
#include "imgui.h"

#include "HeavenGateEditorConstant.h"


namespace HeavenGateEditor {

    template<int c>
    class StoryTable;

    class HeavenGateEditorFileManager;

    class HeavenGateEditorFontSizeTable {
    public:
        HeavenGateEditorFontSizeTable();
        ~HeavenGateEditorFontSizeTable();

        void ShowTableWindow();

        void OpenWindow();
        void CloseWindow();
        bool IsOpenWindow() const;
        bool* GetWindowOpenHandle();

    private:

        void ShowEditorMenuFile();

        bool m_open;

        HeavenGateEditorFileManager* m_fileManager;
        StoryTable<FONT_SIZE_MAX_COLUMN>* m_table;
    };

}

#endif /* HeavenGateEditorFontSizeTable_h */
