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
class HeavenGateWindowFileManager;
class StoryFileManager;

    class HeavenGateEditor : public HeavenGateEditorBaseWindow
    {
        WINDOW_DECLARE("Heaven Gate Editor", Window_Type::MainWindow)

    public:
        HeavenGateEditor() ;
        
        virtual ~HeavenGateEditor() override;

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu() override;

        //void ShowEditorWindow(bool* isOpenPoint);
        //void ShowEditorMenuFile();

        
    private:


        //Is Open Select Story Window

        //bool m_isSavedFileInCurrentWindow;
        bool m_isWritedUnsavedContent;



        json currentStory;
        StoryJson* m_story;
        StoryFileManager* m_storyFileManager;

        HeavenGateWindowSelectStory* m_selectStoryWindow;
        bool* m_selectStoryHandle;

        HeavenGateWindowFileManager* m_fileManagerWindow;
        bool* m_fileManagerHandle;

    };


}


#endif /* HeavenGateEditorWindow_h */
