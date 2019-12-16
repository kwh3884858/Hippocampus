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


#include "HeavenGateEditorConstant.h"
#include "HeavenGateEditorBaseWindow.h"

namespace HeavenGateEditor {

    template<int column>
    class StoryTable;

    class HeavenGatePopupInputFileName;

    class HeavenGateEditorFontSizeTable : public HeavenGateEditorBaseWindow{
        WINDOW_DECLARE("HeavenGateEditorFontSizeTable", Window_Type::MainWindow)

    public:
        HeavenGateEditorFontSizeTable();
        ~HeavenGateEditorFontSizeTable();

    protected:

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu()override;

    private:
        void ShowTableWindow();
        void ShowEditorMenuFile();

        bool m_open;

        HeavenGatePopupInputFileName* m_fileManager;
        StoryTable<FONT_SIZE_MAX_COLUMN>* m_table;
    };

}

#endif /* HeavenGateEditorFontSizeTable_h */
