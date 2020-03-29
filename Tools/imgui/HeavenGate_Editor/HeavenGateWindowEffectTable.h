//
//Copyright (c) 2019 Star Platinum
//
//Created by Wen Yang Wei, 2020-02-05
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//
#pragma once
#ifndef HeavenGateWindowEffectTable_h
#define HeavenGateWindowEffectTable_h

#include "HeavenGateEditorConstant.h"
#include "HeavenGateEditorBaseWindow.h"

namespace HeavenGateEditor {


    class HeavenGateWindowEffectTable : public HeavenGateEditorBaseWindow {
        WINDOW_DECLARE("HeavenGateWindowEffectTable", Window_Type::MainWindow)

    public:
        HeavenGateWindowEffectTable();
        ~HeavenGateWindowEffectTable();
        virtual void Initialize() override;
        virtual void Shutdown() override;

    protected:

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu()override;

    private:
        char m_fullPath[MAX_FOLDER_PATH];

    };
}

#endif /* HeavenGateWindowEffectTable_h */
