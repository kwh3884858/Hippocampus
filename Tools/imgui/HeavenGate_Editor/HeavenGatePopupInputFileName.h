//
//  HeavenGateEditorFileManager.hpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 27/11/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//

#ifndef HeavenGateEditorFileManager_h
#define HeavenGateEditorFileManager_h

#include <stdio.h>

#include "HeavenGateEditorConstant.h"
#include "HeavenGateEditorBaseWindow.h"

namespace HeavenGateEditor {
    class StoryJson;


    class HeavenGatePopupInputFileName : public HeavenGateEditorBaseWindow{
        WINDOW_DECLARE("HeavenGateEditorFileManager", Window_Type::Popup)

    public:
        HeavenGatePopupInputFileName();
        virtual ~HeavenGatePopupInputFileName() override;

        

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu() override {}

        //void SetStoryFileManager(StoryFileManager* pStoryFileManager);
        //void SetStoryJsonPonter(StoryJson** ppStory);
    private:
        void Initialize();

        //bool SaveStoryFile(StoryJson* pStory, bool* pIsSavedFile);
        //void SetNewFilePath(const char* filePath);
        char m_fileName[MAX_FOLDER_PATH];
        char m_filePath[MAX_FILE_NAME];

        //StoryFileManager* m_storyFileManager;
        //StoryJson** m_ppStory;
    };
}


#endif /* HeavenGateEditorFileManager_h */
