//
//  HeavenGateEditorFileManager.cpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 27/11/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//
#include "imgui.h"


#include "HeavenGatePopupInputFileName.h"
#include "HeavenGateWindowStoryEditor.h"
#include "StoryFileManager.h"
#include "StoryJsonManager.h"
#include "StoryJson.h"

namespace HeavenGateEditor {

    HeavenGatePopupInputFileName::HeavenGatePopupInputFileName(HeavenGateEditorBaseWindow* parent)
        : HeavenGateEditorBaseWindow(parent)
    {
    }

    HeavenGatePopupInputFileName::~HeavenGatePopupInputFileName()
    {
    }

    void HeavenGateEditor::HeavenGatePopupInputFileName::UpdateMainWindow()
    {

        ImGui::Text("Please Input New File Name.\n\n");
        ImGui::Separator();

        ImGui::PushStyleVar(ImGuiStyleVar_FramePadding, ImVec2(0, 0));
        ImGui::InputTextWithHint("New File Name", "enter name here", m_fileName, IM_ARRAYSIZE(m_fileName));
        ImGui::PopStyleVar();

        if (ImGui::Button("OK", ImVec2(120, 0))) {
            if (m_parent !=nullptr && m_callback != nullptr)
            {
                HeavenGateWindowStoryEditor* const storyEditor = static_cast<HeavenGateWindowStoryEditor*>(m_parent);
                (storyEditor->*m_callback)(m_fileName);
            }
            else
            {
                printf("Lack of callback");
            }
            SetCallbackAfterClickOk(nullptr);
            CloseWindow();
        }

        ImGui::SetItemDefaultFocus();
        ImGui::SameLine();
        if (ImGui::Button("Cancel", ImVec2(120, 0))) {
            Initialize();

            CloseWindow();
        }

    }


    void HeavenGatePopupInputFileName::Initialize()
    {
        memset(m_fileName, 0, sizeof(m_fileName));
        //memset(m_filePath, 0, sizeof(m_filePath));
        m_callback = nullptr;
    }

    void HeavenGatePopupInputFileName::Shutdown()
    {
        m_callback = nullptr;
    }

    const char* HeavenGatePopupInputFileName::GetFileName()const
    {
        return m_fileName;
    }

    void HeavenGatePopupInputFileName::SetCallbackAfterClickOk(Callback callback)
    {
        m_callback = callback;
    }

    //void HeavenGateWindowFileManager::SetNewFilePath(const char* filePath) {

    //    if (m_storyFileManager != nullptr)
    //    {
    //        m_storyFileManager->SetNewFilePath(pStoryJson);
    //    }
    //}


}
