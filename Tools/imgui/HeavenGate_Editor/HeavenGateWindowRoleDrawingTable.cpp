//
//Copyright (c) 2019 Star Platinum
//
//Created by Wen Yang Wei, 2020-03-14
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//

#include "imgui.h"

#include "HeavenGateWindowRoleDrawingTable.h"
#include "HeavenGateEditorUtility.h"

#include "StoryTableManager.h"
#include "StoryFileManager.h"
#include "StoryTable.h"
namespace HeavenGateEditor {



    HeavenGateWindowRoleDrawingTable::HeavenGateWindowRoleDrawingTable()
    {
        //m_fileManager = new StoryFileManager;



    }

    HeavenGateWindowRoleDrawingTable::~HeavenGateWindowRoleDrawingTable()
    {

        //if (roleDrawingTable)
        //{
        //    delete roleDrawingTable;
        //}
        //roleDrawingTable = nullptr;

    }

    void HeavenGateWindowRoleDrawingTable::Initialize()
    {

        StoryTable<ROLE_DRAWING_COLUMN>*const  roleDrawingTable = StoryTableManager::Instance().GetRoleDrawingTable();
        memset(m_fullPath, 0, sizeof(m_fullPath));

        HeavenGateEditorUtility::GetStoryPath(m_fullPath);
        strcat(m_fullPath, ROLE_DRAWING_TABLE_NAME);

        bool result = StoryFileManager::Instance().LoadTableFile(m_fullPath, roleDrawingTable);
        if (result == false)
        {
            StoryFileManager::Instance().SaveTableFile(m_fullPath, roleDrawingTable);
            StoryFileManager::Instance().LoadTableFile(m_fullPath, roleDrawingTable);
        }
    }

    void HeavenGateWindowRoleDrawingTable::Shutdown()
    {

    }

    void HeavenGateWindowRoleDrawingTable::UpdateMainWindow()
    {
        StoryTable<ROLE_DRAWING_COLUMN>*const  roleDrawingTable = StoryTableManager::Instance().GetRoleDrawingTable();

        if (roleDrawingTable == nullptr)
        {
            return;
        }

        ImGui::Separator();

        ImGui::Text("Role Drawing Table");

        if (ImGui::Button("Add New Row"))
        {
            roleDrawingTable->AddRow();
        }
        ImGui::SameLine();
        if (ImGui::Button("Remove Row"))
        {
            roleDrawingTable->RemoveRow();
        }

        ImGui::Columns(ROLE_DRAWING_COLUMN + 1, "Role Drawing"); // 4-ways, with border
        ImGui::Separator();
        ImGui::Text("Index");    ImGui::NextColumn();
        for (int i = 0; i < ROLE_DRAWING_COLUMN; i++)
        {
            ImGui::Text(roleDrawingTable->GetName(i));   ImGui::NextColumn();
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
        for (int i = 0; i < roleDrawingTable->GetSize(); i++)
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

            for (int j = 0; j < ROLE_DRAWING_COLUMN; j++)
            {
                char * content = roleDrawingTable->GetContent(i, j);

                char constant[16];
                if (j % 2 == 0)
                {
                    strcpy(constant, "Role Drawing ");

                }
                else
                {
                    strcpy(constant, "Alias ");

                }
                strcat(constant, order);

                ImGui::InputText(constant, content, MAX_COLUMNS_CONTENT_LENGTH);
                ImGui::NextColumn();
            }

        }

        ImGui::Columns(1);
        ImGui::Separator();
    }

    void HeavenGateWindowRoleDrawingTable::UpdateMenu()
    {
        //   ImGui::MenuItem("(dummy menu)", NULL, false, false);
        if (ImGui::MenuItem("New")) {

        }

        if (ImGui::MenuItem("Open", "Ctrl+O")) {

        }

        if (ImGui::MenuItem("Save", "Ctrl+S")) {
            StoryTable<ROLE_DRAWING_COLUMN>*const  roleDrawingTable = StoryTableManager::Instance().GetRoleDrawingTable();

            StoryFileManager::Instance().SaveTableFile(m_fullPath, roleDrawingTable);
        }

    }

}
