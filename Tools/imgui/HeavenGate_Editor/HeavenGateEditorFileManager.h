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

#include "StoryJson.h"
#include "HeavenGateEditorConstant.h"

namespace HeavenGateEditor {
class HeavenGateEditorFileManager {

public:
    HeavenGateEditorFileManager();
    ~HeavenGateEditorFileManager();

    bool SaveStoryFile(StoryJson* pStoryJson) const;
    void OpenAskForNameWindow();
    void ShowAskForNewFileNamePopup();

    const char * GetNewFilePath() const;
    bool IsExistNewFilePath() const;
    void SetNewFilePath(const char* filePatj);

    bool FromFileNameToFullPath(char * filePath, const char* fileName) const;

    void Initialize();
private:
    char m_newFilePath[MAX_FOLDER_PATH];
    char m_newFileName[MAX_FILE_NAME];

    bool m_isOpenAskForNewFileNamePopup;
};
}


#endif /* HeavenGateEditorFileManager_h */
