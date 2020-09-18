#pragma once

#ifndef HeavenGateWindowPreview_h
#define HeavenGateWindowPreview_h

#include "HeavenGateEditorBaseWindow.h"
#include <vector>
namespace HeavenGateEditor {

class StoryWord;

    class HeavenGateWindowPreview : public HeavenGateEditorBaseWindow
    {
        WINDOW_DECLARE("Heaven Gate Preview", Window_Type::SubWindow)

    public:
        enum MessageType {
            Error,
            Warning
        };

        HeavenGateWindowPreview() = delete;
        HeavenGateWindowPreview(HeavenGateEditorBaseWindow* parent):HeavenGateEditorBaseWindow(parent)
        {}
        virtual ~HeavenGateWindowPreview() override {}

        virtual void Initialize() override;
        virtual void Shutdown() override;
        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu() override {}

        //Window Related
        int GetWindowIndex() const;
        void SetWindowIndex(int index);

        void SetPreviewWord(const StoryWord& word);

        void AddMessage(MessageType messageType, int index, const char* const message);
        void ClearMessage();
    private:

        StoryWord* m_compiledWord;
        std::vector<MessageType> m_cacheMessageType;
        std::vector<int> m_cacheIndex;
        std::vector<const char*> m_cacheMessaage;
    };

}
#endif /* HeavenGateWindowPreview_h */
