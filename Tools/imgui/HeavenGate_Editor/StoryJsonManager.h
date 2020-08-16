#pragma once

#ifndef StoryJsonManager_h
#define StoryJsonManager_h

#include "StorySingleton.h"
#include <vector>
namespace HeavenGateEditor {

    using std::vector;
    class StoryJson;

    //High level json manager
    class StoryJsonManager final : public StorySingleton<StoryJsonManager>
    {

    public:
        StoryJsonManager() = default;
        virtual ~StoryJsonManager() override = default;
        //initialize function, take the place of constructor
        virtual bool Initialize() override;
        //destroy function, take the  place of destructor
        virtual bool Shutdown() override;

        UniqueID CreateNewStoryJson(const char * const fileName = DEFAULT_FILE_NAME);
        bool DeleteStoryJson(UniqueID Id);
        UniqueID LoadStoryJson(const char* pPath);
//        void AddStoryJson(const StoryJson* const story);
        bool ReplaceStoryJson(StoryJson* storyJson);
        const StoryJson* const GetStoryJson(UniqueID Id)const;
        StoryJson* const GetStoryJson(UniqueID Id);

    private:
         vector<StoryJson*> m_storyJsonArray;
    };



#endif // StoryJsonManager

}
