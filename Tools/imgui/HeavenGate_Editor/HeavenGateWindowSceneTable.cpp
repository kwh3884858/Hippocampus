//
//Copyright (c) 2019 Star Platinum
//
//Created by Wen Yang Wei, 2019-12-25
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//

#include "imgui.h"

#include "HeavenGateWindowSceneTable.h"
#include "HeavenGateEditorUtility.h"

#include "StoryTableManager.h"
#include "StoryFileManager.h"
#include "StoryTable.h"
namespace HeavenGateEditor {



    HeavenGateWindowSceneTable::HeavenGateWindowSceneTable()
    {
     
        StoryTable<SCENE_COLUMN>* const sceneTable = StoryTableManager::Instance().GetSceneTable();


        memset(m_fullPath, 0, sizeof(m_fullPath));

        HeavenGateEditorUtility::GetStoryPath(m_fullPath);
        strcat(m_fullPath, SCENE_TABLE_NAME);

        bool result = StoryFileManager::Instance().LoadTableFile(m_fullPath, sceneTable);
        if (result == false)
        {
            StoryFileManager::Instance().SaveTableFile(m_fullPath, sceneTable);
            StoryFileManager::Instance().LoadTableFile(m_fullPath, sceneTable);
        }


    }

    HeavenGateWindowSceneTable::~HeavenGateWindowSceneTable()
    {

        //if (m_fileManager)
        //{
        //    delete m_fileManager;
        //}
        //m_fileManager = nullptr;

        //if (sceneTable)
        //{
        //    delete sceneTable;
        //}
        //sceneTable = nullptr;

    }

    void HeavenGateWindowSceneTable::UpdateMainWindow()
    {
        StoryTable<SCENE_COLUMN>* const sceneTable = StoryTableManager::Instance().GetSceneTable();

        if (sceneTable == nullptr)
        {
            return;
        }

        ImGui::Separator();

        ImGui::Text("Scene Table");

        if (ImGui::Button("Add New Row"))
        {
            sceneTable->AddRow();
        }
        ImGui::SameLine();
        if (ImGui::Button("Remove Row"))
        {
            sceneTable->RemoveRow();
        }

        ImGui::Columns(SCENE_COLUMN + 1, "Scene"); // 4-ways, with border
        ImGui::Separator();
        ImGui::Text("Index");    ImGui::NextColumn();
        for (int i = 0; i < SCENE_COLUMN; i++)
        {
            ImGui::Text(sceneTable->GetName(i));   ImGui::NextColumn();
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
        for (int i = 0; i < sceneTable->GetSize(); i++)
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

            for (int j = 0; j < SCENE_COLUMN; j++)
            {
                char * content = sceneTable->GetContent(i, j);

                char constant[16];
                switch (j)
                {
                    case 0:
                        strcpy(constant, "scene ");
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

    void HeavenGateWindowSceneTable::UpdateMenu()
    {
        //   ImGui::MenuItem("(dummy menu)", NULL, false, false);
        if (ImGui::MenuItem("New")) {

        }

        if (ImGui::MenuItem("Open", "Ctrl+O")) {

        }

        if (ImGui::MenuItem("Save", "Ctrl+S")) {
            StoryTable<SCENE_COLUMN>* const sceneTable = StoryTableManager::Instance().GetSceneTable();

            StoryFileManager::Instance().SaveTableFile(m_fullPath, sceneTable);
        }

    }

}
