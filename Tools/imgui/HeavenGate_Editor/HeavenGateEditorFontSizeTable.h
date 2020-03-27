//
//  HeavenGateEditorFontSizeTable.hpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 7/12/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//

#ifndef HeavenGateEditorFontSizeTable_h
#define HeavenGateEditorFontSizeTable_h

#include "HeavenGateEditorBaseWindow.h"
#include "HeavenGateEditorConstant.h"

namespace HeavenGateEditor {


    class HeavenGateEditorFontSizeTable : public HeavenGateEditorBaseWindow{
        WINDOW_DECLARE("HeavenGateEditorFontSizeTable", Window_Type::MainWindow);

    public:
        HeavenGateEditorFontSizeTable();
        ~HeavenGateEditorFontSizeTable();
        virtual void Initialize() override;
        virtual void Shutdown() override;

    protected:

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu()override;

    private:
        char m_fullPath[MAX_FOLDER_PATH];
      
    };

}

#endif /* HeavenGateEditorFontSizeTable_h */
