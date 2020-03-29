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
#include "imgui.h"

#include <deque>

namespace HeavenGateEditor {

    using json = nlohmann::json;
    using std::deque;
    class HeavenGateWindowSelectStory;
    class HeavenGatePopupInputFileName;

    //Story node family
    class StoryJson;
    class StoryWord;
    class StoryLabel;
    class StoryJump;
    class StoryExhibit;

    enum class TableType;
    class HeavenGateWindowStoryEditor : public HeavenGateEditorBaseWindow
    {
        WINDOW_DECLARE("Heaven Gate Editor", Window_Type::MainWindow)

    public:

        HeavenGateWindowStoryEditor();
        virtual ~HeavenGateWindowStoryEditor() override;
        virtual void Initialize() override;
        virtual void Shutdown() override;

    protected:

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu() override;

    private:
        static int WordContentCallback(ImGuiInputTextCallbackData* data);

        void ShowStoryWord(StoryWord* word, int index);

        void AddButton(int index);
        void AddNotification(const char * const notification);

        void AutoSaveCallback();

        //Data
        StoryJson* m_storyJson;

        //View
        HeavenGateWindowSelectStory* m_selectStoryWindow;
        HeavenGatePopupInputFileName* m_inputFileNamePopup;


        char m_notification[MAX_CONTENT];
    };


}


#endif /* HeavenGateEditorWindow_h */
