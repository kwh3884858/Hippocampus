
#include "imgui.h"
#include "HeavenGateWindowSelectStory.h"
#include "CharacterUtility.h"

#include <fstream>
#include <iostream>
#include <string>

#ifdef _WIN32

#include "Dirent/dirent.h"
#include <windows.h>

#else
#include <dirent.h>
#include <dlfcn.h>

#endif // _WIN32

namespace HeavenGateEditor {
HeavenGateWindowSelectStory::HeavenGateWindowSelectStory()
{
    m_isInitializedFilesList = false;

    m_story = nullptr;
    m_fileIndex = 0;

    memset(m_filesList, 0, MAX_FOLDER_LIST * MAX_FOLDER_PATH);
    memset(m_storyPath, 0, sizeof(m_storyPath));
    memset(m_fullPath, 0, sizeof(m_fullPath));
    memset(m_content, 0, sizeof(m_content));

    m_selected = 0;
    m_lastSelected = m_selected;

}


HeavenGateWindowSelectStory::~HeavenGateWindowSelectStory()
{
    if (m_story == nullptr) {
        delete m_story;
    }
}

void HeavenGateWindowSelectStory::ShowSelectStoryWindow(){

    if (!m_open) {
        return;
    }

    ImGui::SetNextWindowSize(ImVec2(500, 440), ImGuiCond_FirstUseEver);
    if (ImGui::Begin("Open a story file", &m_open, ImGuiWindowFlags_MenuBar))
    {
        ShowMenuBar();

        // left
        ShowLeftColumn();

        // right
        ShowRightColumn();

    }
    ImGui::End();
}

void HeavenGateWindowSelectStory::OpenWindow(){
    m_open = true;
}
void HeavenGateWindowSelectStory::CloseWindow(){
    m_open = false;
}
bool HeavenGateWindowSelectStory::IsOpenWindow() const{
    return  m_open;
}

void HeavenGateWindowSelectStory::InitFileList(char (* pOutFileList) [MAX_FOLDER_PATH], int maxFileCount){

    char exePath[MAX_FOLDER_PATH];
    DIR *dir;
    struct dirent *ent;
    GetStoryPath(exePath);
    printf("Current Path:%s", exePath);
    m_fileIndex = 0;
    if ((dir = opendir(exePath)) != NULL) {

        /* print all the files and directories within directory */
        while ((ent = readdir(dir)) != NULL) {
            printf("%s\n", ent->d_name);
            strcpy(pOutFileList[m_fileIndex], ent->d_name);
            m_fileIndex++;
        }
        closedir(dir);
    }
    else {
        /* could not open directory */
        perror("");
        printf("Can`t open story folder");
    }

}
void HeavenGateWindowSelectStory::InitStoryPath() {
    GetStoryPath(m_storyPath);
}
char* HeavenGateWindowSelectStory::GetStoryPath() {
    if (strlen(m_storyPath) == 0) {
        InitStoryPath();
    }

    return m_storyPath;
}
void HeavenGateWindowSelectStory::ShowMenuBar(){
    if (ImGui::BeginMenuBar())
    {
        if (ImGui::BeginMenu("File"))
        {
            if (ImGui::MenuItem("Close")) {
                CloseWindow();

            }
            ImGui::EndMenu();
        }
        ImGui::EndMenuBar();
    }
}

void HeavenGateWindowSelectStory::ShowLeftColumn(){
    ImGui::BeginChild("left pane", ImVec2(150, 0), true);

    if (m_isInitializedFilesList == false) {

        InitFileList(m_filesList, MAX_FOLDER_PATH);

        m_isInitializedFilesList = true;
    }

    //Display file list
    for (int i = 2; i < m_fileIndex; i++)
    {
        if (ImGui::Selectable(m_filesList[i], m_selected == i))
            m_selected = i;
    }
    ImGui::EndChild();
    ImGui::SameLine();
}

void HeavenGateWindowSelectStory::ShowRightColumn(){

    ImGui::BeginGroup();

    ShowFileInfo();
    ShowDescription();
    ShowFileButton();

    ImGui::EndGroup();
}

void HeavenGateWindowSelectStory::ShowFileInfo(){

    // Leave room for 1 line below us
    if (m_selected != -1  && m_selected != m_lastSelected)
    {
        memset(m_fullPath, 0, sizeof(m_fullPath));
        strcpy(m_fullPath, GetStoryPath());

#ifdef _WIN32
        strcat(m_fullPath, "\\");
#else
        strcat(m_fullPath, "/");
#endif

        strcat(m_fullPath, m_filesList[m_selected]);
    }
    ImGui::Text("File No: %d", m_selected);
    ImGui::Text("File Path: %s", m_fullPath);
    ImGui::Separator();

}
void HeavenGateWindowSelectStory::ShowDescription(){

    ImGui::BeginChild("item view", ImVec2(0, -ImGui::GetFrameHeightWithSpacing()));

    if (ImGui::BeginTabBar("##Tabs", ImGuiTabBarFlags_None))
    {
        if (ImGui::BeginTabItem("Description"))
        {
            if (m_lastSelected != m_selected) {

                if (m_selected >= 2)
                {
                    GetContent(m_fullPath);
                }
                m_lastSelected = m_selected;

            }
            ImGui::TextWrapped("%s", m_content);
            ImGui::TextWrapped("This is a space for content.");
            ImGui::EndTabItem();
        }
        if (ImGui::BeginTabItem("Details"))
        {
            ImGui::Text("测试中文");
            ImGui::EndTabItem();
        }
        ImGui::EndTabBar();
    }
    ImGui::EndChild();

}
void HeavenGateWindowSelectStory::ShowFileButton(){

    if (ImGui::Button("Write Test")) {
        char fullPath[MAX_FOLDER_PATH] = "";


        strcpy(fullPath, GetStoryPath());

#ifdef _WIN32
        strcat(fullPath, "\\");
#else
        strcat(fullPath, "/");
#endif

        strcat(fullPath, "pretty.json");

        StoryJson sj;
        sj.AddWord("ff", "ff");
        sj.AddWord("dd", "aa");

        json j_test = sj;
        std::ofstream o(fullPath);
        o << j_test << std::endl;

    }
    ImGui::SameLine();
    //

    ImGui::SameLine();

    if (ImGui::Button("Open")) {
        if (m_selected >= 2)
        {
            std::ifstream fins;

            fins.open(m_fullPath);

            // If it could not open the file then exit.
            if (!fins.fail())
            {
                int i = 0;
                while (!fins.eof())
                {
                    fins >> m_content[i++];
                }

                fins.close();
            }
            else
            {
                std::cerr << "Error: " << strerror(errno);
                return;
            }

            if(m_story == nullptr){
                m_story = new StoryJson();
            }
            json a = json::parse(m_content);
            *m_story = a;
            CloseWindow();
            std::cout << a;
        }
    }
}

bool HeavenGateWindowSelectStory::GetStoryPointer(StoryJson* const pStory)const{
    if (m_story == nullptr) {
        return false;
    }
    else{
        return true;
    }
}

void HeavenGateWindowSelectStory::GetContent(char* fullPath){

    memset(m_content, 0, sizeof m_content);
    std::ifstream fin;

    fin.open(fullPath);

    // If it could not open the file then exit.
    if (!fin.fail())
    {
        int i = 0;
        while (!fin.eof())
        {
            fin >> m_content[i++];
        }

        fin.close();
    }


}

void HeavenGateWindowSelectStory::GetStoryPath(char* const pOutExePath)const {

    char cBuffer[MAX_FOLDER_PATH];

#ifdef _WIN32

    wchar_t buffer[MAX_FOLDER_PATH];
    GetModuleFileName(NULL, buffer, MAX_FOLDER_PATH);
    CharacterUtility::convertWcsToMbs(cBuffer, buffer,MAX_FOLDER_PATH);
#else
    bool result = GetModuleFileNameOSX(cBuffer);

    if (!result) {
        return;
    }
#endif


    string::size_type pos = string(cBuffer).find(TOOL_FOLDER_NAME);
    string path(cBuffer);
    path = path.substr(0, pos);
    path = path.append(PATH_FROM_PROJECT_ROOT_TO_STORY_FOLDER);
    printf("  %s  \n", path.c_str());
    strcpy(pOutExePath, path.c_str());

    return;
}


#ifndef _WIN32
bool GetModuleFileNameOSX(char* pOutCurrentPath) {
    Dl_info module_info;
    if (dladdr(reinterpret_cast<void*>(GetModuleFileNameOSX), &module_info) == 0) {
        // Failed to find the symbol we asked for.
        return false;
    }

    CharacterUtility::copyCharPointer(pOutCurrentPath, module_info.dli_fname) ;
    return  true;
}
#endif
}
