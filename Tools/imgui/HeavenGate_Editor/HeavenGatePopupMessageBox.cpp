
#include "HeavenGatePopupMessageBox.h"
#include "imgui.h"



namespace HeavenGateEditor {


    HeavenGatePopupMessageBox::HeavenGatePopupMessageBox()
    {
    }


    void HeavenGatePopupMessageBox::Initialize()
    {
        strcpy(m_message, "Test Message");
        //memset(m_message, '\0', sizeof(m_message));
    }

    void HeavenGatePopupMessageBox::UpdateMainWindow()
    {
        //Already have some content, maybe be is saved file but have some unsaved changes
        ImGui::OpenPopup("Message");

        if (ImGui::BeginPopupModal("Message", NULL, ImGuiWindowFlags_AlwaysAutoResize))
        {
            ImGui::Text(m_message);
            ImGui::Separator();

            if (ImGui::Button("OK", ImVec2(120, 0))) {
                ImGui::CloseCurrentPopup();
                m_open = false;
            }
            ImGui::EndPopup();
        }
    }



    void HeavenGatePopupMessageBox::SetMessage(const char * const message)
    {
        strcpy(m_message, message);
    }

}
