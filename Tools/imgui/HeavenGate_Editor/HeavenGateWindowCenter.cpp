
#include "HeavenGateWindowCenter.h"

#include "HeavenGateWindowStoryEditor.h"
#include "HeavenGateEditorFontSizeTable.h"
#include "HeavenGateWindowColorTable.h"
#include "HeavenGateWindowTipTable.h"
#include "HeavenGateWindowPaintMoveTable.h"

#include "StoryJsonManager.h"
#include "StoryTableManager.h"
#include "StoryFileManager.h"

#include "imgui.h"

namespace HeavenGateEditor {



    HeavenGateWindowCenter::HeavenGateWindowCenter()
    {

        StoryJsonManager::Instance().Initialize();
        StoryFileManager::Instance().Initialize();
        StoryTableManager::Instance().Initialize();

        m_heavenGateEditor = new HeavenGateWindowStoryEditor;
        m_fontSizeTable = new HeavenGateEditorFontSizeTable;
        m_colorTable = new HeavenGateWindowColorTable;
        m_tipTable = new HeavenGateWindowTipTable;
        m_heavenGateWindowPaintMoveTable = new HeavenGateWindowPaintMoveTable;

        show_editor_window = m_heavenGateEditor->GetHandle();
        show_font_size_table_window = m_fontSizeTable->GetHandle();
        show_color_table_window = m_colorTable->GetHandle();
        show_tip_table_window = m_tipTable->GetHandle();
        show_heaven_gate_window_paint_move_table = m_heavenGateWindowPaintMoveTable->GetHandle();

    }

    HeavenGateWindowCenter::~HeavenGateWindowCenter()
    {
        *show_editor_window = false;
        *show_font_size_table_window = false;
        *show_color_table_window = false;
        *show_tip_table_window = false;
        *show_heaven_gate_window_paint_move_table = false;

        show_editor_window = nullptr;
        show_font_size_table_window = nullptr;
        show_color_table_window = nullptr;
        show_tip_table_window = nullptr;
        show_heaven_gate_window_paint_move_table = nullptr;

        //Delete Windows
        delete m_heavenGateEditor;
        delete m_fontSizeTable;
        delete m_colorTable;
        delete m_tipTable;
        delete m_heavenGateWindowPaintMoveTable;

        m_heavenGateEditor = nullptr;
        m_fontSizeTable = nullptr;
        m_colorTable = nullptr;
        m_tipTable = nullptr;
        m_heavenGateWindowPaintMoveTable = nullptr;


        //Delete Data
        StoryTableManager::Instance().Shutdown();
        StoryFileManager::Instance().Shutdown();
        StoryJsonManager::Instance().Shutdown();



    }


    void HeavenGateWindowCenter::UpdateMainWindow()
    {
        ImGui::Checkbox("Story Editor", show_editor_window);
        ImGui::Checkbox("Font Size Table", show_font_size_table_window);
        ImGui::Checkbox("Color Table", show_color_table_window);
        ImGui::Checkbox("Tip Table", show_tip_table_window);
        ImGui::Checkbox("Paint Move Table", show_heaven_gate_window_paint_move_table);

        if (show_editor_window && *show_editor_window)
        {
            m_heavenGateEditor->Update();
        }

        if (show_font_size_table_window && *show_font_size_table_window)
        {
            m_fontSizeTable->Update();
        }

        if (show_color_table_window && *show_color_table_window)
        {
            m_colorTable->Update();
        }

        if (show_tip_table_window && *show_tip_table_window)
        {
            m_tipTable->Update();
        };

        if (show_heaven_gate_window_paint_move_table && *show_heaven_gate_window_paint_move_table)
        {
            m_heavenGateWindowPaintMoveTable->Update();
        };

    }



}
