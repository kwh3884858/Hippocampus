//
//  HeavenGateEditorFontSizeTable.cpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 7/12/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//

#include "HeavenGateEditorFontSizeTable.h"

#include "HeavenGateEditorFileManager.h"


namespace HeavenGateEditor {


    HeavenGateEditorFontSizeTable::HeavenGateEditorFontSizeTable() {
        m_open = false;
    }
    HeavenGateEditorFontSizeTable:: ~HeavenGateEditorFontSizeTable() {

    }

    void HeavenGateEditorFontSizeTable::ShowTableWindow() {

        // We specify a default position/size in case there's no data in the .ini file. Typically this isn't required! We only do it to make the Demo applications a little more welcoming.
        ImGui::SetNextWindowPos(ImVec2(650, 20), ImGuiCond_FirstUseEver);
        ImGui::SetNextWindowSize(ImVec2(550, 680), ImGuiCond_FirstUseEver);

        ImGuiWindowFlags window_flags = 0;
        window_flags |= ImGuiWindowFlags_MenuBar;

        // Main body of the Demo window starts here.
        if (!ImGui::Begin("Heaven Gate Table Editor", &m_open, window_flags))
        {
            // Early out if the window is collapsed, as an optimization.
            ImGui::End();
            return;
        }

        // Most "big" widgets share a common width settings by default.
         //ImGui::PushItemWidth(ImGui::GetWindowWidth() * 0.65f);    // Use 2/3 of the space for widgets and 1/3 for labels (default)
        ImGui::PushItemWidth(ImGui::GetFontSize() * -12);           // Use fixed width for labels (by passing a negative value), the rest goes to widgets. We choose a width proportional to our font size.


        // Menu Bar
        if (ImGui::BeginMenuBar())
        {
            if (ImGui::BeginMenu("Menu"))
            {
                ShowEditorMenuFile();
                ImGui::EndMenu();
            }

            ImGui::EndMenuBar();
        }


        ImGui::Separator();

        ImGui::Text("With border:");
        ImGui::Columns(4, "mycolumns"); // 4-ways, with border
        ImGui::Separator();
        ImGui::Text("ID"); ImGui::NextColumn();
        ImGui::Text("Name"); ImGui::NextColumn();
        ImGui::Text("Path"); ImGui::NextColumn();
        ImGui::Text("Hovered"); ImGui::NextColumn();
        ImGui::Separator();
        const char* names[3] = { "One", "Two", "Three" };
        const char* paths[3] = { "/path/one", "/path/two", "/path/three" };
        static int selected = -1;
        for (int i = 0; i < 3; i++)
        {
            char label[32];
            sprintf(label, "%04d", i);
            if (ImGui::Selectable(label, selected == i, ImGuiSelectableFlags_SpanAllColumns))
                selected = i;
            bool hovered = ImGui::IsItemHovered();
            ImGui::NextColumn();
            ImGui::Text(names[i]); ImGui::NextColumn();
            ImGui::Text(paths[i]); ImGui::NextColumn();
            ImGui::Text("%d", hovered); ImGui::NextColumn();
        }
        ImGui::Columns(1);
        ImGui::Separator();

        ImGui::End();
    }


    void HeavenGateEditorFontSizeTable::OpenWindow() {
        m_open = true;
    }
    void HeavenGateEditorFontSizeTable::CloseWindow() {
        m_open = false;
    }
    bool HeavenGateEditorFontSizeTable::IsOpenWindow() const {
        return  m_open;
    }
    bool * HeavenGateEditorFontSizeTable::GetWindowOpenHandle()
    {
        return &m_open;
    }
    // Note that shortcuts are currently provided for display only (future version will add flags to BeginMenu to process shortcuts)
    void HeavenGateEditorFontSizeTable::ShowEditorMenuFile()
    {

        //   ImGui::MenuItem("(dummy menu)", NULL, false, false);
        if (ImGui::MenuItem("New")) {

        }

        if (ImGui::MenuItem("Open", "Ctrl+O")) {

        }

        if (ImGui::MenuItem("Save", "Ctrl+S")) {

        }
    }


}
