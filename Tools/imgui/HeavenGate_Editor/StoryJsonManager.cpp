

#include "StoryJson.h"

#include "StoryJsonManager.h"

namespace HeavenGateEditor {
    

    bool StoryJsonManager::Initialize()
    {
        m_storyJson = new StoryJson;

        return true;
    }

    bool StoryJsonManager::Shutdown()
    {
        delete m_storyJson;
        m_storyJson = nullptr;

        return true;
    }

    void StoryJsonManager::ReplaceStoryJson(StoryJson* storyJson)
    {
        if (m_storyJson == storyJson)
        {
            return;
        }

        delete m_storyJson;
        m_storyJson = storyJson;
    }


    StoryJson* const  StoryJsonManager::GetStoryJson()
    {
        return const_cast<StoryJson*>(static_cast<const StoryJsonManager*>(this)->GetStoryJson());
    }

    const StoryJson* const StoryJsonManager::GetStoryJson() const
    {
        return m_storyJson;
    }



}
