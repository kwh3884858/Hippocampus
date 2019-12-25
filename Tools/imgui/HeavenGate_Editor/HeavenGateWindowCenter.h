#pragma once

#ifndef HeavenGateWindowCenter_h
#define HeavenGateWindowCenter_h

#include "HeavenGateEditorBaseWindow.h"


namespace HeavenGateEditor {

    class HeavenGateWindowStoryEditor;
    class HeavenGateEditorFontSizeTable;
    class HeavenGateWindowColorTable;
    class HeavenGateWindowTipTable;

    class HeavenGateWindowCenter : public HeavenGateEditorBaseWindow
    {
        WINDOW_DECLARE("HeavenGateWindowCenter", Window_Type::MainWindow)
    public:
        HeavenGateWindowCenter();

        virtual ~HeavenGateWindowCenter() override;

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu() override {}


    private:

           HeavenGateWindowStoryEditor*     m_heavenGateEditor;
           HeavenGateEditorFontSizeTable*   m_fontSizeTable;
           HeavenGateWindowColorTable*      m_colorTable;
           HeavenGateWindowTipTable*        m_tipTable;

        bool* show_editor_window;
        bool* show_font_size_table_window;
        bool* show_color_table_window;
        bool* show_tip_table_window;
    };

}


#endif /* HeavenGateWindowCenter_h */
