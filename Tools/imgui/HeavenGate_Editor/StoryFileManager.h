#pragma once
#ifndef StoryFileManager_h
#define StoryFileManager_h

#include "HeavenGateEditorConstant.h"

namespace HeavenGateEditor {

    class StoryJson;

    class StoryFileManager
    {

    public:
        StoryFileManager();
        ~StoryFileManager();

        //TODO
        bool CreateStoryFIle();
        bool OpenStoryFile();
        bool SaveStoryFile(const StoryJson* pStoryJson);

        //Getter and Setter
        bool GetIsSaveFileExist();

        char* GetNewFileName();
        const char* GetNewFileName()const;

        char* GetNewFilePath();
        const char* GetNewFilePath()const;

        void Initialize();
        bool IsNewFilePathExist() const;

        bool FromFileNameToFullPath(char * filePath, const char* fileName) const;

    private:

        //void SetNewFilePath(const char* filePath);
   

        bool m_isSaveFileExist;

        char m_newFilePath[MAX_FOLDER_PATH];
        char m_newFileName[MAX_FILE_NAME];
    };

}

#endif /* StoryFileManager_h */
