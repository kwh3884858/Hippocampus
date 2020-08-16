#pragma once

#ifndef HeavenGateWindowPreview_h
#define HeavenGateWindowPreview_h

#include "HeavenGateEditorBaseWindow.h"

namespace HeavenGateEditor {

class StoryWord;

    class HeavenGateWindowPreview : public HeavenGateEditorBaseWindow
    {
        WINDOW_DECLARE("Heaven Gate Preview", Window_Type::SubWindow)

    public:

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

    private:

        StoryWord* m_compiledWord;

    };

}
#endif /* HeavenGateWindowPreview_h */
