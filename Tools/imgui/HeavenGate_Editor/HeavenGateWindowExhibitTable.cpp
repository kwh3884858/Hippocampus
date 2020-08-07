//
//Copyright (c) 2019 Star Platinum
//
//Created by Wen Yang Wei, 2020-1-14
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//

#include "imgui.h"

#include "HeavenGateWindowExhibitTable.h"
#include "HeavenGateEditorUtility.h"

#include "StoryTableManager.h"
#include "StoryFileManager.h"
#include "StoryTable.h"
namespace HeavenGateEditor {



    HeavenGateWindowExhibitTable::HeavenGateWindowExhibitTable()
    {
    }

    HeavenGateWindowExhibitTable::~HeavenGateWindowExhibitTable()
    {
    }

    void HeavenGateWindowExhibitTable::Initialize()
    {
        StoryTable<EXHIBIT_COLUMN, EXHIBIT_TABLE_MAX_CONTENT>* const exhibitTable = StoryTableManager::Instance().GetExhibitTable();


        memset(m_fullPath, 0, sizeof(m_fullPath));

        HeavenGateEditorUtility::GetStoryPath(m_fullPath);
        strcat(m_fullPath, EXHIBIT_TABLE_NAME);

        bool result = StoryFileManager::Instance().LoadTableFile(m_fullPath, exhibitTable);
        if (result == false)
        {
            exhibitTable->UpdateName();
            StoryFileManager::Instance().SaveTableFile(m_fullPath, exhibitTable);
            StoryFileManager::Instance().LoadTableFile(m_fullPath, exhibitTable);
        }
    }

    void HeavenGateWindowExhibitTable::Shutdown()
    {

    }

    void HeavenGateWindowExhibitTable::UpdateMainWindow()
    {
        StoryTable<EXHIBIT_COLUMN, EXHIBIT_TABLE_MAX_CONTENT>* const exhibitTable = StoryTableManager::Instance().GetExhibitTable();

        if (exhibitTable == nullptr)
        {
            return;
        }

        ImGui::Separator();

        ImGui::Text("Exhibit Table");

        if (ImGui::Button("Add New Row"))
        {
            exhibitTable->AddRow();
        }
        ImGui::SameLine();
        if (ImGui::Button("Remove Row"))
        {
            exhibitTable->RemoveRow();
        }

        ImGui::Columns(EXHIBIT_COLUMN + 1, "Exhibit"); // 4-ways, with border
        ImGui::Separator();
        ImGui::Text("Index");    ImGui::NextColumn();
        for (int i = 0; i < EXHIBIT_COLUMN; i++)
        {
            ImGui::Text(exhibitTable->GetName(i));   ImGui::NextColumn();
        }
        ImGui::Separator();

        static int selected = -1;

        char order[64] = "";
        char constant[EXHIBIT_TABLE_MAX_CONTENT];
        for (int i = 0; i < exhibitTable->GetSize(); i++)
        {
            char label[32];
            sprintf(label, "%04d", i);
            if (ImGui::Selectable(label, selected == i, ImGuiSelectableFlags_None))
                selected = i;

            sprintf(order, "%d", i);
            ImGui::NextColumn();

            for (int j = 0; j < EXHIBIT_COLUMN; j++)
            {
                char * const content = exhibitTable->GetContent(i, j);
                strcpy(constant, exhibitTable->GetName(j));
                strcat(constant, order);
                ImGui::InputText(constant, content, EXHIBIT_TABLE_MAX_CONTENT - 4);
                ImGui::NextColumn();
            }
        }

        ImGui::Columns(1);
        ImGui::Separator();
    }

    void HeavenGateWindowExhibitTable::UpdateMenu()
    {
        if (ImGui::MenuItem("New")) {

        }

        if (ImGui::MenuItem("Open", "Ctrl+O")) {

        }

        if (ImGui::MenuItem("Save", "Ctrl+S")) {
            StoryTable<EXHIBIT_COLUMN, EXHIBIT_TABLE_MAX_CONTENT>* const exhibitTable = StoryTableManager::Instance().GetExhibitTable();

            StoryFileManager::Instance().SaveTableFile(m_fullPath, exhibitTable);
        }

    }

}
