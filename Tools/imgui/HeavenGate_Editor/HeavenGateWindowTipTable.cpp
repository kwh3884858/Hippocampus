//
//Copyright (c) 2019 Star Platinum
//
//Created by Wen Yang Wei, 2019-12-24
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//

#include "imgui.h"

#include "HeavenGateWindowTipTable.h"
#include "HeavenGateEditorUtility.h"

#include "StoryTableManager.h"
#include "StoryFileManager.h"
#include "StoryTable.h"
namespace HeavenGateEditor {



    HeavenGateWindowTipTable::HeavenGateWindowTipTable()
    {
        //m_fileManager = new StoryFileManager;

        StoryTable<TIP_MAX_COLUMN>*const  tipTable = StoryTableManager::Instance().GetTipTable();
        memset(m_fullPath, 0, sizeof(m_fullPath));

        HeavenGateEditorUtility::GetStoryPath(m_fullPath);
        strcat(m_fullPath, TIP_TABLE_NAME);

        bool result = StoryFileManager::Instance().LoadTableFile(m_fullPath, tipTable);
        if (result == false)
        {
            StoryFileManager::Instance().SaveTableFile(m_fullPath, tipTable);
            StoryFileManager::Instance().LoadTableFile(m_fullPath, tipTable);
        }


    }

    HeavenGateWindowTipTable::~HeavenGateWindowTipTable()
    {

        //if (tipTable)
        //{
        //    delete tipTable;
        //}
        //tipTable = nullptr;

    }

    void HeavenGateWindowTipTable::UpdateMainWindow()
    {
        StoryTable<TIP_MAX_COLUMN>*const  tipTable = StoryTableManager::Instance().GetTipTable();

        if (tipTable == nullptr)
        {
            return;
        }

        ImGui::Separator();

        ImGui::Text("Tip Table");

        if (ImGui::Button("Add New Row"))
        {
            tipTable->AddRow();
        }
        ImGui::SameLine();
        if (ImGui::Button("Remove Row"))
        {
            tipTable->RemoveRow();
        }

        ImGui::Columns(TIP_MAX_COLUMN + 1, "Tip"); // 4-ways, with border
        ImGui::Separator();
        ImGui::Text("Index");    ImGui::NextColumn();
        for (int i = 0; i < TIP_MAX_COLUMN; i++)
        {
            ImGui::Text(tipTable->GetName(i));   ImGui::NextColumn();
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
        for (int i = 0; i < tipTable->GetSize(); i++)
        {
            char label[32];
            sprintf(label, "%04d", i);
            if (ImGui::Selectable(label, selected == i, ImGuiSelectableFlags_None))
                selected = i;

            sprintf(order, "%d", i);
            //bool hovered = ImGui::IsItemHovered();
            ImGui::NextColumn();
            //ImGui::Text(names[i]); ImGui::NextColumn();
            //ImGui::Text(paths[i]); ImGui::NextColumn();
            //ImGui::Text("%d", hovered); ImGui::NextColumn();

            for (int j = 0; j < FONT_SIZE_MAX_COLUMN; j++)
            {
                char * content = tipTable->GetContent(i, j);

                char constant[16];
                if (j % 2 == 0)
                {
                    strcpy(constant, "Tip ");

                }
                else
                {
                    strcpy(constant, "Description ");

                }
                strcat(constant, order);

                ImGui::InputText(constant, content, MAX_COLUMNS_CONTENT_LENGTH);
                ImGui::NextColumn();
            }

        }

        ImGui::Columns(1);
        ImGui::Separator();
    }

    void HeavenGateWindowTipTable::UpdateMenu()
    {
        //   ImGui::MenuItem("(dummy menu)", NULL, false, false);
        if (ImGui::MenuItem("New")) {

        }

        if (ImGui::MenuItem("Open", "Ctrl+O")) {

        }

        if (ImGui::MenuItem("Save", "Ctrl+S")) {
            StoryTable<TIP_MAX_COLUMN>*const  tipTable = StoryTableManager::Instance().GetTipTable();

            StoryFileManager::Instance().SaveTableFile(m_fullPath, tipTable);
        }

    }

}
