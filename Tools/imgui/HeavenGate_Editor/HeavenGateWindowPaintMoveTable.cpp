//
//Copyright (c) 2019 Star Platinum
//
//Created by Wen Yang Wei, 2019-12-25
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//

#include "imgui.h"

#include "HeavenGateWindowPaintMoveTable.h"
#include "HeavenGateEditorUtility.h"

#include "StoryTableManager.h"
#include "StoryFileManager.h"
#include "StoryTable.h"
namespace HeavenGateEditor {



    HeavenGateWindowPaintMoveTable::HeavenGateWindowPaintMoveTable()
    {
     
   


    }

    HeavenGateWindowPaintMoveTable::~HeavenGateWindowPaintMoveTable()
    {

        //if (m_fileManager)
        //{
        //    delete m_fileManager;
        //}
        //m_fileManager = nullptr;

        //if (paintMoveTable)
        //{
        //    delete paintMoveTable;
        //}
        //paintMoveTable = nullptr;

    }

    void HeavenGateWindowPaintMoveTable::Initialize()
    {
        StoryTable<PAINT_MOVE_MAX_COLUMN>* const paintMoveTable = StoryTableManager::Instance().GetPaintMoveTable();


        memset(m_fullPath, 0, sizeof(m_fullPath));

        HeavenGateEditorUtility::GetStoryPath(m_fullPath);
        strcat(m_fullPath, PAINT_MOVE_TABLE_NAME);

        bool result = StoryFileManager::Instance().LoadTableFile(m_fullPath, paintMoveTable);
        if (result == false)
        {
            paintMoveTable->UpdateName();
            StoryFileManager::Instance().SaveTableFile(m_fullPath, paintMoveTable);
            StoryFileManager::Instance().LoadTableFile(m_fullPath, paintMoveTable);
        }
    }

    void HeavenGateWindowPaintMoveTable::Shutdown()
    {

    }

    void HeavenGateWindowPaintMoveTable::UpdateMainWindow()
    {
        StoryTable<PAINT_MOVE_MAX_COLUMN>* const paintMoveTable = StoryTableManager::Instance().GetPaintMoveTable();

        if (paintMoveTable == nullptr)
        {
            return;
        }

        ImGui::Separator();

        ImGui::Text("Paint move Table");

        if (ImGui::Button("Add New Row"))
        {
            paintMoveTable->AddRow();
        }
        ImGui::SameLine();
        if (ImGui::Button("Remove Row"))
        {
            paintMoveTable->RemoveRow();
        }

        ImGui::Columns(PAINT_MOVE_MAX_COLUMN + 1, "Paint Move"); // 4-ways, with border
        ImGui::Separator();
        ImGui::Text("Index");    ImGui::NextColumn();
        for (int i = 0; i < PAINT_MOVE_MAX_COLUMN; i++)
        {
            ImGui::Text(paintMoveTable->GetName(i));   ImGui::NextColumn();
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
        for (int i = 0; i < paintMoveTable->GetSize(); i++)
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

            for (int j = 0; j < PAINT_MOVE_MAX_COLUMN; j++)
            {
                char * content = paintMoveTable->GetContent(i, j);

                char constant[16];
                switch (j)
                {
                    case 0:
                        strcpy(constant, "moveAlias ");
                        break;
                    case 1:
                        strcpy(constant, "startPoint ");
                        break;
                    case 2:
                        strcpy(constant, "endPoint ");
                        break;
                    case 3:
                        strcpy(constant, "moveType ");
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

    void HeavenGateWindowPaintMoveTable::UpdateMenu()
    {
        //   ImGui::MenuItem("(dummy menu)", NULL, false, false);
        if (ImGui::MenuItem("New")) {

        }

        if (ImGui::MenuItem("Open", "Ctrl+O")) {

        }

        if (ImGui::MenuItem("Save", "Ctrl+S")) {
            StoryTable<PAINT_MOVE_MAX_COLUMN>* const paintMoveTable = StoryTableManager::Instance().GetPaintMoveTable();

            StoryFileManager::Instance().SaveTableFile(m_fullPath, paintMoveTable);
        }

    }

}
