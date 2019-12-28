#pragma once

#ifndef StoryJsonManager_h
#define StoryJsonManager_h

#include "StorySingleton.h"
namespace HeavenGateEditor {
    class StoryJson;

    class StoryJsonManager final : public StorySingleton<StoryJsonManager>
    {

    public:
        StoryJsonManager() = default;
        virtual ~StoryJsonManager() override = default;
        //initialize function, take the place of constructor
        virtual bool Initialize() override;
        //destroy function, take the  place of destructor
        virtual bool Shutdown() override;


        void ReplaceStoryJson(StoryJson* storyJson);
        const StoryJson* const GetStoryJson()const;
        StoryJson* const GetStoryJson();


    private:


        const StoryJson* m_storyJson;

    };



#endif // StoryJsonManager

}
