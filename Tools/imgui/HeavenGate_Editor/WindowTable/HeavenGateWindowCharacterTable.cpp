//
//Copyright (c) 2019 Star Platinum
//
//Created by Wen Yang Wei, 2019-12-25
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//

#include "imgui.h"

#include "HeavenGateWindowCharacterTable.h"
#include "HeavenGateEditorUtility.h"

#include "StoryTableManager.h"
#include "StoryFileManager.h"
#include "StoryTable.h"
namespace HeavenGateEditor {



    HeavenGateWindowCharacterTable::HeavenGateWindowCharacterTable()
    {

    }

    HeavenGateWindowCharacterTable::~HeavenGateWindowCharacterTable()
    {

    }

    void HeavenGateWindowCharacterTable::Initialize()
    {

        StoryTable<CHARACTER_COLUMN>* const characterTable = StoryTableManager::Instance().GetCharacterTable();

        memset(m_fullPath, 0, sizeof(m_fullPath));
        HeavenGateEditorUtility::GetStoryPath(m_fullPath);
        strcat(m_fullPath, CHARACTER_TABLE_NAME);

        bool result = StoryFileManager::Instance().LoadTableFile(m_fullPath, characterTable);
        if (result == false)
        {
            characterTable->UpdateName();
            StoryFileManager::Instance().SaveTableFile(m_fullPath, characterTable);
            StoryFileManager::Instance().LoadTableFile(m_fullPath, characterTable);
        }
    }

    void HeavenGateWindowCharacterTable::Shutdown()
    {

    }

    void HeavenGateWindowCharacterTable::UpdateMainWindow()
    {
        StoryTable<CHARACTER_COLUMN>* const characterTable = StoryTableManager::Instance().GetCharacterTable();
        if (characterTable == nullptr)
        {
            return;
        }
        ImGui::Separator();

        ImGui::Text("Chapter Table");
        if (ImGui::Button("Add New Row"))
        {
            characterTable->AddRow();
        }
        ImGui::SameLine();
        if (ImGui::Button("Remove Row"))
        {
            characterTable->RemoveRow();
        }

        ImGui::Columns(CHARACTER_COLUMN + 1, "Character"); // 4-ways, with border
        ImGui::Separator();
        ImGui::Text("Index");    ImGui::NextColumn();
        for (int i = 0; i < CHARACTER_COLUMN; i++)
        {
            ImGui::Text(characterTable->GetName(i));   ImGui::NextColumn();
        }
        ImGui::Separator();

        static int selected = -1;
        char order[8] = "";
        char constant[16] = "";
        for (int i = 0; i < characterTable->GetSize(); i++)
        {
            char label[32];
            sprintf(label, "%04d", i);
            if (ImGui::Selectable(label, selected == i, ImGuiSelectableFlags_None))
                selected = i;

            sprintf(order, "%d", i);
            ImGui::NextColumn();

            for (int j = 0; j < CHARACTER_COLUMN; j++)
            {
                    char * const content = characterTable->GetContent(i, j);
                    strcpy(constant, characterTable->GetName(j));
                    strcat(constant, order);
                    ImGui::InputText(constant, content, MAX_COLUMNS_CONTENT_LENGTH - 4);
                    ImGui::NextColumn();
            }
        }

        ImGui::Columns(1);
        ImGui::Separator();
    }

    void HeavenGateWindowCharacterTable::UpdateMenu()
    {
        //   ImGui::MenuItem("(dummy menu)", NULL, false, false);
        if (ImGui::MenuItem("New")) {

        }

        if (ImGui::MenuItem("Open", "Ctrl+O")) {

        }

        if (ImGui::MenuItem("Save", "Ctrl+S")) {
            StoryTable<CHARACTER_COLUMN>* const characterTable = StoryTableManager::Instance().GetCharacterTable();

            StoryFileManager::Instance().SaveTableFile(m_fullPath, characterTable);
        }

    }

}
