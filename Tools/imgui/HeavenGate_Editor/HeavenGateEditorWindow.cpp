#include "imgui.h"


#include "HeavenGateEditorWindow.h"
#include "CharacterUtility.h"

#include <iostream>
#include <vector>

#include <stdio.h>



using std::vector;

namespace HeavenGateEditor {

    HeavenGateEditor::HeavenGateEditor()
    {

        show_app_layout = false;

        isSavedFile = false;
        strcpy(storyPath, "Untitled");

        m_selectStoryWindow = nullptr;

    }

    HeavenGateEditor::~HeavenGateEditor()
    {
        if (m_selectStoryWindow!= nullptr) {
            delete m_selectStoryWindow;
        }
    }


    void HeavenGateEditor::ShowEditorWindow(bool* isOpenPoint) {


       if (m_selectStoryWindow == nullptr) {
            m_selectStoryWindow =new HeavenGateWindowSelectStory();
        }
        m_selectStoryWindow->ShowSelectStoryWindow();


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
            m_selectStoryWindow->GetStoryPointer(m_story);
            if (m_story == nullptr)
            {
                //Create a new story
                m_story = new StoryJson();

            }
 
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
            m_selectStoryWindow->OpenWindow();
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

//        if (m_selectStoryWindow != nullptr &&
//            m_selectStoryWindow->IsOpenWindow()) {
//
//            if (*p_open) {
//                m_selectStoryWindow->ShowSelectStoryWindow();
//                *p_open =m_selectStoryWindow->IsOpenWindow();
//            }
//        }

    }



}

