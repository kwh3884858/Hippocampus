#pragma once


#include "HeavenGateEditorConstant.h"
#include "HeavenGateEditorBaseWindow.h"

namespace HeavenGateEditor {

class StoryJson;

class HeavenGateWindowSelectStory : public HeavenGateEditorBaseWindow
{
    WINDOW_DECLARE("HeavenGateWindowSelectStory", Window_Type::SubWindow)

public:
    HeavenGateWindowSelectStory();
    ~HeavenGateWindowSelectStory();

    virtual void UpdateMainWindow() override;
    virtual void UpdateMenu() override;

    //void ShowSelectStoryWindow();
    bool GetStoryPointerWindow(StoryJson** ppStory, bool* pIsFileSaved);
   
   

    
    bool GiveUpLoadedStory();
private:
    void Initialize();
    void Destory();

    //void InitStoryPath();
    void InitFileList(char (* pOutFileList) [MAX_FOLDER_PATH], int maxFileCount);

  

    //bool IsLoadedSotry() const;
    bool GetStoryPointer(StoryJson** ppStory) const;
    char* GetStoryPath();
    void GetContent(char* fullPath);

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
