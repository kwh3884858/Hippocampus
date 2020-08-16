#pragma once


#include "HeavenGateEditorConstant.h"
#include "HeavenGateEditorBaseWindow.h"

namespace HeavenGateEditor {

class StoryJson;
//class StoryFileManager;
class HeavenGatePopupResolveConflictFiles;

class HeavenGateWindowSelectStory : public HeavenGateEditorBaseWindow
{
    WINDOW_DECLARE("HeavenGateWindowSelectStory", Window_Type::SubWindow)

public:
    HeavenGateWindowSelectStory() = delete;
    HeavenGateWindowSelectStory(HeavenGateEditorBaseWindow* parent);
    ~HeavenGateWindowSelectStory();

    virtual void Initialize() override;
    virtual void Shutdown() override;

    virtual void UpdateMainWindow() override;
    virtual void UpdateMenu() override;
   
    //Window Related
    int GetWindowIndex()const;
    void SetWindowIndex(int index);
    
    //void ShowSelectStoryWindow();
    //
    //void SetStoryFileManager(StoryFileManager* pStoryFileManager);

private:
    void Refresh();

    //bool OpenStoryFile();
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
    //StoryFileManager* m_fileManager;

    HeavenGatePopupResolveConflictFiles* m_popupResolveConflictFiles;
    bool* m_isOpenPopupResolveConflictFiles;

    int m_windowIndex;
   
};


}
