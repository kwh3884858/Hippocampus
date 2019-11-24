#include "HeavenGateWindowSelectStory.h"


namespace HeavenGateEditor {
HeavenGateWindowSelectStory::HeavenGateWindowSelectStory()
{
    m_isInitializedFilesList = false;


    m_story = nullptr;

    selected = 0;
    m_fileIndex = 0;
    memset(exePath, 0, sizeof(exePath));
    memset(fullPath, 0, sizeof(fullPath));
    lastSelected = selected;
    memset(content, 0, sizeof(content));
}


HeavenGateWindowSelectStory::~HeavenGateWindowSelectStory()
{
}
void HeavenGateWindowSelectStory::ShowSelectStoryWindow(bool* pOpen){

}

bool HeavenGateWindowSelectStory::GetStoryPointer(StoryJson* const pStory)const{
    if (m_story == nullptr) {
        return false;
    }
    else{
        return true;
    }
}

}
