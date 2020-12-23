//
//Copyright (c) 2019 Star Platinum
//
//Created by Wen Yang Wei, 2019-12-25
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//

#include "imgui.h"

#include "HeavenGateWindowPositionTable.h"
#include "HeavenGateEditorUtility.h"

#include "StoryTableManager.h"
#include "StoryFileManager.h"
#include "StoryTable.h"
namespace HeavenGateEditor {



    HeavenGateWindowPositionTable::HeavenGateWindowPositionTable()
    {

    }

    HeavenGateWindowPositionTable::~HeavenGateWindowPositionTable()
    {

    }

    void HeavenGateWindowPositionTable::Initialize()
    {

        StoryTable<POSITION_MAX_COLUMN>* const PositionTable = StoryTableManager::Instance().GetPositionTable();

        memset(m_fullPath, 0, sizeof(m_fullPath));
        HeavenGateEditorUtility::GetStoryPath(m_fullPath);
        strcat(m_fullPath, POSITION_TABLE_NAME);

        bool result = StoryFileManager::Instance().LoadTableFile(m_fullPath, PositionTable);
        if (result == false)
        {
            PositionTable->UpdateName();
            StoryFileManager::Instance().SaveTableFile(m_fullPath, PositionTable);
            StoryFileManager::Instance().LoadTableFile(m_fullPath, PositionTable);
        }
    }

    void HeavenGateWindowPositionTable::Shutdown()
    {

    }

    void HeavenGateWindowPositionTable::UpdateMainWindow()
    {
        StoryTable<POSITION_MAX_COLUMN>* const positionTable = StoryTableManager::Instance().GetPositionTable();
        if (positionTable == nullptr)
        {
            return;
        }
        ImGui::Separator();

        ImGui::Text("position Table");
        if (ImGui::Button("Add New Row"))
        {
            positionTable->AddRow();
        }
        ImGui::SameLine();
        if (ImGui::Button("Remove Row"))
        {
            positionTable->RemoveRow();
        }

        ImGui::Columns(POSITION_MAX_COLUMN + 1, "Position"); // 4-ways, with border
        ImGui::Separator();
        ImGui::Text("Index");    ImGui::NextColumn();
        for (int i = 0; i < POSITION_MAX_COLUMN; i++)
        {
            ImGui::Text(positionTable->GetName(i));   ImGui::NextColumn();
        }
        ImGui::Separator();

        static int selected = -1;
        char order[8] = "";
        char constant[16] = "";
        for (int i = 0; i < positionTable->GetSize(); i++)
        {
            char label[32];
            sprintf(label, "%04d", i);
            if (ImGui::Selectable(label, selected == i, ImGuiSelectableFlags_None))
                selected = i;

            sprintf(order, "%d", i);
            ImGui::NextColumn();

            for (int j = 0; j < POSITION_MAX_COLUMN; j++)
            {
                char * const content = positionTable->GetContent(i, j);
                strcpy(constant, positionTable->GetName(j));
                strcat(constant, order);
                ImGui::InputText(constant, content, MAX_COLUMNS_CONTENT_LENGTH - 4);
                ImGui::NextColumn();
            }
        }

        ImGui::Columns(1);
        ImGui::Separator();
    }

    void HeavenGateWindowPositionTable::UpdateMenu()
    {
        //   ImGui::MenuItem("(dummy menu)", NULL, false, false);
        if (ImGui::MenuItem("New")) {

        }

        if (ImGui::MenuItem("Open", "Ctrl+O")) {

        }

        if (ImGui::MenuItem("Save", "Ctrl+S")) {
            StoryTable<POSITION_MAX_COLUMN>* const positionTable = StoryTableManager::Instance().GetPositionTable();

            StoryFileManager::Instance().SaveTableFile(m_fullPath, positionTable);
        }
    }
}
