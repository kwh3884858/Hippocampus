#pragma once
#ifndef HeavenGateTimeUtilitySingleton_h
#define HeavenGateTimeUtilitySingleton_h

#include <time.h>

namespace HeavenGateEditor {

    //class HeavenGateWindowStoryEditor;

    template<typename T>
    class StoryTimer final
    {
       
        

    public:
        typedef  void(T::*StoryEditorCallback)();
        StoryTimer() = default;
         ~StoryTimer()  = default;
  
        static void Update();
        static time_t GetTime();

        static void AddCallback(int interval, T* storyEditor, StoryEditorCallback callback);
        
      
    private:
        static time_t m_interval;
        static time_t m_lastTime;

        static StoryEditorCallback m_timeCallback;
        static T* m_this;
    };

    template<typename T>
#ifdef _WIN32
    __declspec(selectany)
#endif
T* StoryTimer<T>::m_this = nullptr;


    template<typename T>
    #ifdef _WIN32
        __declspec(selectany)
    #endif
typename StoryTimer<T>::StoryEditorCallback StoryTimer<T>::m_timeCallback  = nullptr;

    //__declspec(selectany) void (T::*StoryTimer<T>::m_timeCallback) () = nullptr;


    template<typename T>
    #ifdef _WIN32
        __declspec(selectany)
    #endif
time_t StoryTimer<T>::m_lastTime = 0;

    template<typename T>
    #ifdef _WIN32
        __declspec(selectany)
    #endif
time_t StoryTimer<T>::m_interval = 0;
 

    //template<typename T>
    //HeavenGateEditor::StoryTimer<T>::StoryTimer()
    //{
    //    m_lastTime = 0;
    //    m_interval = 0;
    //    m_timeCallback = nullptr;
    //    
    //    return true;
    //}

    //template<typename T>
    //HeavenGateEditor::StoryTimer<T>::~StoryTimer()
    //{
    //    m_lastTime = 0;
    //    m_interval = 0;
    //    m_timeCallback = nullptr;

    //    return true;
    //}

    template<typename T>
    void HeavenGateEditor::StoryTimer<T>::Update()
    {
        if (m_this == nullptr)
        {
            return;
        }

        time_t currentTime = GetTime();

        double timeInterval = difftime(currentTime, m_lastTime);

        if (timeInterval >= m_interval)
        {
            if (m_timeCallback != nullptr)
            {
                (m_this->*m_timeCallback)();
            }

            m_lastTime = currentTime;
        }

    }

    template<typename T>
    void HeavenGateEditor::StoryTimer<T>::AddCallback(int interval, T* storyEditor, StoryEditorCallback callback)
    {
        m_interval = interval;
        m_this = storyEditor;
        m_timeCallback = callback;
        m_lastTime = GetTime();
    }

    template<typename T>
    time_t HeavenGateEditor::StoryTimer<T>::GetTime()
    {
        return time(nullptr);
    }
}
#endif /* HeavenGateTimeUtilitySingleton_h */
