#include "HeavenGateEditorBaseWindow.h"
#include "imgui.h"

namespace HeavenGateEditor {
    HeavenGateEditorBaseWindow::HeavenGateEditorBaseWindow(){
        m_open = false;
        SetParentWindow(nullptr);
    }
    HeavenGateEditorBaseWindow::HeavenGateEditorBaseWindow(HeavenGateEditorBaseWindow* parent)
    {
        m_open = false;
        SetParentWindow(parent);
    }

    HeavenGateEditorBaseWindow::~HeavenGateEditorBaseWindow()
    {
        m_open = false;
        m_parent = nullptr;
    }

    void HeavenGateEditorBaseWindow::Update()
    {
        if (!m_open)
        {
            return;
        }

        //Only for popup
        if (GetWindowType() == Window_Type::Popup)
        {
            ImGui::OpenPopup(GetWindiwName());

            if (ImGui::BeginPopupModal(GetWindiwName(), NULL, ImGuiWindowFlags_AlwaysAutoResize))
            {
                UpdateMainWindow();

                ImGui::EndPopup();
            }

            return;
        }

        //For regular windows
        ImGuiWindowFlags window_flags = 0;
        window_flags |= ImGuiWindowFlags_MenuBar;

        switch (GetWindowType())
        {
        case Window_Type::MainWindow:
        {
            // We specify a default position/size in case there's no data in the .ini file. Typically this isn't required! We only do it to make the Demo applications a little more welcoming.
            ImGui::SetNextWindowPos(ImVec2(650, 20), ImGuiCond_FirstUseEver);
            ImGui::SetNextWindowSize(ImVec2(550, 680), ImGuiCond_FirstUseEver);

            break;
        }

        case Window_Type::SubWindow:
        {
            ImGui::SetNextWindowSize(ImVec2(500, 440), ImGuiCond_FirstUseEver);

            break;
        }

        default:
            break;
        }



        // Main body of the Demo window starts here.
        if (!ImGui::Begin(GetWindiwName(), &m_open, window_flags))
        {
            // Early out if the window is collapsed, as an optimization.
            ImGui::End();
            return;
        }

        // Most "big" widgets share a common width settings by default.
        //ImGui::PushItemWidth(ImGui::GetWindowWidth() * 0.65f);    // Use 2/3 of the space for widgets and 1/3 for labels (default)
        ImGui::PushItemWidth(ImGui::GetFontSize() * -12);           // Use fixed width for labels (by passing a negative value), the rest goes to widgets. We choose a width proportional to our font size.

        UpdateMainWindow();

        // Menu Bar
        if (ImGui::BeginMenuBar())
        {
            if (ImGui::BeginMenu("Menu"))
            {
                UpdateMenu();
                ImGui::EndMenu();
            }

            ImGui::EndMenuBar();
        }


        ImGui::End();
    }

    void HeavenGateEditorBaseWindow::UpdateMainWindow()
    {
        ImGui::Text("Placeholder");
    }

    void HeavenGateEditorBaseWindow::UpdateMenu()
    {
        //   ImGui::MenuItem("(dummy menu)", NULL, false, false);
        if (ImGui::MenuItem("New")) {

        }

        if (ImGui::MenuItem("Open", "Ctrl+O")) {

        }

        if (ImGui::MenuItem("Save", "Ctrl+S")) {

        }
    }
    const char* HeavenGateEditorBaseWindow::GetWindiwName() const {
        return "WindowTemplate";
    }
    HeavenGateEditorBaseWindow::Window_Type HeavenGateEditorBaseWindow::GetWindowType() const
    {
        return Window_Type::MainWindow;
    }

    void HeavenGateEditorBaseWindow::SetParentWindow(HeavenGateEditorBaseWindow * parent)
    {
        if (parent)
        {
            m_parent = parent;
        }
    }


    bool*const HeavenGateEditorBaseWindow::GetHandle()
    {
        return &m_open;
    }

    void HeavenGateEditorBaseWindow::OpenWindow()
    {
        m_open = true;
    }

    void HeavenGateEditorBaseWindow::CloseWindow() {
        switch (GetWindowType())
        {

        case Window_Type::Popup:
        {
            ImGui::CloseCurrentPopup();
            break;
        }
        default:
            break;
        }

        m_open = false;
    }
    bool HeavenGateEditorBaseWindow::IsWindowOpen() const
    {
        return  m_open;
    }


}
