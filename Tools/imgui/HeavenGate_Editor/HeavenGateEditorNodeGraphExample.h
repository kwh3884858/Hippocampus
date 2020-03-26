#pragma once
#ifndef HeavenGateEditorNodeGraphExample_h
#define HeavenGateEditorNodeGraphExample_h

#include "HeavenGateEditorBaseWindow.h"

#include "HeavenGateEditorConstant.h"

namespace HeavenGateEditor {
    class HeavenGateEditorNodeGraphExample : public HeavenGateEditorBaseWindow
    {
        WINDOW_DECLARE("HeavenGateEditorNodeGraphExample", Window_Type::MainWindow)
    public:
        HeavenGateEditorNodeGraphExample();
        ~HeavenGateEditorNodeGraphExample();

    protected:

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu()override;

    };

}
#endif /* HeavenGateEditorNodeGraphExample_h */
