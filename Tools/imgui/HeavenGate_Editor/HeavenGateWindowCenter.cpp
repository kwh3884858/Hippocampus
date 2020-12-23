
#include "HeavenGateWindowCenter.h"

#include "HeavenGateWindowStoryEditor.h"
#include "HeavenGateEditorFontSizeTable.h"
#include "HeavenGateWindowColorTable.h"
#include "HeavenGateWindowTipTable.h"
#include "HeavenGateWindowTachieMoveTable.h"
#include "HeavenGateWindowCharacterTable.h"
#include "HeavenGateWindowPauseTable.h"
#include "HeavenGateWindowExhibitTable.h"
#include "HeavenGateWindowEffectTable.h"
#include "HeavenGateWindowBgmTable.h"
#include "HeavenGateWindowTachieTable.h"
#include "HeavenGateEditorNodeGraphExample.h"
#include "HeavenGatePopupMessageBox.h"
#include "HeavenGateWindowTachiePositionTable.h"
#include "HeavenGateWindowPositionTable.h"
#include "HeavenGateWindowRotationTable.h"

#include "StoryJsonManager.h"
#include "StoryTableManager.h"
#include "StoryFileManager.h"
#include "StoryJsonContentCompiler.h"
#include "StoryJsonChecker.h"


#include "imgui.h"

#include "HeavenGateEditorUtility.h"
#include "ArtistTool/ArtistToolOpenFolder.h"

namespace HeavenGateEditor {



    HeavenGateWindowCenter::HeavenGateWindowCenter()
    {

    }

    HeavenGateWindowCenter::~HeavenGateWindowCenter()
    {
  

    }


    void HeavenGateWindowCenter::Initialize()
    {

        StoryJsonManager::Instance().Initialize();
        StoryFileManager::Instance().Initialize();
        StoryTableManager::Instance().Initialize();
        StoryJsonContentCompiler::Instance().Initialize();
        StoryJsonChecker::Instance().Initialize();

        m_fontSizeTable = new HeavenGateEditorFontSizeTable;
        m_colorTable = new HeavenGateWindowColorTable;
        m_tipTable = new HeavenGateWindowTipTable;
        m_heavenGateWindowPaintMoveTable = new HeavenGateWindowTachieMoveTable;
        m_characterTable = new HeavenGateWindowCharacterTable;
        m_pauseTable = new HeavenGateWindowPauseTable;
        m_exhibitTable = new HeavenGateWindowExhibitTable;
        m_effectTable = new HeavenGateWindowEffectTable;
        m_bgmTable = new HeavenGateWindowBgmTable;
        m_tachieTable = new HeavenGateWindowTachieTable;
        m_nodeGraphExample = new HeavenGateEditorNodeGraphExample;
        m_tachiePositionTable = new HeavenGateWindowTachiePositionTable;
        m_positionTable = new HeavenGateWindowPositionTable;
        m_rotationTable = new HeavenGateWindowRotationTable;

        m_fontSizeTable->Initialize();
        m_colorTable->Initialize();
        m_tipTable->Initialize();
        m_heavenGateWindowPaintMoveTable->Initialize();
        m_characterTable->Initialize();
        m_pauseTable->Initialize();
        m_exhibitTable->Initialize();
        m_effectTable->Initialize();
        m_bgmTable->Initialize();
        m_tachieTable->Initialize();
        m_nodeGraphExample->Initialize();
        m_tachiePositionTable->Initialize();
        m_positionTable->Initialize();
        m_rotationTable->Initialize();

        show_font_size_table_window = m_fontSizeTable->GetHandle();
        show_color_table_window = m_colorTable->GetHandle();
        show_tip_table_window = m_tipTable->GetHandle();
        show_heaven_gate_window_paint_move_table = m_heavenGateWindowPaintMoveTable->GetHandle();
        show_character_table = m_characterTable->GetHandle();
        show_pause_table = m_pauseTable->GetHandle();
        show_exhibit_table = m_exhibitTable->GetHandle();
        show_effect_table = m_effectTable->GetHandle();
        show_bgm_table = m_bgmTable->GetHandle();
        show_tachie_table = m_tachieTable->GetHandle();
        show_node_graph_example = m_nodeGraphExample->GetHandle();
        show_tachie_poisition_table = m_tachiePositionTable->GetHandle();
        show_poisition_table = m_positionTable->GetHandle();
        show_rotation_table = m_rotationTable->GetHandle();
    }

    void HeavenGateWindowCenter::Shutdown()
    {

        *show_font_size_table_window = false;
        *show_color_table_window = false;
        *show_tip_table_window = false;
        *show_heaven_gate_window_paint_move_table = false;
        *show_character_table = false;
        *show_pause_table = false;
        *show_exhibit_table = false;
        *show_effect_table = false;
        *show_bgm_table = false;
        *show_tachie_table = false;
        *show_node_graph_example = false;
        *show_tachie_poisition_table = false;
        *show_poisition_table = false;
        *show_rotation_table = false;

        show_font_size_table_window = nullptr;
        show_color_table_window = nullptr;
        show_tip_table_window = nullptr;
        show_heaven_gate_window_paint_move_table = nullptr;
        show_character_table = nullptr;
        show_pause_table = nullptr;
        show_exhibit_table = nullptr;
        show_effect_table = nullptr;
        show_bgm_table = nullptr;
        show_tachie_table = nullptr;
        show_node_graph_example = nullptr;
        show_tachie_poisition_table = nullptr;
        show_poisition_table = nullptr;
        show_rotation_table = nullptr;

        for (auto iter = m_heavenGateEditor.begin(); iter != m_heavenGateEditor.end(); iter++) {
            (*iter)->Shutdown();
            delete *iter;
            *iter = nullptr;
        }
        m_fontSizeTable->Shutdown();
        m_colorTable->Shutdown();
        m_tipTable->Shutdown();
        m_heavenGateWindowPaintMoveTable->Shutdown();
        m_characterTable->Shutdown();
        m_pauseTable->Shutdown();
        m_exhibitTable->Shutdown();
        m_effectTable->Shutdown();
        m_bgmTable->Shutdown();
        m_tachieTable->Shutdown();
        m_nodeGraphExample->Shutdown();
        m_tachiePositionTable->Shutdown();
        m_positionTable->Shutdown();
        m_rotationTable->Shutdown();

        //Delete Windows
        m_heavenGateEditor.clear();
        delete m_fontSizeTable;
        delete m_colorTable;
        delete m_tipTable;
        delete m_heavenGateWindowPaintMoveTable;
        delete m_characterTable;
        delete m_pauseTable;
        delete m_exhibitTable;
        delete m_effectTable;
        delete m_bgmTable;
        delete m_tachieTable;
        delete m_nodeGraphExample;
        delete m_tachiePositionTable;
        delete m_positionTable;
        delete m_rotationTable;

//        m_heavenGateEditor = nullptr;
        m_fontSizeTable = nullptr;
        m_colorTable = nullptr;
        m_tipTable = nullptr;
        m_heavenGateWindowPaintMoveTable = nullptr;
        m_characterTable = nullptr;
        m_pauseTable = nullptr;
        m_exhibitTable = nullptr;
        m_effectTable = nullptr;
        m_bgmTable = nullptr;
        m_tachieTable = nullptr;
        m_nodeGraphExample = nullptr;
        m_tachiePositionTable = nullptr;
        m_positionTable = nullptr;
        m_rotationTable = nullptr;

        //Delete Data
        StoryTableManager::Instance().Shutdown();
        StoryFileManager::Instance().Shutdown();
        StoryJsonManager::Instance().Shutdown();
        StoryJsonContentCompiler::Instance().Shutdown();
        StoryJsonChecker::Instance().Shutdown();

    }

    void HeavenGateWindowCenter::UpdateMainWindow()
    {
        ImGui::Text("[Heaven Gate Editor]  --version: %s", EDITOR_VERSION);
        ImGui::Text("[Release Note] \n %s", RELEASE_NOTE);

        if (ImGui::Button("New Story Editor"))
        {
            static int i = 0;
            HeavenGateWindowStoryEditor* storyEditor = new HeavenGateWindowStoryEditor;
            storyEditor->Initialize();
            storyEditor->OpenWindow();
            storyEditor->SetWindowIndex(i++);
            m_heavenGateEditor.push_back(storyEditor);
        }

        for (auto iter = m_heavenGateEditor.begin(); iter != m_heavenGateEditor.end(); ) {
            bool* const isOpen = (*iter)->GetHandle();
            if (*isOpen == false) {
                (*iter)->Shutdown();
                delete *iter;
                *iter = nullptr;
                iter = m_heavenGateEditor.erase(iter);
            }
            if(iter != m_heavenGateEditor.end()){
                (*iter)->Update();
                iter++;
            }
        }

        ImGui::Checkbox("Font Size Table", show_font_size_table_window);
        ImGui::Checkbox("Color Table", show_color_table_window);
        ImGui::Checkbox("Tip Table", show_tip_table_window);
        ImGui::Checkbox("Character Table", show_character_table);
        ImGui::Checkbox("Pause Table", show_pause_table);
        ImGui::Checkbox("Exhibit Table", show_exhibit_table);
        ImGui::Checkbox("Effect Table", show_effect_table);
        ImGui::Checkbox("Bgm Table", show_bgm_table);
        ImGui::Checkbox("Tachie Table", show_tachie_table);
        ImGui::Checkbox("Tachie Position Table", show_tachie_poisition_table);
        ImGui::Checkbox("Node Graph Example", show_node_graph_example);
        ImGui::Checkbox("Tachie Move Table", show_heaven_gate_window_paint_move_table);
        ImGui::Checkbox("Position Table", show_poisition_table);
        ImGui::Checkbox("Rotation Table", show_rotation_table);


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

        if (show_tachie_table && *show_tachie_table)
        {
            m_tachieTable->Update();
        };

        if (show_node_graph_example && *show_node_graph_example)
        {
            m_nodeGraphExample->Update();
        }

        if (show_tachie_poisition_table && *show_tachie_poisition_table)
        {
            m_tachiePositionTable->Update();
        }

        if (show_poisition_table && *show_poisition_table)
        {
            m_positionTable->Update();
        }

        if (show_rotation_table && *show_rotation_table)
        {
            m_rotationTable->Update();
        }

#ifdef _WIN32

        if (ImGui::Button("Open Tachie Folder"))
        {
            char rootPath[MAX_FOLDER_PATH];
            HeavenGateEditorUtility::GetRootPath(rootPath);
            strcat(rootPath, PATH_FROM_PROJECT_ROOT_TO_TACHIE);
            ArtistTool::ArtistToolOpenFolder::OpenFolder(rootPath);
        }

        if (ImGui::Button("Open Interactable CG Folder"))
        {
            char rootPath[MAX_FOLDER_PATH];
            HeavenGateEditorUtility::GetRootPath(rootPath);
            strcat(rootPath, PATH_FROM_PROJECT_ROOT_TO_INTERACTABLE_CG);
            ArtistTool::ArtistToolOpenFolder::OpenFolder(rootPath);
        }

        if (ImGui::Button("Open Talk Background"))
        {
            char rootPath[MAX_FOLDER_PATH];
            HeavenGateEditorUtility::GetRootPath(rootPath);
            strcat(rootPath, PATH_FROM_PROJECT_ROOT_TO_TALK_BACKGROUND);
            ArtistTool::ArtistToolOpenFolder::OpenFolder(rootPath);
        }

        if (ImGui::Button("Open BGM"))
        {
            char rootPath[MAX_FOLDER_PATH];
            HeavenGateEditorUtility::GetRootPath(rootPath);
            strcat(rootPath, PATH_FROM_PROJECT_ROOT_TO_BGM);
            ArtistTool::ArtistToolOpenFolder::OpenFolder(rootPath);
        }

        if (ImGui::Button("Open Effect"))
        {
            char rootPath[MAX_FOLDER_PATH];
            HeavenGateEditorUtility::GetRootPath(rootPath);
            strcat(rootPath, PATH_FROM_PROJECT_ROOT_TO_EFFECT);
            ArtistTool::ArtistToolOpenFolder::OpenFolder(rootPath);
        }
#endif // _WIN32
    }
}

