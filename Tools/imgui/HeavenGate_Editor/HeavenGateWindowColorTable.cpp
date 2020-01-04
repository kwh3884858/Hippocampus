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

        memset(m_fullPath, '/0', sizeof(m_fullPath));
        memset(m_color, 0, sizeof(m_color));
        m_colorAlpha = nullptr;
        m_colorAlpha = &m_color[3];
        m_color[3] = 1;
        r = new char[10];
        g = new char[10];
        b = new char[10];
        a = new char[10];
        //memset(r, '/0', sizeof(r));
        //memset(g, '/0', sizeof(g));
        //memset(b, '/0', sizeof(b));
        //memset(a, '/0', sizeof(a));
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

        ImGui::Text("Color Table");
        ImGui::ColorEdit4("Color", m_color);
        ImGui::SliderFloat("Color Alpha", m_colorAlpha,0,1);

        if (ImGui::Button("Add New Row"))
        {
            colorTable->AddRow();
        }
        ImGui::SameLine();
        if (ImGui::Button("Remove Row"))
        {
            colorTable->RemoveRow();
        }
        //if (ImGui::Button("Add Color To Cur Row"))
        //{
        //    //char buf2[4];
        //    //sprintf(buf2, "%d", 200);
        //    //printf("%s", buf2);
        //    int count = colorTable->GetSize();
        //    r = colorTable->GetContent(count -1, 1);
        //    g = colorTable->GetContent(count -1, 2);
        //    b = colorTable->GetContent(count -1, 3);
        //    a = colorTable->GetContent(count -1, 4);
        //    //char * tmp = new char[10];
        //    //itoa(m_color[0] * 255, tmp, 10);
        //    //*r = (m_color[0] * 255);
        //    sprintf(r, "%d", (int)(m_color[0] * 255 + 0.5));
        //    sprintf(g, "%d", (int)(m_color[1] * 255 + 0.5));
        //    sprintf(b, "%d", (int)(m_color[2] * 255 + 0.5));
        //    sprintf(a, "%d", (int)(m_color[3] * 255 + 0.5));
        //    //printf("%s", r);
        //    //itoa(m_color[1] * 255, g, 10);
        //    //itoa(m_color[2] * 255, b, 10);
        //    //itoa(m_color[3] * 255, a, 10);
        //    colorTable->RemoveRow();
        //    StoryRow<5>* row = colorTable->AddRow();
        //    //row->Push(r);
        //    row->Set(1, r);
        //    row->Set(2, g);
        //    row->Set(3, b);
        //    row->Set(4, a);
        //    //colorTable->PushRow()
        //    //curContent = 
        //}

        //if (ImGui::Button("Color"))
        //{
        //    for (int i = 0; i < COLOR_VALUE_COLUMN; i++)
        //    {
        //        printf("%f", m_color[i]);
        //    }
        //    ImVec4 tmp = ImVec4(m_color[0], m_color[1], m_color[2], m_color[3]);
        //    ImU32 rgb = ImGui::ColorConvertFloat4ToU32(tmp);
        //    printf("%f ", rgb);
        //    //ImGuiCol ttt = 
        //    ImVec4 rgba = ImGui::ColorConvertU32ToFloat4(rgb);
        //    printf("%f ", rgba);
        //}

        ImGui::Columns(COLOR_MAX_COLUMN + 1, "Color"); // 4-ways, with border
        ImGui::Separator();
        ImGui::Text("Index");    ImGui::NextColumn();
        for (int i = 0; i < COLOR_MAX_COLUMN; i++)
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

            for (int j = 0; j < COLOR_MAX_COLUMN; j++)
            {
                char * content = colorTable->GetContent(i, j);

                char constant[16];
                switch (j)
                {
                case 0:
                    strcpy(constant, "Alias ");
                    strcat(constant, order);
                    break;
                case 1:
                    strcpy(constant, "R ");
                    strcat(constant, order);
                    break;
                case 2:
                    strcpy(constant, "G ");
                    strcat(constant, order);
                    break;
                case 3:
                    strcpy(constant, "B ");
                    strcat(constant, order);
                    break;
                case 4:
                    strcpy(constant, "A ");
                    strcat(constant, order);
                    break;
                default:
                    break;
                }
                ImGui::InputText(constant, content, MAX_COLUMNS_CONTENT_LENGTH);
                ImGui::NextColumn();
            }
            char buf[14] = "Add Color";
            char bufi[4];
            itoa(i, bufi, 10);
            strcat(buf, bufi);
            if (ImGui::Button(buf))
            {
                //char buf2[4];
                //sprintf(buf2, "%d", 200);
                //printf("%s", buf2);
                r = colorTable->GetContent(i, 1);
                g = colorTable->GetContent(i, 2);
                b = colorTable->GetContent(i, 3);
                a = colorTable->GetContent(i, 4);
                //char * tmp = new char[10];
                //itoa(m_color[0] * 255, tmp, 10);
                //*r = (m_color[0] * 255);
                sprintf(r, "%d", (int)(m_color[0] * 255 + 0.5));
                sprintf(g, "%d", (int)(m_color[1] * 255 + 0.5));
                sprintf(b, "%d", (int)(m_color[2] * 255 + 0.5));
                sprintf(a, "%d", (int)(m_color[3] * 255 + 0.5));
                //printf("%s", r);
                //itoa(m_color[1] * 255, g, 10);
                //itoa(m_color[2] * 255, b, 10);
                //itoa(m_color[3] * 255, a, 10);
                //colorTable->RemoveRow();
                //StoryRow<5>* row = colorTable->AddRow();
                //row->Push(r);
                //row->Set(1, r);
                //row->Set(2, g);
                //row->Set(3, b);
                //row->Set(4, a);
                //colorTable->PushRow()
                //curContent = 
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
