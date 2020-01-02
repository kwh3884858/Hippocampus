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

#include <deque>

namespace HeavenGateEditor {

    using json = nlohmann::json;
    using std::deque;
    class HeavenGateWindowSelectStory;
    class HeavenGatePopupInputFileName;
    //class StoryFileManager;
    class StoryJson;
    enum class TableType;
    class HeavenGateWindowStoryEditor : public HeavenGateEditorBaseWindow
    {
        WINDOW_DECLARE("Heaven Gate Editor", Window_Type::MainWindow)

    public:
       

        HeavenGateWindowStoryEditor();

        virtual ~HeavenGateWindowStoryEditor() override;

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu() override;

        //void ShowEditorWindow(bool* isOpenPoint);
        //void ShowEditorMenuFile();


    private:
        void AddButton(int index);

        bool m_isWritedUnsavedContent;

        //Model
        json currentStory;
        StoryJson* m_storyJson;

        //Controller
        //StoryFileManager* m_storyFileManager;

        //View
        HeavenGateWindowSelectStory* m_selectStoryWindow;
        HeavenGatePopupInputFileName* m_inputFileNamePopup;

        TableType m_currentState;
        deque<TableType> m_editorState;
        bool m_isReadCloseLabel;
    };


}


#endif /* HeavenGateEditorWindow_h */
