//
//Copyright (c) 2019 Star Platinum
//
//Created by Kong Wei Hang, 2019-12-10
//example_win32_directx11
//
//Add Description
//
#pragma once

#ifndef HeavenGateEditorBaseWindow_h
#define HeavenGateEditorBaseWindow_h


namespace HeavenGateEditor {

    //Window Name, String for identify a special windows
//    //Window Type, Window appearance depend on it
//
//#define WINDOW_DECLARE(windowClass, windowType) \
//    WINDOW_DECLARE_IMPLEMENT(windowClass, windowType, 0)

//#define WINDOW_DECLARE_COMPLEX(windowName, windowType, indexFunction) \
//    const char*         GetWindiwName() const override \
//    { \
//        int index = indexFunction(); \
//        return COMBINE_STRING(windowName,STR(index)); \
//    } \
//    Window_Type         GetWindowType() const override          { return windowType; } \

#define COMBINE(left, right) left##right

#define STR(str) #str

#define WINDOW_DECLARE(windowName, windowType) \
public: \
    const char*         GetWindiwName() const override          { return windowName; } \
    Window_Type         GetWindowType() const override          { return windowType; } \


    class HeavenGateEditorBaseWindow
    {
    public:
        enum class Window_Type
        {
            MainWindow,
            SubWindow,
            Popup
        };
        HeavenGateEditorBaseWindow();
        HeavenGateEditorBaseWindow(const HeavenGateEditorBaseWindow&) = delete;
        HeavenGateEditorBaseWindow(HeavenGateEditorBaseWindow* parent);
        virtual ~HeavenGateEditorBaseWindow();

        void Update();

        bool*const GetHandle();
        void OpenWindow();
        void CloseWindow();
        bool IsWindowOpen() const;

        virtual void Initialize() = 0;
        virtual void Shutdown() = 0;

        int GetWindowIndex() const { return m_index; }

    protected:
        virtual void UpdateMainWindow() = 0;
        virtual void UpdateMenu() = 0;
        virtual const char* GetWindiwName() const = 0;
        virtual Window_Type GetWindowType() const = 0;
        void SetParentWindow(HeavenGateEditorBaseWindow * parent);

    protected:
        static int indexCount;
        bool m_open;
        HeavenGateEditorBaseWindow *  m_parent;
        int m_index;
    };




}
#endif /* HeavenGateEditorBaseWindow_h */
