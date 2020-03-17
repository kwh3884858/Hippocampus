
#include "HeavenGateWindowCenter.h"

#include "HeavenGateWindowStoryEditor.h"
#include "HeavenGateEditorFontSizeTable.h"
#include "HeavenGateWindowColorTable.h"
#include "HeavenGateWindowTipTable.h"
#include "HeavenGateWindowPaintMoveTable.h"
#include "HeavenGateWindowChapterTable.h"
#include "HeavenGateWindowSceneTable.h"
#include "HeavenGateWindowCharacterTable.h"
#include "HeavenGateWindowPauseTable.h"
#include "HeavenGateWindowExhibitTable.h"
#include "HeavenGateWindowEffectTable.h"
#include "HeavenGateWindowBgmTable.h"
#include "HeavenGateWindowRoleDrawingTable.h"

#include "StoryJsonManager.h"
#include "StoryTableManager.h"
#include "StoryFileManager.h"
#include "StoryJsonContentCompiler.h"
#include "StoryJsonChecker.h"


#include "imgui.h"

namespace HeavenGateEditor {



    HeavenGateWindowCenter::HeavenGateWindowCenter()
    {

        StoryJsonManager::Instance().Initialize();
        StoryFileManager::Instance().Initialize();
        StoryTableManager::Instance().Initialize();
        StoryJsonContentCompiler::Instance().Initialize();
        StoryJsonChecker::Instance().Initialize();


        m_heavenGateEditor = new HeavenGateWindowStoryEditor;
        m_fontSizeTable = new HeavenGateEditorFontSizeTable;
        m_colorTable = new HeavenGateWindowColorTable;
        m_tipTable = new HeavenGateWindowTipTable;
        m_heavenGateWindowPaintMoveTable = new HeavenGateWindowPaintMoveTable;
        m_chapterTable = new HeavenGateWindowChapterTable;
        m_sceneTable = new HeavenGateWindowSceneTable;
        m_characterTable = new HeavenGateWindowCharacterTable;
        m_pauseTable = new HeavenGateWindowPauseTable;
        m_exhibitTable = new HeavenGateWindowExhibitTable;
        m_effectTable = new HeavenGateWindowEffectTable;
        m_bgmTable = new HeavenGateWindowBgmTable;
        m_roleDrawingTable = new HeavenGateWindowRoleDrawingTable;

        show_editor_window = m_heavenGateEditor->GetHandle();
        show_font_size_table_window = m_fontSizeTable->GetHandle();
        show_color_table_window = m_colorTable->GetHandle();
        show_tip_table_window = m_tipTable->GetHandle();
        show_heaven_gate_window_paint_move_table = m_heavenGateWindowPaintMoveTable->GetHandle();
        show_chapter_table = m_chapterTable->GetHandle();
        show_scene_table = m_sceneTable->GetHandle();
        show_character_table = m_characterTable->GetHandle();
        show_pause_table = m_pauseTable->GetHandle();
        show_exhibit_table = m_exhibitTable->GetHandle();
        show_effect_table = m_effectTable->GetHandle();
        show_bgm_table = m_bgmTable->GetHandle();
        show_role_drawing_table = m_roleDrawingTable->GetHandle();

    }

    HeavenGateWindowCenter::~HeavenGateWindowCenter()
    {
        *show_editor_window = false;
        *show_font_size_table_window = false;
        *show_color_table_window = false;
        *show_tip_table_window = false;
        *show_heaven_gate_window_paint_move_table = false;
        *show_chapter_table = false;
        *show_scene_table = false;
        *show_character_table = false;
        *show_pause_table = false;
        *show_exhibit_table = false;
        *show_effect_table = false;
        *show_bgm_table = false;
        *show_role_drawing_table = false;

        show_editor_window = nullptr;
        show_font_size_table_window = nullptr;
        show_color_table_window = nullptr;
        show_tip_table_window = nullptr;
        show_heaven_gate_window_paint_move_table = nullptr;
        show_chapter_table = nullptr;
        show_scene_table = nullptr;
        show_character_table = nullptr;
        show_pause_table = nullptr;
        show_exhibit_table = nullptr;
        show_effect_table = nullptr;
        show_bgm_table = nullptr;
        show_role_drawing_table = nullptr;

        //Delete Windows
        delete m_heavenGateEditor;
        delete m_fontSizeTable;
        delete m_colorTable;
        delete m_tipTable;
        delete m_heavenGateWindowPaintMoveTable;
        delete m_chapterTable;
        delete m_sceneTable;
        delete m_characterTable;
        delete m_pauseTable;
        delete m_exhibitTable;
        delete m_effectTable;
        delete m_bgmTable;
        delete m_roleDrawingTable;

        m_heavenGateEditor = nullptr;
        m_fontSizeTable = nullptr;
        m_colorTable = nullptr;
        m_tipTable = nullptr;
        m_heavenGateWindowPaintMoveTable = nullptr;
        m_chapterTable = nullptr;
        m_sceneTable = nullptr;
        m_characterTable = nullptr;
        m_pauseTable = nullptr;
        m_exhibitTable = nullptr;
        m_effectTable = nullptr;
        m_bgmTable = nullptr;
        m_roleDrawingTable = nullptr;


        //Delete Data
        StoryTableManager::Instance().Shutdown();
        StoryFileManager::Instance().Shutdown();
        StoryJsonManager::Instance().Shutdown();
        StoryJsonContentCompiler::Instance().Shutdown();
        StoryJsonChecker::Instance().Shutdown();


    }


    void HeavenGateWindowCenter::UpdateMainWindow()
    {
        ImGui::Checkbox("Story Editor", show_editor_window);
        ImGui::Checkbox("Font Size Table", show_font_size_table_window);
        ImGui::Checkbox("Color Table", show_color_table_window);
        ImGui::Checkbox("Tip Table", show_tip_table_window);
        ImGui::Checkbox("Paint Move Table", show_heaven_gate_window_paint_move_table);
        ImGui::Checkbox("Chapter Table", show_chapter_table);
        ImGui::Checkbox("Scene Table", show_scene_table);
        ImGui::Checkbox("Character Table", show_character_table);
        ImGui::Checkbox("Pause Table", show_pause_table);
        ImGui::Checkbox("Exhibit Table", show_exhibit_table);
        ImGui::Checkbox("Effect Table", show_effect_table);
        ImGui::Checkbox("Bgm Table", show_bgm_table);
        ImGui::Checkbox("Role Drawing Table", show_role_drawing_table);

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

        if (show_chapter_table && *show_chapter_table)
        {
            m_chapterTable->Update();
        };

        if (show_scene_table && *show_scene_table)
        {
            m_sceneTable->Update();
        };

        if (show_character_table && *show_character_table)
        {
            m_characterTable->Update();
        };

        if (show_pause_table && *show_pause_table)
        {
            m_pauseTable->Update();
        };

        if (show_exhibit_table && *show_exhibit_table)
        {
            m_exhibitTable->Update();
        };

        if (show_effect_table && *show_effect_table)
        {
            m_effectTable->Update();
        };

        if (show_bgm_table && *show_bgm_table)
        {
            m_bgmTable->Update();
        };

        if (show_role_drawing_table && *show_role_drawing_table)
        {
            m_roleDrawingTable->Update();
        };
    }



}
