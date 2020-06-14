#pragma once

#ifndef HeavenGatePopupMessageBox_h
#define HeavenGatePopupMessageBox_h



#include "HeavenGateEditorBaseWindow.h"
#include "HeavenGateEditorConstant.h"

namespace HeavenGateEditor {

    class HeavenGatePopupMessageBox : public HeavenGateEditorBaseWindow
    {
        WINDOW_DECLARE("HeavenGatePopupMessageBox", Window_Type::Popup)

    public:

        HeavenGatePopupMessageBox();
        virtual ~HeavenGatePopupMessageBox() override {}

        virtual void Initialize() override;
        virtual void Shutdown() override { }
        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu() override {}

        void SetMessage(const char* const message);
    private:
        char m_message[MAX_CONTENT];
    };




}
#endif /* HeavenGatePopupResolveConflictFiles_h */
