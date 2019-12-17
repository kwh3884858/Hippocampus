
#include "HeavenGatePopupResolveConflictFiles.h"
#include "imgui.h"

#include "StoryJson.h"
#include "StoryFileManager.h"

namespace HeavenGateEditor {


    HeavenGatePopupResolveConflictFiles::HeavenGatePopupResolveConflictFiles()
    {
        ResetIsDiscardCurrentFile();
    }

    void HeavenGatePopupResolveConflictFiles::UpdateMainWindow()
    {
        //Already have some content, maybe be is saved file but have some unsaved changes
        ImGui::OpenPopup("File already exists in the editor");

        if (ImGui::BeginPopupModal("File already exists in the editor", NULL, ImGuiWindowFlags_AlwaysAutoResize))
        {
            ImGui::Text("If you wish to open a new file, click on Discard Current File.\nIf you wish to stop opening new files, click Cancel. \n\n");
            ImGui::Separator();


            if (ImGui::Button("Discard Current File", ImVec2(120, 0))) {


                m_isDiscardCurrentFile = ResolveConflictFileSelection::DiscardCurrentFile;


                ImGui::CloseCurrentPopup();

            }
            ImGui::SetItemDefaultFocus();
            ImGui::SameLine();
            if (ImGui::Button("Cancel", ImVec2(120, 0))) {


                m_isDiscardCurrentFile = ResolveConflictFileSelection::Cancel;

                ImGui::CloseCurrentPopup();
            }
            ImGui::EndPopup();
        }
    }



    HeavenGatePopupResolveConflictFiles::ResolveConflictFileSelection HeavenGatePopupResolveConflictFiles::GetIsDiscardCurrentFile() const
    {
        return m_isDiscardCurrentFile;  
    }

    void HeavenGatePopupResolveConflictFiles::ResetIsDiscardCurrentFile() 
    {
        m_isDiscardCurrentFile = ResolveConflictFileSelection::None;
    }

}
