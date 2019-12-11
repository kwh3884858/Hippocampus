#include "HeavenGateEditorBaseWindow.h"
#include "imgui.h"

namespace HeavenGateEditor {

    HeavenGateEditorBaseWindow::HeavenGateEditorBaseWindow()
    {
        m_open = false;
    }

    HeavenGateEditorBaseWindow::HeavenGateEditorBaseWindow(const char * windowName)
    {
        strcpy(m_windowName, windowName);
    }

    HeavenGateEditorBaseWindow::~HeavenGateEditorBaseWindow()
    {
        m_open = false;
    }

    void HeavenGateEditorBaseWindow::Update()
    {
        UpdateMainWindow();

        UpdateMenu();
    }

     void HeavenGateEditorBaseWindow::UpdateMainWindow()
     {
         ImGuiWindowFlags window_flags = 0;
         window_flags |= ImGuiWindowFlags_MenuBar;


         // We specify a default position/size in case there's no data in the .ini file. Typically this isn't required! We only do it to make the Demo applications a little more welcoming.
         ImGui::SetNextWindowPos(ImVec2(650, 20), ImGuiCond_FirstUseEver);
         ImGui::SetNextWindowSize(ImVec2(550, 680), ImGuiCond_FirstUseEver);


         // Main body of the Demo window starts here.
         if (!ImGui::Begin("Base Window Template", &m_open, window_flags))
         {
             // Early out if the window is collapsed, as an optimization.
             ImGui::End();
             return;
         }

         // Most "big" widgets share a common width settings by default.
   //ImGui::PushItemWidth(ImGui::GetWindowWidth() * 0.65f);    // Use 2/3 of the space for widgets and 1/3 for labels (default)
         ImGui::PushItemWidth(ImGui::GetFontSize() * -12);           // Use fixed width for labels (by passing a negative value), the rest goes to widgets. We choose a width proportional to our font size.

         ImGui::Text("Placeholder");

         ImGui::End();

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

     bool * HeavenGateEditorBaseWindow::GetHandle()
     {
         return &m_open;
     }

     void HeavenGateEditorBaseWindow::OpenWindow() {
         m_open = true;
     }
     void HeavenGateEditorBaseWindow::CloseWindow() {
         m_open = false;
     }
     bool HeavenGateEditorBaseWindow::IsWindowOpen() const
     {
         return  m_open;
     }


}
