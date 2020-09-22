//
//Copyright (c) 2019 Star Platinum
//
//Created by Wen Yang Wei, 2019-12-25
//example_win32_directx11, HeavenGateEditor
//
//Add Description
//
#pragma once
#ifndef HeavenGateWindowPaintMoveTable_h
#define HeavenGateWindowPaintMoveTable_h

#include "HeavenGateEditorConstant.h"
#include "HeavenGateEditorBaseWindow.h"

namespace HeavenGateEditor {

enum class Ease : int
{
    Unset = 0,
    Linear,
    InSine,
    OutSine,
    InOutSine,
    InQuad,
    OutQuad,
    InOutQuad,
    InCubic,
    OutCubic,
    InOutCubic,
    InQuart,
    OutQuart,
    InOutQuart,
    InQuint,
    OutQuint,
    InOutQuint,
    InExpo,
    OutExpo,
    InOutExpo,
    InCirc,
    OutCirc,
    InOutCirc,
    InElastic,
    OutElastic,
    InOutElastic,
    InBack,
    OutBack,
    InOutBack,
    InBounce,
    OutBounce,
    InOutBounce,
    Flash,
    InFlash,
    OutFlash,
    InOutFlash,

    Amount
};

const char EaseString[][MAX_ENUM_LENGTH] = {
    "Unset",
    "Linear",
    "InSine",
    "OutSine",
    "InOutSine",
    "InQuad",
    "OutQuad",
    "InOutQuad",
    "InCubic",
    "OutCubic",
    "InOutCubic",
    "InQuart",
    "OutQuart",
    "InOutQuart",
    "InQuint",
    "OutQuint",
    "InOutQuint",
    "InExpo",
    "OutExpo",
    "InOutExpo",
    "InCirc",
    "OutCirc",
    "InOutCirc",
    "InElastic",
    "OutElastic",
    "InOutElastic",
    "InBack",
    "OutBack",
    "InOutBack",
    "InBounce",
    "OutBounce",
    "InOutBounce",
    "Flash",
    "InFlash",
    "OutFlash",
    "InOutFlash"
};

    class HeavenGateWindowTachieMoveTable : public HeavenGateEditorBaseWindow {
        WINDOW_DECLARE("HeavenGateWindowTachieMoveTable", Window_Type::MainWindow)

    public:
        HeavenGateWindowTachieMoveTable();
        ~HeavenGateWindowTachieMoveTable();
        virtual void Initialize() override;
        virtual void Shutdown() override;
    protected:

        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu()override;

    private:
        char m_fullPath[MAX_FOLDER_PATH];

    };
}

#endif /* HeavenGateWindowPaintMoveTable_h */
