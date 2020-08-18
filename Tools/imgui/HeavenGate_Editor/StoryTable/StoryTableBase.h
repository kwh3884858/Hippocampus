
#pragma once
#ifndef StoryTableBase_h
#define StoryTableBase_h

#include "nlohmann/json.hpp"


namespace HeavenGateStoryTable {
    using json = nlohmann::json;

    class StoryTableManager
    {
    public:
        void FromJson();
        void ToJson();
    }

#endif

