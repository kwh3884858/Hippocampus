#include "imgui.h"


#include "HeavenGateEditorWindow.h"
#include "CharacterUtility.h"

#include <iostream>
#include <vector>
#include <fstream>
#include <stdio.h>

#ifdef _WIN32

#include "Dirent/dirent.h"
#include <windows.h>

#else
#include <dirent.h>
#include <dlfcn.h>

#endif // _WIN32

using std::vector;

namespace HeavenGateEditor {

    //Constant
    const int MAX_NUM_OF_DISPLAY_FORLDERS = 50;
    const char* const HeavenGateEditor::TOOL_FOLDER_NAME = "Tools";

    const char* const HeavenGateEditor::PATH_FROM_PROJECT_ROOT_TO_STORY_FOLDER =
#ifdef _WIN32
"Assets\\Storys";
#else
"Assets/Storys";
#endif

    HeavenGateEditor::HeavenGateEditor()
    {

        show_app_layout = false;

        isSavedFile = false;
        strcpy(storyPath, "Untitled");

    }

    HeavenGateEditor::~HeavenGateEditor()
    {

    }


    void HeavenGateEditor::ShowEditorWindow(bool* isOpenPoint) {


        if (show_app_layout)              OpenSelectStoryWindow(&show_app_layout);

        // Demonstrate the various window flags. Typically you would just use the default!
        static bool no_titlebar = false;
        static bool no_scrollbar = false;
        static bool no_menu = false;
        static bool no_move = false;
        static bool no_resize = false;
        static bool no_collapse = false;
        static bool no_close = false;
        static bool no_nav = false;
        static bool no_background = false;
        static bool no_bring_to_front = false;

        ImGuiWindowFlags window_flags = 0;
        if (no_titlebar)        window_flags |= ImGuiWindowFlags_NoTitleBar;
        if (no_scrollbar)       window_flags |= ImGuiWindowFlags_NoScrollbar;
        if (!no_menu)           window_flags |= ImGuiWindowFlags_MenuBar;
        if (no_move)            window_flags |= ImGuiWindowFlags_NoMove;
        if (no_resize)          window_flags |= ImGuiWindowFlags_NoResize;
        if (no_collapse)        window_flags |= ImGuiWindowFlags_NoCollapse;
        if (no_nav)             window_flags |= ImGuiWindowFlags_NoNav;
        if (no_background)      window_flags |= ImGuiWindowFlags_NoBackground;
        if (no_bring_to_front)  window_flags |= ImGuiWindowFlags_NoBringToFrontOnFocus;
        if (no_close)           isOpenPoint = NULL; // Don't pass our bool* to Begin


      // We specify a default position/size in case there's no data in the .ini file. Typically this isn't required! We only do it to make the Demo applications a little more welcoming.
        ImGui::SetNextWindowPos(ImVec2(650, 20), ImGuiCond_FirstUseEver);
        ImGui::SetNextWindowSize(ImVec2(550, 680), ImGuiCond_FirstUseEver);

        // Main body of the Demo window starts here.
        if (!ImGui::Begin("Heaven Gate", isOpenPoint, window_flags))
        {
            // Early out if the window is collapsed, as an optimization.
            ImGui::End();
            return;
        }

        // Most "big" widgets share a common width settings by default.
        //ImGui::PushItemWidth(ImGui::GetWindowWidth() * 0.65f);    // Use 2/3 of the space for widgets and 1/3 for labels (default)
        ImGui::PushItemWidth(ImGui::GetFontSize() * -12);           // Use fixed width for labels (by passing a negative value), the rest goes to widgets. We choose a width proportional to our font size.

        // Menu Bar
        if (ImGui::BeginMenuBar())
        {
            if (ImGui::BeginMenu("Menu"))
            {
                ShowEditorMenuFile();
                ImGui::EndMenu();
            }

            ImGui::EndMenuBar();
        }

        ImGui::Text("Heaven Gate says hello. (%s)", IMGUI_VERSION);
        ImGui::Spacing();

        ImGui::Text("Current story path: %s", storyPath);


        if (!isSavedFile && m_story == nullptr)
        {
            //Create a new story
            m_story = new StoryJson();

        }
        static ImGuiInputTextFlags flags = ImGuiInputTextFlags_AllowTabInput;
        ImGui::CheckboxFlags("ImGuiInputTextFlags_ReadOnly", (unsigned int*)&flags, ImGuiInputTextFlags_ReadOnly);
        ImGui::CheckboxFlags("ImGuiInputTextFlags_AllowTabInput", (unsigned int*)&flags, ImGuiInputTextFlags_AllowTabInput);
        ImGui::CheckboxFlags("ImGuiInputTextFlags_CtrlEnterForNewLine", (unsigned int*)&flags, ImGuiInputTextFlags_CtrlEnterForNewLine);
        static char name[64 * 16];
        static char content[1024 * 16];
        for (int i = 0; i < m_story->Size(); i++)
        {

            strcpy(name, m_story->GetWord(i)->m_name.c_str());
            strcpy(content, m_story->GetWord(i)->m_content.c_str());
            ImGui::InputTextMultiline("##source", name, IM_ARRAYSIZE(name), ImVec2(-FLT_MIN, ImGui::GetTextLineHeight() * 16), flags);
            ImGui::InputTextMultiline("##source", content, IM_ARRAYSIZE(content), ImVec2(-FLT_MIN, ImGui::GetTextLineHeight() * 16), flags);

        }
        if (ImGui::Button("Add new story")) {
            if (m_story != nullptr)
            {
                m_story->AddWord("", "");
            }
        }

        //
        //        for (int i = 0; i < currentStory.size(); i++)
        //        {
        //
        //        }

                // End of ShowDemoWindow()
        ImGui::End();

    }

    // Note that shortcuts are currently provided for display only (future version will add flags to BeginMenu to process shortcuts)
    void HeavenGateEditor::ShowEditorMenuFile()
    {

        //   ImGui::MenuItem("(dummy menu)", NULL, false, false);
        if (ImGui::MenuItem("New")) {

        }
        if (ImGui::MenuItem("Open", "Ctrl+O")) {
            show_app_layout = true;
        }
        if (ImGui::BeginMenu("Open Recent"))
        {
            ImGui::MenuItem("fish_hat.c");
            ImGui::MenuItem("fish_hat.inl");
            ImGui::MenuItem("fish_hat.h");
            if (ImGui::BeginMenu("More.."))
            {
                ImGui::MenuItem("Hello");
                ImGui::MenuItem("Sailor");
                if (ImGui::BeginMenu("Recurse.."))
                {
                    ShowEditorMenuFile();
                    ImGui::EndMenu();
                }
                ImGui::EndMenu();
            }
            ImGui::EndMenu();
        }
        if (ImGui::MenuItem("Save", "Ctrl+S")) {}
        if (ImGui::MenuItem("Save As..")) {}
        ImGui::Separator();
        if (ImGui::BeginMenu("Options"))
        {
            static bool enabled = true;
            ImGui::MenuItem("Enabled", "", &enabled);
            ImGui::BeginChild("child", ImVec2(0, 60), true);
            for (int i = 0; i < 10; i++)
                ImGui::Text("Scrolling Text %d", i);
            ImGui::EndChild();
            static float f = 0.5f;
            static int n = 0;
            static bool b = true;
            ImGui::SliderFloat("Value", &f, 0.0f, 1.0f);
            ImGui::InputFloat("Input", &f, 0.1f);
            ImGui::Combo("Combo", &n, "Yes\0No\0Maybe\0\0");
            ImGui::Checkbox("Check", &b);
            ImGui::EndMenu();
        }
        if (ImGui::BeginMenu("Colors"))
        {
            float sz = ImGui::GetTextLineHeight();
            for (int i = 0; i < ImGuiCol_COUNT; i++)
            {
                const char* name = ImGui::GetStyleColorName((ImGuiCol)i);
                ImVec2 p = ImGui::GetCursorScreenPos();
                ImGui::GetWindowDrawList()->AddRectFilled(p, ImVec2(p.x + sz, p.y + sz), ImGui::GetColorU32((ImGuiCol)i));
                ImGui::Dummy(ImVec2(sz, sz));
                ImGui::SameLine();
                ImGui::MenuItem(name);
            }
            ImGui::EndMenu();
        }
        if (ImGui::BeginMenu("Disabled", false)) // Disabled
        {
            IM_ASSERT(0);
        }
        if (ImGui::MenuItem("Checked", NULL, true)) {}
        if (ImGui::MenuItem("Quit", "Alt+F4")) {}
    }

    void HeavenGateEditor::OpenSelectStoryWindow(bool* p_open) {

        ImGui::SetNextWindowSize(ImVec2(500, 440), ImGuiCond_FirstUseEver);
        if (ImGui::Begin("Open a story file", p_open, ImGuiWindowFlags_MenuBar))
        {
            if (ImGui::BeginMenuBar())
            {
                if (ImGui::BeginMenu("File"))
                {
                    if (ImGui::MenuItem("Close")) *p_open = false;
                    ImGui::EndMenu();
                }
                ImGui::EndMenuBar();
            }

            // left

            ImGui::BeginChild("left pane", ImVec2(150, 0), true);

          

            if (m_isInitializedFilesList == false) {

                DIR *dir;
                struct dirent *ent;
                ExePath(exePath);
                printf("Current Path:%s", exePath);
                m_fileIndex = 0;
                if ((dir = opendir(exePath)) != NULL) {

                    /* print all the files and directories within directory */
                    while ((ent = readdir(dir)) != NULL) {
                        printf("%s\n", ent->d_name);
                        strcpy(filesList[m_fileIndex], ent->d_name);
                        m_fileIndex++;
                    }
                    closedir(dir);
                }
                else {
                    /* could not open directory */
                    perror("");
                    printf("Can`t open story folder");
                }

                m_isInitializedFilesList = true;
            }

            for (int i = 2; i < m_fileIndex; i++)
            {


                if (ImGui::Selectable(filesList[i], selected == i))
                    selected = i;
            }
            ImGui::EndChild();
            ImGui::SameLine();

            // right


            ImGui::BeginGroup();
            ImGui::BeginChild("item view", ImVec2(0, -ImGui::GetFrameHeightWithSpacing())); // Leave room for 1 line below us
            if (selected != -1)
            {
                strcpy(fullPath, exePath);

#ifdef _WIN32
                strcat(fullPath, "\\");
#else
                strcat(fullPath, "/");
#endif

                strcat(fullPath, filesList[selected]);
            }
            ImGui::Text("File No: %d", selected);
            ImGui::Text("File Path: %s", fullPath);
            ImGui::Separator();

//            static bool isCurrentFileChange = true;

            if (ImGui::BeginTabBar("##Tabs", ImGuiTabBarFlags_None))
            {
                if (ImGui::BeginTabItem("Description"))
                {
                    if (lastSelected != selected) {

                        if (selected >= 2)
                        {
                            memset(content, 0, sizeof content);
                            std::ifstream fin;

                            fin.open(fullPath);

                            // If it could not open the file then exit.
                            if (!fin.fail())
                            {
                                int i = 0;
                                while (!fin.eof())
                                {
                                    fin >> content[i++];
                                }

                                fin.close();
                            }


                        }
                        lastSelected = selected;

                    }
                    ImGui::TextWrapped(content);
                    ImGui::TextWrapped("This is a space for content.");
                    ImGui::EndTabItem();
                }
                if (ImGui::BeginTabItem("Details"))
                {
                    ImGui::Text("ID: 0123456789");
                    ImGui::EndTabItem();
                }
                ImGui::EndTabBar();
            }
            ImGui::EndChild();

            if (ImGui::Button("Revert")) {
                char fullPath[MAX_FOLDER_PATH] = "";

                ExePath(exePath);
                strcpy(fullPath, exePath);
                #ifdef _WIN32
                                strcat(fullPath, "\\");
                #else
                                strcat(fullPath, "/");
                #endif
                strcat(fullPath, "pretty.json");
                /*        std::vector<StoryWord> c_vector;
                        StoryWord word;
                        word.m_name = "dd";
                        word.m_content = "dsa";
                        c_vector.push_back(word);
                        word.m_name = "aa";
                        word.m_content = "fffa";
                        c_vector.push_back(word);*/
                StoryJson sj;
                sj.AddWord("ff", "ff");
                sj.AddWord("dd", "aa");
                //json j_vec(c_vector);
                json j_test = sj;
                std::ofstream o(fullPath);
                o << j_test << std::endl;

            }
            ImGui::SameLine();

            if (ImGui::Button("Load")) {
                char fullPath[MAX_FOLDER_PATH] = "";
                ExePath(exePath);
                strcpy(fullPath, exePath);
                #ifdef _WIN32
                                strcat(fullPath, "\\");
                #else
                                strcat(fullPath, "/");
                #endif
                strcat(fullPath, "pretty.json");

                std::ifstream fins;
                /*   if (!i.fail())
                   {
                       int i = 0;
                       while (!i.eof())
                       {
                           std::cout << i;
                       }

                       i.close();
                   }*/
                fins.open(fullPath);

                // If it could not open the file then exit.
                if (!fins.fail())
                {
                    int i = 0;
                    while (!fins.eof())
                    {
                        fins >> content[i++];
                    }

                    fins.close();
                }
                else
                {
                    std::cerr << "Error: " << strerror(errno);
                }

                json a = json::parse(content);
                *m_story = a;
                std::cout << a;
                /* StoryJson sj;
                 sj = a;
                 for (int i = 0 ; i < sj.Size(); i++)
                 {
                     std::cout << sj.GetWord(i)->m_name;
                     std::cout << sj.GetWord(i)->m_content;

                 }*/
            }
            ImGui::SameLine();
            if (ImGui::Button("Open")) {
                if (selected >= 2)
                {
                    currentStory.clear();
                    currentStory = json::parse(content);
                    strcpy(storyPath, fullPath);
                    isSavedFile = true;

                    *p_open = false;
                }
            }
            ImGui::EndGroup();
        }
        ImGui::End();

    }



    void HeavenGateEditor::ExePath(char* const pOutExePath) {

        char cBuffer[MAX_FOLDER_PATH];

#ifdef _WIN32
        wchar_t buffer[MAX_FOLDER_PATH];
        GetModuleFileName(NULL, buffer, MAX_FOLDER_PATH);
        CharacterUtility::convertWcsToMbs(cBuffer, buffer,MAX_FOLDER_PATH);
#else
        bool result = GetModuleFileNameOSX(cBuffer);
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

