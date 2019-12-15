#include "StoryFileManager.h"
#include "HeavenGateEditorUtility.h"
#include "StoryJson.h"

#include <fstream>

namespace HeavenGateEditor {

    StoryFileManager::StoryFileManager()
    {
        Initialize();
    }

    StoryFileManager::~StoryFileManager()
    {
    }

    bool StoryFileManager::SaveStoryFile(const StoryJson* pStoryJson)
    {
        if (pStoryJson == nullptr) {
            return false;
        }

        strcpy( m_newFilePath , pStoryJson->GetFullPath());

        if (strlen(m_newFilePath) <= 0) {
            return false;
        }

        json j_test = *pStoryJson;
        std::ofstream o(m_newFilePath);
        o << j_test << std::endl;

        o.close();

        Initialize();

        return true;
    }
    bool StoryFileManager::GetIsSaveFileExist()
    {
        return m_isSaveFileExist;
    }
 
    char * StoryFileManager::GetNewFileName()
    {
        return const_cast<char*>(const_cast<const StoryFileManager*>(this)->GetNewFileName());
    }

    const char * StoryFileManager::GetNewFileName() const
    {
        return m_newFileName;
    }

    char * StoryFileManager::GetNewFilePath()
    {
        return const_cast<char *>(const_cast<const StoryFileManager*>(this)->GetNewFilePath());
    }

    const char * StoryFileManager::GetNewFilePath() const
    {
        return m_newFilePath;
    }

    void StoryFileManager::Initialize()
    {
        memset(m_newFilePath, 0, sizeof(m_newFilePath));
        memset(m_newFileName, 0, sizeof(m_newFileName));
    }

    bool StoryFileManager::IsNewFilePathExist() const
    {
        if (strlen(m_newFilePath) > 0) {
            return true;
        }
        else {
            return false;
        }
    }

    bool StoryFileManager::FromFileNameToFullPath(char * filePath, const char * fileName) const
    {
        int length = static_cast<int>(strlen(fileName));

        if (length <= 0 || length >= MAX_FILE_NAME) {
            return false;
        }

        memset(filePath, 0, MAX_FOLDER_PATH);

        char storyPath[MAX_FOLDER_PATH];

        HeavenGateEditorUtility::GetStoryPath(storyPath);
        strcat(filePath, storyPath);

#ifdef _WIN32
        strcat(filePath, "\\");
#else
        strcat(filePath, "/");
#endif
        strcat(filePath, fileName);

        strcat(filePath, ".json");

        return true;
    }

    //void StoryFileManager::SetNewFilePath(const char * filePath)
    //{
    //    strcpy(m_newFilePath, filePath);
    //}
 
}
