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

namespace HeavenGateEditor {

    using json = nlohmann::json;

    class HeavenGateWindowSelectStory;
    class HeavenGatePopupInputFileName;
    class HeavenGatePopupMessageBox;
class HeavenGateWindowPreview;

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
        bool ImportContentInternal(StoryJson* const importedStory, const char* const needImportedContent);

        static int WordContentCallback(ImGuiInputTextCallbackData* data);

        void ShowStoryWord(StoryWord* word, int index);
        void AddButton(int index);
        void AddNotification(const char * const notification);
        void AutoSaveCallback();
        void SaveStoryFile();
        bool IsNum(const char * const);

        void CallbackNewFile(const char* fileName);
        void CallbackRenameFile(const char* fileName);

        bool CheckStringLength(const char* string, int stringLengthLimit);

        //Data
        StoryJson* m_storyJson;

        //View
        HeavenGateWindowSelectStory* m_selectStoryWindow;
        HeavenGatePopupInputFileName* m_inputFileNamePopup;
        HeavenGatePopupMessageBox* m_messageBoxPopup;
        HeavenGateWindowPreview* m_previewWindow;

        char m_notification[MAX_CONTENT];
    };


}


#endif /* HeavenGateEditorWindow_h */
