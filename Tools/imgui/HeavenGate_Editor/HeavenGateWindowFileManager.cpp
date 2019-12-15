//
//  HeavenGateEditorFileManager.cpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 27/11/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//
#include "imgui.h"


#include "HeavenGateWindowFileManager.h"
#include "StoryFileManager.h"
#include "StoryJson.h"

namespace HeavenGateEditor {

    HeavenGateWindowFileManager::HeavenGateWindowFileManager() {


    }

    HeavenGateWindowFileManager::~HeavenGateWindowFileManager() {
        if (m_storyFileManager)
        {
            delete m_storyFileManager;
        }

        m_storyFileManager = nullptr;

    }

    //bool HeavenGateWindowFileManager::SaveStoryFile(const StoryJson* pStoryJson) {

    //    if (m_storyFileManager != nullptr)
    //    {

    //        if (!m_storyFileManager->SaveStoryFile(pStoryJson)) {
    //            printf("new file path is illegal");
    //            return false;
    //        }
    //        m_storyFileManager->Initialize();

    //    }
    //    else {

    //        OpenWindow();

    //    }
    //}



    void HeavenGateEditor::HeavenGateWindowFileManager::UpdateMainWindow()
    {

        ImGui::Text("Pleae Input New File Name.\n\n");
        ImGui::Separator();

        char* fileNameHandle = m_storyFileManager->GetNewFileName();
        char* filePathHandle = m_storyFileManager->GetNewFilePath();
        ImGui::PushStyleVar(ImGuiStyleVar_FramePadding, ImVec2(0, 0));
        ImGui::InputTextWithHint("New File Name", "enter name here", fileNameHandle, IM_ARRAYSIZE(fileNameHandle));
        ImGui::PopStyleVar();

        if (ImGui::Button("OK", ImVec2(120, 0))) {
            if (m_storyFileManager->FromFileNameToFullPath(filePathHandle, fileNameHandle)) {
                //TODO
                m_storyFileManager->OpenStoryFile();
                m_storyFileManager->Initialize();


                ImGui::CloseCurrentPopup();

            }
            else {
                printf("Illegal File Name");
                //strcpy(fileNameHandle, "Illegal File Name");
            }
        }

        ImGui::SetItemDefaultFocus();
        ImGui::SameLine();
        if (ImGui::Button("Cancel", ImVec2(120, 0))) {

            ImGui::CloseCurrentPopup();
        }

    }
    //
    //    bool HeavenGateWindowFileManager::SaveStoryFile(StoryJson* pStory, bool* pIsSavedFile)
    //{
    //        if (m_storyFileManager == nullptr||pStory == nullptr || pIsSavedFile==nullptr)
    //        {
    //            return false;
    //        }
    //
    //        //Length of string new file path must large than 0 
    //        if (m_storyFileManager->IsNewFilePathExist())
    //        {
    //            if (!SaveStoryFile(pStory)) {
    //                return;
    //            }
    //            const char* filePath = m_storyFileManager->GetNewFilePath();
    //            pStory->SetFullPath(filePath);
    //            *pIsSavedFile = true;
    //
    //            //Clear
    //            m_storyFileManager->Initialize();
    //        }
    //    }
    //
    void HeavenGateWindowFileManager::SetStoryFileManager(StoryFileManager* pStoryFileManager)
    {
        m_storyFileManager = pStoryFileManager;
    }


    //void HeavenGateWindowFileManager::SetNewFilePath(const char* filePath) {

    //    if (m_storyFileManager != nullptr)
    //    {
    //        m_storyFileManager->SetNewFilePath(pStoryJson);
    //    }
    //}


}
