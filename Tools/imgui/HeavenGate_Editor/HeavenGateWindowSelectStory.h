#pragma once

#include "StoryJson.h"

#include "HeavenGateEditorConstant.h"

namespace HeavenGateEditor {
class HeavenGateWindowSelectStory
{
public:
	HeavenGateWindowSelectStory();
	~HeavenGateWindowSelectStory();

    void ShowSelectStoryWindow(bool* pOpen);
    bool GetStoryPointer(StoryJson* const pStory)const;

private:

     //Prevent multi-call for directory path;
     bool m_isInitializedFilesList;
     int selected;
     char filesList[MAX_FOLDER_LIST][MAX_FOLDER_PATH];
      int  m_fileIndex ;
     char exePath[MAX_FOLDER_PATH];
     char fullPath[MAX_FOLDER_PATH] ;
     int lastSelected = 0;
     char content[MAX_CONTENT];

    StoryJson* m_story;
};

}
