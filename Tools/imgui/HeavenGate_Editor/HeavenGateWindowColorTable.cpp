//
//Copyright (c) 2019 Star Platinum
//
//Created by Kong Wei Hang, 2019-12-19
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//

#include "imgui.h"

#include "HeavenGateWindowColorTable.h"
#include "HeavenGateEditorUtility.h"

#include "StoryTableManager.h"
#include "StoryFileManager.h"
#include "StoryTable.h"


namespace HeavenGateEditor {



    HeavenGateWindowColorTable::HeavenGateWindowColorTable()
    {

        memset(m_fullPath, 0, sizeof(m_fullPath));
        memset(m_color, 0, sizeof(m_color));
        //m_color[COLOR_VALUE_COLUMN - 1] = 0;

        HeavenGateEditorUtility::GetStoryPath(m_fullPath);
        strcat(m_fullPath, COLOR_TABLE_NAME);

        StoryTable<COLOR_MAX_COLUMN>* const colorTable = StoryTableManager::Instance().GetColorTable();

        bool result = StoryFileManager::Instance().LoadTableFile(m_fullPath, colorTable);
        if (result == false)
        {
            StoryFileManager::Instance().SaveTableFile(m_fullPath, colorTable);
            StoryFileManager::Instance().LoadTableFile(m_fullPath, colorTable);
        }


    }

    HeavenGateWindowColorTable::~HeavenGateWindowColorTable()
    {

    }

    void HeavenGateWindowColorTable::UpdateMainWindow()
    {
        StoryTable<COLOR_MAX_COLUMN>* const colorTable = StoryTableManager::Instance().GetColorTable();

        if (colorTable == nullptr)
        {
            return;
        }

        ImGui::Separator();
        ImGui::ColorEdit4("Color", m_color);

        ImGui::Text("Color Table");

        if (ImGui::Button("Add New Row"))
        {
            colorTable->AddRow();
        }
        ImGui::SameLine();
        if (ImGui::Button("Remove Row"))
        {
            colorTable->RemoveRow();
        }

        //if (ImGui::Button("Color"))
        //{
        //    for (int i = 0; i < COLOR_VALUE_COLUMN; i++)
        //    {
        //        printf("%f", m_color[i]);
        //    }
        //    //ImVec4 tmp = ImVec4(m_color[0], m_color[1], m_color[2], m_color[3]);
        //    //ImU32 rgb = ImGui::ColorConvertFloat4ToU32(tmp);
        //    //printf("%f ", rgb);
        //    //ImGuiCol ttt = 
        //    //ImVec4 rgba = ImGui::ColorConvertU32ToFloat4(rgb);
        //    //printf("%f ", rgba);
        //}

        ImGui::Columns(FONT_SIZE_MAX_COLUMN + 1, "Color"); // 4-ways, with border
        ImGui::Separator();
        ImGui::Text("Index");    ImGui::NextColumn();
        for (int i = 0; i < FONT_SIZE_MAX_COLUMN; i++)
        {
            ImGui::Text(colorTable->GetName(i));   ImGui::NextColumn();
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
        for (int i = 0; i < colorTable->GetSize(); i++)
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
                char * content = colorTable->GetContent(i, j);

                char constant[16];
                if (j % 2 == 0)
                {
                    strcpy(constant, "Alias ");

                }
                else
                {
                    strcpy(constant, "Value ");

                }
                strcat(constant, order);

                ImGui::InputText(constant, content, MAX_COLUMNS_CONTENT_LENGTH);
                ImGui::NextColumn();
            }

        }

        ImGui::Columns(1);
        ImGui::Separator();
    }

    void HeavenGateWindowColorTable::UpdateMenu()
    {
        //   ImGui::MenuItem("(dummy menu)", NULL, false, false);
        if (ImGui::MenuItem("New")) {

        }

        if (ImGui::MenuItem("Open", "Ctrl+O")) {

        }

        if (ImGui::MenuItem("Save", "Ctrl+S")) {

            StoryTable<COLOR_MAX_COLUMN>* const colorTable = StoryTableManager::Instance().GetColorTable();

            StoryFileManager::Instance().SaveTableFile(m_fullPath, colorTable);

        }

    }

}
