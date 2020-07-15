//
//Copyright (c) 2019 Star Platinum
//
//Created by Wen Yang Wei, 2020-02-05
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//

#include "imgui.h"

#include "HeavenGateWindowEffectTable.h"
#include "HeavenGateEditorUtility.h"

#include "StoryTableManager.h"
#include "StoryFileManager.h"
#include "StoryTable.h"
namespace HeavenGateEditor {



    HeavenGateWindowEffectTable::HeavenGateWindowEffectTable()
    {
     
    }

    HeavenGateWindowEffectTable::~HeavenGateWindowEffectTable()
    {

        //if (m_fileManager)
        //{
        //    delete m_fileManager;
        //}
        //m_fileManager = nullptr;

        //if (effectTable)
        //{
        //    delete effectTable;
        //}
        //effectTable = nullptr;

    }

    void HeavenGateWindowEffectTable::Initialize()
    {
        StoryTable<EFFECT_MAX_COLUMN>* const effectTable = StoryTableManager::Instance().GetEffectTable();


        memset(m_fullPath, 0, sizeof(m_fullPath));

        HeavenGateEditorUtility::GetStoryPath(m_fullPath);
        strcat(m_fullPath, EFFECT_TABLE_NAME);

        bool result = StoryFileManager::Instance().LoadTableFile(m_fullPath, effectTable);
        if (result == false)
        {
            effectTable->UpdateName();
            StoryFileManager::Instance().SaveTableFile(m_fullPath, effectTable);
            StoryFileManager::Instance().LoadTableFile(m_fullPath, effectTable);
        }
    }

    void HeavenGateWindowEffectTable::Shutdown()
    {

    }

    void HeavenGateWindowEffectTable::UpdateMainWindow()
    {
        StoryTable<EFFECT_MAX_COLUMN>* const effectTable = StoryTableManager::Instance().GetEffectTable();

        if (effectTable == nullptr)
        {
            return;
        }

        ImGui::Separator();

        ImGui::Text("Effect Table");

        if (ImGui::Button("Add New Row"))
        {
            effectTable->AddRow();
        }
        ImGui::SameLine();
        if (ImGui::Button("Remove Row"))
        {
            effectTable->RemoveRow();
        }

        ImGui::Columns(EFFECT_MAX_COLUMN + 1, "Effect"); // 4-ways, with border
        ImGui::Separator();
        ImGui::Text("Index");    ImGui::NextColumn();
        for (int i = 0; i < EFFECT_MAX_COLUMN; i++)
        {
            ImGui::Text(effectTable->GetName(i));   ImGui::NextColumn();
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
        for (int i = 0; i < effectTable->GetSize(); i++)
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

            for (int j = 0; j < EFFECT_MAX_COLUMN; j++)
            {
                char * content = effectTable->GetContent(i, j);

                char constant[16];
                switch (j)
                {
                    case 0:
                        strcpy(constant, "effect ");
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

    void HeavenGateWindowEffectTable::UpdateMenu()
    {
        //   ImGui::MenuItem("(dummy menu)", NULL, false, false);
        if (ImGui::MenuItem("New")) {

        }

        if (ImGui::MenuItem("Open", "Ctrl+O")) {

        }

        if (ImGui::MenuItem("Save", "Ctrl+S")) {
            StoryTable<EFFECT_MAX_COLUMN>* const effectTable = StoryTableManager::Instance().GetEffectTable();

            StoryFileManager::Instance().SaveTableFile(m_fullPath, effectTable);
        }

    }

}
