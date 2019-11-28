//
//  HeavenGateEditorFileManager.cpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 27/11/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//
#include "imgui.h"
#include "HeavenGateEditorFileManager.h"
#include "HeavenGateEditorUtility.h"

#include <fstream>


namespace HeavenGateEditor {

HeavenGateEditorFileManager::HeavenGateEditorFileManager(){
    Initialize();
}

HeavenGateEditorFileManager::~HeavenGateEditorFileManager(){
   
}

bool HeavenGateEditorFileManager::SaveStoryFile(StoryJson* pStoryJson) const{
    if (strlen(m_newFilePath) <= 0) {
        return false;
    }

        json j_test = *pStoryJson;
        std::ofstream o(m_newFilePath);
        o << j_test << std::endl;

    o.close();

    return true;
}

void HeavenGateEditorFileManager::OpenAskForNameWindow() 
{
    m_isOpenAskForNewFileNamePopup = true;
}

void HeavenGateEditorFileManager::ShowAskForNewFileNamePopup(){
    if (!m_isOpenAskForNewFileNamePopup)
    {
        return;
    }

    ImGui::OpenPopup("New File Name");

    if (ImGui::BeginPopupModal("New File Name", NULL, ImGuiWindowFlags_AlwaysAutoResize))
    {
        ImGui::Text("Pleae Input New File Name.\n\n");
        ImGui::Separator();

        //static int dummy_i = 0;
        //ImGui::Combo("Combo", &dummy_i, "Delete\0Delete harder\0");


        ImGui::PushStyleVar(ImGuiStyleVar_FramePadding, ImVec2(0, 0));
                 ImGui::InputTextWithHint("New File Name", "enter name here", m_newFileName, IM_ARRAYSIZE(m_newFileName));
        ImGui::PopStyleVar();

        if (ImGui::Button("OK", ImVec2(120, 0))) {
            if(FromFileNameToFullPath(m_newFilePath, m_newFileName)){
                m_isOpenAskForNewFileNamePopup = false;

                ImGui::CloseCurrentPopup();

            }else{
                strcpy(m_newFileName, "Illegal File Name");
            }
        }
        ImGui::SetItemDefaultFocus();
        ImGui::SameLine();
        if (ImGui::Button("Cancel", ImVec2(120, 0))) {
            m_isOpenAskForNewFileNamePopup = false;
            ImGui::CloseCurrentPopup();
        }
        ImGui::EndPopup();
    }

}
const char * HeavenGateEditorFileManager::GetNewFilePath() const{
    return m_newFilePath;
}

bool HeavenGateEditorFileManager::IsExistNewFilePath() const{
    if (strlen(m_newFilePath) > 0) {
        return true;
    }
    else   {
        return false;
    }
}
void HeavenGateEditorFileManager::SetNewFilePath(const char* filePath){
    strcpy(m_newFilePath, filePath);
}


bool HeavenGateEditorFileManager::FromFileNameToFullPath(char * filePath, const char* fileName) const{
    int length = static_cast<int>(strlen(fileName));
             if (length <= 0 || length >= MAX_FILE_NAME) {
                 return false;
             }

    memset(filePath, 0, MAX_FOLDER_PATH);

    char storyPath[MAX_FOLDER_PATH];

           HeavenGateEditorUtility::GetStoryPath(storyPath);
    strcat(filePath, storyPath );

    #ifdef _WIN32
            strcat(filePath, "\\");
    #else
            strcat(filePath, "/");
    #endif
    strcat(filePath, fileName );

    strcat(filePath, ".json");

    return true;
}

void HeavenGateEditorFileManager::Initialize()
{
    memset(m_newFilePath, 0, sizeof(m_newFilePath));
    memset(m_newFileName, 0, sizeof(m_newFileName));
    m_isOpenAskForNewFileNamePopup = false;
}

}
