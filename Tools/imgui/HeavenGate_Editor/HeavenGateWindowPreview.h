#pragma once

#ifndef HeavenGateWindowPreview_h
#define HeavenGateWindowPreview_h

#include "HeavenGateEditorBaseWindow.h"

namespace HeavenGateEditor {

    class HeavenGateWindowPreview : public HeavenGateEditorBaseWindow
    {
        WINDOW_DECLARE("HeavenGateWindowPreview", Window_Type::SubWindow)

    public:

        HeavenGateWindowPreview();
        virtual ~HeavenGateWindowPreview() override {}

        virtual void Initialize() override;
        virtual void Shutdown() override { }
        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu() override {}

    private:
    };

}
#endif /* HeavenGateWindowPreview_h */
