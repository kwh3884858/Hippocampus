
#include "imgui.h"
#include "HeavenGateWindowSelectStory.h"
#include "HeavenGateEditorUtility.h"
#include "StoryJson.h"
#include "CharacterUtility.h"

#include <fstream>
#include <iostream>


#ifdef _WIN32

#include "Dirent/dirent.h"


#else
#include <dirent.h>


#endif // _WIN32

namespace HeavenGateEditor {
    HeavenGateWindowSelectStory::HeavenGateWindowSelectStory()
    {
        Initialize();

    }


    HeavenGateWindowSelectStory::~HeavenGateWindowSelectStory()
    {
        Destory();

    }

    void HeavenGateWindowSelectStory::UpdateMainWindow()
    {
        // left
        ShowLeftColumn();

        // right
        ShowRightColumn();
    }

    void HeavenGateWindowSelectStory::UpdateMenu()
    {
        ShowMenuBar();
    }

    void HeavenGateWindowSelectStory::ShowSelectStoryWindow() {

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

    bool HeavenGateWindowSelectStory::GetStoryPointerWindow(StoryJson** ppStory, bool* pIsFileSaved)
    {
        // Is Loaded Story
        if (m_story != nullptr) {

            if (*ppStory == nullptr)
            {
                //Current main window don`t have any story

                GetStoryPointer(ppStory);
                GiveUpLoadedStory();
                *pIsFileSaved = true;
            }
            else
            {
                //Already have some content, maybe be is saved file but have some unsaved changes
                ImGui::OpenPopup("Have Unsaved Content");

                if (ImGui::BeginPopupModal("Have Unsaved Content", NULL, ImGuiWindowFlags_AlwaysAutoResize))
                {
                    ImGui::Text("You open a new file, but now workspace already have changes.\n Do you want to abandon changes to open new file or keep them?\n\n");
                    ImGui::Separator();


                    if (ImGui::Button("Keep Changes", ImVec2(120, 0))) {

                        GiveUpLoadedStory();
                        ImGui::CloseCurrentPopup();

                    }
                    ImGui::SetItemDefaultFocus();
                    ImGui::SameLine();
                    if (ImGui::Button("Give Up Changes", ImVec2(120, 0))) {

                        delete *ppStory;
                        *ppStory = nullptr;

                        GetStoryPointer(ppStory);
                        GiveUpLoadedStory();

                        *pIsFileSaved = true;
                        ImGui::CloseCurrentPopup();
                    }
                    ImGui::EndPopup();
                }
            }

        }

        return true;
    }

    //void HeavenGateWindowSelectStory::OpenWindow() {
    //    m_open = true;
    //}
    //void HeavenGateWindowSelectStory::CloseWindow() {
    //    m_open = false;
    //}
    //bool HeavenGateWindowSelectStory::IsOpenWindow() const {
    //    return  m_open;
    //}
    //bool * HeavenGateWindowSelectStory::GetWindowHandle()
    //{
    //    return &m_open;
    //}
    //bool HeavenGateWindowSelectStory::IsLoadedSotry() const {
    //    return ;
    //}

    bool HeavenGateWindowSelectStory::GiveUpLoadedStory()
    {
        if (m_story == nullptr)
        {
            return false;
        }
        Destory();

        Initialize();

        return true;
    }

    void HeavenGateWindowSelectStory::Initialize()
    {
        m_isInitializedFilesList = false;
        m_open = false;
        m_story = nullptr;
        m_fileIndex = 0;

        memset(m_filesList, 0, MAX_FOLDER_LIST * MAX_FOLDER_PATH);
        memset(m_storyPath, 0, sizeof(m_storyPath));
        memset(m_fullPath, 0, sizeof(m_fullPath));
        memset(m_fullContent, 0, sizeof(m_fullContent));

        m_selected = 0;
        m_lastSelected = m_selected;
    }

    void HeavenGateWindowSelectStory::Destory()
    {
        if (m_story == nullptr) {
            delete m_story;
        }
        m_story = nullptr;
    }

    void HeavenGateWindowSelectStory::InitFileList(char(*pOutFileList)[MAX_FOLDER_PATH], int maxFileCount) {

        char exePath[MAX_FOLDER_PATH];
        DIR *dir;
        struct dirent *ent;
        HeavenGateEditorUtility::GetStoryPath(exePath);
        printf("Current Path:%s", exePath);
        m_fileIndex = 0;
        if ((dir = opendir(exePath)) != NULL) {

            /* print all the files and directories within directory */
            while ((ent = readdir(dir)) != NULL) {
                printf("%s\n", ent->d_name);
                if (CharacterUtility::Find(
                    ent->d_name,
                    sizeof(ent->d_name),
                    ".meta",
                    sizeof(".meta")) != -1)
                {

                }
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
    //void HeavenGateWindowSelectStory::InitStoryPath() {

    //    HeavenGateEditorUtility::GetStoryPath(m_storyPath);

    //}
    char* HeavenGateWindowSelectStory::GetStoryPath() {
        if (strlen(m_storyPath) == 0) {
            HeavenGateEditorUtility::GetStoryPath(m_storyPath);
        }

        return m_storyPath;
    }
    void HeavenGateWindowSelectStory::ShowMenuBar() {
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

    void HeavenGateWindowSelectStory::ShowLeftColumn() {
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

    void HeavenGateWindowSelectStory::ShowRightColumn() {

        ImGui::BeginGroup();

        ShowFileInfo();
        ShowDescription();
        ShowFileButton();

        ImGui::EndGroup();
    }

    void HeavenGateWindowSelectStory::ShowFileInfo() {

        // Leave room for 1 line below us
        if (m_selected != -1 && m_selected != m_lastSelected)
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
    void HeavenGateWindowSelectStory::ShowDescription() {

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
                ImGui::TextWrapped("%s", m_fullContent);
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
    void HeavenGateWindowSelectStory::ShowFileButton() {

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
            o.close();
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
                        fins >> m_fullContent[i++];
                    }

                    fins.close();
                }
                else
                {
                    std::cerr << "Error: " << strerror(errno);
                    return;
                }

                if (m_story == nullptr) {
                    m_story = new StoryJson();
                }
                json a = json::parse(m_fullContent);

                //            json j = a[0].at("name");
                //            const char * name = a[0].at("name") .get_ptr<json::string_t *>()->c_str();
                //            printf("%s", name);
                *m_story = a;
                m_story->SetFullPath(m_fullPath);
                CloseWindow();
                std::cout << a;
                fins.close();
            }
        }
    }

    bool HeavenGateWindowSelectStory::GetStoryPointer(StoryJson** ppStory)const {
        if (m_story == nullptr) {
            return false;
        }
        else {
            *ppStory = m_story;
            return true;
        }
    }

    void HeavenGateWindowSelectStory::GetContent(char* fullPath) {

        memset(m_fullContent, 0, sizeof m_fullContent);
        std::ifstream fin;

        fin.open(fullPath);

        // If it could not open the file then exit.
        if (!fin.fail())
        {
            int i = 0;
            while (!fin.eof())
            {
                if (i >= MAX_FULL_CONTENT) {
                    std::cerr << "Out of max content limit";
                }
                fin >> m_fullContent[i++];
            }

            fin.close();
        }


    }

}
