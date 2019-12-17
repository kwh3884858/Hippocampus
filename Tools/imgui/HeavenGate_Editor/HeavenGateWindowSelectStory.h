#pragma once


#include "HeavenGateEditorConstant.h"
#include "HeavenGateEditorBaseWindow.h"

namespace HeavenGateEditor {

class StoryJson;
class StoryFileManager;
class HeavenGatePopupResolveConflictFiles;

class HeavenGateWindowSelectStory : public HeavenGateEditorBaseWindow
{
    WINDOW_DECLARE("HeavenGateWindowSelectStory", Window_Type::SubWindow)

public:
    HeavenGateWindowSelectStory();
    ~HeavenGateWindowSelectStory();

    virtual void UpdateMainWindow() override;
    virtual void UpdateMenu() override;
   

    //void ShowSelectStoryWindow();
    //
    void SetStoryFileManager(StoryFileManager* pStoryFileManager);
    void SetStoryJsonPonter(StoryJson** ppStory);
private:
    void Initialize();
    void Destory();

    bool GiveUpLoadedStory();
    bool OpenStoryFile();
    //void InitStoryPath();

    //bool IsLoadedSotry() const;
    //bool GetStoryPointer(StoryJson** ppStory) const;
    char* GetStoryPath();
    //void GetContent(char* fullPath);

    void ShowMenuBar();
    void ShowLeftColumn();
    void ShowRightColumn();
    void ShowFileInfo();
    void ShowDescription();
    void ShowFileButton();
    void ShowPopup();
    //Prevent multi-call for directory path;
    bool m_isInitializedFilesList;
    int  m_fileCount ;

    char m_filesList[MAX_FOLDER_LIST][MAX_FOLDER_PATH];
    char m_storyPath[MAX_FOLDER_PATH];
    char m_fullPath[MAX_FOLDER_PATH] ;
    char m_fullContent[MAX_FULL_CONTENT];

    int m_selected;
    int m_lastSelected ;


    StoryJson** m_ppStory;
    StoryFileManager* m_fileManager;

    HeavenGatePopupResolveConflictFiles* m_popupResolveConflictFiles;
    bool* m_isOpenPopupResolveConflictFiles;
   
};


}
