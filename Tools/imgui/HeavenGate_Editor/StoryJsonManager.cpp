

#include "StoryJson.h"

#include "StoryJsonManager.h"
#include "StoryFileManager.h"

namespace HeavenGateEditor {
    

    bool StoryJsonManager::Initialize()
    {
        m_storyJson = new StoryJson;

        char filePath[MAX_FOLDER_PATH];
        StoryFileManager::Instance().FromFileNameToFullPath(filePath, DEFAULT_FILE_NAME);

        StoryJson* storyJson = const_cast<StoryJson*>(m_storyJson);
        storyJson->SetFullPath( const_cast<const char*>( filePath ) );


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
