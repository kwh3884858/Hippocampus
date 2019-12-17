#pragma once

#ifndef HeavenGatePopupResolveConflictFiles_h
#define HeavenGatePopupResolveConflictFiles_h



#include "HeavenGateEditorBaseWindow.h"




namespace HeavenGateEditor {

    class HeavenGatePopupResolveConflictFiles : public HeavenGateEditorBaseWindow
    {
        WINDOW_DECLARE("HeavenGatePopupResolveConflictFiles", Window_Type::Popup)

    public:
        enum class ResolveConflictFileSelection
        {
            None,
            DiscardCurrentFile,
            Cancel
        };
        HeavenGatePopupResolveConflictFiles();
        virtual ~HeavenGatePopupResolveConflictFiles() override {}

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu() override {}

        ResolveConflictFileSelection GetIsDiscardCurrentFile()const;
        void ResetIsDiscardCurrentFile();
    private:
        ResolveConflictFileSelection m_isDiscardCurrentFile;
    };




}
#endif /* HeavenGatePopupResolveConflictFiles_h */
