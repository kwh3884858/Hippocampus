//
//Copyright (c) 2019 Star Platinum
//
//Created by Kong Wei Hang, 2019-11-17
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//
#ifndef HeavenGateEditorWindow_h
#define HeavenGateEditorWindow_h
#include "StoryJson.h"

#include "HeavenGateEditorConstant.h"
#include "HeavenGateWindowSelectStory.h"
#include "HeavenGateEditorFileManager.h"

using std::string;
using json = nlohmann::json;

namespace HeavenGateEditor {
    class HeavenGateEditor
    {
    public:

    public:
        HeavenGateEditor();
        ~HeavenGateEditor();
        void ShowEditorWindow(bool* isOpenPoint);
        void ShowEditorMenuFile();
        void OpenSelectStoryWindow(bool* p_open);

    private:
        void ExePath(char* const outExePath);

        //Is Open Select Story Window
        bool show_app_layout;

        bool m_isSavedFile = false;
        char storyPath[MAX_FOLDER_PATH];



        json currentStory;
        StoryJson* m_story;

        HeavenGateWindowSelectStory* m_selectStoryWindow;
        HeavenGateEditorFileManager* m_fileManager;
    };


}


#endif /* HeavenGateEditorWindow_h */
