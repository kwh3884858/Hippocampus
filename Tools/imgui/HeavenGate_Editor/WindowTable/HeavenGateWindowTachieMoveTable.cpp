//
//Copyright (c) 2019 Star Platinum
//
//Created by Wen Yang Wei, 2019-12-25
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//

#include "imgui.h"

#include "HeavenGateWindowTachieMoveTable.h"
#include "HeavenGateEditorUtility.h"

#include "StoryTableManager.h"
#include "StoryFileManager.h"
#include "StoryTable.h"
namespace HeavenGateEditor {



    HeavenGateWindowTachieMoveTable::HeavenGateWindowTachieMoveTable()
    {
    }

    HeavenGateWindowTachieMoveTable::~HeavenGateWindowTachieMoveTable()
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

    void HeavenGateWindowTachieMoveTable::Initialize()
    {
        StoryTable<TACHIE_MOVE_MAX_COLUMN>* const paintMoveTable = StoryTableManager::Instance().GetPaintMoveTable();


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

    void HeavenGateWindowTachieMoveTable::Shutdown()
    {

    }

    void HeavenGateWindowTachieMoveTable::UpdateMainWindow()
    {
        StoryTable<TACHIE_MOVE_MAX_COLUMN>* const tachieMoveTable = StoryTableManager::Instance().GetPaintMoveTable();

        if (tachieMoveTable == nullptr)
        {
            return;
        }

        ImGui::Separator();

        ImGui::Text("Tachie move Table");

        if (ImGui::Button("Add New Row"))
        {
            tachieMoveTable->AddRow();
        }
        ImGui::SameLine();
        if (ImGui::Button("Remove Row"))
        {
            tachieMoveTable->RemoveRow();
        }

        ImGui::Columns(TACHIE_MOVE_MAX_COLUMN + 1, "Paint Move"); // 4-ways, with border
        ImGui::Separator();
        ImGui::Text("Index");    ImGui::NextColumn();
        for (int i = 0; i < TACHIE_MOVE_MAX_COLUMN; i++)
        {
            ImGui::Text(tachieMoveTable->GetName(i));   ImGui::NextColumn();
        }

        ImGui::Separator();

        static int selected = -1;
        char order[8] = "";
        for (int i = 0; i < tachieMoveTable->GetSize(); i++)
        {
            char label[32];
            sprintf(label, "%04d", i);
            if (ImGui::Selectable(label, selected == i, ImGuiSelectableFlags_None))
                selected = i;

            sprintf(order, "%d", i);
            ImGui::NextColumn();


            for (int j = 0; j < TACHIE_MOVE_MAX_COLUMN; j++)
            {
                char* content = tachieMoveTable->GetContent(i, j);

                char constant[32];
                char comboContent[16] = "MoveCurve";
                sprintf(order, "%d", i);
                strcat(comboContent, order);

                for (int k = 0; k < (int)TachieMoveTableLayout::Amount; k++)
                {
                    if (j == k)
                    {
                        strcpy(constant, tachieMoveTable->GetHeaderName(k + 1));
                        break;
                    }
                }

                strcat(constant, order);

                if (j == MappingLayoutToArrayIndex((int)TachieMoveTableLayout::MoveCurve))
                {
                    if (ImGui::BeginCombo(comboContent, content, 0))
                    {
                        for (int i = 0; i < (int)Ease::Amount; i++)
                        {
                            bool isSelected = strcmp(content, EaseString[i]) == 0 ? true : false;
                            if (ImGui::Selectable(EaseString[i], isSelected))
                            {
                                strcpy(content, EaseString[i]);
                            }
                            if (isSelected)
                                ImGui::SetItemDefaultFocus();   // Set the initial focus when opening the combo (scrolling + for keyboard navigation support in the upcoming navigation branch)
                        }
                        ImGui::EndCombo();
                    }
                }
                else {
                    ImGui::InputText(constant, content, MAX_COLUMNS_CONTENT_LENGTH);
                }
                ImGui::NextColumn();
            }

        }

        ImGui::Columns(1);
        ImGui::Separator();
    }

    void HeavenGateWindowTachieMoveTable::UpdateMenu()
    {
        //   ImGui::MenuItem("(dummy menu)", NULL, false, false);
        if (ImGui::MenuItem("New")) {

        }

        if (ImGui::MenuItem("Open", "Ctrl+O")) {

        }

        if (ImGui::MenuItem("Save", "Ctrl+S")) {
            StoryTable<TACHIE_MOVE_MAX_COLUMN>* const paintMoveTable = StoryTableManager::Instance().GetPaintMoveTable();

            StoryFileManager::Instance().SaveTableFile(m_fullPath, paintMoveTable);
        }

    }

}
