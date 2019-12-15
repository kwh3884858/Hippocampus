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

#include "HeavenGateEditorBaseWindow.h"

namespace HeavenGateEditor {
    class StoryJson;
    class StoryFileManager;

    class HeavenGateWindowFileManager : public HeavenGateEditorBaseWindow{
        WINDOW_DECLARE("HeavenGateEditorFileManager", Window_Type::SubWindow)

    public:
        HeavenGateWindowFileManager();
        virtual ~HeavenGateWindowFileManager() override;

        

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu() override {}

        void SetStoryFileManager(StoryFileManager* pStoryFileManager);

    private:

        //bool SaveStoryFile(StoryJson* pStory, bool* pIsSavedFile);
        //void SetNewFilePath(const char* filePath);

        StoryFileManager* m_storyFileManager;

    };
}


#endif /* HeavenGateEditorFileManager_h */
