//
//  HeavenGateEditorWindow.cpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 29/10/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//
#include "imgui.h"
#include "Dirent/dirent.h"
#include "HeavenGateEditorWindow.h"
#include "CharacterUtility.h"
#include "nlohmann/json.hpp"


#include <fstream>
#ifdef _WIN32
#include <windows.h>
#endif // _WIN32


namespace HeavenGateEditor {
    using json = nlohmann::json;

    static bool show_app_layout = false;
    bool isInitializeFilesList = false;
    bool isModifiedStory = false;
    bool isSavedFile = false;
    char storyPath[MAX_PATH] = "Untitled";
    
    static json currentStory;

    void ShowEditorWindow(bool* isOpenPoint) {


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

     
        if (!isSavedFile)
        {
            //currentStory
        }

        if (ImGui::Button("Add new story")) {}

      /*  for (int i = 0; i < currentStory.count(); i++)
        {
        }*/
        static ImGuiInputTextFlags flags = ImGuiInputTextFlags_AllowTabInput;
        ImGui::CheckboxFlags("ImGuiInputTextFlags_ReadOnly", (unsigned int*)&flags, ImGuiInputTextFlags_ReadOnly);
        ImGui::CheckboxFlags("ImGuiInputTextFlags_AllowTabInput", (unsigned int*)&flags, ImGuiInputTextFlags_AllowTabInput);
        ImGui::CheckboxFlags("ImGuiInputTextFlags_CtrlEnterForNewLine", (unsigned int*)&flags, ImGuiInputTextFlags_CtrlEnterForNewLine);


        // End of ShowDemoWindow()
        ImGui::End();

    }

    // Note that shortcuts are currently provided for display only (future version will add flags to BeginMenu to process shortcuts)
    static void ShowEditorMenuFile()
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

    static void OpenSelectStoryWindow(bool* p_open) {
        char storyDirectory[] = "../";

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
            static int selected = 0;
            ImGui::BeginChild("left pane", ImVec2(150, 0), true);

            static char filesList[20][100];
            static int  count = 0;
            static string exePath;

            if (isInitializeFilesList == false) {

                DIR *dir;
                struct dirent *ent;
                exePath = ExePath();
                printf("Current Path:%s", exePath.c_str());
                if ((dir = opendir(exePath.c_str())) != NULL) {
                    /* print all the files and directories within directory */

                    while ((ent = readdir(dir)) != NULL) {
                        printf("%s\n", ent->d_name);
                        strcpy(filesList[count], ent->d_name);
                        count++;
                    }
                    closedir(dir);
                }
                else {
                    /* could not open directory */
                    perror("");
                    printf("Can`t open story folder");
                }




                isInitializeFilesList = true;
            }

            for (int i = 2; i < count; i++)
            {

               
                if (ImGui::Selectable(filesList[i], selected == i))
                    selected = i;
            }
            ImGui::EndChild();
            ImGui::SameLine();

            // right
            char fullPath[MAX_PATH] = "";

            ImGui::BeginGroup();
            ImGui::BeginChild("item view", ImVec2(0, -ImGui::GetFrameHeightWithSpacing())); // Leave room for 1 line below us
            if (selected != -1)
            {
                strcpy(fullPath, exePath.c_str());
                strcat(fullPath, "\\");
                strcat(fullPath, filesList[selected]);
            }
            ImGui::Text("File No: %d", selected);
            ImGui::Text("File Path: %s", fullPath);
            ImGui::Separator();

            static bool isCurrentFileChange = true;
            static int lastSelected = 0;
            static char content[500];
            if (ImGui::BeginTabBar("##Tabs", ImGuiTabBarFlags_None))
            {
                if (ImGui::BeginTabItem("Description"))
                {
                    if (lastSelected != selected) {

                        if (selected >= 2)
                        {
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
            if (ImGui::Button("Revert")) {}
            ImGui::SameLine();
            if (ImGui::Button("Open")) {
                if (selected >= 2)
                {
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



    string ExePath() {
        wchar_t buffer[100];
        char cBuffer[100];
        GetModuleFileName(NULL, buffer, 100);
        CharacterUtility::convertWcsToMbs(cBuffer, buffer);
        string::size_type pos = string(cBuffer).find("Tools");
        string path(cBuffer);
        path = path.substr(0, pos);
        path = path.append("Assets\\Storys");
        printf("  %s  \n", path.c_str());
        return path;
    }

}

