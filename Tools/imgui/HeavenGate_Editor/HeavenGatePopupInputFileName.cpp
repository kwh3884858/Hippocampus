//
//  HeavenGateEditorFileManager.cpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 27/11/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//
#include "imgui.h"


#include "HeavenGatePopupInputFileName.h"
#include "StoryFileManager.h"
#include "StoryJson.h"

namespace HeavenGateEditor {

    HeavenGatePopupInputFileName::HeavenGatePopupInputFileName() {

        Initialize();

    }

    HeavenGatePopupInputFileName::~HeavenGatePopupInputFileName() {
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



    void HeavenGateEditor::HeavenGatePopupInputFileName::UpdateMainWindow()
    {

        if (m_storyFileManager == nullptr || m_ppStory == nullptr)
        {
            return;
        }

        StoryJson* story = *m_ppStory;

        if (story != nullptr )
        {
            if (story->IsExistFullPath() != true) {
                printf("Error story");
                return;
            }

            m_storyFileManager->SaveStoryFile(story);
            story->Clear();
        }
        else {
            story = new StoryJson;
        }

        

        ImGui::Text("Please Input New File Name.\n\n");
        ImGui::Separator();

        ImGui::PushStyleVar(ImGuiStyleVar_FramePadding, ImVec2(0, 0));
        ImGui::InputTextWithHint("New File Name", "enter name here", m_fileName, IM_ARRAYSIZE(m_fileName));
        ImGui::PopStyleVar();

        if (ImGui::Button("OK", ImVec2(120, 0))) {
            if (m_storyFileManager->FromFileNameToFullPath(m_filePath, m_fileName)) {
                //TODO
                //m_storyFileManager->SaveStoryFile(story);
                //m_storyFileManager->Initialize();
                story->SetFullPath(m_filePath);

                Initialize();

                CloseWindow();

            }
            else {
                printf("Illegal File Name");
                //strcpy(fileNameHandle, "Illegal File Name");
            }
        }

        ImGui::SetItemDefaultFocus();
        ImGui::SameLine();
        if (ImGui::Button("Cancel", ImVec2(120, 0))) {
            Initialize();

            CloseWindow();
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
    void HeavenGatePopupInputFileName::SetStoryFileManager(StoryFileManager* pStoryFileManager)
    {
        m_storyFileManager = pStoryFileManager;
    }


    void HeavenGatePopupInputFileName::SetStoryJsonPonter(StoryJson** ppStory)
    {
        m_ppStory = ppStory;
    }

    void HeavenGatePopupInputFileName::Initialize()
    {
        memset(m_fileName, 0, sizeof(m_fileName));
        memset(m_filePath, 0, sizeof(m_filePath));

    }

    //void HeavenGateWindowFileManager::SetNewFilePath(const char* filePath) {

    //    if (m_storyFileManager != nullptr)
    //    {
    //        m_storyFileManager->SetNewFilePath(pStoryJson);
    //    }
    //}


}
