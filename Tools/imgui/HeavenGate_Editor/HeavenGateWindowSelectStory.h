#pragma once


#include "HeavenGateEditorConstant.h"

namespace HeavenGateEditor {

class StoryJson;

class HeavenGateWindowSelectStory
{
public:
    HeavenGateWindowSelectStory();
    ~HeavenGateWindowSelectStory();

    void ShowSelectStoryWindow();
    bool GetStoryPointerWindow(StoryJson** ppStory, bool* pIsFileSaved);
    bool GetStoryPointer(StoryJson** ppStory)const;
    char* GetStoryPath();

    void OpenWindow();
    void CloseWindow();
    bool IsOpenWindow() const;
    bool* GetWindowHandle();

    bool IsLoadedSotry() const;
    bool GiveUpLoadedStory();
private:
    void Initialize();
    void Destory();

    void InitFileList(char (* pOutFileList) [MAX_FOLDER_PATH], int maxFileCount);
    void GetContent(char* fullPath);

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
    char m_fullContent[MAX_FULL_CONTENT];

    int m_selected;
    int m_lastSelected ;


    StoryJson* m_story;

    bool m_open;
};


}
