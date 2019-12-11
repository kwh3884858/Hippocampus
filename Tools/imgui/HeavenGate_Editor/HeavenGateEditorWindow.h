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

#include "HeavenGateEditorConstant.h"
#include "HeavenGateEditorBaseWindow.h"

#include "nlohmann/json.hpp"


namespace HeavenGateEditor {

    using json = nlohmann::json;

class StoryJson;
class HeavenGateWindowSelectStory;
class HeavenGateEditorFileManager;

    class HeavenGateEditor : public HeavenGateEditorBaseWindow
    {

    public:
        HeavenGateEditor() ;
        
        virtual ~HeavenGateEditor() override;

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu() override;

        void ShowEditorWindow(bool* isOpenPoint);
        void ShowEditorMenuFile();

        
    private:


        //Is Open Select Story Window

        bool m_isSavedFile;
        bool m_isWritedUnsavedContent;



        json currentStory;
        StoryJson* m_story;

        HeavenGateWindowSelectStory* m_selectStoryWindow;

        HeavenGateEditorFileManager* m_fileManager;

    };


}


#endif /* HeavenGateEditorWindow_h */
