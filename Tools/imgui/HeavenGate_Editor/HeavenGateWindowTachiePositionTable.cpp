
//
//Copyright (c) 2019 Star Platinum
//
//Created by Wen Yang Wei, 2020-03-14
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//

#include "imgui.h"

#include "HeavenGateWindowTachiePositionTable.h"
#include "HeavenGateEditorUtility.h"

#include "StoryTableManager.h"
#include "StoryFileManager.h"
#include "StoryTable.h"
namespace HeavenGateEditor {



    HeavenGateWindowTachiePositionTable::HeavenGateWindowTachiePositionTable()
    {
    }

    HeavenGateWindowTachiePositionTable::~HeavenGateWindowTachiePositionTable()
    {
    }

    void HeavenGateWindowTachiePositionTable::Initialize()
    {

        StoryTable<TACHIE_POSITION_COLUMN>*const  tachiePositionTable = StoryTableManager::Instance().GetTachiePositionTable();
        memset(m_fullPath, 0, sizeof(m_fullPath));

        HeavenGateEditorUtility::GetStoryPath(m_fullPath);
        strcat(m_fullPath, TACHIE_TABLE_NAME);

        bool result = StoryFileManager::Instance().LoadTableFile(m_fullPath, tachiePositionTable);
        if (result == false)
        {
            StoryFileManager::Instance().SaveTableFile(m_fullPath, tachiePositionTable);
            StoryFileManager::Instance().LoadTableFile(m_fullPath, tachiePositionTable);
        }
    }

    void HeavenGateWindowTachiePositionTable::Shutdown()
    {

    }

    void HeavenGateWindowTachiePositionTable::UpdateMainWindow()
    {
        StoryTable<TACHIE_POSITION_COLUMN>*const  tachiePositionTable = StoryTableManager::Instance().GetTachiePositionTable();

        if (tachiePositionTable == nullptr)
        {
            return;
        }

        ImGui::Separator();

        ImGui::Text("Tachie Position Table");

        if (ImGui::Button("Add New Row"))
        {
            tachiePositionTable->AddRow();
        }
        ImGui::SameLine();
        if (ImGui::Button("Remove Row"))
        {
            tachiePositionTable->RemoveRow();
        }

        ImGui::Columns(TACHIE_POSITION_COLUMN + 1, "Tachie Position"); // 4-ways, with border
        ImGui::Separator();
        ImGui::Text("Index");    ImGui::NextColumn();
        for (int i = 0; i < TACHIE_POSITION_COLUMN; i++)
        {
            const char* name = tachiePositionTable->GetName(i);
            if (name != nullptr) {
                ImGui::Text(name);
            }

            ImGui::NextColumn();
        }

        //ImGui::Text("ID"); ImGui::NextColumn();
        //ImGui::Text("Name"); ImGui::NextColumn();
        //ImGui::Text("Path"); ImGui::NextColumn();
        //ImGui::Text("Hovered"); ImGui::NextColumn();
        ImGui::Separator();
        //const char* names[3] = { "One", "Two", "Three" };
        //const char* paths[3] = { "/path/one", "/path/two", "/path/three" };
        static int selected = -1;


        char order[8] = "";
        for (int i = 0; i < tachiePositionTable->GetSize(); i++)
        {
            char label[32];
            sprintf(label, "%04d", i);
            if (ImGui::Selectable(label, selected == i, ImGuiSelectableFlags_None))
                selected = i;

            sprintf(order, "%d", i);
            ImGui::NextColumn();

            for (int j = 0; j < TACHIE_POSITION_COLUMN; j++)
            {
                char * content = tachiePositionTable->GetContent(i, j);

                char constant[16];
//                if (j % 2 == 0)
//                {
//                    strcpy(constant, "Role Drawing ");
//
//                }
//                else
//                {
//                    strcpy(constant, "Alias ");
//
//                }
                strcat(constant, order);

                ImGui::InputText(constant, content, MAX_COLUMNS_CONTENT_LENGTH);
                ImGui::NextColumn();
            }

        }

        ImGui::Columns(1);
        ImGui::Separator();
    }

    void HeavenGateWindowTachiePositionTable::UpdateMenu()
    {
        if (ImGui::MenuItem("New")) {

        }

        if (ImGui::MenuItem("Open", "Ctrl+O")) {

        }

        if (ImGui::MenuItem("Save", "Ctrl+S")) {
             StoryTable<TACHIE_POSITION_COLUMN>*const  tachiePositionTable = StoryTableManager::Instance().GetTachiePositionTable();

            StoryFileManager::Instance().SaveTableFile(m_fullPath, tachiePositionTable);
        }

    }

}
