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

#include "StoryFileManager.h"
#include "StoryTable.h"

namespace HeavenGateEditor {


    HeavenGateEditorFontSizeTable::HeavenGateEditorFontSizeTable() {


        m_fileManager = new StoryFileManager;

        m_table = new StoryTable< FONT_SIZE_MAX_COLUMN>;

        memset(m_fullPath, 0, sizeof(m_fullPath));

        HeavenGateEditorUtility::GetStoryPath(m_fullPath);
        strcat(m_fullPath, FONT_TABLE_NAME);

       bool result = m_fileManager->LoadTableFile(m_fullPath, m_table);
       if (result == false)
       {
           m_fileManager->SaveTableFile(m_fullPath, m_table);
           m_fileManager->LoadTableFile(m_fullPath, m_table);
       }

       m_table->PushName("Size Key");
       m_table->PushName("Font Size Value");
    }

    HeavenGateEditorFontSizeTable:: ~HeavenGateEditorFontSizeTable() {



        if (m_fileManager)
        {
            delete m_fileManager;
        }
        m_fileManager = nullptr;

        if (m_table)
        {
            delete m_table;
        }
        m_table = nullptr;

    }


    void HeavenGateEditorFontSizeTable::UpdateMainWindow()
    {
        if (m_table == nullptr)
        {
            return;
        }

        ImGui::Separator();

        ImGui::Text("Font Size Table");

        if (ImGui::Button("Add New Row"))
        {
            m_table->AddRow();
        }

        ImGui::Columns(FONT_SIZE_MAX_COLUMN, "Font Size"); // 4-ways, with border
        ImGui::Separator();
        for (int i = 0; i < FONT_SIZE_MAX_COLUMN; i++)
        {
            ImGui::Text(m_table->GetName(i)); ImGui::NextColumn();
        }

        //ImGui::Text("ID"); ImGui::NextColumn();
        //ImGui::Text("Name"); ImGui::NextColumn();
        //ImGui::Text("Path"); ImGui::NextColumn();
        //ImGui::Text("Hovered"); ImGui::NextColumn();
        ImGui::Separator();
        //const char* names[3] = { "One", "Two", "Three" };
        //const char* paths[3] = { "/path/one", "/path/two", "/path/three" };
        static int selected = -1;



        for (int i = 0; i < m_table->GetSize(); i++)
        {
            char label[32];
            sprintf(label, "%04d", i);
            if (ImGui::Selectable(label, selected == i, ImGuiSelectableFlags_SpanAllColumns))
                selected = i;
            //bool hovered = ImGui::IsItemHovered();
            ImGui::NextColumn();
            //ImGui::Text(names[i]); ImGui::NextColumn();
            //ImGui::Text(paths[i]); ImGui::NextColumn();
            //ImGui::Text("%d", hovered); ImGui::NextColumn();

            for (int j = 0; j < FONT_SIZE_MAX_COLUMN; j++)
            {
                ImGui::Text(m_table->GetContent(i, j)); ImGui::NextColumn();

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
            m_fileManager->SaveTableFile(m_fullPath, m_table);
        }

    }




}
