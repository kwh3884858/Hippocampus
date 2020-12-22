#pragma once

#ifndef HeavenGateWindowCenter_h
#define HeavenGateWindowCenter_h

#include "HeavenGateEditorBaseWindow.h"
#include <vector>


namespace HeavenGateEditor {

    class HeavenGateWindowStoryEditor;
    class HeavenGateEditorFontSizeTable;
    class HeavenGateWindowColorTable;
    class HeavenGateWindowTipTable;
    class HeavenGateWindowTachieMoveTable;
    class HeavenGateWindowCharacterTable;
    class HeavenGateWindowPauseTable;
    class HeavenGateWindowExhibitTable;
    class HeavenGateWindowEffectTable;
    class HeavenGateWindowBgmTable;
    class HeavenGateWindowTachieTable;
    class HeavenGateEditorNodeGraphExample;
    class HeavenGateWindowTachiePositionTable;
    class HeavenGateWindowPositionTable;
    class HeavenGateWindowRotationTable;

    class HeavenGateWindowCenter : public HeavenGateEditorBaseWindow
    {
        WINDOW_DECLARE("HeavenGateWindowCenter", Window_Type::MainWindow)
    public:
        HeavenGateWindowCenter();

        virtual ~HeavenGateWindowCenter() override;


        virtual void Initialize() override;
        virtual void Shutdown() override;
        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu() override {}


    private:

        std::vector<HeavenGateWindowStoryEditor*>            m_heavenGateEditor;
        HeavenGateEditorFontSizeTable*          m_fontSizeTable;
        HeavenGateWindowColorTable*             m_colorTable;
        HeavenGateWindowTipTable*               m_tipTable;
        HeavenGateWindowTachieMoveTable*         m_heavenGateWindowPaintMoveTable;
        HeavenGateWindowCharacterTable*         m_characterTable;
        HeavenGateWindowPauseTable*             m_pauseTable;
        HeavenGateWindowExhibitTable*           m_exhibitTable;
        HeavenGateWindowEffectTable*            m_effectTable;
        HeavenGateWindowBgmTable*               m_bgmTable;
        HeavenGateWindowTachieTable*            m_tachieTable;
        HeavenGateEditorNodeGraphExample*       m_nodeGraphExample;
        HeavenGateWindowTachiePositionTable*    m_tachiePositionTable;
        HeavenGateWindowPositionTable*          m_positionTable;
        HeavenGateWindowRotationTable*          m_rotationTable;

//        bool* show_editor_window;
        bool* show_font_size_table_window;
        bool* show_color_table_window;
        bool* show_tip_table_window;
        bool* show_heaven_gate_window_paint_move_table;
        bool* show_character_table;
        bool* show_pause_table;
        bool* show_exhibit_table;
        bool* show_effect_table;
        bool* show_bgm_table;
        bool* show_tachie_table;
        bool* show_node_graph_example;
        bool* show_tachie_poisition_table;
        bool* show_poisition_table;
        bool* show_rotation_table;
    };
}


#endif /* HeavenGateWindowCenter_h */
