
#include "imgui.h"
#include "HeavenGateWindowSelectStory.h"
#include "HeavenGatePopupResolveConflictFiles.h"
#include "HeavenGateEditorUtility.h"

#include "StoryJsonManager.h"
#include "StoryFileManager.h"
#include "StoryJson.h"




namespace HeavenGateEditor {
    HeavenGateWindowSelectStory::HeavenGateWindowSelectStory()
    {

    }


    HeavenGateWindowSelectStory::~HeavenGateWindowSelectStory()
    {

     
    }

    void HeavenGateWindowSelectStory::Initialize()
    {
        m_popupResolveConflictFiles = new HeavenGatePopupResolveConflictFiles;
        m_isOpenPopupResolveConflictFiles = m_popupResolveConflictFiles->GetHandle();

        Refresh();
    }

    void HeavenGateWindowSelectStory::Shutdown()
    {
        m_isOpenPopupResolveConflictFiles = nullptr;

        if (m_popupResolveConflictFiles)
        {
            delete m_popupResolveConflictFiles;
        }
        m_popupResolveConflictFiles = nullptr;

        Refresh();
    }

    void HeavenGateWindowSelectStory::Refresh()
    {
        m_isInitializedFilesList = false;
        m_fileCount = 0;

        memset(m_filesList, 0, MAX_FOLDER_LIST * MAX_FOLDER_PATH);
        memset(m_storyPath, 0, sizeof(m_storyPath));
        memset(m_fullPath, 0, sizeof(m_fullPath));
        memset(m_fullContent, 0, sizeof(m_fullContent));

        m_selected = 0;
        m_lastSelected = m_selected;


    }
    void HeavenGateWindowSelectStory::UpdateMainWindow()
    {
        // left
        ShowLeftColumn();

        // right
        ShowRightColumn();

        //Pop up
        ShowPopup();
    }

    void HeavenGateWindowSelectStory::UpdateMenu()
    {
        ShowMenuBar();
    }

    //void HeavenGateWindowSelectStory::ShowSelectStoryWindow() {

    //    if (!m_open) {
    //        return;
    //    }

    //    ImGui::SetNextWindowSize(ImVec2(500, 440), ImGuiCond_FirstUseEver);

    //    if (ImGui::Begin("Open a story file", &m_open, ImGuiWindowFlags_MenuBar))
    //    {
    //        ShowMenuBar();

    //        // left
    //        ShowLeftColumn();

    //        // right
    //        ShowRightColumn();

    //    }
    //    ImGui::End();
    //}

    //bool HeavenGateWindowSelectStory::OpenStoryFile()
    //{
    //    // Is Loaded Story

    //    if (StoryJsonManager::get().GetStoryJson() != nullptr) {

    //        if (*m_ppStory == nullptr)
    //        {
    //            //Current main window don`t have any story

    //            *m_ppStory = new StoryJson;
    //            m_fileManager->LoadStoryFile(m_fullPath, *m_ppStory);
    //            Initialize();
    //            CloseWindow();
    //            /*   *pIsFileSaved = true;*/
    //        }
    //        else
    //        {
    //            *m_isOpenPopupResolveConflictFiles = true;
    //            ////Already have some content, maybe be is saved file but have some unsaved changes
    //            //ImGui::OpenPopup("File already exists in the editor");

    //            //if (ImGui::BeginPopupModal("File already exists in the editor", NULL, ImGuiWindowFlags_AlwaysAutoResize))
    //            //{
    //            //    ImGui::Text("If you wish to open a new file, click on Discard Current File.\nIf you wish to stop opening new files, click Cancel. \n\n");
    //            //    ImGui::Separator();


    //            //    if (ImGui::Button("Discard Current File", ImVec2(120, 0))) {

    //            //        Initialize();
    //            //        ImGui::CloseCurrentPopup();
    //            //        CloseWindow();
    //            //    }
    //            //    ImGui::SetItemDefaultFocus();
    //            //    ImGui::SameLine();
    //            //    if (ImGui::Button("Cancel", ImVec2(120, 0))) {


    //            //        (*m_ppStory)->Clear();

    //            //        m_fileManager->OpenStoryFile(m_fullPath, *m_ppStory);
    //            //        Initialize();

    //            //        /*               *pIsFileSaved = true;*/
    //            //        ImGui::CloseCurrentPopup();
    //            //        CloseWindow();
    //            //    }
    //            //    ImGui::EndPopup();
    //            //}
    //        }

    //    }

    //    return true;
    //}



    //void HeavenGateWindowSelectStory::SetStoryFileManager(StoryFileManager* pStoryFileManager)
    //{
    //    m_fileManager = pStoryFileManager;
    //}

    //void HeavenGateWindowSelectStory::SetStoryJsonPonter(StoryJson** ppStory)
    //{
    //    m_ppStory = ppStory;
    //}






    //void HeavenGateWindowSelectStory::InitFileList(char(*pOutFileList)[MAX_FOLDER_PATH], int maxFileCount) {

    //    char exePath[MAX_FOLDER_PATH];
    //    DIR *dir;
    //    struct dirent *ent;
    //    HeavenGateEditorUtility::GetStoryPath(exePath);
    //    printf("Current Path:%s", exePath);
    //    m_fileIndex = 0;
    //    if ((dir = opendir(exePath)) != NULL) {

    //        /* print all the files and directories within directory */
    //        while ((ent = readdir(dir)) != NULL) {

    //            printf("%s\n", ent->d_name);
    //            //Unity meta files
    //            if (CharacterUtility::Find(
    //                ent->d_name,
    //                strlen(ent->d_name),
    //                ".meta",
    //                strlen(".meta")) != -1)
    //            {
    //                break;
    //            }

    //            //MacOS system files
    //            if (CharacterUtility::Find(
    //                ent->d_name,
    //                strlen(ent->d_name),
    //                ".DS_Store",
    //                strlen(".DS_Store")) != -1)
    //            {
    //                break;
    //            }

    //            strcpy(pOutFileList[m_fileIndex], ent->d_name);
    //            m_fileIndex++;
    //        }
    //        closedir(dir);
    //    }
    //    else {
    //        /* could not open directory */
    //        perror("");
    //        printf("Can`t open story folder");
    //    }

    //}
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

            StoryFileManager::Instance().InitFileList(m_filesList, &m_fileCount);

            m_isInitializedFilesList = true;
        }

        //Display file list
        for (int i = 2; i < m_fileCount; i++)
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
                        StoryFileManager::Instance().GetFileContent(m_fullPath, m_fullContent);
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

        //        if (ImGui::Button("Write Test")) {
        //            char fullPath[MAX_FOLDER_PATH] = "";
        //
        //
        //            strcpy(fullPath, GetStoryPath());
        //
        //#ifdef _WIN32
        //            strcat(fullPath, "\\");
        //#else
        //            strcat(fullPath, "/");
        //#endif
        //
        //            strcat(fullPath, "pretty.json");
        //
        //            StoryJson sj;
        //            sj.AddWord("ff", "ff");
        //            sj.AddWord("dd", "aa");
        //
        //            json j_test = sj;
        //            std::ofstream o(fullPath);
        //            o << j_test << std::endl;
        //            o.close();
        //        }
        //ImGui::SameLine();
        ////

        if (ImGui::Button("Refresh")) {
            Refresh();
        }

        ImGui::SameLine();

        if (ImGui::Button("Open")) {
            if (m_selected >= 2)
            {
                *m_isOpenPopupResolveConflictFiles = true;

            }
        }
    }

    void HeavenGateWindowSelectStory::ShowPopup()
    {
        m_popupResolveConflictFiles->Update();

        switch (m_popupResolveConflictFiles->GetIsDiscardCurrentFile())
        {
        case HeavenGatePopupResolveConflictFiles::ResolveConflictFileSelection::DiscardCurrentFile:
        {

            StoryJson* story = StoryJsonManager::Instance().GetStoryJson();
            story->Clear();
            StoryFileManager::Instance().LoadStoryFile(m_fullPath, story);

            m_popupResolveConflictFiles->ResetIsDiscardCurrentFile();
            m_popupResolveConflictFiles->CloseWindow();

            Refresh();
            CloseWindow();
            break;
        }
        case HeavenGatePopupResolveConflictFiles::ResolveConflictFileSelection::Cancel:
        {

            m_popupResolveConflictFiles->ResetIsDiscardCurrentFile();
            m_popupResolveConflictFiles->CloseWindow();

            Refresh();
            CloseWindow();

            break;
        }

        default:
            break;
        }

      
    }

    //bool HeavenGateWindowSelectStory::GetStoryPointer(StoryJson** ppStory)const {
    //    
    //}

    //void HeavenGateWindowSelectStory::GetContent(char* fullPath) {

    //    memset(m_fullContent, 0, sizeof m_fullContent);
    //    std::ifstream fin;

    //    fin.open(fullPath);

    //    // If it could not open the file then exit.
    //    if (!fin.fail())
    //    {
    //        int i = 0;
    //        while (!fin.eof())
    //        {
    //            if (i >= MAX_FULL_CONTENT) {
    //                std::cerr << "Out of max content limit";
    //            }
    //            fin >> m_fullContent[i++];
    //        }

    //        fin.close();
    //    }


    //}

}
