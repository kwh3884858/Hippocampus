//
//  HeavenGateEditorFontSizeTable.cpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 7/12/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//
#include "imgui.h"

#include "HeavenGateEditorFontSizeTable.h"

#include "HeavenGateEditorConstant.h"
#include "HeavenGateEditorUtility.h"

#include "StoryTableManager.h"
#include "StoryFileManager.h"
#include "StoryTable.h"

namespace HeavenGateEditor {

    HeavenGateEditorFontSizeTable::HeavenGateEditorFontSizeTable() {

        StoryTable<FONT_SIZE_MAX_COLUMN>* fontSizeTable = StoryTableManager::Instance().GetFontSizeTable();

        memset(m_fullPath, 0, sizeof(m_fullPath));
      

        HeavenGateEditorUtility::GetStoryPath(m_fullPath);
        strcat(m_fullPath, FONT_TABLE_NAME);

        bool result = StoryFileManager::Instance().LoadTableFile(m_fullPath, fontSizeTable);
        if (result == false)
        {
            StoryFileManager::Instance().SaveTableFile(m_fullPath, fontSizeTable);
            StoryFileManager::Instance().LoadTableFile(m_fullPath, fontSizeTable);
        }

    }

    HeavenGateEditorFontSizeTable:: ~HeavenGateEditorFontSizeTable() {



        //if (m_fileManager)
        //{
        //    delete m_fileManager;
        //}
        //m_fileManager = nullptr;

        //if (fontSizeTable)
        //{
        //    delete fontSizeTable;
        //}
        //fontSizeTable = nullptr;

    }


    void HeavenGateEditorFontSizeTable::UpdateMainWindow()
    {
        StoryTable<FONT_SIZE_MAX_COLUMN>* fontSizeTable = StoryTableManager::Instance().GetFontSizeTable();

        if (fontSizeTable == nullptr)
        {
            return;
        }

        ImGui::Separator();

        ImGui::Text("Font Size Table");

        if (ImGui::Button("Add New Row"))
        {
            fontSizeTable->AddRow();
        }
        ImGui::SameLine();
        if (ImGui::Button("Remove Row"))
        {
            fontSizeTable->RemoveRow();
        }

        ImGui::Columns(FONT_SIZE_MAX_COLUMN + 1, "Font Size"); // 4-ways, with border
        ImGui::Separator();
        ImGui::Text("Index");    ImGui::NextColumn();
        for (int i = 0; i < FONT_SIZE_MAX_COLUMN; i++)
        {
            ImGui::Text(fontSizeTable->GetName(i));   ImGui::NextColumn();
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
        for (int i = 0; i < fontSizeTable->GetSize(); i++)
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
                char * content = fontSizeTable->GetContent(i, j);

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

    void HeavenGateEditorFontSizeTable::UpdateMenu()
    {
        //   ImGui::MenuItem("(dummy menu)", NULL, false, false);
        if (ImGui::MenuItem("New")) {

        }

        if (ImGui::MenuItem("Open", "Ctrl+O")) {

        }

        if (ImGui::MenuItem("Save", "Ctrl+S")) {
            StoryTable<FONT_SIZE_MAX_COLUMN>* fontSizeTable = StoryTableManager::Instance().GetFontSizeTable();

            StoryFileManager::Instance().SaveTableFile(m_fullPath, fontSizeTable);
        }

    }




}
