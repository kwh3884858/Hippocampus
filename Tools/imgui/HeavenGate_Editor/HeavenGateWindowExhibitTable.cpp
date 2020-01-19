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
     
        StoryTable<EXHIBIT_COLUMN>* const exhibitTable = StoryTableManager::Instance().GetExhibitTable();


        memset(m_fullPath, 0, sizeof(m_fullPath));

        HeavenGateEditorUtility::GetStoryPath(m_fullPath);
        strcat(m_fullPath, EXHIBIT_TABLE_NAME);

        bool result = StoryFileManager::Instance().LoadTableFile(m_fullPath, exhibitTable);
        if (result == false)
        {
            StoryFileManager::Instance().SaveTableFile(m_fullPath, exhibitTable);
            StoryFileManager::Instance().LoadTableFile(m_fullPath, exhibitTable);
        }


    }

    HeavenGateWindowExhibitTable::~HeavenGateWindowExhibitTable()
    {

        //if (m_fileManager)
        //{
        //    delete m_fileManager;
        //}
        //m_fileManager = nullptr;

        //if (exhibitTable)
        //{
        //    delete exhibitTable;
        //}
        //exhibitTable = nullptr;

    }

    void HeavenGateWindowExhibitTable::UpdateMainWindow()
    {
        StoryTable<EXHIBIT_COLUMN>* const exhibitTable = StoryTableManager::Instance().GetExhibitTable();

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

        //ImGui::Text("ID"); ImGui::NextColumn();
        //ImGui::Text("Name"); ImGui::NextColumn();
        //ImGui::Text("Path"); ImGui::NextColumn();
        //ImGui::Text("Hovered"); ImGui::NextColumn();
        ImGui::Separator();
        //const char* names[3] = { "One", "Two", "Three" };
        //const char* paths[3] = { "/path/one", "/path/two", "/path/three" };
        static int selected = -1;


        char order[8] = "";
        for (int i = 0; i < exhibitTable->GetSize(); i++)
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

            for (int j = 0; j < EXHIBIT_COLUMN; j++)
            {
                char * content = exhibitTable->GetContent(i, j);

                char constant[16];
                switch (j)
                {
                    case 0:
                        strcpy(constant, "exhibit ");
                        break;
                    case 1:
                        strcpy(constant, "description ");
                        break;
                        break;
                default:
                    break;
                }
                //if (j % 2 == 0)
                //{
                //    strcpy(constant, "Angle ");

                //}
                //else
                //{
                //    strcpy(constant, "Distance ");

                //}
                strcat(constant, order);

                ImGui::InputText(constant, content, MAX_COLUMNS_CONTENT_LENGTH);
                ImGui::NextColumn();
            }

        }

        ImGui::Columns(1);
        ImGui::Separator();
    }

    void HeavenGateWindowExhibitTable::UpdateMenu()
    {
        //   ImGui::MenuItem("(dummy menu)", NULL, false, false);
        if (ImGui::MenuItem("New")) {

        }

        if (ImGui::MenuItem("Open", "Ctrl+O")) {

        }

        if (ImGui::MenuItem("Save", "Ctrl+S")) {
            StoryTable<SCENE_COLUMN>* const exhibitTable = StoryTableManager::Instance().GetExhibitTable();

            StoryFileManager::Instance().SaveTableFile(m_fullPath, exhibitTable);
        }

    }

}
