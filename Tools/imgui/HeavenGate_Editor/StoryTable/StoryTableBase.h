
#pragma once
#ifndef StoryTableBase_h
#define StoryTableBase_h

#include "nlohmann/json.hpp"


namespace HeavenGateStoryTable {
    using json = nlohmann::json;

    class StoryTableManager
    {
    public:
        

        virtual void FromJson(json& j, void* storyTable) = 0;
        virtual void ToJson(json& j, void* storyTable) = 0;
    }

#endif

