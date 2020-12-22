//
//Copyright (c) 2019 Star Platinum
//
//Created by Wen Yang Wei, 2019-12-25
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//

#include "imgui.h"

#include "HeavenGateWindowRotationTable.h"
#include "HeavenGateEditorUtility.h"

#include "StoryTableManager.h"
#include "StoryFileManager.h"
#include "StoryTable.h"
namespace HeavenGateEditor {



    HeavenGateWindowRotationTable::HeavenGateWindowRotationTable()
    {

    }

    HeavenGateWindowRotationTable::~HeavenGateWindowRotationTable()
    {

    }

    void HeavenGateWindowRotationTable::Initialize()
    {

        StoryTable<ROTATION_MAX_COLUMN>* const rotationTable = StoryTableManager::Instance().GetRotationTable();

        memset(m_fullPath, 0, sizeof(m_fullPath));
        HeavenGateEditorUtility::GetStoryPath(m_fullPath);
        strcat(m_fullPath, ROTATION_TABLE_NAME);

        bool result = StoryFileManager::Instance().LoadTableFile(m_fullPath, rotationTable);
        if (result == false)
        {
            rotationTable->UpdateName();
            StoryFileManager::Instance().SaveTableFile(m_fullPath, rotationTable);
            StoryFileManager::Instance().LoadTableFile(m_fullPath, rotationTable);
        }
    }

    void HeavenGateWindowRotationTable::Shutdown()
    {

    }

    void HeavenGateWindowRotationTable::UpdateMainWindow()
    {
        StoryTable<ROTATION_MAX_COLUMN>* const rotationTable = StoryTableManager::Instance().GetRotationTable();
        if (rotationTable == nullptr)
        {
            return;
        }
        ImGui::Separator();

        ImGui::Text("Chapter Table");
        if (ImGui::Button("Add New Row"))
        {
            rotationTable->AddRow();
        }
        ImGui::SameLine();
        if (ImGui::Button("Remove Row"))
        {
            rotationTable->RemoveRow();
        }

        ImGui::Columns(ROTATION_MAX_COLUMN + 1, "Rotation"); // 4-ways, with border
        ImGui::Separator();
        ImGui::Text("Index");    ImGui::NextColumn();
        for (int i = 0; i < ROTATION_MAX_COLUMN; i++)
        {
            ImGui::Text(rotationTable->GetName(i));   ImGui::NextColumn();
        }
        ImGui::Separator();

        static int selected = -1;
        char order[8] = "";
        char constant[16] = "";
        for (int i = 0; i < rotationTable->GetSize(); i++)
        {
            char label[32];
            sprintf(label, "%04d", i);
            if (ImGui::Selectable(label, selected == i, ImGuiSelectableFlags_None))
                selected = i;

            sprintf(order, "%d", i);
            ImGui::NextColumn();

            for (int j = 0; j < ROTATION_MAX_COLUMN; j++)
            {
                char * const content = rotationTable->GetContent(i, j);
                strcpy(constant, rotationTable->GetName(j));
                strcat(constant, order);
                ImGui::InputText(constant, content, MAX_COLUMNS_CONTENT_LENGTH - 4);
                ImGui::NextColumn();
            }
        }

        ImGui::Columns(1);
        ImGui::Separator();
    }

    void HeavenGateWindowRotationTable::UpdateMenu()
    {
        //   ImGui::MenuItem("(dummy menu)", NULL, false, false);
        if (ImGui::MenuItem("New")) {

        }

        if (ImGui::MenuItem("Open", "Ctrl+O")) {

        }

        if (ImGui::MenuItem("Save", "Ctrl+S")) {
            StoryTable<ROTATION_MAX_COLUMN>* const rotationTable = StoryTableManager::Instance().GetRotationTable();

            StoryFileManager::Instance().SaveTableFile(m_fullPath, rotationTable);
        }

    }

}
