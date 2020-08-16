

#include "StoryJson.h"

#include "StoryJsonManager.h"
#include "StoryFileManager.h"
#include "StoryJsonUniqueId.h"

namespace HeavenGateEditor {
    

    bool StoryJsonManager::Initialize()
    {
//        m_storyJson = new StoryJson;
//        char filePath[MAX_FOLDER_PATH];
//        StoryFileManager::Instance().FromFileNameToFullPath(filePath, DEFAULT_FILE_NAME);
//
//        StoryJson* storyJson = const_cast<StoryJson*>(m_storyJson);
//        storyJson->SetFullPath( const_cast<const char*>( filePath ) );

        return true;
    }

    bool StoryJsonManager::Shutdown()
    {
//        delete m_storyJsonArray;
//        m_storyJsonArray = nullptr;
//
//        return true;

        for (auto iter = m_storyJsonArray.begin(); iter != m_storyJsonArray.end(); iter++) {
            if (*iter != nullptr) {
                delete *iter;
                *iter = nullptr;
            }
        }
        m_storyJsonArray.clear();
        return true;
    }

    bool StoryJsonManager::ReplaceStoryJson(StoryJson* storyJson)
    {
        UniqueID storyId = storyJson->GetID();
        for (auto iter = m_storyJsonArray.begin(); iter != m_storyJsonArray.end(); iter++) {
            if ((*iter)->GetID() == storyId) {
                delete *iter;
                *iter = storyJson;
                return true;
            }
        }
        return false;
    }

    UniqueID StoryJsonManager::CreateNewStoryJson(const char * const fileName){
        StoryJson* storyJson = new StoryJson;
        char filePath[MAX_FOLDER_PATH];

        StoryFileManager::Instance().FromFileNameToFullPath(filePath, fileName);
        storyJson->SetFullPath( const_cast<const char*>( filePath ) );

        m_storyJsonArray.push_back(storyJson);
        return storyJson->GetID();
    }

    bool StoryJsonManager::DeleteStoryJson(UniqueID Id){
        for (auto iter = m_storyJsonArray.cbegin(); iter != m_storyJsonArray.cend(); iter++) {
            if ((*iter)->GetID() == Id)
            {
                iter = m_storyJsonArray.erase(iter);
                return true;
            }
        }
        return false;
    }

UniqueID StoryJsonManager::LoadStoryJson(const char* pPath){
    UniqueID id = CreateNewStoryJson();
    StoryJson* const storyJson = GetStoryJson(id);
    storyJson->Clear();

    bool result = StoryFileManager::Instance().LoadStoryFile(pPath, storyJson);
    assert(result == true);
    assert(storyJson->IsIdValid() == true);

    return storyJson->GetID();
}

    const StoryJson* const StoryJsonManager::GetStoryJson(unsigned long int Id) const
    {
        for (auto iter = m_storyJsonArray.begin(); iter != m_storyJsonArray.end(); iter++) {
            if ((*iter)->GetID() == Id) {
                return *iter;
            }
        }
        return nullptr;
    }
    StoryJson* const  StoryJsonManager::GetStoryJson(unsigned long int Id)
    {
        return const_cast<StoryJson*>(static_cast<const StoryJsonManager*>(this)->GetStoryJson(Id));
    }




}
