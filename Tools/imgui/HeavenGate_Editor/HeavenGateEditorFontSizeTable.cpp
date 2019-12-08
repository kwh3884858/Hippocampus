//
//  HeavenGateEditorFontSizeTable.cpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 7/12/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//

#include "HeavenGateEditorFontSizeTable.h"

namespace HeavenGateEditor {


HeavenGateEditorFontSizeTable:: HeavenGateEditorFontSizeTable(){

}
HeavenGateEditorFontSizeTable:: ~HeavenGateEditorFontSizeTable(){

}

void HeavenGateEditorFontSizeTable::ShowTableWindow(){
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

}

void HeavenGateEditorFontSizeTable::CloseTableWindow(){

}

}
