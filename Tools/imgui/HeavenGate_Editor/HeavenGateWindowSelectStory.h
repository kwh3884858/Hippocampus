#pragma once

#include "StoryJson.h"

#include "HeavenGateEditorConstant.h"

namespace HeavenGateEditor {
class HeavenGateWindowSelectStory
{
public:
    HeavenGateWindowSelectStory();
    ~HeavenGateWindowSelectStory();

    void ShowSelectStoryWindow();
    bool GetStoryPointer(StoryJson* const pStory)const;
    char* GetStoryPath();

    void OpenWindow();
    void CloseWindow();
    bool IsOpenWindow() const;

private:
    void InitFileList(char (* pOutFileList) [MAX_FOLDER_PATH], int maxFileCount);
    void GetContent(char* fullPath);
    void GetStoryPath(char* const outExePath)const;
    void InitStoryPath();


    void ShowMenuBar();
    void ShowLeftColumn();
    void ShowRightColumn();
    void ShowFileInfo();
    void ShowDescription();
    void ShowFileButton();

    //Prevent multi-call for directory path;
    bool m_isInitializedFilesList;
    int  m_fileIndex ;

    char m_filesList[MAX_FOLDER_LIST][MAX_FOLDER_PATH];
    char m_storyPath[MAX_FOLDER_PATH];
    char m_fullPath[MAX_FOLDER_PATH] ;
    char m_content[MAX_CONTENT];

    int m_selected;
    int m_lastSelected ;


    StoryJson* m_story;

    bool m_open;
};

#ifndef _WIN32
bool GetModuleFileNameOSX(char *  pOutCurrentPath);
#endif

}
