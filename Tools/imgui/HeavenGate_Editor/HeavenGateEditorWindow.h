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



namespace HeavenGateEditor {

    using std::string;
    using json = nlohmann::json;


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


        //Is Open Select Story Window
        bool show_app_layout;

        bool m_isSavedFile;
        bool m_isWritedUnsavedContent;



        json currentStory;
        StoryJson* m_story;

        HeavenGateWindowSelectStory* m_selectStoryWindow;
        HeavenGateEditorFileManager* m_fileManager;
    };


}


#endif /* HeavenGateEditorWindow_h */
