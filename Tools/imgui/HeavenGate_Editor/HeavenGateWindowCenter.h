#pragma once

#ifndef HeavenGateWindowCenter_h
#define HeavenGateWindowCenter_h

#include "HeavenGateEditorBaseWindow.h"


namespace HeavenGateEditor {

    class HeavenGateWindowStoryEditor;
    class HeavenGateEditorFontSizeTable;
    class HeavenGateWindowColorTable;
    class HeavenGateWindowTipTable;
    class HeavenGateWindowPaintMoveTable;
    class HeavenGateWindowChapterTable;
    class HeavenGateWindowSceneTable;
    class HeavenGateWindowCharacterTable;
    class HeavenGateWindowPauseTable;
    class HeavenGateWindowExhibitTable;
    class HeavenGateWindowEffectTable;
    class HeavenGateWindowBgmTable;
    class HeavenGateWindowRoleDrawingTable;
    class HeavenGateEditorNodeGraphExample;

    class HeavenGateWindowCenter : public HeavenGateEditorBaseWindow
    {
        WINDOW_DECLARE("HeavenGateWindowCenter", Window_Type::MainWindow)
    public:
        HeavenGateWindowCenter();

        virtual ~HeavenGateWindowCenter() override;

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu() override {}


    private:

        HeavenGateWindowStoryEditor*        m_heavenGateEditor;
        HeavenGateEditorFontSizeTable*      m_fontSizeTable;
        HeavenGateWindowColorTable*         m_colorTable;
        HeavenGateWindowTipTable*           m_tipTable;
        HeavenGateWindowPaintMoveTable*     m_heavenGateWindowPaintMoveTable;
        HeavenGateWindowChapterTable*       m_chapterTable;
        HeavenGateWindowSceneTable*         m_sceneTable;
        HeavenGateWindowCharacterTable*     m_characterTable;
        HeavenGateWindowPauseTable*         m_pauseTable;
        HeavenGateWindowExhibitTable*       m_exhibitTable;
        HeavenGateWindowEffectTable*        m_effectTable;
        HeavenGateWindowBgmTable*           m_bgmTable;
        HeavenGateWindowRoleDrawingTable*   m_roleDrawingTable;
        HeavenGateEditorNodeGraphExample*   m_nodeGraphExample;

        bool* show_editor_window;
        bool* show_font_size_table_window;
        bool* show_color_table_window;
        bool* show_tip_table_window;
        bool* show_heaven_gate_window_paint_move_table;
        bool* show_chapter_table;
        bool* show_scene_table;
        bool* show_character_table;
        bool* show_pause_table;
        bool* show_exhibit_table;
        bool* show_effect_table;
        bool* show_bgm_table;
        bool* show_role_drawing_table;
        bool* show_node_graph_example;
    };

}


#endif /* HeavenGateWindowCenter_h */
