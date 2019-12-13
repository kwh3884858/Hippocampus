#include "imgui.h"

#include "HeavenGateEditorWindow.h"
#include "CharacterUtility.h"
#include "StoryJson.h"
#include "HeavenGateWindowSelectStory.h"
#include "HeavenGateEditorFileManager.h"

#include <iostream>
#include <vector>

#include <stdio.h>



using std::vector;

namespace HeavenGateEditor {
    //using NodeType = StoryNode::NodeType;

    HeavenGateEditor::HeavenGateEditor()
    {



        m_isSavedFile = false;
        m_isWritedUnsavedContent = false;


        m_selectStoryWindow = new HeavenGateWindowSelectStory();

        m_fileManager = new HeavenGateEditorFileManager();

    }

    HeavenGateEditor::~HeavenGateEditor()
    {
        if (m_selectStoryWindow != nullptr) {
            delete m_selectStoryWindow;
        }
        if (m_fileManager != nullptr) {
            delete m_fileManager;
        }
    }

    void HeavenGateEditor::UpdateMainWindow()
    {
        m_selectStoryWindow->Update();
        m_selectStoryWindow->GetStoryPointerWindow(&m_story, &m_isSavedFile);


        //For save as new file
        m_fileManager->ShowAskForNewFileNamePopup();
        if (m_fileManager->IsExistNewFilePath())
        {
            if (!m_fileManager->SaveStoryFile(m_story)) {
                return;
            }
            const char* filePath = m_fileManager->GetNewFilePath();
            m_story->SetFullPath(filePath);
            m_isSavedFile = true;
            m_fileManager->Initialize();
        }

        ImGui::Text("Heaven Gate says hello. (%s)", IMGUI_VERSION);
        ImGui::Spacing();
        if (m_isSavedFile == true &&
            m_story != nullptr &&
            m_story->IsExistFullPath() == true) {
            ImGui::Text("Current story path: %s", m_story->GetFullPath());

        }


        static ImGuiInputTextFlags flags = ImGuiInputTextFlags_AllowTabInput;
        ImGui::CheckboxFlags("ImGuiInputTextFlags_ReadOnly", (unsigned int*)&flags, ImGuiInputTextFlags_ReadOnly);
        ImGui::CheckboxFlags("ImGuiInputTextFlags_AllowTabInput", (unsigned int*)&flags, ImGuiInputTextFlags_AllowTabInput);
        ImGui::CheckboxFlags("ImGuiInputTextFlags_CtrlEnterForNewLine", (unsigned int*)&flags, ImGuiInputTextFlags_CtrlEnterForNewLine);
        static char* name = nullptr;
        static char* content = nullptr;
        static char* label = nullptr;
        static char* jump = nullptr;

        char order[8] = "";
        ImGui::LabelText("label", "Value");
        if (m_story != nullptr) {
            for (int i = 0; i < m_story->Size(); i++)
            {



                sprintf(order, "%d", i);

                StoryNode* node = m_story->GetNode(i);
                switch (node->m_nodeType) {
                case NodeType::Word:
                {
                    char nameConstant[16] = "Name";
                    char contentConstant[16] = "Content";
                    strcat(nameConstant, order);
                    strcat(contentConstant, order);

                    StoryWord* word = static_cast<StoryWord*>(node);
                    name = word->m_name;
                    content = word->m_content;


                    ImGui::InputTextWithHint(nameConstant, "Enter name here", name, MAX_NAME);
                    ImGui::InputTextWithHint(contentConstant, "Enter Content here", content, MAX_CONTENT);

                     break;
                }

                case NodeType::Jump:
                {
                    char jumpConstant[16] = "Jump";
                    strcat(jumpConstant, order);

                    StoryJump* pJump = static_cast<StoryJump*>(node);
                    jump = pJump->m_jumpId;

                    ImGui::InputTextWithHint(jumpConstant, "Enter jump ID here", jump, MAX_NAME);
                    break;
                }

                case NodeType::Label:
                {
                    char LabelConstant[16] = "Label";
                    strcat(LabelConstant, order);

                    StoryLabel *pLabel = static_cast<StoryLabel*>(node);
                    label = pLabel->m_labelId;

                    ImGui::InputTextWithHint(LabelConstant, "Enter label ID here", label, MAX_NAME);
                    break;
                }

                default:
                    break;
                }

            }
        }

        if (ImGui::Button("Add new story")) {
            //If story not exist
            if (m_story == nullptr)
            {
                m_story = new StoryJson;
            }
            //If already exist story
            if (m_story != nullptr)
            {
                m_story->AddWord("", "");
            }
        }

        ImGui::SameLine();

        if (ImGui::Button("Add new label")) {
            //If story not exist
            if (m_story == nullptr)
            {
                m_story = new StoryJson;
            }
            //If already exist story
            if (m_story != nullptr)
            {
                m_story->AddLabel("");
            }
        }
        ImGui::SameLine();

        if (ImGui::Button("Add new jump")) {
            //If story not exist
            if (m_story == nullptr)
            {
                m_story = new StoryJson;
            }
            //If already exist story
            if (m_story != nullptr)
            {
                m_story->AddJump("");
            }
        }

    }

    void HeavenGateEditor::UpdateMenu()
    {
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
        if (ImGui::MenuItem("Save", "Ctrl+S")) {
            if (m_isSavedFile) {
                m_fileManager->SetNewFilePath(m_story->GetFullPath());
                if (!m_fileManager->SaveStoryFile(m_story)) {
                    return;
                }
                m_fileManager->Initialize();

            }
            else {
                m_fileManager->OpenAskForNameWindow();

            }
        }
        if (ImGui::MenuItem("Save As..")) {


        }




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


    void HeavenGateEditor::ShowEditorWindow(bool* isOpenPoint) {


        m_selectStoryWindow->ShowSelectStoryWindow();
        m_selectStoryWindow->GetStoryPointerWindow(&m_story, &m_isSavedFile);

        //if (m_selectStoryWindow->IsLoadedSotry()) {
        //    if (m_story == nullptr) {
        //        m_selectStoryWindow->GetStoryPointer(&m_story);
        //        m_selectStoryWindow->GiveUpLoadedStory();
        //        m_isSavedFile = true;
        //    }
        //    else
        //    {
        //        ImGui::OpenPopup("Have Unsaved Content");

        //        if (ImGui::BeginPopupModal("Have Unsaved Content", NULL, ImGuiWindowFlags_AlwaysAutoResize))
        //        {
        //            ImGui::Text("You open a new file, but now workspace already have changes.\n Do you want to abandon changes to open new file or keep them?\n\n");
        //            ImGui::Separator();

        //            if (ImGui::Button("Keep Changes", ImVec2(120, 0))) {

        //                m_selectStoryWindow->GiveUpLoadedStory();
        //                ImGui::CloseCurrentPopup();

        //            }
        //            ImGui::SetItemDefaultFocus();
        //            ImGui::SameLine();
        //            if (ImGui::Button("Give Up Changes", ImVec2(120, 0))) {

        //                delete m_story;
        //                m_story = nullptr;

        //                m_selectStoryWindow->GetStoryPointer(&m_story);
        //                m_selectStoryWindow->GiveUpLoadedStory();

        //                m_isSavedFile = true;
        //                ImGui::CloseCurrentPopup();
        //            }
        //            ImGui::EndPopup();
        //        }
        //    }

        //}

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

        //For save as new file
        m_fileManager->ShowAskForNewFileNamePopup();
        if (m_fileManager->IsExistNewFilePath())
        {
            if (!m_fileManager->SaveStoryFile(m_story)) {
                return;
            }
            const char* filePath = m_fileManager->GetNewFilePath();
            m_story->SetFullPath(filePath);
            m_isSavedFile = true;
            m_fileManager->Initialize();
        }

        ImGui::Text("Heaven Gate says hello. (%s)", IMGUI_VERSION);
        ImGui::Spacing();
        if (m_isSavedFile == true &&
            m_story != nullptr &&
            m_story->IsExistFullPath() == true) {
            ImGui::Text("Current story path: %s", m_story->GetFullPath());

        }


        //        if (!isSavedFile && m_story == nullptr)
        //        {
        //            m_selectStoryWindow->GetStoryPointer(m_story);
        //            if (m_story == nullptr)
        //            {
        //                //Create a new story
        //                m_story = new StoryJson();
        //
        //            }
        //
        //        }
        static ImGuiInputTextFlags flags = ImGuiInputTextFlags_AllowTabInput;
        ImGui::CheckboxFlags("ImGuiInputTextFlags_ReadOnly", (unsigned int*)&flags, ImGuiInputTextFlags_ReadOnly);
        ImGui::CheckboxFlags("ImGuiInputTextFlags_AllowTabInput", (unsigned int*)&flags, ImGuiInputTextFlags_AllowTabInput);
        ImGui::CheckboxFlags("ImGuiInputTextFlags_CtrlEnterForNewLine", (unsigned int*)&flags, ImGuiInputTextFlags_CtrlEnterForNewLine);
        static char* name = nullptr;
        static char* content = nullptr;
        static char* label = nullptr;
        static char* jump = nullptr;

        char order[8] = "";
        ImGui::LabelText("label", "Value");
        if (m_story != nullptr) {
            for (int i = 0; i < m_story->Size(); i++)
            {

      
                
                sprintf(order, "%d", i);
         
                StoryNode* node = m_story->GetNode(i);
                switch (node->m_nodeType) {
                case NodeType::Word:
                {
                    char nameConstant[16] = "Name";
                    char contentConstant[16] = "Content";
                    strcat(nameConstant, order);
                    strcat(contentConstant, order);

                    StoryWord* word = static_cast<StoryWord*>(node);
                    name = word->m_name;
                    content = word->m_content;


                    ImGui::InputTextWithHint(nameConstant, "Enter name here", name, MAX_NAME);
                    ImGui::InputTextWithHint(contentConstant, "Enter Content here", content, MAX_CONTENT);

                    //Multiline Version
                    /*
                    ImGui::InputTextMultiline(nameConstant, name, MAX_NAME, ImVec2(-FLT_MIN, ImGui::GetTextLineHeight() * 2), flags);
                    ImGui::InputTextMultiline(contentConstant, content, MAX_CONTENT, ImVec2(-FLT_MIN, ImGui::GetTextLineHeight() * 6), flags);
    */
                    break;
                }

                case NodeType::Jump:
                {
                    char jumpConstant[16] = "Jump";
                    strcat(jumpConstant, order);

                    StoryJump* pJump = static_cast<StoryJump*>(node);
                    jump = pJump->m_jumpId;

                    ImGui::InputTextWithHint(jumpConstant, "Enter jump ID here", jump, MAX_NAME);
                    break;
                }

                case NodeType::Label:
                {
                    char LabelConstant[16] = "Label";
                    strcat(LabelConstant, order);

                    StoryLabel *pLabel = static_cast<StoryLabel*>(node);
                    label = pLabel->m_labelId;

                    ImGui::InputTextWithHint(LabelConstant, "Enter label ID here", label, MAX_NAME);
                    break;
                }

                default:
                    break;
                }

            }
        }

        if (ImGui::Button("Add new story")) {
            //If story not exist
            if (m_story == nullptr)
            {
                m_story = new StoryJson;
            }
            //If already exist story
            if (m_story != nullptr)
            {
                m_story->AddWord("", "");
            }
        }

        ImGui::SameLine();

        if (ImGui::Button("Add new label")) {
            //If story not exist
            if (m_story == nullptr)
            {
                m_story = new StoryJson;
            }
            //If already exist story
            if (m_story != nullptr)
            {
                m_story->AddLabel("");
            }
        }
        ImGui::SameLine();

        if (ImGui::Button("Add new jump")) {
            //If story not exist
            if (m_story == nullptr)
            {
                m_story = new StoryJson;
            }
            //If already exist story
            if (m_story != nullptr)
            {
                m_story->AddJump("");
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
        if (ImGui::MenuItem("Save", "Ctrl+S")) {
            if (m_isSavedFile) {
                m_fileManager->SetNewFilePath(m_story->GetFullPath());
                if (!m_fileManager->SaveStoryFile(m_story)) {
                    return;
                }
                m_fileManager->Initialize();

            }
            else {
                m_fileManager->OpenAskForNameWindow();

            }
        }
        if (ImGui::MenuItem("Save As..")) {


        }




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





}

